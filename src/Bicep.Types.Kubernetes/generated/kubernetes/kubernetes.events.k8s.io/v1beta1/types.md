# kubernetes.events.k8s.io @ v1beta1

## Resource kubernetes.events.k8s.io/Event@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **action**: string: action is what action was taken/failed regarding to the regarding object. It is machine-readable. This field can have at most 128 characters.
* **apiVersion**: 'events.k8s.io/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **deprecatedCount**: int: deprecatedCount is the deprecated field assuring backward compatibility with core.v1 Event type.
* **deprecatedFirstTimestamp**: string: Time is a wrapper around time.Time which supports correct marshaling to YAML and JSON.  Wrappers are provided for many of the factory methods that the time package offers.
* **deprecatedLastTimestamp**: string: Time is a wrapper around time.Time which supports correct marshaling to YAML and JSON.  Wrappers are provided for many of the factory methods that the time package offers.
* **deprecatedSource**: [IoK8SApiCoreV1EventSource](#iok8sapicorev1eventsource): EventSource contains information for an event.
* **eventTime**: string (Required): MicroTime is version of Time with microsecond level precision.
* **kind**: 'Event' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **note**: string: note is a human-readable description of the status of this operation. Maximal length of the note is 1kB, but libraries should be prepared to handle values up to 64kB.
* **reason**: string: reason is why the action was taken. It is human-readable. This field can have at most 128 characters.
* **regarding**: [IoK8SApiCoreV1ObjectReference](#iok8sapicorev1objectreference): ObjectReference contains enough information to let you inspect or modify the referred object.
* **related**: [IoK8SApiCoreV1ObjectReference](#iok8sapicorev1objectreference): ObjectReference contains enough information to let you inspect or modify the referred object.
* **reportingController**: string: reportingController is the name of the controller that emitted this Event, e.g. `kubernetes.io/kubelet`. This field cannot be empty for new Events.
* **reportingInstance**: string: reportingInstance is the ID of the controller instance, e.g. `kubelet-xyzf`. This field cannot be empty for new Events and it can have at most 128 characters.
* **series**: [IoK8SApiEventsV1Beta1EventSeries](#iok8sapieventsv1beta1eventseries): EventSeries contain information on series of events, i.e. thing that was/is happening continuously for some time.
* **type**: string: type is the type of this event (Normal, Warning), new types could be added in the future. It is machine-readable.

## IoK8SApiCoreV1EventSource
### Properties
* **component**: string: Component from which the event is generated.
* **host**: string: Node name on which the event is generated.

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

## IoK8SApiCoreV1ObjectReference
### Properties
* **apiVersion**: string: API version of the referent.
* **fieldPath**: string: If referring to a piece of an object instead of an entire object, this string should contain a valid JSON/Go field access statement, such as desiredState.manifest.containers[2]. For example, if the object reference is to a container within a pod, this would take on a value like: "spec.containers{name}" (where "name" refers to the name of the container that triggered the event) or if no container name is specified "spec.containers[2]" (container with index 2 in this pod). This syntax is chosen only to have some well-defined way of referencing a part of an object.
* **kind**: string: Kind of the referent. More info: https://git.k8s.io/community/contributors/devel/sig-architecture/api-conventions.md#types-kinds
* **name**: string: Name of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/names/#names
* **namespace**: string: Namespace of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/namespaces/
* **resourceVersion**: string: Specific resourceVersion to which this reference is made, if any. More info: https://git.k8s.io/community/contributors/devel/sig-architecture/api-conventions.md#concurrency-control-and-consistency
* **uid**: string: UID of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/names/#uids

## IoK8SApiEventsV1Beta1EventSeries
### Properties
* **count**: int (Required): count is the number of occurrences in this series up to the last heartbeat time.
* **lastObservedTime**: string (Required): MicroTime is version of Time with microsecond level precision.

