// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.TypeSystem;
using Bicep.Core.Resources;
using Bicep.Core.Semantics;

namespace Bicep.Core.TypeSystem.Radius.V3
{
    public static class KnownComponents
    {
        public class ComponentData
        {
            public ThreePartType Type { get; set; } = default!;

            public CommonBindings.BindingData? Binding { get; set; }

            public List<TypeProperty> Properties { get; } = new List<TypeProperty>();
        }

        public static ComponentData MakeService()
        {
            var rolesType = new TypedArrayType(
                itemReference: LanguageConstants.String,
                validationFlags: TypeSymbolValidationFlags.Default);

            var connectionType = new DiscriminatedObjectType(
                name: "connection",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: CommonBindings.AllBindingData.Select(b =>
                {
                    return new ObjectType(
                        name: $"connection {b.Type.FormatKind()}",
                        validationFlags: TypeSymbolValidationFlags.Default,
                        properties: new[]
                        {
                            new TypeProperty("kind", new StringLiteralType(b.Type.FormatKind()), TypePropertyFlags.Required, "The kind of connection"),
                            new TypeProperty("source", LanguageConstants.String, TypePropertyFlags.Required, "The source of the connection"),
                            new TypeProperty("roles", rolesType, TypePropertyFlags.None, "RBAC configuration to be applied on the connection"),
                        },
                        additionalPropertiesType: null,
                        additionalPropertiesFlags: TypePropertyFlags.None,
                        functions: null);
                }));

            var connectionsType = new ObjectType(
                name: "connections",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: connectionType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var connectionsProperty = new TypeProperty(
                "connections",
                connectionsType,
                TypePropertyFlags.None,
                @"Specify named connections to databases, routes, and other resources for the component.

The connections can be used to grant access to resources as well as to inject configuration value into the container.
Injected configuration values use environment variables by default and follow the naming pattern: `CONNECTION_<name>_<configuration-value>`.

For example the following connection:

```bicep
connections: {
  db: {
    kind: 'mongodb.com/MongoDB'
    source: db.id
  }
}
```

Defines the environment variable `CONNECTION_DB_CONNECTIONSTRING` containing the database's connection string. The set of configuration values
available varies depending on the kind of connection. See the documentation for examples.
");

            var nameProperty = new TypeProperty("name", LanguageConstants.LooseString, TypePropertyFlags.Required);
            var workingDirectoryProperty = new TypeProperty("workingDirectory", LanguageConstants.LooseString, TypePropertyFlags.None);

            var argsType = new TypedArrayType(
                itemReference: LanguageConstants.LooseString,
                validationFlags: TypeSymbolValidationFlags.Default);
            var argsProperty = new TypeProperty("args", argsType, TypePropertyFlags.None);

            var executableType = new ObjectType(
                name: "executable",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new []
                {
                    new TypeProperty("kind", new StringLiteralType("executable"), TypePropertyFlags.Required),
                    nameProperty,
                    workingDirectoryProperty,
                    argsProperty,
                },
                additionalPropertiesType: LanguageConstants.LooseString,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var imageProperty = new TypeProperty(
                "image",
                LanguageConstants.String,
                TypePropertyFlags.Required,
                description: "Specifies the container image to run.");
            var containerType = new ObjectType(
                "container",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty("kind", new StringLiteralType("container"), TypePropertyFlags.Required),
                    imageProperty,
                },
                additionalPropertiesType: LanguageConstants.Any,
                additionalPropertiesFlags: TypePropertyFlags.None);

            var runnableType = new DiscriminatedObjectType(
                "run config",
                TypeSymbolValidationFlags.Default,
                "kind",
                new ITypeReference[]
                {
                    containerType,
                    executableType,
                });
            var runProperty = new TypeProperty(
                "run",
                runnableType,
                TypePropertyFlags.None,
                description: "Specifies configuration for running the service.");

            var envType = new ObjectType(
                name: "env",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: LanguageConstants.LooseString,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var envProperty = new TypeProperty("env", envType, TypePropertyFlags.None);

            var portType = new ObjectType(
                name: "port",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("port", LanguageConstants.Int, TypePropertyFlags.None),
                    new TypeProperty("dynamic", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("env", LanguageConstants.String, TypePropertyFlags.None),
                    new TypeProperty(
                        "protocol",
                        new UnionType("protocol", ImmutableArray.Create<ITypeReference>(new StringLiteralType("TCP"), new StringLiteralType("UDP"))),
                        TypePropertyFlags.None),
                    new TypeProperty("provides", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var portsType = new ObjectType(
                name: "ports",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: portType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var portsProperty = new TypeProperty(
                "ports",
                portsType,
                TypePropertyFlags.None,
                description: @"Specify listening ports for the container. Ports may be used to connect the container to route types like `HttpRoute` for service discovery, or may just serve as documentation.

Ports use the format: `name: { ... }'`. The name is provided for documentation purposes

For example, the following code defines a listening port named `web` which is used to connect an `HttpRoute` to the container:

```bicep
web: {
  containerPort: 3000
  provides: myRoute.id
}

In this example the `web` port documents that the container is listening on port `3000`. The variable `myRoute` refers to an `HttpRoute` resource (definition not shownn here).
```
");

            var replicasProperty = new TypeProperty("replicas", LanguageConstants.LooseString, TypePropertyFlags.None);

            return new ComponentData()
            {
                Type = new ThreePartType(null, "Service", ""),
                Properties =
                {
                    connectionsProperty,
                    runProperty,
                    envProperty,
                    portsProperty,
                    replicasProperty
                },
            };
        }

        public static ComponentData MakeExecutable()
        {
            var members = new List<ObjectType>();

            var executableProperty = new TypeProperty("executable", LanguageConstants.LooseString, TypePropertyFlags.Required);

            var workingDirectoryProperty = new TypeProperty("workingDirectory", LanguageConstants.LooseString, TypePropertyFlags.None);

            var argsType = new TypedArrayType(
                itemReference: LanguageConstants.LooseString,
                validationFlags: TypeSymbolValidationFlags.Default);
            var argsProperty = new TypeProperty("args", argsType, TypePropertyFlags.None);

            var envType = new ObjectType(
                name: "env",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: LanguageConstants.LooseString,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var envProperty = new TypeProperty("env", envType, TypePropertyFlags.None);

            var replicasProperty = new TypeProperty("replicas", LanguageConstants.LooseString, TypePropertyFlags.None);

            return new ComponentData()
            {
                Type = new ThreePartType(null, "Executable", ""),
                Properties =
                {
                    executableProperty,
                    workingDirectoryProperty,
                    argsProperty,
                    envProperty,
                    replicasProperty
                },
            };
        }

        public static ComponentData MakeContainer()
        {
            var members = new List<ObjectType>();

            var rolesType = new TypedArrayType(
                itemReference: LanguageConstants.String,
                validationFlags: TypeSymbolValidationFlags.Default);

            var connectionType = new DiscriminatedObjectType(
                name: "connection",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: CommonBindings.AllBindingData.Select(b =>
                {
                    return new ObjectType(
                        name: $"connection {b.Type.FormatKind()}",
                        validationFlags: TypeSymbolValidationFlags.Default,
                        properties: new[]
                        {
                            new TypeProperty("kind", new StringLiteralType(b.Type.FormatKind()), TypePropertyFlags.Required, "The kind of connection"),
                            new TypeProperty("source", LanguageConstants.String, TypePropertyFlags.Required, "The source of the connection"),
                            new TypeProperty("roles", rolesType, TypePropertyFlags.None, "RBAC configuration to be applied on the connection"),
                        },
                        additionalPropertiesType: null,
                        additionalPropertiesFlags: TypePropertyFlags.None,
                        functions: null);
                }));

            var connectionsType = new ObjectType(
                name: "connections",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: connectionType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var connectionsProperty = new TypeProperty(
                "connections",
                connectionsType,
                TypePropertyFlags.None,
                @"Specify named connections to databases, routes, and other resources for the component.

The connections can be used to grant access to resources as well as to inject configuration value into the container.
Injected configuration values use environment variables by default and follow the naming pattern: `CONNECTION_<name>_<configuration-value>`.

For example the following connection:

```bicep
connections: {
  db: {
    kind: 'mongodb.com/MongoDB'
    source: db.id
  }
}
```

Defines the environment variable `CONNECTION_DB_CONNECTIONSTRING` containing the database's connection string. The set of configuration values
available varies depending on the kind of connection. See the documentation for examples.
");

            var envItemType = LanguageConstants.LooseString;

            var envType = new ObjectType(
                name: "env",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: envItemType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var envProperty = new TypeProperty(
                "env",
                envType,
                TypePropertyFlags.None,
                description: @"Specify environment variables for the container. Environment variables may contain static values as well as references to parameters, variables and other resources.

Environment variables use the format `NAME: 'VALUE'`

For example, the following code defines the environment variables `A_STATIC_VALUE`, `A_STRING_INTERPOLATED_VALUE`, and `A_SECRET_VALUE`:

```bicep
env: {
  A_STATIC_VALUE: 'This is a hardcoded value'
  A_STRING_INTERPOLATED_VALUE: 'This value text and a ${other.value} value'
  A_SECRET_VALUE: db.connectionString()
}
```
");

            var portType = new ObjectType(
                name: "port",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("containerPort", LanguageConstants.Int, TypePropertyFlags.Required),
                    new TypeProperty(
                        "protocol",
                        new UnionType("protocol", ImmutableArray.Create<ITypeReference>(new StringLiteralType("TCP"), new StringLiteralType("UDP"))),
                        TypePropertyFlags.None),
                    new TypeProperty("provides", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var portsType = new ObjectType(
                name: "ports",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: portType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var portsProperty = new TypeProperty(
                "ports",
                portsType,
                TypePropertyFlags.None,
                description: @"Specify listening ports for the container. Ports may be used to connect the container to route types like `HttpRoute` for service discovery, or may just serve as documentation.

Ports use the format: `name: { ... }'`. The name is provided for documentation purposes

For example, the following code defines a listening port named `web` which is used to connect an `HttpRoute` to the container:

```bicep
web: {
  containerPort: 3000
  provides: myRoute.id
}

In this example the `web` port documents that the container is listening on port `3000`. The variable `myRoute` refers to an `HttpRoute` resource (definition not shownn here).
```
");

            var ephemeralVolume = new ObjectType(
                name: "ephemeral",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("kind", new StringLiteralType("ephemeral"), TypePropertyFlags.Required, "Volume Kind"),
                    new TypeProperty("mountPath", LanguageConstants.String, TypePropertyFlags.Required, "The path where the volume is mounted"),
                    new TypeProperty(
                        "managedStore",
                        new UnionType("managed store", ImmutableArray.Create<ITypeReference>(new StringLiteralType("memory"), new StringLiteralType("disk"))),
                        TypePropertyFlags.Required,
                        "Backing store for the ephemeral volume"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var persistentVolume = new ObjectType(
                name: "persistent",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("kind", new StringLiteralType("persistent"), TypePropertyFlags.Required, "Volume kind"),
                    new TypeProperty("mountPath", LanguageConstants.String, TypePropertyFlags.Required, "The path where the volume is mounted"),
                    new TypeProperty("source", LanguageConstants.String, TypePropertyFlags.Required, "The source of the volume"),
                    new TypeProperty(
                        "rbac",
                        new UnionType("rbac", ImmutableArray.Create<ITypeReference>(new StringLiteralType("read"), new StringLiteralType("write"))),
                        TypePropertyFlags.None, "Container read/write access to the volume"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var volumeItemType = new DiscriminatedObjectType(
                name: "volume",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: new ITypeReference[] { ephemeralVolume, persistentVolume });

            var volumesType = new ObjectType(
                name: "volumes",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: volumeItemType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);
            var volumesProperty = new TypeProperty("volumes", volumesType, TypePropertyFlags.None);

            var headersType = new ObjectType(
                name: "headers",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: LanguageConstants.String,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var httpGet = new ObjectType(
                name: "httpGet",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("containerPort", LanguageConstants.Int, TypePropertyFlags.Required, "The listening port number"),
                    new TypeProperty("path", LanguageConstants.String, TypePropertyFlags.Required, "The route to make the HTTP request on"),
                    new TypeProperty("headers", headersType, TypePropertyFlags.None, "Custom HTTP headers to add to the get request"),
                    new TypeProperty("kind", new StringLiteralType("httpGet"), TypePropertyFlags.Required, "Health probe kind"),
                    new TypeProperty("initialDelaySeconds", LanguageConstants.Int, TypePropertyFlags.None, "Initial delay in seconds before probing for readiness/liveness"),
                    new TypeProperty("failureThreshold", LanguageConstants.Int, TypePropertyFlags.None, "Threshold number of times the probe fails after which a failure would be reported"),
                    new TypeProperty("periodSeconds", LanguageConstants.Int, TypePropertyFlags.None, "Interval for the readiness/liveness probe in seconds"),
                },
                additionalPropertiesType: null,
                functions: null);

            var tcp = new ObjectType(
                name: "tcp",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("containerPort", LanguageConstants.Int, TypePropertyFlags.Required, "The listening port number"),
                    new TypeProperty("kind", new StringLiteralType("tcp"), TypePropertyFlags.Required, "Health probe kind"),
                    new TypeProperty("initialDelaySeconds", LanguageConstants.Int, TypePropertyFlags.None, "Initial delay in seconds before probing for readiness/liveness"),
                    new TypeProperty("failureThreshold", LanguageConstants.Int, TypePropertyFlags.None, "Threshold number of times the probe fails after which a failure would be reported"),
                    new TypeProperty("periodSeconds", LanguageConstants.Int, TypePropertyFlags.None, "Interval for the readiness/liveness probe in seconds"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var exec = new ObjectType(
                name: "exec",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("command", LanguageConstants.String, TypePropertyFlags.Required, "Command to execute to probe readiness/liveness"),
                    new TypeProperty("kind", new StringLiteralType("exec"), TypePropertyFlags.Required, "Health probe kind"),
                    new TypeProperty("initialDelaySeconds", LanguageConstants.Int, TypePropertyFlags.None, "Initial delay in seconds before probing for readiness/liveness"),
                    new TypeProperty("failureThreshold", LanguageConstants.Int, TypePropertyFlags.None, "Threshold number of times the probe fails after which a failure would be reported"),
                    new TypeProperty("periodSeconds", LanguageConstants.Int, TypePropertyFlags.None, "Interval for the readiness/liveness probe in seconds"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var healthProbeType = new DiscriminatedObjectType(
                name: "healthProbe",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: new ITypeReference[] { httpGet, tcp, exec }
                );

            var readinessProperty = new TypeProperty("readinessProbe", healthProbeType, TypePropertyFlags.None, "Readiness health probe");
            var livessProperty = new TypeProperty("livenessProbe", healthProbeType, TypePropertyFlags.None, "Liveness health probe");

            var imageProperty = new TypeProperty(
                "image",
                LanguageConstants.String,
                TypePropertyFlags.Required,
                description: "Specifies the container image to run.");
            var containerType = new ObjectType(
                "container",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    imageProperty,
                    envProperty,
                    portsProperty,
                    volumesProperty,
                    readinessProperty,
                    livessProperty,
                },
                additionalPropertiesType: LanguageConstants.Any,
                additionalPropertiesFlags: TypePropertyFlags.None);
            var containerProperty = new TypeProperty(
                "container",
                containerType,
                TypePropertyFlags.Required,
                description: "Specifies configuration for running the container.");

            return new ComponentData()
            {
                Type = new ThreePartType(null, "Container", ""),
                Properties =
                {
                    connectionsProperty,
                    containerProperty,
                    CommonProperties.Traits,
                },
            };
        }

        public static ResourceTypeComponents MakeDaprStateStore()
        {
            var azureTableStorageStateStoreType = new ObjectType(
                name: "state.azure.tablestorage",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("stateStoreName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "State store name"),
                    new TypeProperty("kind", new StringLiteralType("state.azure.tablestorage"), TypePropertyFlags.Required, "The Dapr State Store kind. These strings match the format used by Dapr Kubernetes configuration format"),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None, "Indicates if the resource is Radius-managed. If false, a resource is required"),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var sqlServerStateStoreType = new ObjectType(
                name: "state.sqlserver",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("stateStoreName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "State store name"),
                    new TypeProperty("kind", new StringLiteralType("state.sqlserver"), TypePropertyFlags.Required, "The Dapr State Store kind. These strings match the format used by Dapr Kubernetes configuration format"),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None, "Indicates if the resource is Radius-managed. If false, a resource is required"),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var redisStateStoreType = new ObjectType(
                name: "state.redis",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("stateStoreName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "State store name"),
                    new TypeProperty("kind", new StringLiteralType("state.redis"), TypePropertyFlags.Required, "The Dapr State Store kind. These strings match the format used by Dapr Kubernetes configuration format"),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None, "Indicates if the resource is Radius-managed. If false, a resource is required"),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var anyStateStoreType = new ObjectType(
                name: "any",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("stateStoreName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "State store name"),
                    new TypeProperty("kind", new StringLiteralType("any"), TypePropertyFlags.Required, "The Dapr State Store kind. These strings match the format used by Dapr Kubernetes configuration format"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var genericPubSubType = new ObjectType(
                name: "generic",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("stateStoreName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "State store name"),
                    new TypeProperty("kind", new StringLiteralType("generic"), TypePropertyFlags.Required, "The Dapr State Store kind"),
                    new TypeProperty("type", LanguageConstants.String, TypePropertyFlags.Required, "The Dapr State Store type. These strings match the format used by Dapr Kubernetes configuration format"),
                    new TypeProperty("version", LanguageConstants.String, TypePropertyFlags.Required, "Dapr component version"),
                    new TypeProperty("metadata", LanguageConstants.Object, TypePropertyFlags.WriteOnly | TypePropertyFlags.Required, "Metadata for the State Store resource. This should match the Dapr component spec"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var daprStateStoreKindType = new DiscriminatedObjectType(
                name: "dapr state store kind",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: new ITypeReference[] { azureTableStorageStateStoreType, sqlServerStateStoreType, redisStateStoreType, genericPubSubType, anyStateStoreType });

            var propertiesType = new DiscriminatedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: new ITypeReference[] { azureTableStorageStateStoreType, sqlServerStateStoreType, redisStateStoreType, genericPubSubType, anyStateStoreType });
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var typeName = $"{RadiusResources.ApplicationResourceType}/dapr.io.StateStore@{RadiusResources.ResourceApiVersion}";
            var bodyType = new ObjectType(
                name: typeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    // Top level properties are predefined
                    CommonProperties.Id,
                    CommonProperties.Name,
                    new TypeProperty("type", new StringLiteralType(typeName), TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.ReadOnly),
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            return new ResourceTypeComponents(
                ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}/dapr.io.StateStore@{RadiusResources.ResourceApiVersion}"),
                ResourceScope.ResourceGroup,
                bodyType);
        }

public static ResourceTypeComponents MakeDaprSecretStore()
        {
            var genericSecretStoreType = new ObjectType(
                name: "generic",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("secretstoreName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "Secret store name"),
                    new TypeProperty("kind", new StringLiteralType("generic"), TypePropertyFlags.Required, "The secret store kind"),
                    new TypeProperty("type", LanguageConstants.String, TypePropertyFlags.Required, "The Dapr Secret Store type. These strings match the format used by Dapr Kubernetes configuration format"),
                    new TypeProperty("version", LanguageConstants.String, TypePropertyFlags.Required, "Dapr component version"),
                    new TypeProperty("metadata", LanguageConstants.Object, TypePropertyFlags.WriteOnly | TypePropertyFlags.Required, "Metadata for the secret store resource. This should match the Dapr component spec"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var propertiesType = new DiscriminatedObjectType(
                    "properties",
                    validationFlags: TypeSymbolValidationFlags.Default,
                    discriminatorKey: "kind",
                    unionMembers: new ITypeReference[] { genericSecretStoreType });
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var typeName = $"{RadiusResources.ApplicationResourceType}/dapr.io.SecretStore@{RadiusResources.ResourceApiVersion}";
            var bodyType = new ObjectType(
                name: typeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    // Top level properties are predefined
                    CommonProperties.Id,
                    CommonProperties.Name,
                    new TypeProperty("type", new StringLiteralType(typeName), TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.ReadOnly),
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            return new ResourceTypeComponents(
                ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}/dapr.io.SecretStore@{RadiusResources.ResourceApiVersion}"),
                ResourceScope.ResourceGroup,
                bodyType);
            
        }
        
        public static ResourceTypeComponents MakeDaprPubSubTopic()
        {
            var azureServiceBusPubSubType = new ObjectType(
                name: "pubsub.azure.servicebus",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[]
                {
                    new TypeProperty("pubSubName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "Pub/Sub name"),
                    new TypeProperty("kind", new StringLiteralType("pubsub.azure.servicebus"), TypePropertyFlags.Required, "The Dapr Pub/Sub kind. These strings match the format used by Dapr Kubernetes configuration format"),
                    new TypeProperty("topic", LanguageConstants.String, TypePropertyFlags.None, "PubSub topic"),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None, "PubSub resource, for unmanaged"),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None, "Indicates if the resource is Radius-managed. If false, a resource is required"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var anyPubSubType = new ObjectType(
                name: "any",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("pubSubName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "Pub/Sub name"),
                    new TypeProperty("kind", new StringLiteralType("any"), TypePropertyFlags.Required, "The Dapr Pub/Sub kind"),
                    new TypeProperty("topic", LanguageConstants.String, TypePropertyFlags.None, "PubSub topic"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var genericPubSubType = new ObjectType(
                name: "generic",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("pubSubName", LanguageConstants.String, TypePropertyFlags.ReadOnly, "Pub/Sub name"),
                    new TypeProperty("kind", new StringLiteralType("generic"), TypePropertyFlags.Required, "The Dapr Pub/Sub kind"),
                    new TypeProperty("type", LanguageConstants.String, TypePropertyFlags.Required, "The Dapr Pub/Sub type. These strings match the format used by Dapr Kubernetes configuration format"),
                    new TypeProperty("version", LanguageConstants.String, TypePropertyFlags.Required, "Dapr component version"),
                    new TypeProperty("metadata", LanguageConstants.Object, TypePropertyFlags.WriteOnly | TypePropertyFlags.Required, "Metadata for the Pub/Sub resource. This should match the Dapr component spec"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var daprPubSubKindType = new DiscriminatedObjectType(
                name: "dapr pubsub kind",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: new ITypeReference[] { azureServiceBusPubSubType, anyPubSubType, genericPubSubType });

            var propertiesType = new DiscriminatedObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.Default,
                discriminatorKey: "kind",
                unionMembers: new ITypeReference[] { azureServiceBusPubSubType, anyPubSubType, genericPubSubType });
            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var typeName = $"{RadiusResources.ApplicationResourceType}/dapr.io.PubSubTopic@{RadiusResources.ResourceApiVersion}";
            var bodyType = new ObjectType(
                name: typeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    // Top level properties are predefined
                    CommonProperties.Id,
                    CommonProperties.Name,
                    new TypeProperty("type", new StringLiteralType(typeName), TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.ReadOnly),
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            return new ResourceTypeComponents(
                ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}/dapr.io.PubSubTopic@{RadiusResources.ResourceApiVersion}"),
                ResourceScope.ResourceGroup,
                bodyType);
        }

        public static ResourceTypeComponents MakeGeneric(IEnumerable<FunctionOverload> resourceFunctions)
        {
            var propertiesType = new ObjectType(
                "properties",
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    new TypeProperty(
                        "secrets",
                        new ObjectType(
                            "secrets",
                            validationFlags: TypeSymbolValidationFlags.Default,
                            properties: Array.Empty<TypeProperty>(),
                            additionalPropertiesType: LanguageConstants.String),
                        TypePropertyFlags.WriteOnly),
                },
                additionalPropertiesType: LanguageConstants.Any);


            var propertiesProperty = new TypeProperty("properties", propertiesType, TypePropertyFlags.Required);

            var typeName = $"{RadiusResources.ApplicationResourceType}/Generic@{RadiusResources.ResourceApiVersion}";
            var bodyType = new ObjectType(
                name: typeName,
                validationFlags: TypeSymbolValidationFlags.WarnOnTypeMismatch,
                properties: new[]
                {
                    // Top level properties are predefined
                    CommonProperties.Id,
                    CommonProperties.Name,
                    new TypeProperty("type", new StringLiteralType(typeName), TypePropertyFlags.DeployTimeConstant | TypePropertyFlags.ReadOnly),
                    CommonProperties.ApiVersion,
                    CommonProperties.DependsOn,
                    CommonProperties.Tags,
                    propertiesProperty,
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: resourceFunctions);

            return new ResourceTypeComponents(
                ResourceTypeReference.Parse($"{RadiusResources.ApplicationResourceType}/Generic@{RadiusResources.ResourceApiVersion}"),
                ResourceScope.ResourceGroup,
                bodyType);
        }

        public static ComponentData MakeServiceBusQueue()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("azure.com", "ServiceBusQueue", ""),
                Binding = CommonBindings.BindingDataServiceBusQueue,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                    new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeRedis()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("redislabs.com", "RedisCache", ""),
                Binding = CommonBindings.BindingDataRedis,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeRabbitMQ()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("rabbitmq.com", "MessageQueue", ""),
                Binding = CommonBindings.BindingDataRabbitMQ,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("queue", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeMicrosoftSQL()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("microsoft.com", "SQLDatabase", ""),
                Binding = CommonBindings.BindingDataSQL,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None, description:
                    @"Specifies the backing resource of this component. This or `server/database` must be set when using `managed: false` (default).

For Azure, this property will accept a resource ID of a `Microsoft.Sql/servers/databases` resource.

Resources provided by the developer will not be modified or deleted by Radius. Use this property to attach resources created with Bicep or any other mechanism."),
                    new TypeProperty("server", LanguageConstants.String, TypePropertyFlags.None, description:
                    "The name of the SQL database."),
                    new TypeProperty("database", LanguageConstants.String, TypePropertyFlags.None, description:
                    "The fully qualified domain name of the SQL database."),
                },
            };
        }

        public static ComponentData MakeKeyVault()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("azure.com", "KeyVault", ""),
                Binding = CommonBindings.BindingDataKeyVault,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeMongoDB()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("mongo.com", "MongoDatabase", ""),
                Binding = CommonBindings.BindingDataMongo,
                Properties =
                {
                    new TypeProperty(
                        "managed",
                        LanguageConstants.Bool,
                        TypePropertyFlags.None,
                        description: @"Specifies whether the lifecycle of the resource is controlled by Radius. The default value is `false`.

When using `managed: true`, Radius will create the backing resources (databases, message queues, etc) when creating this component.
The backing resources will also be deleted when this component is deleted.

When using `managed: false` (default) the developer must specify the backing resources to use by providing a value for the `resource` property.
Resources provided by the developer will not be modified or deleted by Radius. Use `managed: false` to attach resources created with Bicep or any other mechanism."),
                    new TypeProperty(
                        "resource",
                        LanguageConstants.String,
                        TypePropertyFlags.None,
                        description: @"Specifies the backing resource of this component. This is required when using `managed: false` (default).

For Azure, this property will accept a resource ID of a `Microsoft.DocumentDB/accounts/mongoDatabases` resource (CosmosDB MongoDB API).

Resources provided by the developer will not be modified or deleted by Radius. Use this property to attach resources created with Bicep or any other mechanism."),
                },
            };
        }

        public static ComponentData MakeVolume()
        {
            var encodingType = new UnionType("encoding", ImmutableArray.Create<ITypeReference>(new StringLiteralType("utf-8"), new StringLiteralType("hex"), new StringLiteralType("base64")));
            var formatType = new UnionType("format", ImmutableArray.Create<ITypeReference>(new StringLiteralType("pfx"), new StringLiteralType("pem")));
            var valueType = new UnionType("value", ImmutableArray.Create<ITypeReference>(new StringLiteralType("certificate"), new StringLiteralType("publickey"), new StringLiteralType("privatekey")));

            var secretItemType = new ObjectType(
                name: "secret",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required, "Name of the Azure KeyVault secret"),
                    new TypeProperty("version", LanguageConstants.String, TypePropertyFlags.None, "Version of the secret. Latest by default"),
                    new TypeProperty("encoding", encodingType, TypePropertyFlags.None, "Encoding format of the secret"),
                },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var secrets = new ObjectType(
                name: "secrets",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: secretItemType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var keysItemType = new ObjectType(
                name: "key",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required, "Name of the Azure KeyVault key"),
                    new TypeProperty("version", LanguageConstants.String, TypePropertyFlags.None, "Version of the key. Latest by default"),
                    },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var keys = new ObjectType(
                name: "keys",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: keysItemType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var certificatesItemType = new ObjectType(
                name: "certificates",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: new TypeProperty[] {
                    new TypeProperty("name", LanguageConstants.String, TypePropertyFlags.Required, "Name of the Azure KeyVault certificate"),
                    new TypeProperty("version", LanguageConstants.String, TypePropertyFlags.None, "Version of the certificate. Latest by default"),
                    new TypeProperty("encoding", encodingType, TypePropertyFlags.None, "Encoding format of the certificate"),
                    new TypeProperty("format", formatType, TypePropertyFlags.None, "Format of the certificate"),
                    new TypeProperty("value", valueType, TypePropertyFlags.Required,"Value to be downloaded from the Azure KeyVault"),
                    },
                additionalPropertiesType: null,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);

            var certificates = new ObjectType(
                name: "certificates",
                validationFlags: TypeSymbolValidationFlags.Default,
                properties: Array.Empty<TypeProperty>(),
                additionalPropertiesType: certificatesItemType,
                additionalPropertiesFlags: TypePropertyFlags.None,
                functions: null);


            var persistentVolumeKindType = new UnionType(
                "persistent volume kind",
                ImmutableArray.Create<ITypeReference>(
                    new StringLiteralType("azure.com.fileshare"),
                    new StringLiteralType("azure.com.keyvault")
                )
            );

            return new ComponentData()
            {
                Type = new ThreePartType(null, "Volume", ""),
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None, "Managed by Radius"),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None, "Resource ID for user managed resource"),
                    new TypeProperty("kind", persistentVolumeKindType, TypePropertyFlags.Required, "Persistent volume kind"),
                    new TypeProperty("secrets", secrets, TypePropertyFlags.None, "Secrets from the Azure KeyVault to be mounted"),
                    new TypeProperty("keys", keys, TypePropertyFlags.None, "Keys from the Azure KeyVault to be mounted"),
                    new TypeProperty("certificates", certificates, TypePropertyFlags.None, "Certificates from the Azure KeyVault to be mounted")
                }
            };
        }
    }
}
