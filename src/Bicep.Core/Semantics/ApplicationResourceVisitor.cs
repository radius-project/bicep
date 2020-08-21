// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Bicep.Core.Diagnostics;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    internal class ApplicationResourceVisitor : SymbolVisitor
    {
        private Dictionary<SyntaxBase, ApplicationSymbol> _applications;
        private Dictionary<(SyntaxBase, SyntaxBase), ComponentSymbol> _components;
        private Dictionary<(SyntaxBase, SyntaxBase), DeploymentSymbol> _deployments;
        private Dictionary<(SyntaxBase, SyntaxBase), InstanceSymbol> _instances;
        private List<Diagnostic> _diagnostics;

        public ApplicationResourceVisitor()
        {
            _applications = new Dictionary<SyntaxBase, ApplicationSymbol>();
            _components = new Dictionary<(SyntaxBase, SyntaxBase), ComponentSymbol>();
            _deployments = new Dictionary<(SyntaxBase, SyntaxBase), DeploymentSymbol>();
            _instances = new Dictionary<(SyntaxBase, SyntaxBase), InstanceSymbol>();
            
            _diagnostics = new List<Diagnostic>();
        }

        public IReadOnlyDictionary<SyntaxBase, ApplicationSymbol> Applications => _applications;

        public IReadOnlyDictionary<(SyntaxBase, SyntaxBase), ComponentSymbol> Components => _components;

        public IReadOnlyDictionary<(SyntaxBase, SyntaxBase), DeploymentSymbol> Deployments => _deployments;

        public IReadOnlyDictionary<(SyntaxBase, SyntaxBase), InstanceSymbol> Instances => _instances;

        public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics;

        public override void VisitApplicationSymbol(ApplicationSymbol symbol)
        {
            if (TryGetObjectBody(symbol, symbol.Body) is ObjectSyntax body &&
                TryGetName(symbol, body) is SyntaxBase name)
            {
                _applications.Add(name, symbol);
            }

            VisitDescendants(symbol);
        }

        public override void VisitComponentSymbol(ComponentSymbol symbol)
        {
            if (symbol.Parent is ApplicationSymbol parent && 
                TryGetObjectBody(parent, parent.Body) is ObjectSyntax parentBody &&
                TryGetName(parent, parentBody) is SyntaxBase application &&
                TryGetObjectBody(symbol, symbol.Body) is ObjectSyntax body &&
                TryGetName(symbol, body) is SyntaxBase name)
            {
                _components.Add((application, name), symbol);
            }
            else if (TryGetObjectBody(symbol, symbol.Body) is ObjectSyntax body2 &&
                TryGetName(symbol, body2) is SyntaxBase name2 &&
                TryGetApplication(symbol, body2) is SyntaxBase application2)
            {
                _components.Add((application2, name2), symbol);
            }
            else
            {
                _diagnostics.Add(DiagnosticBuilder.ForPosition(symbol.DeclaringSyntax.Span).ComponentMissingApplicationProperty(symbol.Name));
            }

            VisitDescendants(symbol);
        }

        public override void VisitDeploymentSymbol(DeploymentSymbol symbol)
        {
            if (symbol.Parent is ApplicationSymbol parent && 
                TryGetObjectBody(parent, parent.Body) is ObjectSyntax parentBody &&
                TryGetName(parent, parentBody) is SyntaxBase application &&
                TryGetObjectBody(symbol, symbol.Body) is ObjectSyntax body &&
                TryGetName(symbol, body) is SyntaxBase name)
            {
                _deployments.Add((application, name), symbol);
            }
            else if (TryGetObjectBody(symbol, symbol.Body) is ObjectSyntax body2 &&
                TryGetName(symbol, body2) is SyntaxBase name2 &&
                TryGetApplication(symbol, body2) is SyntaxBase application2)
            {
                _deployments.Add((application2, name2), symbol);
            }
            else
            {
                _diagnostics.Add(DiagnosticBuilder.ForPosition(symbol.DeclaringSyntax.Span).DeploymentMissingApplicationProperty(symbol.Name));
            }

            VisitDescendants(symbol);
        }

        public override void VisitInstanceSymbol(InstanceSymbol symbol)
        {
            if (symbol.Parent is ApplicationSymbol parent && 
                TryGetObjectBody(parent, parent.Body) is ObjectSyntax parentBody &&
                TryGetName(parent, parentBody) is SyntaxBase application &&
                TryGetObjectBody(symbol, symbol.Body) is ObjectSyntax body &&
                TryGetName(symbol, body) is SyntaxBase name)
            {
                _instances.Add((application, name), symbol);
            }
            else if (TryGetObjectBody(symbol, symbol.Body) is ObjectSyntax body2 &&
                TryGetName(symbol, body2) is SyntaxBase name2 &&
                TryGetApplication(symbol, body2) is SyntaxBase application2)
            {
                _instances.Add((application2, name2), symbol);
            }
            else
            {
                _diagnostics.Add(DiagnosticBuilder.ForPosition(symbol.DeclaringSyntax.Span).InstanceMissingApplicationProperty(symbol.Name));
            }

            VisitDescendants(symbol);
        }

        private ObjectSyntax? TryGetObjectBody(DeclaredSymbol symbol, SyntaxBase syntax)
        {
            if (syntax is ObjectSyntax obj)
            {
                return obj;
            }

            return null;
        }

        private SyntaxBase? TryGetName(DeclaredSymbol symbol, ObjectSyntax obj)
        {
            if (!obj.ToNamedPropertyDictionary().TryGetValue("name", out var nameProperty))
            {
                return null;
            }

            return nameProperty.Value;
        }

        private SyntaxBase? TryGetApplication(DeclaredSymbol symbol, ObjectSyntax obj)
        {
            if (!obj.ToNamedPropertyDictionary().TryGetValue("application", out var applicationProperty))
            {
                return null;
            }

            return applicationProperty.Value;
        }


        private string? TryGetLiteralApplication(DeclaredSymbol symbol, ObjectSyntax obj)
        {
            if (!obj.ToNamedPropertyDictionary().TryGetValue("application", out var applicationProperty))
            {
                return default;
            }

            if (applicationProperty.Value is StringSyntax stringSyntax && stringSyntax.TryGetLiteralValue() is string value)
            {
                return value;
            }

            return default;
        }
    }
}