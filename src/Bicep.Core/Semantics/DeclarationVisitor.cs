// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    public class DeclarationVisitor: SyntaxVisitor
    {
        private readonly ISymbolContext context;

        private readonly IList<DeclaredSymbol> declaredSymbols;
        private ApplicationSymbol? applicationSymbol;

        public DeclarationVisitor(ISymbolContext context, IList<DeclaredSymbol> declaredSymbols)
        {
            this.context = context;
            this.declaredSymbols = declaredSymbols;
        }

        public override void VisitParameterDeclarationSyntax(ParameterDeclarationSyntax syntax)
        {
            base.VisitParameterDeclarationSyntax(syntax);

            var symbol = new ParameterSymbol(this.context, syntax.Name.IdentifierName, syntax, syntax.Modifier);
            this.declaredSymbols.Add(symbol);
        }

        public override void VisitVariableDeclarationSyntax(VariableDeclarationSyntax syntax)
        {
            base.VisitVariableDeclarationSyntax(syntax);

            var symbol = new VariableSymbol(this.context, syntax.Name.IdentifierName, syntax, syntax.Value);
            this.declaredSymbols.Add(symbol);
        }

        public override void VisitResourceDeclarationSyntax(ResourceDeclarationSyntax syntax)
        {
            base.VisitResourceDeclarationSyntax(syntax);

            var symbol = new ResourceSymbol(this.context, syntax.Name.IdentifierName, syntax);
            this.declaredSymbols.Add(symbol);
        }

        public override void VisitApplicationDeclarationSyntax(ApplicationDeclarationSyntax syntax)
        {
            var symbol = new ApplicationSymbol(this.context, syntax.Name.IdentifierName, syntax);
            this.applicationSymbol = symbol;
            base.VisitApplicationDeclarationSyntax(syntax);
            this.applicationSymbol = null;

            this.declaredSymbols.Add(symbol);
        }

        public override void VisitComponentDeclarationSyntax(ComponentDeclarationSyntax syntax)
        {
            base.VisitComponentDeclarationSyntax(syntax);

            var symbol = new ComponentSymbol(this.context, syntax.Name.IdentifierName, syntax, this.applicationSymbol);
            this.declaredSymbols.Add(symbol);
        }

        public override void VisitDeploymentDeclarationSyntax(DeploymentDeclarationSyntax syntax)
        {
            base.VisitDeploymentDeclarationSyntax(syntax);

            var symbol = new DeploymentSymbol(this.context, syntax.Name.IdentifierName, syntax, this.applicationSymbol);
            this.declaredSymbols.Add(symbol);
        }

        public override void VisitInstanceDeclarationSyntax(InstanceDeclarationSyntax syntax)
        {
            base.VisitInstanceDeclarationSyntax(syntax);

            var symbol = new InstanceSymbol(this.context, syntax.Name.IdentifierName, syntax, this.applicationSymbol);
            this.declaredSymbols.Add(symbol);
        }

        public override void VisitModuleDeclarationSyntax(ModuleDeclarationSyntax syntax)
        {
            base.VisitModuleDeclarationSyntax(syntax);

            var symbol = new ModuleSymbol(this.context, syntax.Name.IdentifierName, syntax);
            this.declaredSymbols.Add(symbol);
        }

        public override void VisitOutputDeclarationSyntax(OutputDeclarationSyntax syntax)
        {
            base.VisitOutputDeclarationSyntax(syntax);

            var symbol = new OutputSymbol(this.context, syntax.Name.IdentifierName, syntax, syntax.Value);
            this.declaredSymbols.Add(symbol);
        }
    }
}

