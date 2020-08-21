// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Bicep.Core.Syntax;

namespace Bicep.Core.Semantics
{
    public static class SymbolExtensions
    {
        public static SyntaxBase? SafeGetBodyPropertyValue(this ResourceSymbol resourceSymbol, string propertyName)
        {
            if (resourceSymbol.DeclaringResource.Body is not ObjectSyntax body)
            {
                return null;
            }

            return body.SafeGetPropertyByName(propertyName)?.Value;
        }

        public static SyntaxBase? SafeGetBodyPropertyValue(this ApplicationSymbol applicationSymbol, string propertyName)
        {
            if (applicationSymbol.DeclaringApplication.Body is not ObjectSyntax body)
            {
                return null;
            }

            return body.SafeGetPropertyByName(propertyName)?.Value;
        }

        public static SyntaxBase? SafeGetBodyPropertyValue(this ComponentSymbol componentSymbol, string propertyName)
        {
            if (componentSymbol.DeclaringComponent.Body is not ObjectSyntax body)
            {
                return null;
            }

            return body.SafeGetPropertyByName(propertyName)?.Value;
        }

        public static SyntaxBase? SafeGetBodyPropertyValue(this InstanceSymbol instanceSymbol, string propertyName)
        {
            if (instanceSymbol.DeclaringInstance.Body is not ObjectSyntax body)
            {
                return null;
            }

            return body.SafeGetPropertyByName(propertyName)?.Value;
        }

        public static SyntaxBase? SafeGetBodyPropertyValue(this DeploymentSymbol deploymentSymbol, string propertyName)
        {
            if (deploymentSymbol.DeclaringDeployment.Body is not ObjectSyntax body)
            {
                return null;
            }

            return body.SafeGetPropertyByName(propertyName)?.Value;
        }

        public static SyntaxBase? SafeGetBodyPropertyValue(this ModuleSymbol moduleSymbol, string propertyName)
        {
            if (moduleSymbol.DeclaringModule.Body is not ObjectSyntax body)
            {
                return null;
            }

            return body.SafeGetPropertyByName(propertyName)?.Value;
        }
    }
}