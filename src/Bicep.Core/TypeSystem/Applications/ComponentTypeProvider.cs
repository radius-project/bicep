// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bicep.Core.TypeSystem.Applications
{
    public class ComponentTypeProvider : IComponentTypeProvider
    {
        private readonly ImmutableDictionary<ComponentTypeReference, NamedObjectType> knownTypes;

        private readonly NamedObjectType defaultRecipe;

        public ComponentTypeProvider()
        {
            var types = ImmutableDictionary.CreateBuilder<ComponentTypeReference, NamedObjectType>();
            types.Add(
                ComponentTypeReference.Parse("radius.dev/Container@v1alpha1"),
                KnownTypes.MakeContainer());
            types.Add(
                ComponentTypeReference.Parse("azure.com/Function@v1alpha1"),
                KnownTypes.MakeFunction());
            types.Add(
                ComponentTypeReference.Parse("azure.com/WebApp@v1alpha1"),
                KnownTypes.MakeWebApp());
            types.Add(
                ComponentTypeReference.Parse("dapr.io/Component@v1alpha1"),
                KnownTypes.MakeDaprComponent());
            types.Add(
                ComponentTypeReference.Parse("dapr.io/StateStore@v1alpha1"),
                KnownTypes.MakeDaprComponent());

            knownTypes = types.ToImmutableDictionary();

            defaultRecipe = KnownTypes.MakeGeneric();
        }

        public bool HasType(ComponentTypeReference reference)
        {
            return knownTypes.ContainsKey(reference);
        }

        public ComponentType GetComponentType(ComponentTypeReference reference)
        {
            if (knownTypes.TryGetValue(reference, out var recipe))
            {
                return TypeFactory.CreateComponentType(reference, recipe);
            }

            return TypeFactory.CreateComponentType(reference, defaultRecipe);
        }

        public InstanceType GetInstanceType(ComponentTypeReference reference)
        {
            if (knownTypes.TryGetValue(reference, out var recipe))
            {
                return TypeFactory.CreateInstanceType(reference, recipe);
            }

            return TypeFactory.CreateInstanceType(reference, defaultRecipe);
        }
    }
}