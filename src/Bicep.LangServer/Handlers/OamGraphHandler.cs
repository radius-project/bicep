// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using Bicep.LanguageServer.CompilationManager;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol;

namespace Bicep.LanguageServer.Handlers
{
    public class GraphRequest: IRequest<GraphResponse>
    {
        [JsonProperty("uri")]
        public string? Uri { get; set; }
    }

    public class GraphResponse
    {
        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("text")]
        public string? Text { get; set; }
    }

    [Method("makegraph")]
    public partial class OamGraphHandler : IJsonRpcRequestHandler<GraphRequest, GraphResponse>
    {
        private readonly ILogger logger;
        private readonly ICompilationManager compilationManager;

        public OamGraphHandler(ILogger<OamGraphHandler> logger, ICompilationManager compilationManager)
        {
            this.logger = logger;
            this.compilationManager = compilationManager;
        }

        Task<GraphResponse> IRequestHandler<GraphRequest, GraphResponse>.Handle(GraphRequest request, CancellationToken cancellationToken)
        {
            if (request.Uri is null)
            {
                return ErrorAsync("uri must be specified");
            }

            var context = this.compilationManager.GetCompilation(DocumentUri.Parse(request.Uri));
            if (context is null)
            {
                return ErrorAsync($"could not find context for {request.Uri}");
            }

            var model = context.Compilation.GetEntrypointSemanticModel();
            var visitor = new ApplicationResourceVisitor();
            visitor.Visit(model.Root);

            var applications = new List<ApplicationSymbol>(model.Root.ApplicationDeclarations);
            var graph = new Graph();

            // TODO what if there isn't one application? More? None?
            if (applications.Count > 0)
            {
                var application = applications[0];
                var applicationName = GetProperty((ObjectSyntax)application.DeclaringApplication.Body, "name");

                var items = GetComponentItems(model);

                var scopes = new HashSet<ScopeItem>();
                var outputs = new Dictionary<string, ComponentItem>();
    
                foreach (var item in items)
                {
                    var node = new Node(item.Name);
                    graph.Nodes.Add(node);
                    node.Attributes.Add("label", item.Name);

                    foreach (var output in item.Provides)
                    {
                        outputs.Add(output.Name, item);
                    }

                    foreach (var scope in item.Scopes)
                    {
                        scopes.Add(scope);
                    }
                }

                foreach (var item in items)
                {
                    foreach (var dependency in item.Dependencies)
                    {
                        var start = graph.Nodes.Single(n => n.Name == item.Name);
                        var end = graph.Nodes.Where(n => n.Name == outputs[dependency.Name].Name).FirstOrDefault();
                        if (end is null)
                        {
                            continue;
                        }

                        graph.Edges.Add(new Edge(start, end)
                        {
                            Attributes = 
                            {
                                { "label", dependency.Kind }
                            },
                        });
                    }
                }

                foreach (var scope in scopes)
                {
                    var subgraph = new Subgraph("cluster_" + graph.Subgraphs.Count);
                    subgraph.Attributes.Add("label", $"Scope - {scope.Kind}: {scope.Name}");
                    subgraph.Attributes.Add("color", graph.GetNextColor());
                    graph.Subgraphs.Add(subgraph);

                    foreach (var item in items)
                    {
                        if (item.Scopes.Contains(scope))
                        {
                            subgraph.Nodes.Add(graph.Nodes.Single(n => n.Name == item.Name));
                        }
                    }
                }
            }
            
            using var writer = new StringWriter();
            graph.WriteTo(writer);
            var text = writer.ToString();

            return Task.FromResult(new GraphResponse() { Text = text, });
        }

        private IReadOnlyList<ComponentItem> GetComponentItems(SemanticModel model)
        {
            var results = new List<ComponentItem>();
            foreach (var component in model.Root.ComponentDeclarations)
            {
                var name = GetStringValue(GetProperty((ObjectSyntax)component.Body, "name"));
                if (name is null)
                {
                    continue;
                }

                var inputs = GetDependenciesItems(component);
                var outputs = GetProvidesItems(component);
                var scopes = GetScopeItems(component);
                results.Add(new ComponentItem(name, inputs, outputs, scopes));
            }

            foreach (var instance in model.Root.InstanceDeclarations)
            {
                var name = GetStringValue(GetProperty((ObjectSyntax)instance.Body, "name"));
                if (name is null)
                {
                    continue;
                }

                var inputs = GetDependenciesItems(instance);
                var outputs = GetProvidesItems(instance);
                results.Add(new ComponentItem(name, inputs, outputs, Array.Empty<ScopeItem>()));
            }

            return results;
        }

        private IReadOnlyList<DependsOnItem> GetDependenciesItems(InstanceSymbol instance) => GetDependenciesCore((ObjectSyntax)instance.Body);

        private IReadOnlyList<DependsOnItem> GetDependenciesItems(ComponentSymbol component) => GetDependenciesCore((ObjectSyntax)component.Body);

        private IReadOnlyList<DependsOnItem> GetDependenciesCore(ObjectSyntax body)
        {
            var inputs = GetProperty(body, "properties", "dependsOn")?.Value as ArraySyntax;
            if (inputs is null)
            {
                return Array.Empty<DependsOnItem>();
            }

            var results = new List<DependsOnItem>();
            foreach (var item in inputs.Items)
            {
                var obj = item.Value as ObjectSyntax;
                if (obj is null)
                {
                    continue;
                }

                var name = GetStringValue(GetProperty(obj, "name"));
                if (name is null)
                {
                    continue;
                }

                var kind = GetStringValue(GetProperty(obj, "kind"));
                if (kind is null)
                {
                    continue;
                }

                results.Add(new DependsOnItem(name, kind));
            }

            return results;
        }

        private IReadOnlyList<ProvidesItem> GetProvidesItems(InstanceSymbol instance) => GetProvidesItemsCore((ObjectSyntax)instance.Body);
        private IReadOnlyList<ProvidesItem> GetProvidesItems(ComponentSymbol component) => GetProvidesItemsCore((ObjectSyntax)component.Body);
        private IReadOnlyList<ProvidesItem> GetProvidesItemsCore(ObjectSyntax body)
        {
            var inputs = GetProperty(body, "properties", "provides")?.Value as ArraySyntax;
            if (inputs is null)
            {
                return Array.Empty<ProvidesItem>();
            }

            var results = new List<ProvidesItem>();
            foreach (var item in inputs.Items)
            {
                var obj = item.Value as ObjectSyntax;
                if (obj is null)
                {
                    continue;
                }

                var name = GetStringValue(GetProperty(obj, "name"));
                if (name is null)
                {
                    continue;
                }

                var kind = GetStringValue(GetProperty(obj, "kind"));
                if (kind is null)
                {
                    continue;
                }

                results.Add(new ProvidesItem(name, kind));
            }

            return results;
        }

        private IReadOnlyList<ScopeItem> GetScopeItems(ComponentSymbol component)
        {
            return Array.Empty<ScopeItem>();
        }

        private ObjectPropertySyntax? GetProperty(ObjectSyntax obj, string propertyName, params string[] propertyNames)
        {
            var i = 0;
            while (true)
            {
                var properties = obj.ToNamedPropertyDictionary();
                if (!properties.TryGetValue(propertyName, out var property))
                {
                    return property;
                }

                if (i == propertyNames.Length)
                {
                    return property;
                }

                if (property?.Value as ObjectSyntax is null)
                {
                    return null;
                }

                obj = (ObjectSyntax)property.Value;
                propertyName = propertyNames[i++];
            }
        }

        private string? GetStringValue(ObjectPropertySyntax? obj)
        {
            return obj?.Value is StringSyntax @string && !@string.IsInterpolated() ? @string.TryGetLiteralValue() : null;
        }

        private Task<GraphResponse> ErrorAsync(string error)
        {
            return Task.FromResult(new GraphResponse()
            {
                Error = error,
            });
        }

        private class ComponentItem
        {
            public ComponentItem(string name, IReadOnlyList<DependsOnItem> dependencies, IReadOnlyList<ProvidesItem> outputs, IReadOnlyList<ScopeItem> scopes)
            {
                Name = name;
                Dependencies = dependencies;
                Provides = outputs;
                Scopes = scopes;
            }

            public string Name { get; }
            public IReadOnlyList<DependsOnItem> Dependencies { get; }
            public IReadOnlyList<ProvidesItem> Provides { get; }
            public IReadOnlyList<ScopeItem> Scopes { get; }
        }

        public readonly struct DependsOnItem
        {
            public DependsOnItem(string name, string kind)
            {
                Name = name;
                Kind = kind;
            }
            public string Name { get; }

            public string Kind { get; }
        }

        private readonly struct ProvidesItem
        {
            public ProvidesItem(string name, string kind)
            {
                Name = name;
                Kind = kind;
            }
            public string Name { get; }

            public string Kind { get; }
        }

        private readonly struct ScopeItem
        {
            public ScopeItem(string name, string apiVersion, string kind)
            {
                Name = name;
                ApiVersion = apiVersion;
                Kind = kind;
            }

            public string Name { get; }
            public string ApiVersion { get; }
            public string Kind { get; }
        }

        private class Node
        {
            public Node(string name)
            {
                Name = name;
            }

            public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();
            public string Name { get; }
        }

        private class Edge
        {
            public Edge(Node start, Node end)
            {
                Start = start;
                End = end;
            }

            public Node Start { get; }
            public Node End { get; }

            public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        }

        private class Subgraph
        {
            public Subgraph(string name)
            {
                Name = name;
            }

            public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

            public string Name { get; }

            public List<Node> Nodes { get; } = new List<Node>();
        }

        private class Graph
        {
            private string indent = string.Empty;

            public Graph()
            {
                Attributes.Add("rankdir", "LR");
                Attributes.Add("penwidth", "3.0");

                NodeAttributes.Add("shape", "rectangle");

                Name = "Application";
            }

            public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();
            public List<Edge> Edges { get; } = new List<Edge>();
            public string Name { get; }
            public Dictionary<string, string> NodeAttributes { get; } = new Dictionary<string, string>();
            public List<Node> Nodes { get; } = new List<Node>(); 
            public List<Subgraph> Subgraphs { get; } = new List<Subgraph>();

            public void WriteTo(TextWriter writer)
            {
                writer.Write(this.indent);
                writer.WriteLine($"digraph {Name} {{"); // just digraphs for now
                Indent();

                if (NodeAttributes.Count > 0)
                {
                    writer.Write(this.indent);
                    writer.Write("node [");
                    writer.Write(string.Join(", ", NodeAttributes.Select(kvp => $"{kvp.Key}=\"{EscapeAttribute(kvp.Value)}\"")));
                    writer.WriteLine("];");
                }

                foreach (var kvp in Attributes)
                {
                    writer.Write(this.indent);
                    writer.WriteLine($"{kvp.Key}=\"{EscapeAttribute(kvp.Value)}\";");
                }

                foreach (var node in Nodes)
                {
                    if (node.Attributes.Count == 0)
                    {
                        writer.Write(this.indent);
                        writer.WriteLine($"{EscapeName(node.Name)};");
                    }
                    else
                    {
                        writer.Write(this.indent);
                        writer.Write($"{EscapeName(node.Name)} [");
                        writer.Write(string.Join(", ", node.Attributes.Select(kvp => $"{kvp.Key}=\"{EscapeAttribute(kvp.Value)}\"")));
                        writer.WriteLine("];");
                    }
                }

                foreach (var edge in Edges)
                {
                    if (edge.Attributes.Count == 0)
                    {
                        writer.Write(this.indent);
                        writer.WriteLine($"{EscapeName(edge.Start.Name)} -> {EscapeName(edge.End.Name)};");
                    }
                    else
                    {
                        writer.Write(this.indent);
                        writer.Write($"{EscapeName(edge.Start.Name)} -> {EscapeName(edge.End.Name)} [");
                        writer.Write(string.Join(", ", edge.Attributes.Select(kvp => $"{kvp.Key}=\"{EscapeAttribute(kvp.Value)}\"")));
                        writer.WriteLine("];");
                    }
                }

                foreach (var subgraph in Subgraphs)
                {
                    writer.Write(this.indent);
                    writer.WriteLine($"subgraph {subgraph.Name} {{");
                    Indent();

                    foreach (var kvp in subgraph.Attributes)
                    {
                        writer.Write(this.indent);
                        writer.WriteLine($"{kvp.Key} =\"{EscapeAttribute(kvp.Value)}\";");
                    }

                    foreach (var node in subgraph.Nodes)
                    {
                        writer.Write(this.indent);
                        writer.WriteLine($"{EscapeName(node.Name)};");
                    }

                    Deindent();
                    writer.WriteLine("}");
                }

                Deindent();
                writer.WriteLine("}");
            }

            private void Indent()
            {
                this.indent += "    ";
            }

            private void Deindent()
            {
                this.indent = this.indent.Substring(0, this.indent.Length - 4);
            }

            // #YOLO
            private string EscapeAttribute(string value) => value;

            private string EscapeName(string name)
            {
                return name.Replace('-', '_');
            }

            public string GetNextColor()
            {
                return Colors[Subgraphs.Count];
            }
        }
    }
}