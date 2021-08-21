// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Bicep.Core.Extensions;
using Bicep.Core.Syntax;
using Bicep.Core.TypeSystem;

namespace Bicep.Core.Semantics
{
    public static class SymbolExtensions
    {
        public static ObjectPropertySyntax? SafeGetBodyProperty(this ResourceSymbol resourceSymbol, TypePropertyFlags flags)
        {
            var type = resourceSymbol.TryGetResourceType()?.Body?.Type as ObjectType;
            if (type is null)
            {
                return null;
            }

            var types = new HashSet<ObjectType>();
            var names = new List<string>();
            bool Visit(ObjectType type)
            {
                RuntimeHelpers.EnsureSufficientExecutionStack();

                if (!types.Add(type))
                {
                    return false;
                }

                foreach (var property in type.Properties)
                {
                    if ((property.Value.Flags & flags) == flags)
                    {
                        names.Add(property.Key);
                        return true;
                    }

                    // recurse into nested object
                    if (property.Value.TypeReference.Type is ObjectType next)
                    {
                        names.Add(property.Key); // Push
                        if (Visit(next))
                        {
                            return true;
                        }
                        names.RemoveAt(names.Count - 1); // Pop
                    }
                }

                types.Remove(type);
                return false;
            }

            if (Visit(type))
            {
                return resourceSymbol.DeclaringResource.TryGetBody()?.SafeGetPropertyByNameRecursive(names);
            }

            return null;
        }

        public static SyntaxBase? SafeGetBodyPropertyValue(this ResourceSymbol resourceSymbol, TypePropertyFlags flags)
            => SafeGetBodyProperty(resourceSymbol, flags)?.Value;

        public static ObjectPropertySyntax? SafeGetBodyProperty(this ResourceSymbol resourceSymbol, string propertyName)
            => resourceSymbol.DeclaringResource.TryGetBody()?.SafeGetPropertyByName(propertyName);

        public static SyntaxBase? SafeGetBodyPropertyValue(this ResourceSymbol resourceSymbol, string propertyName)
            => SafeGetBodyProperty(resourceSymbol, propertyName)?.Value;

        public static ObjectPropertySyntax UnsafeGetBodyProperty(this ResourceSymbol resourceSymbol, string propertyName)
            => resourceSymbol.SafeGetBodyProperty(propertyName) ?? throw new ArgumentException($"Expected resource syntax body to contain property '{propertyName}'");

        public static SyntaxBase UnsafeGetBodyPropertyValue(this ResourceSymbol resourceSymbol, string propertyName)
            => resourceSymbol.SafeGetBodyPropertyValue(propertyName) ?? throw new ArgumentException($"Expected resource syntax body to contain property '{propertyName}'");

        public static ObjectPropertySyntax? SafeGetBodyProperty(this ModuleSymbol moduleSymbol, string propertyName)
            => moduleSymbol.DeclaringModule.TryGetBody()?.SafeGetPropertyByName(propertyName);

        public static SyntaxBase? SafeGetBodyPropertyValue(this ModuleSymbol moduleSymbol, string propertyName)
            => SafeGetBodyProperty(moduleSymbol, propertyName)?.Value;

        public static bool IsSecure(this ParameterSymbol parameterSymbol)
        {
            // local function
            bool isSecure(DecoratorSyntax? value) => value?.Expression is FunctionCallSyntax functionCallSyntax && functionCallSyntax.NameEquals("secure");

            if (parameterSymbol?.DeclaringSyntax is ParameterDeclarationSyntax paramDeclaration)
            {
                return paramDeclaration.Decorators.Any(d => isSecure(d));
            }
            return false;
        }
    }
}
