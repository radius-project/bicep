# kubernetes.policy @ v1beta1

## Resource kubernetes.policy/PodDisruptionBudget@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'policy/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'PodDisruptionBudget' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **spec**: [IoK8SApiPolicyV1Beta1PodDisruptionBudgetSpec](#iok8sapipolicyv1beta1poddisruptionbudgetspec): PodDisruptionBudgetSpec is a description of a PodDisruptionBudget.
* **status**: [IoK8SApiPolicyV1Beta1PodDisruptionBudgetStatus](#iok8sapipolicyv1beta1poddisruptionbudgetstatus): PodDisruptionBudgetStatus represents information about the status of a PodDisruptionBudget. Status may trail the actual state of a system.

## Resource kubernetes.policy/PodSecurityPolicy@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'policy/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'PodSecurityPolicy' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **spec**: [IoK8SApiPolicyV1Beta1PodSecurityPolicySpec](#iok8sapipolicyv1beta1podsecuritypolicyspec): PodSecurityPolicySpec defines the policy enforced.

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

## IoK8SApiPolicyV1Beta1PodDisruptionBudgetSpec
### Properties
* **maxUnavailable**: string: IntOrString is a type that can hold an int32 or a string.  When used in JSON or YAML marshalling and unmarshalling, it produces or consumes the inner type.  This allows you to have, for example, a JSON field that can accept a name or number.
* **minAvailable**: string: IntOrString is a type that can hold an int32 or a string.  When used in JSON or YAML marshalling and unmarshalling, it produces or consumes the inner type.  This allows you to have, for example, a JSON field that can accept a name or number.
* **selector**: [IoK8SApimachineryPkgApisMetaV1LabelSelector](#iok8sapimachinerypkgapismetav1labelselector): A label selector is a label query over a set of resources. The result of matchLabels and matchExpressions are ANDed. An empty label selector matches all objects. A null label selector matches no objects.

## IoK8SApimachineryPkgApisMetaV1LabelSelector
### Properties
* **matchExpressions**: [IoK8SApimachineryPkgApisMetaV1LabelSelectorRequirement](#iok8sapimachinerypkgapismetav1labelselectorrequirement)[]: matchExpressions is a list of label selector requirements. The requirements are ANDed.
* **matchLabels**: [IoK8SApimachineryPkgApisMetaV1LabelSelectorMatchLabels](#iok8sapimachinerypkgapismetav1labelselectormatchlabels): matchLabels is a map of {key,value} pairs. A single {key,value} in the matchLabels map is equivalent to an element of matchExpressions, whose key field is "key", the operator is "In", and the values array contains only "value". The requirements are ANDed.

## IoK8SApimachineryPkgApisMetaV1LabelSelectorRequirement
### Properties
* **key**: string (Required): key is the label key that the selector applies to.
* **operator**: string (Required): operator represents a key's relationship to a set of values. Valid operators are In, NotIn, Exists and DoesNotExist.
* **values**: string[]: values is an array of string values. If the operator is In or NotIn, the values array must be non-empty. If the operator is Exists or DoesNotExist, the values array must be empty. This array is replaced during a strategic merge patch.

## IoK8SApimachineryPkgApisMetaV1LabelSelectorMatchLabels
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoK8SApiPolicyV1Beta1PodDisruptionBudgetStatus
### Properties
* **currentHealthy**: int (Required): current number of healthy pods
* **desiredHealthy**: int (Required): minimum desired number of healthy pods
* **disruptedPods**: [IoK8SApiPolicyV1Beta1PodDisruptionBudgetStatusDisruptedPods](#iok8sapipolicyv1beta1poddisruptionbudgetstatusdisruptedpods): DisruptedPods contains information about pods whose eviction was processed by the API server eviction subresource handler but has not yet been observed by the PodDisruptionBudget controller. A pod will be in this map from the time when the API server processed the eviction request to the time when the pod is seen by PDB controller as having been marked for deletion (or after a timeout). The key in the map is the name of the pod and the value is the time when the API server processed the eviction request. If the deletion didn't occur and a pod is still there it will be removed from the list automatically by PodDisruptionBudget controller after some time. If everything goes smooth this map should be empty for the most of the time. Large number of entries in the map may indicate problems with pod deletions.
* **disruptionsAllowed**: int (Required): Number of pod disruptions that are currently allowed.
* **expectedPods**: int (Required): total number of pods counted by this disruption budget
* **observedGeneration**: int: Most recent generation observed when updating this PDB status. DisruptionsAllowed and other status information is valid only if observedGeneration equals to PDB's object generation.

## IoK8SApiPolicyV1Beta1PodDisruptionBudgetStatusDisruptedPods
### Properties
### Additional Properties
* **Additional Properties Type**: string

## metadata
### Properties
* **annotations**: [annotations](#annotations): The annotations for the resource.
* **labels**: [labels](#labels): The labels for the resource.
* **name**: string (Required, DeployTimeConstant): The name of the resource.

## annotations
### Properties
### Additional Properties
* **Additional Properties Type**: string

## labels
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoK8SApiPolicyV1Beta1PodSecurityPolicySpec
### Properties
* **allowedCapabilities**: string[]: allowedCapabilities is a list of capabilities that can be requested to add to the container. Capabilities in this field may be added at the pod author's discretion. You must not list a capability in both allowedCapabilities and requiredDropCapabilities.
* **allowedCSIDrivers**: [IoK8SApiPolicyV1Beta1AllowedCSIDriver](#iok8sapipolicyv1beta1allowedcsidriver)[]: AllowedCSIDrivers is an allowlist of inline CSI drivers that must be explicitly set to be embedded within a pod spec. An empty value indicates that any CSI driver can be used for inline ephemeral volumes. This is a beta field, and is only honored if the API server enables the CSIInlineVolume feature gate.
* **allowedFlexVolumes**: [IoK8SApiPolicyV1Beta1AllowedFlexVolume](#iok8sapipolicyv1beta1allowedflexvolume)[]: allowedFlexVolumes is an allowlist of Flexvolumes.  Empty or nil indicates that all Flexvolumes may be used.  This parameter is effective only when the usage of the Flexvolumes is allowed in the "volumes" field.
* **allowedHostPaths**: [IoK8SApiPolicyV1Beta1AllowedHostPath](#iok8sapipolicyv1beta1allowedhostpath)[]: allowedHostPaths is an allowlist of host paths. Empty indicates that all host paths may be used.
* **allowedProcMountTypes**: string[]: AllowedProcMountTypes is an allowlist of allowed ProcMountTypes. Empty or nil indicates that only the DefaultProcMountType may be used. This requires the ProcMountType feature flag to be enabled.
* **allowedUnsafeSysctls**: string[]: allowedUnsafeSysctls is a list of explicitly allowed unsafe sysctls, defaults to none. Each entry is either a plain sysctl name or ends in "*" in which case it is considered as a prefix of allowed sysctls. Single * means all unsafe sysctls are allowed. Kubelet has to allowlist all allowed unsafe sysctls explicitly to avoid rejection.

Examples: e.g. "foo/*" allows "foo/bar", "foo/baz", etc. e.g. "foo.*" allows "foo.bar", "foo.baz", etc.
* **allowPrivilegeEscalation**: bool: allowPrivilegeEscalation determines if a pod can request to allow privilege escalation. If unspecified, defaults to true.
* **defaultAddCapabilities**: string[]: defaultAddCapabilities is the default set of capabilities that will be added to the container unless the pod spec specifically drops the capability.  You may not list a capability in both defaultAddCapabilities and requiredDropCapabilities. Capabilities added here are implicitly allowed, and need not be included in the allowedCapabilities list.
* **defaultAllowPrivilegeEscalation**: bool: defaultAllowPrivilegeEscalation controls the default setting for whether a process can gain more privileges than its parent process.
* **forbiddenSysctls**: string[]: forbiddenSysctls is a list of explicitly forbidden sysctls, defaults to none. Each entry is either a plain sysctl name or ends in "*" in which case it is considered as a prefix of forbidden sysctls. Single * means all sysctls are forbidden.

Examples: e.g. "foo/*" forbids "foo/bar", "foo/baz", etc. e.g. "foo.*" forbids "foo.bar", "foo.baz", etc.
* **fsGroup**: [IoK8SApiPolicyV1Beta1FSGroupStrategyOptions](#iok8sapipolicyv1beta1fsgroupstrategyoptions) (Required): FSGroupStrategyOptions defines the strategy type and options used to create the strategy.
* **hostIPC**: bool: hostIPC determines if the policy allows the use of HostIPC in the pod spec.
* **hostNetwork**: bool: hostNetwork determines if the policy allows the use of HostNetwork in the pod spec.
* **hostPID**: bool: hostPID determines if the policy allows the use of HostPID in the pod spec.
* **hostPorts**: [IoK8SApiPolicyV1Beta1HostPortRange](#iok8sapipolicyv1beta1hostportrange)[]: hostPorts determines which host port ranges are allowed to be exposed.
* **privileged**: bool: privileged determines if a pod can request to be run as privileged.
* **readOnlyRootFilesystem**: bool: readOnlyRootFilesystem when set to true will force containers to run with a read only root file system.  If the container specifically requests to run with a non-read only root file system the PSP should deny the pod. If set to false the container may run with a read only root file system if it wishes but it will not be forced to.
* **requiredDropCapabilities**: string[]: requiredDropCapabilities are the capabilities that will be dropped from the container.  These are required to be dropped and cannot be added.
* **runAsGroup**: [IoK8SApiPolicyV1Beta1RunAsGroupStrategyOptions](#iok8sapipolicyv1beta1runasgroupstrategyoptions): RunAsGroupStrategyOptions defines the strategy type and any options used to create the strategy.
* **runAsUser**: [IoK8SApiPolicyV1Beta1RunAsUserStrategyOptions](#iok8sapipolicyv1beta1runasuserstrategyoptions) (Required): RunAsUserStrategyOptions defines the strategy type and any options used to create the strategy.
* **runtimeClass**: [IoK8SApiPolicyV1Beta1RuntimeClassStrategyOptions](#iok8sapipolicyv1beta1runtimeclassstrategyoptions): RuntimeClassStrategyOptions define the strategy that will dictate the allowable RuntimeClasses for a pod.
* **seLinux**: [IoK8SApiPolicyV1Beta1SELinuxStrategyOptions](#iok8sapipolicyv1beta1selinuxstrategyoptions) (Required): SELinuxStrategyOptions defines the strategy type and any options used to create the strategy.
* **supplementalGroups**: [IoK8SApiPolicyV1Beta1SupplementalGroupsStrategyOptions](#iok8sapipolicyv1beta1supplementalgroupsstrategyoptions) (Required): SupplementalGroupsStrategyOptions defines the strategy type and options used to create the strategy.
* **volumes**: string[]: volumes is an allowlist of volume plugins. Empty indicates that no volumes may be used. To allow all volumes you may use '*'.

## IoK8SApiPolicyV1Beta1AllowedCSIDriver
### Properties
* **name**: string (Required): Name is the registered name of the CSI driver

## IoK8SApiPolicyV1Beta1AllowedFlexVolume
### Properties
* **driver**: string (Required): driver is the name of the Flexvolume driver.

## IoK8SApiPolicyV1Beta1AllowedHostPath
### Properties
* **pathPrefix**: string: pathPrefix is the path prefix that the host volume must match. It does not support `*`. Trailing slashes are trimmed when validating the path prefix with a host path.

Examples: `/foo` would allow `/foo`, `/foo/` and `/foo/bar` `/foo` would not allow `/food` or `/etc/foo`
* **readOnly**: bool: when set to true, will allow host volumes matching the pathPrefix only if all volume mounts are readOnly.

## IoK8SApiPolicyV1Beta1FSGroupStrategyOptions
### Properties
* **ranges**: [IoK8SApiPolicyV1Beta1IDRange](#iok8sapipolicyv1beta1idrange)[]: ranges are the allowed ranges of fs groups.  If you would like to force a single fs group then supply a single range with the same start and end. Required for MustRunAs.
* **rule**: string: rule is the strategy that will dictate what FSGroup is used in the SecurityContext.

## IoK8SApiPolicyV1Beta1IDRange
### Properties
* **max**: int (Required): max is the end of the range, inclusive.
* **min**: int (Required): min is the start of the range, inclusive.

## IoK8SApiPolicyV1Beta1HostPortRange
### Properties
* **max**: int (Required): max is the end of the range, inclusive.
* **min**: int (Required): min is the start of the range, inclusive.

## IoK8SApiPolicyV1Beta1RunAsGroupStrategyOptions
### Properties
* **ranges**: [IoK8SApiPolicyV1Beta1IDRange](#iok8sapipolicyv1beta1idrange)[]: ranges are the allowed ranges of gids that may be used. If you would like to force a single gid then supply a single range with the same start and end. Required for MustRunAs.
* **rule**: string (Required): rule is the strategy that will dictate the allowable RunAsGroup values that may be set.

## IoK8SApiPolicyV1Beta1RunAsUserStrategyOptions
### Properties
* **ranges**: [IoK8SApiPolicyV1Beta1IDRange](#iok8sapipolicyv1beta1idrange)[]: ranges are the allowed ranges of uids that may be used. If you would like to force a single uid then supply a single range with the same start and end. Required for MustRunAs.
* **rule**: string (Required): rule is the strategy that will dictate the allowable RunAsUser values that may be set.

## IoK8SApiPolicyV1Beta1RuntimeClassStrategyOptions
### Properties
* **allowedRuntimeClassNames**: string[] (Required): allowedRuntimeClassNames is an allowlist of RuntimeClass names that may be specified on a pod. A value of "*" means that any RuntimeClass name is allowed, and must be the only item in the list. An empty list requires the RuntimeClassName field to be unset.
* **defaultRuntimeClassName**: string: defaultRuntimeClassName is the default RuntimeClassName to set on the pod. The default MUST be allowed by the allowedRuntimeClassNames list. A value of nil does not mutate the Pod.

## IoK8SApiPolicyV1Beta1SELinuxStrategyOptions
### Properties
* **rule**: string (Required): rule is the strategy that will dictate the allowable labels that may be set.
* **seLinuxOptions**: [IoK8SApiCoreV1SELinuxOptions](#iok8sapicorev1selinuxoptions): SELinuxOptions are the labels to be applied to the container

## IoK8SApiCoreV1SELinuxOptions
### Properties
* **level**: string: Level is SELinux level label that applies to the container.
* **role**: string: Role is a SELinux role label that applies to the container.
* **type**: string: Type is a SELinux type label that applies to the container.
* **user**: string: User is a SELinux user label that applies to the container.

## IoK8SApiPolicyV1Beta1SupplementalGroupsStrategyOptions
### Properties
* **ranges**: [IoK8SApiPolicyV1Beta1IDRange](#iok8sapipolicyv1beta1idrange)[]: ranges are the allowed ranges of supplemental groups.  If you would like to force a single supplemental group then supply a single range with the same start and end. Required for MustRunAs.
* **rule**: string: rule is the strategy that will dictate what supplemental groups is used in the SecurityContext.

