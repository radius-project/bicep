// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;

namespace Bicep.Core.Emit
{
    public class ResourceDependencyVisitor : SyntaxVisitor
    {
        private readonly SemanticModel model;
        private IDictionary<DeclaredSymbol, HashSet<DeclaredSymbol>> resourceDependencies;
        private DeclaredSymbol? currentDeclaration;

        public static ImmutableDictionary<DeclaredSymbol, ImmutableHashSet<DeclaredSymbol>> GetResourceDependencies(SemanticModel model)
        {
            var visitor = new ResourceDependencyVisitor(model);
            visitor.Visit(model.Root.Syntax);

            var output = new Dictionary<DeclaredSymbol, ImmutableHashSet<DeclaredSymbol>>();
            foreach (var kvp in visitor.resourceDependencies)
            {
                if (kvp.Key is ResourceSymbol resourceSymbol)
                {
                    output[resourceSymbol] = kvp.Value.ToImmutableHashSet();
                }
                if (kvp.Key is ApplicationSymbol applicationSymbol)
                {
                    output[applicationSymbol] = kvp.Value.ToImmutableHashSet();
                }
                if (kvp.Key is ComponentSymbol componentSymbol)
                {
                    output[componentSymbol] = kvp.Value.ToImmutableHashSet();
                }
                if (kvp.Key is DeploymentSymbol deploymentSymbol)
                {
                    output[deploymentSymbol] = kvp.Value.ToImmutableHashSet();
                }
                if (kvp.Key is InstanceSymbol instanceSymbol)
                {
                    output[instanceSymbol] = kvp.Value.ToImmutableHashSet();
                }
                if (kvp.Key is ModuleSymbol moduleSymbol)
                {
                    output[moduleSymbol] = kvp.Value.ToImmutableHashSet();
                }
            }
            return output.ToImmutableDictionary();
        }

        private ResourceDependencyVisitor(SemanticModel model)
        {
            this.model = model;
            this.resourceDependencies = new Dictionary<DeclaredSymbol, HashSet<DeclaredSymbol>>();
            this.currentDeclaration = null;
        }

        public override void VisitResourceDeclarationSyntax(ResourceDeclarationSyntax syntax)
        {
            if (!(this.model.GetSymbolInfo(syntax) is ResourceSymbol resourceSymbol))
            {
                throw new InvalidOperationException("Unbound declaration");
            }

            // save previous declaration as we may call this recursively
            var prevDeclaration = this.currentDeclaration;

            this.currentDeclaration = resourceSymbol;
            this.resourceDependencies[resourceSymbol] = new HashSet<DeclaredSymbol>();
            base.VisitResourceDeclarationSyntax(syntax);

            // restore previous declaration
            this.currentDeclaration = prevDeclaration;
        }

        public override void VisitApplicationDeclarationSyntax(ApplicationDeclarationSyntax syntax)
        {
            if (!(this.model.GetSymbolInfo(syntax) is ApplicationSymbol symbol))
            {
                throw new InvalidOperationException("Unbound declaration");
            }

            // save previous declaration as we may call this recursively
            var prevDeclaration = this.currentDeclaration;

            this.currentDeclaration = symbol;
            this.resourceDependencies[symbol] = new HashSet<DeclaredSymbol>();
            base.VisitApplicationDeclarationSyntax(syntax);

            // restore previous declaration
            this.currentDeclaration = prevDeclaration;
        }

        public override void VisitComponentDeclarationSyntax(ComponentDeclarationSyntax syntax)
        {
            if (!(this.model.GetSymbolInfo(syntax) is ComponentSymbol symbol))
            {
                throw new InvalidOperationException("Unbound declaration");
            }

            // save previous declaration as we may call this recursively
            var prevDeclaration = this.currentDeclaration;

            this.currentDeclaration = symbol;
            this.resourceDependencies[symbol] = new HashSet<DeclaredSymbol>();
            base.VisitComponentDeclarationSyntax(syntax);

            // restore previous declaration
            this.currentDeclaration = prevDeclaration;
        }

