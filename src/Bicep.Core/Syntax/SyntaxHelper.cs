// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Linq;
using Bicep.Core.Diagnostics;
using Bicep.Core.Extensions;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Syntax
{
    public static class SyntaxHelper
    {
        private static SyntaxBase? TryGetObjectProperty(ObjectSyntax objectSyntax, string propertyName)
            => objectSyntax.Properties.SingleOrDefault(p => p.TryGetKeyText() == propertyName)?.Value;

        public static ArraySyntax? TryGetAllowedSyntax(ParameterDeclarationSyntax parameterDeclarationSyntax)
        {
            if (!(parameterDeclarationSyntax.Modifier is ObjectSyntax modifierObject))
            {
                return null;
            }

            var allowedValuesSyntax = TryGetObjectProperty(modifierObject, LanguageConstants.ParameterAllowedPropertyName);
            if (!(allowedValuesSyntax is ArraySyntax allowedArraySyntax))
            {
                return null;
            }

            return allowedArraySyntax;
        }

        public static string? TryGetModulePath(ModuleDeclarationSyntax moduleDeclarationSyntax, out DiagnosticBuilder.ErrorBuilderDelegate? failureBuilder)
        {
            var pathSyntax = moduleDeclarationSyntax.TryGetPath();
            if (pathSyntax == null)
            {
                failureBuilder = x => x.ModulePathHasNotBeenSpecified();
                return null;
            }

            var pathValue = pathSyntax.TryGetLiteralValue();
            if (pathValue == null)
            {
                failureBuilder = x => x.ModulePathInterpolationUnsupported();
                return null;                
            }
            
            failureBuilder = null;
            return pathValue;
        }

        public static TypeSymbol? TryGetPrimitiveType(ParameterDeclarationSyntax parameterDeclarationSyntax)
            => LanguageConstants.TryGetDeclarationType(parameterDeclarationSyntax.ParameterType?.TypeName);

        public static ResourceScopeType GetTargetScope(TargetScopeSyntax targetScopeSyntax)
        {
            // TODO: Revisit when adding support for multiple target scopes

            // Type checking will pick up any errors if we fail to process the syntax correctly in this function.
            // There's no need to do error checking here - just return "None" as the scope type.

            if (!(targetScopeSyntax.Value is StringSyntax stringSyntax))
            {
                return ResourceScopeType.None;
            }

            var literalValue = stringSyntax.TryGetLiteralValue();
            if (literalValue == null)
            {
                return ResourceScopeType.None;
            }

            return literalValue switch {
                LanguageConstants.TargetScopeTypeTenant => ResourceScopeType.TenantScope,
                LanguageConstants.TargetScopeTypeManagementGroup => ResourceScopeType.ManagementGroupScope,
                LanguageConstants.TargetScopeTypeSubscription => ResourceScopeType.SubscriptionScope,
                LanguageConstants.TargetScopeTypeResourceGroup => ResourceScopeType.ResourceGroupScope,
                _ => ResourceScopeType.None,
            };
        }

        public static ResourceScopeType GetTargetScope(SyntaxTree syntaxTree)
        {
            var defaultTargetScope = ResourceScopeType.ResourceGroupScope;
            var targetSyntax = syntaxTree.ProgramSyntax.Children.OfType<TargetScopeSyntax>().FirstOrDefault();
            if (targetSyntax == null)
            {
                return defaultTargetScope;
            }

            var targetScope = SyntaxHelper.GetTargetScope(targetSyntax);
            if (targetScope == ResourceScopeType.None)
            {
                return defaultTargetScope;
            }

            return targetScope;
        }

        public static SyntaxBase? TryGetDefaultValue(ParameterDeclarationSyntax parameterDeclarationSyntax)
        {
            if (parameterDeclarationSyntax.Modifier is ParameterDefaultValueSyntax defaultValueSyntax)
            {
                return defaultValueSyntax.DefaultValue;
            }

            if (parameterDeclarationSyntax.Modifier is ObjectSyntax modifierObject)
            {
                return TryGetObjectProperty(modifierObject, LanguageConstants.ParameterDefaultPropertyName);
            }

            return null;
        }

        private static TypeSymbol GetDeclaredTypeFromAllowed(TypeSymbol declaredType, ArraySyntax allowedSyntax)
        {
            if (!object.ReferenceEquals(declaredType, LanguageConstants.LooseString))
            {
                return declaredType;
            }

            var allowedTypes = new List<StringLiteralType>();
            
            foreach (var syntax in allowedSyntax.Items)
            {
                var literalValue = (syntax.Value as StringSyntax)?.TryGetLiteralValue();

                if (literalValue is null)
                {
                    return declaredType;
                }

                allowedTypes.Add(new StringLiteralType(literalValue));
            }

            return UnionType.Create(allowedTypes);
        }

        public static TypeSymbol GetDeclaredType(ParameterDeclarationSyntax syntax)
        {
            // assume "any" type when the parameter has parse errors (either missing or was skipped)
            var declaredType = syntax.ParameterType == null
                ? LanguageConstants.Any
                : LanguageConstants.TryGetDeclarationType(syntax.ParameterType.TypeName);

            if (declaredType is null)
            {
                return ErrorType.Create(DiagnosticBuilder.ForPosition(syntax.Type).InvalidParameterType());
            }

            return declaredType;
        }

        public static TypeSymbol GetAssignedType(ParameterDeclarationSyntax syntax)
        {
            var declaredType = GetDeclaredType(syntax);

            if (object.ReferenceEquals(declaredType, LanguageConstants.String))
            {
                // In order to support assignment for a generic string to enum-typed properties (which generally is forbidden),
                // we need to relax the validation for string parameters without 'allowed' values specified.
                declaredType = LanguageConstants.LooseString;
            }

            var allowedSyntax = TryGetAllowedSyntax(syntax);
            if (allowedSyntax is null)
            {
                return declaredType;
            }

            if (!allowedSyntax.Items.Any())
            {
                return ErrorType.Create(DiagnosticBuilder.ForPosition(allowedSyntax).AllowedMustContainItems());
            }

            return GetDeclaredTypeFromAllowed(declaredType, allowedSyntax);
        }
    }
}