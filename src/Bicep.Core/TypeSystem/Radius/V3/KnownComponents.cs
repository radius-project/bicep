// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
                Type = new ThreePartType(null, "Executable", RadiusResources.CategoryComponent),
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
                Type = new ThreePartType(null, "Container", RadiusResources.CategoryComponent),
                Properties =
                {
                    connectionsProperty,
                    containerProperty,
                    CommonProperties.Traits,
                },
            };
        }

        public static ComponentData MakeDaprStateStore()
        {
            var configKindType = new UnionType(
                "state store kind",
                ImmutableArray.Create<ITypeReference>(
                    new StringLiteralType("state.azure.tablestorage"),
                    new StringLiteralType("state.sqlserver"),
                    new StringLiteralType("state.redis"),
                    new StringLiteralType("any")));

            return new ComponentData()
            {
                Type = new ThreePartType("dapr.io", "StateStore", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataDaprStateStore,
                Properties =
                {
                    new TypeProperty("kind", configKindType, TypePropertyFlags.Required),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeDaprPubSubTopic()
        {
            var configKindType = new UnionType(
                "pubsub kind",
                ImmutableArray.Create<ITypeReference>(
                    new StringLiteralType("pubsub.azure.servicebus"),
                    new StringLiteralType("any")));

            return new ComponentData()
            {
                Type = new ThreePartType("dapr.io", "PubSubTopic", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataDaprPubSubTopic,
                Properties =
                {
                    new TypeProperty("kind", configKindType, TypePropertyFlags.Required),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                    new TypeProperty("topic", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeServiceBusQueue()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("azure.com", "ServiceBusQueue", RadiusResources.CategoryComponent),
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
                Type = new ThreePartType("redislabs.com", "Redis", RadiusResources.CategoryComponent),
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
                Type = new ThreePartType("rabbitmq.com", "MessageQueue", RadiusResources.CategoryComponent),
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
                Type = new ThreePartType("microsoft.com", "SQL", RadiusResources.CategoryComponent),
                Binding = CommonBindings.BindingDataSQL,
                Properties =
                {
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }

        public static ComponentData MakeKeyVault()
        {
            return new ComponentData()
            {
                Type = new ThreePartType("azure.com", "KeyVault", RadiusResources.CategoryComponent),
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
                Type = new ThreePartType("mongodb.com", "MongoDB", RadiusResources.CategoryComponent),
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
            var configKindType = new UnionType(
                "volume kind",
                ImmutableArray.Create<ITypeReference>(new StringLiteralType("azure.com.fileshare")));

            return new ComponentData()
            {
                Type = new ThreePartType(null, "Volume", ""),
                Properties =
                {
                    new TypeProperty("kind", configKindType, TypePropertyFlags.Required),
                    new TypeProperty("managed", LanguageConstants.Bool, TypePropertyFlags.None),
                    new TypeProperty("resource", LanguageConstants.String, TypePropertyFlags.None),
                },
            };
        }
    }
}
