# kubernetes.discovery.k8s.io @ v1beta1

## Resource kubernetes.discovery.k8s.io/EndpointSlice@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **addressType**: string (Required): addressType specifies the type of address carried by this EndpointSlice. All addresses in this slice must be the same type. This field is immutable after creation. The following address types are currently supported: * IPv4: Represents an IPv4 Address. * IPv6: Represents an IPv6 Address. * FQDN: Represents a Fully Qualified Domain Name.
* **apiVersion**: 'discovery.k8s.io/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **endpoints**: [IoK8SApiDiscoveryV1Beta1Endpoint](#iok8sapidiscoveryv1beta1endpoint)[] (Required): endpoints is a list of unique endpoints in this slice. Each slice may include a maximum of 1000 endpoints.
* **kind**: 'EndpointSlice' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **ports**: [IoK8SApiDiscoveryV1Beta1EndpointPort](#iok8sapidiscoveryv1beta1endpointport)[]: ports specifies the list of network ports exposed by each endpoint in this slice. Each port must have a unique name. When ports is empty, it indicates that there are no defined ports. When a port is defined with a nil port value, it indicates "all ports". Each slice may include a maximum of 100 ports.

## IoK8SApiDiscoveryV1Beta1Endpoint
### Properties
* **addresses**: string[] (Required): addresses of this endpoint. The contents of this field are interpreted according to the corresponding EndpointSlice addressType field. Consumers must handle different types of addresses in the context of their own capabilities. This must contain at least one address but no more than 100.
* **conditions**: [IoK8SApiDiscoveryV1Beta1EndpointConditions](#iok8sapidiscoveryv1beta1endpointconditions): EndpointConditions represents the current condition of an endpoint.
* **hostname**: string: hostname of this endpoint. This field may be used by consumers of endpoints to distinguish endpoints from each other (e.g. in DNS names). Multiple endpoints which use the same hostname should be considered fungible (e.g. multiple A values in DNS). Must pass DNS Label (RFC 1123) validation.
* **targetRef**: [IoK8SApiCoreV1ObjectReference](#iok8sapicorev1objectreference): ObjectReference contains enough information to let you inspect or modify the referred object.
* **topology**: [IoK8SApiDiscoveryV1Beta1EndpointTopology](#iok8sapidiscoveryv1beta1endpointtopology): topology contains arbitrary topology information associated with the endpoint. These key/value pairs must conform with the label format. https://kubernetes.io/docs/concepts/overview/working-with-objects/labels Topology may include a maximum of 16 key/value pairs. This includes, but is not limited to the following well known keys: * kubernetes.io/hostname: the value indicates the hostname of the node
  where the endpoint is located. This should match the corresponding
  node label.
* topology.kubernetes.io/zone: the value indicates the zone where the
  endpoint is located. This should match the corresponding node label.
* topology.kubernetes.io/region: the value indicates the region where the
  endpoint is located. This should match the corresponding node label.

## IoK8SApiDiscoveryV1Beta1EndpointConditions
### Properties
* **ready**: bool: ready indicates that this endpoint is prepared to receive traffic, according to whatever system is managing the endpoint. A nil value indicates an unknown state. In most cases consumers should interpret this unknown state as ready.

## IoK8SApiCoreV1ObjectReference
### Properties
* **apiVersion**: string: API version of the referent.
* **fieldPath**: string: If referring to a piece of an object instead of an entire object, this string should contain a valid JSON/Go field access statement, such as desiredState.manifest.containers[2]. For example, if the object reference is to a container within a pod, this would take on a value like: "spec.containers{name}" (where "name" refers to the name of the container that triggered the event) or if no container name is specified "spec.containers[2]" (container with index 2 in this pod). This syntax is chosen only to have some well-defined way of referencing a part of an object.
* **kind**: string: Kind of the referent. More info: https://git.k8s.io/community/contributors/devel/sig-architecture/api-conventions.md#types-kinds
* **name**: string: Name of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/names/#names
* **namespace**: string: Namespace of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/namespaces/
* **resourceVersion**: string: Specific resourceVersion to which this reference is made, if any. More info: https://git.k8s.io/community/contributors/devel/sig-architecture/api-conventions.md#concurrency-control-and-consistency
* **uid**: string: UID of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/names/#uids

## IoK8SApiDiscoveryV1Beta1EndpointTopology
### Properties
### Additional Properties
* **Additional Properties Type**: string

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

## IoK8SApiDiscoveryV1Beta1EndpointPort
### Properties
* **appProtocol**: string: The application protocol for this port. This field follows standard Kubernetes label syntax. Un-prefixed names are reserved for IANA standard service names (as per RFC-6335 and http://www.iana.org/assignments/service-names). Non-standard protocols should use prefixed names such as mycompany.com/my-custom-protocol.
* **name**: string: The name of this port. All ports in an EndpointSlice must have a unique name. If the EndpointSlice is dervied from a Kubernetes service, this corresponds to the Service.ports[].name. Name must either be an empty string or pass DNS_LABEL validation: * must be no more than 63 characters long. * must consist of lower case alphanumeric characters or '-'. * must start and end with an alphanumeric character. Default is empty string.
* **port**: int: The port number of the endpoint. If this is not specified, ports are not restricted and must be interpreted in the context of the specific consumer.
* **protocol**: string: The IP protocol for this port. Must be UDP, TCP, or SCTP. Default is TCP.