        public override void VisitDeploymentDeclarationSyntax(DeploymentDeclarationSyntax syntax)
        {
            if (!(this.model.GetSymbolInfo(syntax) is DeploymentSymbol symbol))
            {
                throw new InvalidOperationException("Unbound declaration");
            }

            // save previous declaration as we may call this recursively
            var prevDeclaration = this.currentDeclaration;

            this.currentDeclaration = symbol;
            this.resourceDependencies[symbol] = new HashSet<DeclaredSymbol>();
            base.VisitDeploymentDeclarationSyntax(syntax);

            // restore previous declaration
            this.currentDeclaration = prevDeclaration;
        }

        public override void VisitInstanceDeclarationSyntax(InstanceDeclarationSyntax syntax)
        {
            if (!(this.model.GetSymbolInfo(syntax) is InstanceSymbol symbol))
            {
                throw new InvalidOperationException("Unbound declaration");
            }

            // save previous declaration as we may call this recursively
            var prevDeclaration = this.currentDeclaration;

            this.currentDeclaration = symbol;
            this.resourceDependencies[symbol] = new HashSet<DeclaredSymbol>();
            base.VisitInstanceDeclarationSyntax(syntax);

            // restore previous declaration
            this.currentDeclaration = prevDeclaration;
        }

        public override void VisitModuleDeclarationSyntax(ModuleDeclarationSyntax syntax)
        {
            if (!(this.model.GetSymbolInfo(syntax) is ModuleSymbol moduleSymbol))
            {
                throw new InvalidOperationException("Unbound declaration");
            }

            // save previous declaration as we may call this recursively
            var prevDeclaration = this.currentDeclaration;

            this.currentDeclaration = moduleSymbol;
            this.resourceDependencies[moduleSymbol] = new HashSet<DeclaredSymbol>();
            base.VisitModuleDeclarationSyntax(syntax);

            // restore previous declaration
            this.currentDeclaration = prevDeclaration;
        }

        public override void VisitVariableDeclarationSyntax(VariableDeclarationSyntax syntax)
        {
            if (!(this.model.GetSymbolInfo(syntax) is VariableSymbol variableSymbol))
            {
                throw new InvalidOperationException("Unbound declaration");
            }

            // save previous declaration as we may call this recursively
            var prevDeclaration = this.currentDeclaration;

            this.currentDeclaration = variableSymbol;
            this.resourceDependencies[variableSymbol] = new HashSet<DeclaredSymbol>();
            base.VisitVariableDeclarationSyntax(syntax);

            // restore previous declaration
            this.currentDeclaration = prevDeclaration;
        }

        public override void VisitVariableAccessSyntax(VariableAccessSyntax syntax)
        {
            VisitVariableAccessSyntaxInternal(syntax);
            base.VisitVariableAccessSyntax(syntax);
        }

        private void VisitVariableAccessSyntaxInternal(VariableAccessSyntax syntax)
        {
            if (currentDeclaration == null)
            {
                return;
            }

            switch (model.GetSymbolInfo(syntax))
            {
                case VariableSymbol variableSymbol:
                    if (!resourceDependencies.TryGetValue(variableSymbol, out var dependencies))
                    {
                        // recursively visit dependent variables
                        this.Visit(variableSymbol.DeclaringSyntax);

                        dependencies = resourceDependencies[variableSymbol];
                    }

                    foreach (var dependency in dependencies)
                    {
                        resourceDependencies[currentDeclaration].Add(dependency);
                    }
                    return;

                case ResourceSymbol resourceSymbol:
                    resourceDependencies[currentDeclaration].Add(resourceSymbol);
                    return;
                case ApplicationSymbol applicationSymbol:
                    resourceDependencies[currentDeclaration].Add(applicationSymbol);
                    return;
                case ComponentSymbol componentSymbol:
                    resourceDependencies[currentDeclaration].Add(componentSymbol);
                    return;
                case DeploymentSymbol deploymentSymbol:
                    resourceDependencies[currentDeclaration].Add(deploymentSymbol);
                    return;
                case InstanceSymbol instanceSymbol:
                    resourceDependencies[currentDeclaration].Add(instanceSymbol);
                    return;
                case ModuleSymbol moduleSymbol:
                    resourceDependencies[currentDeclaration].Add(moduleSymbol);
                    return;
            }
        }
    }
}