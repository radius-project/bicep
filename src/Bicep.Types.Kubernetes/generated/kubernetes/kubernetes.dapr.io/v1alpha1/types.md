# kubernetes.dapr.io @ v1alpha1

## Resource kubernetes.dapr.io/Component@v1alpha1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'dapr.io/v1alpha1' (ReadOnly, DeployTimeConstant): The api version.
* **auth**: [IoDaprV1Alpha1ComponentAuth](#iodaprv1alpha1componentauth): Auth represents authentication details for the component
* **kind**: 'Component' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **scopes**: string[]: Array of IoDaprV1Alpha1ComponentScopesItem
* **spec**: [IoDaprV1Alpha1ComponentSpec](#iodaprv1alpha1componentspec): ComponentSpec is the spec for a component

## Resource kubernetes.dapr.io/Configuration@v1alpha1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'dapr.io/v1alpha1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'Configuration' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **spec**: [IoDaprV1Alpha1ConfigurationSpec](#iodaprv1alpha1configurationspec): ConfigurationSpec is the spec for an configuration

## Resource kubernetes.dapr.io/Subscription@v1alpha1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'dapr.io/v1alpha1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'Subscription' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **scopes**: string[]: Array of IoDaprV1Alpha1SubscriptionScopesItem
* **spec**: [IoDaprV1Alpha1SubscriptionSpec](#iodaprv1alpha1subscriptionspec): SubscriptionSpec is the spec for an event subscription

## IoDaprV1Alpha1ComponentAuth
### Properties
* **secretStore**: string (Required)

## metadata
### Properties
* **annotations**: [annotations](#annotations): The annotations for the resource.
* **labels**: [labels](#labels): The labels for the resource.
* **name**: string (Required, DeployTimeConstant): The name of the resource.
* **namespace**: string (DeployTimeConstant): The namespace of the resource.

## annotations
### Properties
### Additional Properties
* **Additional Properties Type**: string

## labels
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoDaprV1Alpha1ComponentSpec
### Properties
* **ignoreErrors**: bool
* **initTimeout**: string
* **metadata**: [IoDaprV1Alpha1ComponentSpecMetadataItem](#iodaprv1alpha1componentspecmetadataitem)[] (Required): Array of io.dapr.v1alpha1.Component-spec-metadataItem
* **type**: string (Required)
* **version**: string (Required)

## IoDaprV1Alpha1ComponentSpecMetadataItem
### Properties
* **name**: string (Required)
* **secretKeyRef**: [IoDaprV1Alpha1ComponentSpecMetadataItemSecretKeyRef](#iodaprv1alpha1componentspecmetadataitemsecretkeyref): SecretKeyRef is a reference to a secret holding the value for the metadata item. Name is the secret name, and key is the field in the secret.
* **value**: any: Any object

## IoDaprV1Alpha1ComponentSpecMetadataItemSecretKeyRef
### Properties
* **key**: string (Required)
* **name**: string (Required)

## metadata
### Properties
* **annotations**: [annotations](#annotations): The annotations for the resource.
* **labels**: [labels](#labels): The labels for the resource.
* **name**: string (Required, DeployTimeConstant): The name of the resource.
* **namespace**: string (DeployTimeConstant): The namespace of the resource.

## annotations
### Properties
### Additional Properties
* **Additional Properties Type**: string

## labels
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoDaprV1Alpha1ConfigurationSpec
### Properties
* **accessControl**: [IoDaprV1Alpha1ConfigurationSpecAccessControl](#iodaprv1alpha1configurationspecaccesscontrol): AccessControlSpec is the spec object in ConfigurationSpec
* **httpPipeline**: [IoDaprV1Alpha1ConfigurationSpecHttpPipeline](#iodaprv1alpha1configurationspechttppipeline): PipelineSpec defines the middleware pipeline
* **metric**: [IoDaprV1Alpha1ConfigurationSpecMetric](#iodaprv1alpha1configurationspecmetric): MetricSpec defines metrics configuration
* **mtls**: [IoDaprV1Alpha1ConfigurationSpecMtls](#iodaprv1alpha1configurationspecmtls): MTLSSpec defines mTLS configuration
* **secrets**: [IoDaprV1Alpha1ConfigurationSpecSecrets](#iodaprv1alpha1configurationspecsecrets): SecretsSpec is the spec for secrets configuration
* **tracing**: [IoDaprV1Alpha1ConfigurationSpecTracing](#iodaprv1alpha1configurationspectracing): TracingSpec is the spec object in ConfigurationSpec

## IoDaprV1Alpha1ConfigurationSpecAccessControl
### Properties
* **defaultAction**: string
* **policies**: [IoDaprV1Alpha1ConfigurationSpecAccessControlPoliciesItem](#iodaprv1alpha1configurationspecaccesscontrolpoliciesitem)[]: Array of io.dapr.v1alpha1.Configuration-spec-accessControl-policiesItem
* **trustDomain**: string

## IoDaprV1Alpha1ConfigurationSpecAccessControlPoliciesItem
### Properties
* **appId**: string (Required)
* **defaultAction**: string
* **namespace**: string
* **operations**: [IoDaprV1Alpha1ConfigurationSpecAccessControlPoliciesPropertiesItemsItem](#iodaprv1alpha1configurationspecaccesscontrolpoliciespropertiesitemsitem)[]: Array of io.dapr.v1alpha1.Configuration-spec-accessControl-policies-properties-itemsItem
* **trustDomain**: string

## IoDaprV1Alpha1ConfigurationSpecAccessControlPoliciesPropertiesItemsItem
### Properties
* **action**: string (Required)
* **httpVerb**: string[]: Array of IoDaprV1Alpha1ConfigurationSpecAccessControlPoliciesPropertiesItemsHttpVerbItem
* **name**: string (Required)

## IoDaprV1Alpha1ConfigurationSpecHttpPipeline
### Properties
* **handlers**: [IoDaprV1Alpha1ConfigurationSpecHttpPipelineHandlersItem](#iodaprv1alpha1configurationspechttppipelinehandlersitem)[] (Required): Array of io.dapr.v1alpha1.Configuration-spec-httpPipeline-handlersItem

## IoDaprV1Alpha1ConfigurationSpecHttpPipelineHandlersItem
### Properties
* **name**: string (Required)
* **selector**: [IoDaprV1Alpha1ConfigurationSpecHttpPipelineHandlersItemSelector](#iodaprv1alpha1configurationspechttppipelinehandlersitemselector): SelectorSpec selects target services to which the handler is to be applied
* **type**: string (Required)

## IoDaprV1Alpha1ConfigurationSpecHttpPipelineHandlersItemSelector
### Properties
* **fields**: [IoDaprV1Alpha1ConfigurationSpecHttpPipelineHandlersPropertiesItemsItem](#iodaprv1alpha1configurationspechttppipelinehandlerspropertiesitemsitem)[] (Required): Array of io.dapr.v1alpha1.Configuration-spec-httpPipeline-handlers-properties-properties-itemsItem

## IoDaprV1Alpha1ConfigurationSpecHttpPipelineHandlersPropertiesItemsItem
### Properties
* **field**: string (Required)
* **value**: string (Required)

## IoDaprV1Alpha1ConfigurationSpecMetric
### Properties
* **enabled**: bool (Required)

## IoDaprV1Alpha1ConfigurationSpecMtls
### Properties
* **allowedClockSkew**: string
* **enabled**: bool (Required)
* **workloadCertTTL**: string

## IoDaprV1Alpha1ConfigurationSpecSecrets
### Properties
* **scopes**: [IoDaprV1Alpha1ConfigurationSpecSecretsScopesItem](#iodaprv1alpha1configurationspecsecretsscopesitem)[] (Required): Array of io.dapr.v1alpha1.Configuration-spec-secrets-scopesItem

## IoDaprV1Alpha1ConfigurationSpecSecretsScopesItem
### Properties
* **allowedSecrets**: string[]: Array of IoDaprV1Alpha1ConfigurationSpecSecretsScopesPropertiesItemsItem
* **defaultAccess**: string
* **deniedSecrets**: string[]: Array of String
* **storeName**: string (Required)

## IoDaprV1Alpha1ConfigurationSpecTracing
### Properties
* **samplingRate**: string (Required)
* **zipkin**: [IoDaprV1Alpha1ConfigurationSpecTracingZipkin](#iodaprv1alpha1configurationspectracingzipkin): Defines the Zipkin trace configurations

## IoDaprV1Alpha1ConfigurationSpecTracingZipkin
### Properties
* **endpointAddress**: string: The endpoint address of Zipkin server to receive traces

## metadata
### Properties
* **annotations**: [annotations](#annotations): The annotations for the resource.
* **labels**: [labels](#labels): The labels for the resource.
* **name**: string (Required, DeployTimeConstant): The name of the resource.
* **namespace**: string (DeployTimeConstant): The namespace of the resource.

## annotations
### Properties
### Additional Properties
* **Additional Properties Type**: string

## labels
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoDaprV1Alpha1SubscriptionSpec
### Properties
* **pubsubname**: string (Required)
* **route**: string (Required)
* **topic**: string (Required)

