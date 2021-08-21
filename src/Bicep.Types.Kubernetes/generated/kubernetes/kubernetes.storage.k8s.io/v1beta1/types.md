# kubernetes.storage.k8s.io @ v1beta1

## Resource kubernetes.storage.k8s.io/CSIDriver@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'storage.k8s.io/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'CSIDriver' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **spec**: [IoK8SApiStorageV1Beta1CSIDriverSpec](#iok8sapistoragev1beta1csidriverspec) (Required): CSIDriverSpec is the specification of a CSIDriver.

## Resource kubernetes.storage.k8s.io/CSINode@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'storage.k8s.io/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'CSINode' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **spec**: [IoK8SApiStorageV1Beta1CSINodeSpec](#iok8sapistoragev1beta1csinodespec) (Required): CSINodeSpec holds information about the specification of all CSI drivers installed on a node

## Resource kubernetes.storage.k8s.io/StorageClass@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **allowedTopologies**: [IoK8SApiCoreV1TopologySelectorTerm](#iok8sapicorev1topologyselectorterm)[]: Restrict the node topologies where volumes can be dynamically provisioned. Each volume plugin defines its own supported topology specifications. An empty TopologySelectorTerm list means there is no topology restriction. This field is only honored by servers that enable the VolumeScheduling feature.
* **allowVolumeExpansion**: bool: AllowVolumeExpansion shows whether the storage class allow volume expand
* **apiVersion**: 'storage.k8s.io/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'StorageClass' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **mountOptions**: string[]: Dynamically provisioned PersistentVolumes of this storage class are created with these mountOptions, e.g. ["ro", "soft"]. Not validated - mount of the PVs will simply fail if one is invalid.
* **parameters**: [IoK8SApiStorageV1Beta1StorageClassParameters](#iok8sapistoragev1beta1storageclassparameters): Parameters holds the parameters for the provisioner that should create volumes of this storage class.
* **provisioner**: string (Required): Provisioner indicates the type of the provisioner.
* **reclaimPolicy**: string: Dynamically provisioned PersistentVolumes of this storage class are created with this reclaimPolicy. Defaults to Delete.
* **volumeBindingMode**: string: VolumeBindingMode indicates how PersistentVolumeClaims should be provisioned and bound.  When unset, VolumeBindingImmediate is used. This field is only honored by servers that enable the VolumeScheduling feature.

## Resource kubernetes.storage.k8s.io/VolumeAttachment@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'storage.k8s.io/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'VolumeAttachment' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **spec**: [IoK8SApiStorageV1Beta1VolumeAttachmentSpec](#iok8sapistoragev1beta1volumeattachmentspec) (Required): VolumeAttachmentSpec is the specification of a VolumeAttachment request.
* **status**: [IoK8SApiStorageV1Beta1VolumeAttachmentStatus](#iok8sapistoragev1beta1volumeattachmentstatus): VolumeAttachmentStatus is the status of a VolumeAttachment request.

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

## IoK8SApiStorageV1Beta1CSIDriverSpec
### Properties
* **attachRequired**: bool: attachRequired indicates this CSI volume driver requires an attach operation (because it implements the CSI ControllerPublishVolume() method), and that the Kubernetes attach detach controller should call the attach volume interface which checks the volumeattachment status and waits until the volume is attached before proceeding to mounting. The CSI external-attacher coordinates with CSI volume driver and updates the volumeattachment status when the attach operation is complete. If the CSIDriverRegistry feature gate is enabled and the value is specified to false, the attach operation will be skipped. Otherwise the attach operation will be called.
* **fsGroupPolicy**: string: Defines if the underlying volume supports changing ownership and permission of the volume before being mounted. Refer to the specific FSGroupPolicy values for additional details. This field is alpha-level, and is only honored by servers that enable the CSIVolumeFSGroupPolicy feature gate.
* **podInfoOnMount**: bool: If set to true, podInfoOnMount indicates this CSI volume driver requires additional pod information (like podName, podUID, etc.) during mount operations. If set to false, pod information will not be passed on mount. Default is false. The CSI driver specifies podInfoOnMount as part of driver deployment. If true, Kubelet will pass pod information as VolumeContext in the CSI NodePublishVolume() calls. The CSI driver is responsible for parsing and validating the information passed in as VolumeContext. The following VolumeConext will be passed if podInfoOnMount is set to true. This list might grow, but the prefix will be used. "csi.storage.k8s.io/pod.name": pod.Name "csi.storage.k8s.io/pod.namespace": pod.Namespace "csi.storage.k8s.io/pod.uid": string(pod.UID) "csi.storage.k8s.io/ephemeral": "true" iff the volume is an ephemeral inline volume
                                defined by a CSIVolumeSource, otherwise "false"

"csi.storage.k8s.io/ephemeral" is a new feature in Kubernetes 1.16. It is only required for drivers which support both the "Persistent" and "Ephemeral" VolumeLifecycleMode. Other drivers can leave pod info disabled and/or ignore this field. As Kubernetes 1.15 doesn't support this field, drivers can only support one mode when deployed on such a cluster and the deployment determines which mode that is, for example via a command line parameter of the driver.
* **storageCapacity**: bool: If set to true, storageCapacity indicates that the CSI volume driver wants pod scheduling to consider the storage capacity that the driver deployment will report by creating CSIStorageCapacity objects with capacity information.

The check can be enabled immediately when deploying a driver. In that case, provisioning new volumes with late binding will pause until the driver deployment has published some suitable CSIStorageCapacity object.

Alternatively, the driver can be deployed with the field unset or false and it can be flipped later when storage capacity information has been published.

This is an alpha field and only available when the CSIStorageCapacity feature is enabled. The default is false.
* **volumeLifecycleModes**: string[]: VolumeLifecycleModes defines what kind of volumes this CSI volume driver supports. The default if the list is empty is "Persistent", which is the usage defined by the CSI specification and implemented in Kubernetes via the usual PV/PVC mechanism. The other mode is "Ephemeral". In this mode, volumes are defined inline inside the pod spec with CSIVolumeSource and their lifecycle is tied to the lifecycle of that pod. A driver has to be aware of this because it is only going to get a NodePublishVolume call for such a volume. For more information about implementing this mode, see https://kubernetes-csi.github.io/docs/ephemeral-local-volumes.html A driver can support one or more of these modes and more modes may be added in the future.

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

## IoK8SApiStorageV1Beta1CSINodeSpec
### Properties
* **drivers**: [IoK8SApiStorageV1Beta1CSINodeDriver](#iok8sapistoragev1beta1csinodedriver)[] (Required): drivers is a list of information of all CSI Drivers existing on a node. If all drivers in the list are uninstalled, this can become empty.

## IoK8SApiStorageV1Beta1CSINodeDriver
### Properties
* **allocatable**: [IoK8SApiStorageV1Beta1VolumeNodeResources](#iok8sapistoragev1beta1volumenoderesources): VolumeNodeResources is a set of resource limits for scheduling of volumes.
* **name**: string (Required): This is the name of the CSI driver that this object refers to. This MUST be the same name returned by the CSI GetPluginName() call for that driver.
* **nodeID**: string (Required): nodeID of the node from the driver point of view. This field enables Kubernetes to communicate with storage systems that do not share the same nomenclature for nodes. For example, Kubernetes may refer to a given node as "node1", but the storage system may refer to the same node as "nodeA". When Kubernetes issues a command to the storage system to attach a volume to a specific node, it can use this field to refer to the node name using the ID that the storage system will understand, e.g. "nodeA" instead of "node1". This field is required.
* **topologyKeys**: string[]: topologyKeys is the list of keys supported by the driver. When a driver is initialized on a cluster, it provides a set of topology keys that it understands (e.g. "company.com/zone", "company.com/region"). When a driver is initialized on a node, it provides the same topology keys along with values. Kubelet will expose these topology keys as labels on its own node object. When Kubernetes does topology aware provisioning, it can use this list to determine which labels it should retrieve from the node object and pass back to the driver. It is possible for different nodes to use different topology keys. This can be empty if driver does not support topology.

## IoK8SApiStorageV1Beta1VolumeNodeResources
### Properties
* **count**: int: Maximum number of unique volumes managed by the CSI driver that can be used on a node. A volume that is both attached and mounted on a node is considered to be used once, not twice. The same rule applies for a unique volume that is shared among multiple pods on the same node. If this field is nil, then the supported number of volumes on this node is unbounded.

## IoK8SApiCoreV1TopologySelectorTerm
### Properties
* **matchLabelExpressions**: [IoK8SApiCoreV1TopologySelectorLabelRequirement](#iok8sapicorev1topologyselectorlabelrequirement)[]: A list of topology selector requirements by labels.

## IoK8SApiCoreV1TopologySelectorLabelRequirement
### Properties
* **key**: string (Required): The label key that the selector applies to.
* **values**: string[] (Required): An array of string values. One value must match the label to be selected. Each entry in Values is ORed.

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

## IoK8SApiStorageV1Beta1StorageClassParameters
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

## IoK8SApiStorageV1Beta1VolumeAttachmentSpec
### Properties
* **attacher**: string (Required): Attacher indicates the name of the volume driver that MUST handle this request. This is the name returned by GetPluginName().
* **nodeName**: string (Required): The node that the volume should be attached to.
* **source**: [IoK8SApiStorageV1Beta1VolumeAttachmentSource](#iok8sapistoragev1beta1volumeattachmentsource) (Required): VolumeAttachmentSource represents a volume that should be attached. Right now only PersistenVolumes can be attached via external attacher, in future we may allow also inline volumes in pods. Exactly one member can be set.

## IoK8SApiStorageV1Beta1VolumeAttachmentSource
### Properties
* **inlineVolumeSpec**: [IoK8SApiCoreV1PersistentVolumeSpec](#iok8sapicorev1persistentvolumespec): PersistentVolumeSpec is the specification of a persistent volume.
* **persistentVolumeName**: string: Name of the persistent volume to attach.

## IoK8SApiCoreV1PersistentVolumeSpec
### Properties
* **accessModes**: string[]: AccessModes contains all ways the volume can be mounted. More info: https://kubernetes.io/docs/concepts/storage/persistent-volumes#access-modes
* **awsElasticBlockStore**: [IoK8SApiCoreV1AWSElasticBlockStoreVolumeSource](#iok8sapicorev1awselasticblockstorevolumesource): Represents a Persistent Disk resource in AWS.

An AWS EBS disk must exist before mounting to a container. The disk must also be in the same AWS zone as the kubelet. An AWS EBS disk can only be mounted as read/write once. AWS EBS volumes support ownership management and SELinux relabeling.
* **azureDisk**: [IoK8SApiCoreV1AzureDiskVolumeSource](#iok8sapicorev1azurediskvolumesource): AzureDisk represents an Azure Data Disk mount on the host and bind mount to the pod.
* **azureFile**: [IoK8SApiCoreV1AzureFilePersistentVolumeSource](#iok8sapicorev1azurefilepersistentvolumesource): AzureFile represents an Azure File Service mount on the host and bind mount to the pod.
* **capacity**: [IoK8SApiCoreV1PersistentVolumeSpecCapacity](#iok8sapicorev1persistentvolumespeccapacity): A description of the persistent volume's resources and capacity. More info: https://kubernetes.io/docs/concepts/storage/persistent-volumes#capacity
* **cephfs**: [IoK8SApiCoreV1CephFSPersistentVolumeSource](#iok8sapicorev1cephfspersistentvolumesource): Represents a Ceph Filesystem mount that lasts the lifetime of a pod Cephfs volumes do not support ownership management or SELinux relabeling.
* **cinder**: [IoK8SApiCoreV1CinderPersistentVolumeSource](#iok8sapicorev1cinderpersistentvolumesource): Represents a cinder volume resource in Openstack. A Cinder volume must exist before mounting to a container. The volume must also be in the same region as the kubelet. Cinder volumes support ownership management and SELinux relabeling.
* **claimRef**: [IoK8SApiCoreV1ObjectReference](#iok8sapicorev1objectreference): ObjectReference contains enough information to let you inspect or modify the referred object.
* **csi**: [IoK8SApiCoreV1CSIPersistentVolumeSource](#iok8sapicorev1csipersistentvolumesource): Represents storage that is managed by an external CSI volume driver (Beta feature)
* **fc**: [IoK8SApiCoreV1FCVolumeSource](#iok8sapicorev1fcvolumesource): Represents a Fibre Channel volume. Fibre Channel volumes can only be mounted as read/write once. Fibre Channel volumes support ownership management and SELinux relabeling.
* **flexVolume**: [IoK8SApiCoreV1FlexPersistentVolumeSource](#iok8sapicorev1flexpersistentvolumesource): FlexPersistentVolumeSource represents a generic persistent volume resource that is provisioned/attached using an exec based plugin.
* **flocker**: [IoK8SApiCoreV1FlockerVolumeSource](#iok8sapicorev1flockervolumesource): Represents a Flocker volume mounted by the Flocker agent. One and only one of datasetName and datasetUUID should be set. Flocker volumes do not support ownership management or SELinux relabeling.
* **gcePersistentDisk**: [IoK8SApiCoreV1GCEPersistentDiskVolumeSource](#iok8sapicorev1gcepersistentdiskvolumesource): Represents a Persistent Disk resource in Google Compute Engine.

A GCE PD must exist before mounting to a container. The disk must also be in the same GCE project and zone as the kubelet. A GCE PD can only be mounted as read/write once or read-only many times. GCE PDs support ownership management and SELinux relabeling.
* **glusterfs**: [IoK8SApiCoreV1GlusterfsPersistentVolumeSource](#iok8sapicorev1glusterfspersistentvolumesource): Represents a Glusterfs mount that lasts the lifetime of a pod. Glusterfs volumes do not support ownership management or SELinux relabeling.
* **hostPath**: [IoK8SApiCoreV1HostPathVolumeSource](#iok8sapicorev1hostpathvolumesource): Represents a host path mapped into a pod. Host path volumes do not support ownership management or SELinux relabeling.
* **iscsi**: [IoK8SApiCoreV1IscsiPersistentVolumeSource](#iok8sapicorev1iscsipersistentvolumesource): ISCSIPersistentVolumeSource represents an ISCSI disk. ISCSI volumes can only be mounted as read/write once. ISCSI volumes support ownership management and SELinux relabeling.
* **local**: [IoK8SApiCoreV1LocalVolumeSource](#iok8sapicorev1localvolumesource): Local represents directly-attached storage with node affinity (Beta feature)
* **mountOptions**: string[]: A list of mount options, e.g. ["ro", "soft"]. Not validated - mount will simply fail if one is invalid. More info: https://kubernetes.io/docs/concepts/storage/persistent-volumes/#mount-options
* **nfs**: [IoK8SApiCoreV1NFSVolumeSource](#iok8sapicorev1nfsvolumesource): Represents an NFS mount that lasts the lifetime of a pod. NFS volumes do not support ownership management or SELinux relabeling.
* **nodeAffinity**: [IoK8SApiCoreV1VolumeNodeAffinity](#iok8sapicorev1volumenodeaffinity): VolumeNodeAffinity defines constraints that limit what nodes this volume can be accessed from.
* **persistentVolumeReclaimPolicy**: string: What happens to a persistent volume when released from its claim. Valid options are Retain (default for manually created PersistentVolumes), Delete (default for dynamically provisioned PersistentVolumes), and Recycle (deprecated). Recycle must be supported by the volume plugin underlying this PersistentVolume. More info: https://kubernetes.io/docs/concepts/storage/persistent-volumes#reclaiming
* **photonPersistentDisk**: [IoK8SApiCoreV1PhotonPersistentDiskVolumeSource](#iok8sapicorev1photonpersistentdiskvolumesource): Represents a Photon Controller persistent disk resource.
* **portworxVolume**: [IoK8SApiCoreV1PortworxVolumeSource](#iok8sapicorev1portworxvolumesource): PortworxVolumeSource represents a Portworx volume resource.
* **quobyte**: [IoK8SApiCoreV1QuobyteVolumeSource](#iok8sapicorev1quobytevolumesource): Represents a Quobyte mount that lasts the lifetime of a pod. Quobyte volumes do not support ownership management or SELinux relabeling.
* **rbd**: [IoK8SApiCoreV1RBDPersistentVolumeSource](#iok8sapicorev1rbdpersistentvolumesource): Represents a Rados Block Device mount that lasts the lifetime of a pod. RBD volumes support ownership management and SELinux relabeling.
* **scaleIO**: [IoK8SApiCoreV1ScaleIOPersistentVolumeSource](#iok8sapicorev1scaleiopersistentvolumesource): ScaleIOPersistentVolumeSource represents a persistent ScaleIO volume
* **storageClassName**: string: Name of StorageClass to which this persistent volume belongs. Empty value means that this volume does not belong to any StorageClass.
* **storageos**: [IoK8SApiCoreV1StorageOSPersistentVolumeSource](#iok8sapicorev1storageospersistentvolumesource): Represents a StorageOS persistent volume resource.
* **volumeMode**: string: volumeMode defines if a volume is intended to be used with a formatted filesystem or to remain in raw block state. Value of Filesystem is implied when not included in spec.
* **vsphereVolume**: [IoK8SApiCoreV1VsphereVirtualDiskVolumeSource](#iok8sapicorev1vspherevirtualdiskvolumesource): Represents a vSphere volume resource.

## IoK8SApiCoreV1AWSElasticBlockStoreVolumeSource
### Properties
* **fsType**: string: Filesystem type of the volume that you want to mount. Tip: Ensure that the filesystem type is supported by the host operating system. Examples: "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified. More info: https://kubernetes.io/docs/concepts/storage/volumes#awselasticblockstore
* **partition**: int: The partition in the volume that you want to mount. If omitted, the default is to mount by volume name. Examples: For volume /dev/sda1, you specify the partition as "1". Similarly, the volume partition for /dev/sda is "0" (or you can leave the property empty).
* **readOnly**: bool: Specify "true" to force and set the ReadOnly property in VolumeMounts to "true". If omitted, the default is "false". More info: https://kubernetes.io/docs/concepts/storage/volumes#awselasticblockstore
* **volumeID**: string (Required): Unique ID of the persistent disk resource in AWS (Amazon EBS volume). More info: https://kubernetes.io/docs/concepts/storage/volumes#awselasticblockstore

## IoK8SApiCoreV1AzureDiskVolumeSource
### Properties
* **cachingMode**: string: Host Caching mode: None, Read Only, Read Write.
* **diskName**: string (Required): The Name of the data disk in the blob storage
* **diskURI**: string (Required): The URI the data disk in the blob storage
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified.
* **kind**: string: Expected values Shared: multiple blob disks per storage account  Dedicated: single blob disk per storage account  Managed: azure managed data disk (only in managed availability set). defaults to shared
* **readOnly**: bool: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts.

## IoK8SApiCoreV1AzureFilePersistentVolumeSource
### Properties
* **readOnly**: bool: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts.
* **secretName**: string (Required): the name of secret that contains Azure Storage Account Name and Key
* **secretNamespace**: string: the namespace of the secret that contains Azure Storage Account Name and Key default is the same as the Pod
* **shareName**: string (Required): Share Name

## IoK8SApiCoreV1PersistentVolumeSpecCapacity
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoK8SApiCoreV1CephFSPersistentVolumeSource
### Properties
* **monitors**: string[] (Required): Required: Monitors is a collection of Ceph monitors More info: https://examples.k8s.io/volumes/cephfs/README.md#how-to-use-it
* **path**: string: Optional: Used as the mounted root, rather than the full Ceph tree, default is /
* **readOnly**: bool: Optional: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts. More info: https://examples.k8s.io/volumes/cephfs/README.md#how-to-use-it
* **secretFile**: string: Optional: SecretFile is the path to key ring for User, default is /etc/ceph/user.secret More info: https://examples.k8s.io/volumes/cephfs/README.md#how-to-use-it
* **secretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **user**: string: Optional: User is the rados user name, default is admin More info: https://examples.k8s.io/volumes/cephfs/README.md#how-to-use-it

## IoK8SApiCoreV1SecretReference
### Properties
* **name**: string: Name is unique within a namespace to reference a secret resource.
* **namespace**: string: Namespace defines the space within which the secret name must be unique.

## IoK8SApiCoreV1CinderPersistentVolumeSource
### Properties
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Examples: "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified. More info: https://examples.k8s.io/mysql-cinder-pd/README.md
* **readOnly**: bool: Optional: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts. More info: https://examples.k8s.io/mysql-cinder-pd/README.md
* **secretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **volumeID**: string (Required): volume id used to identify the volume in cinder. More info: https://examples.k8s.io/mysql-cinder-pd/README.md

## IoK8SApiCoreV1ObjectReference
### Properties
* **apiVersion**: string: API version of the referent.
* **fieldPath**: string: If referring to a piece of an object instead of an entire object, this string should contain a valid JSON/Go field access statement, such as desiredState.manifest.containers[2]. For example, if the object reference is to a container within a pod, this would take on a value like: "spec.containers{name}" (where "name" refers to the name of the container that triggered the event) or if no container name is specified "spec.containers[2]" (container with index 2 in this pod). This syntax is chosen only to have some well-defined way of referencing a part of an object.
* **kind**: string: Kind of the referent. More info: https://git.k8s.io/community/contributors/devel/sig-architecture/api-conventions.md#types-kinds
* **name**: string: Name of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/names/#names
* **namespace**: string: Namespace of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/namespaces/
* **resourceVersion**: string: Specific resourceVersion to which this reference is made, if any. More info: https://git.k8s.io/community/contributors/devel/sig-architecture/api-conventions.md#concurrency-control-and-consistency
* **uid**: string: UID of the referent. More info: https://kubernetes.io/docs/concepts/overview/working-with-objects/names/#uids

## IoK8SApiCoreV1CSIPersistentVolumeSource
### Properties
* **controllerExpandSecretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **controllerPublishSecretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **driver**: string (Required): Driver is the name of the driver to use for this volume. Required.
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs".
* **nodePublishSecretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **nodeStageSecretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **readOnly**: bool: Optional: The value to pass to ControllerPublishVolumeRequest. Defaults to false (read/write).
* **volumeAttributes**: [IoK8SApiCoreV1CSIPersistentVolumeSourceVolumeAttributes](#iok8sapicorev1csipersistentvolumesourcevolumeattributes): Attributes of the volume to publish.
* **volumeHandle**: string (Required): VolumeHandle is the unique volume name returned by the CSI volume plugin’s CreateVolume to refer to the volume on all subsequent calls. Required.

## IoK8SApiCoreV1CSIPersistentVolumeSourceVolumeAttributes
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoK8SApiCoreV1FCVolumeSource
### Properties
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified.
* **lun**: int: Optional: FC target lun number
* **readOnly**: bool: Optional: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts.
* **targetWWNs**: string[]: Optional: FC target worldwide names (WWNs)
* **wwids**: string[]: Optional: FC volume world wide identifiers (wwids) Either wwids or combination of targetWWNs and lun must be set, but not both simultaneously.

## IoK8SApiCoreV1FlexPersistentVolumeSource
### Properties
* **driver**: string (Required): Driver is the name of the driver to use for this volume.
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs". The default filesystem depends on FlexVolume script.
* **options**: [IoK8SApiCoreV1FlexPersistentVolumeSourceOptions](#iok8sapicorev1flexpersistentvolumesourceoptions): Optional: Extra command options if any.
* **readOnly**: bool: Optional: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts.
* **secretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace

## IoK8SApiCoreV1FlexPersistentVolumeSourceOptions
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoK8SApiCoreV1FlockerVolumeSource
### Properties
* **datasetName**: string: Name of the dataset stored as metadata -> name on the dataset for Flocker should be considered as deprecated
* **datasetUUID**: string: UUID of the dataset. This is unique identifier of a Flocker dataset

## IoK8SApiCoreV1GCEPersistentDiskVolumeSource
### Properties
* **fsType**: string: Filesystem type of the volume that you want to mount. Tip: Ensure that the filesystem type is supported by the host operating system. Examples: "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified. More info: https://kubernetes.io/docs/concepts/storage/volumes#gcepersistentdisk
* **partition**: int: The partition in the volume that you want to mount. If omitted, the default is to mount by volume name. Examples: For volume /dev/sda1, you specify the partition as "1". Similarly, the volume partition for /dev/sda is "0" (or you can leave the property empty). More info: https://kubernetes.io/docs/concepts/storage/volumes#gcepersistentdisk
* **pdName**: string (Required): Unique name of the PD resource in GCE. Used to identify the disk in GCE. More info: https://kubernetes.io/docs/concepts/storage/volumes#gcepersistentdisk
* **readOnly**: bool: ReadOnly here will force the ReadOnly setting in VolumeMounts. Defaults to false. More info: https://kubernetes.io/docs/concepts/storage/volumes#gcepersistentdisk

## IoK8SApiCoreV1GlusterfsPersistentVolumeSource
### Properties
* **endpoints**: string (Required): EndpointsName is the endpoint name that details Glusterfs topology. More info: https://examples.k8s.io/volumes/glusterfs/README.md#create-a-pod
* **endpointsNamespace**: string: EndpointsNamespace is the namespace that contains Glusterfs endpoint. If this field is empty, the EndpointNamespace defaults to the same namespace as the bound PVC. More info: https://examples.k8s.io/volumes/glusterfs/README.md#create-a-pod
* **path**: string (Required): Path is the Glusterfs volume path. More info: https://examples.k8s.io/volumes/glusterfs/README.md#create-a-pod
* **readOnly**: bool: ReadOnly here will force the Glusterfs volume to be mounted with read-only permissions. Defaults to false. More info: https://examples.k8s.io/volumes/glusterfs/README.md#create-a-pod

## IoK8SApiCoreV1HostPathVolumeSource
### Properties
* **path**: string (Required): Path of the directory on the host. If the path is a symlink, it will follow the link to the real path. More info: https://kubernetes.io/docs/concepts/storage/volumes#hostpath
* **type**: string: Type for HostPath Volume Defaults to "" More info: https://kubernetes.io/docs/concepts/storage/volumes#hostpath

## IoK8SApiCoreV1IscsiPersistentVolumeSource
### Properties
* **chapAuthDiscovery**: bool: whether support iSCSI Discovery CHAP authentication
* **chapAuthSession**: bool: whether support iSCSI Session CHAP authentication
* **fsType**: string: Filesystem type of the volume that you want to mount. Tip: Ensure that the filesystem type is supported by the host operating system. Examples: "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified. More info: https://kubernetes.io/docs/concepts/storage/volumes#iscsi
* **initiatorName**: string: Custom iSCSI Initiator Name. If initiatorName is specified with iscsiInterface simultaneously, new iSCSI interface <target portal>:<volume name> will be created for the connection.
* **iqn**: string (Required): Target iSCSI Qualified Name.
* **iscsiInterface**: string: iSCSI Interface Name that uses an iSCSI transport. Defaults to 'default' (tcp).
* **lun**: int (Required): iSCSI Target Lun number.
* **portals**: string[]: iSCSI Target Portal List. The Portal is either an IP or ip_addr:port if the port is other than default (typically TCP ports 860 and 3260).
* **readOnly**: bool: ReadOnly here will force the ReadOnly setting in VolumeMounts. Defaults to false.
* **secretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **targetPortal**: string (Required): iSCSI Target Portal. The Portal is either an IP or ip_addr:port if the port is other than default (typically TCP ports 860 and 3260).

## IoK8SApiCoreV1LocalVolumeSource
### Properties
* **fsType**: string: Filesystem type to mount. It applies only when the Path is a block device. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs". The default value is to auto-select a fileystem if unspecified.
* **path**: string (Required): The full path to the volume on the node. It can be either a directory or block device (disk, partition, ...).

## IoK8SApiCoreV1NFSVolumeSource
### Properties
* **path**: string (Required): Path that is exported by the NFS server. More info: https://kubernetes.io/docs/concepts/storage/volumes#nfs
* **readOnly**: bool: ReadOnly here will force the NFS export to be mounted with read-only permissions. Defaults to false. More info: https://kubernetes.io/docs/concepts/storage/volumes#nfs
* **server**: string (Required): Server is the hostname or IP address of the NFS server. More info: https://kubernetes.io/docs/concepts/storage/volumes#nfs

## IoK8SApiCoreV1VolumeNodeAffinity
### Properties
* **required**: [IoK8SApiCoreV1NodeSelector](#iok8sapicorev1nodeselector): A node selector represents the union of the results of one or more label queries over a set of nodes; that is, it represents the OR of the selectors represented by the node selector terms.

## IoK8SApiCoreV1NodeSelector
### Properties
* **nodeSelectorTerms**: [IoK8SApiCoreV1NodeSelectorTerm](#iok8sapicorev1nodeselectorterm)[] (Required): Required. A list of node selector terms. The terms are ORed.

## IoK8SApiCoreV1NodeSelectorTerm
### Properties
* **matchExpressions**: [IoK8SApiCoreV1NodeSelectorRequirement](#iok8sapicorev1nodeselectorrequirement)[]: A list of node selector requirements by node's labels.
* **matchFields**: [IoK8SApiCoreV1NodeSelectorRequirement](#iok8sapicorev1nodeselectorrequirement)[]: A list of node selector requirements by node's fields.

## IoK8SApiCoreV1NodeSelectorRequirement
### Properties
* **key**: string (Required): The label key that the selector applies to.
* **operator**: string (Required): Represents a key's relationship to a set of values. Valid operators are In, NotIn, Exists, DoesNotExist. Gt, and Lt.
* **values**: string[]: An array of string values. If the operator is In or NotIn, the values array must be non-empty. If the operator is Exists or DoesNotExist, the values array must be empty. If the operator is Gt or Lt, the values array must have a single element, which will be interpreted as an integer. This array is replaced during a strategic merge patch.

## IoK8SApiCoreV1PhotonPersistentDiskVolumeSource
### Properties
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified.
* **pdID**: string (Required): ID that identifies Photon Controller persistent disk

## IoK8SApiCoreV1PortworxVolumeSource
### Properties
* **fsType**: string: FSType represents the filesystem type to mount Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs". Implicitly inferred to be "ext4" if unspecified.
* **readOnly**: bool: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts.
* **volumeID**: string (Required): VolumeID uniquely identifies a Portworx volume

## IoK8SApiCoreV1QuobyteVolumeSource
### Properties
* **group**: string: Group to map volume access to Default is no group
* **readOnly**: bool: ReadOnly here will force the Quobyte volume to be mounted with read-only permissions. Defaults to false.
* **registry**: string (Required): Registry represents a single or multiple Quobyte Registry services specified as a string as host:port pair (multiple entries are separated with commas) which acts as the central registry for volumes
* **tenant**: string: Tenant owning the given Quobyte volume in the Backend Used with dynamically provisioned Quobyte volumes, value is set by the plugin
* **user**: string: User to map volume access to Defaults to serivceaccount user
* **volume**: string (Required): Volume is a string that references an already created Quobyte volume by name.

## IoK8SApiCoreV1RBDPersistentVolumeSource
### Properties
* **fsType**: string: Filesystem type of the volume that you want to mount. Tip: Ensure that the filesystem type is supported by the host operating system. Examples: "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified. More info: https://kubernetes.io/docs/concepts/storage/volumes#rbd
* **image**: string (Required): The rados image name. More info: https://examples.k8s.io/volumes/rbd/README.md#how-to-use-it
* **keyring**: string: Keyring is the path to key ring for RBDUser. Default is /etc/ceph/keyring. More info: https://examples.k8s.io/volumes/rbd/README.md#how-to-use-it
* **monitors**: string[] (Required): A collection of Ceph monitors. More info: https://examples.k8s.io/volumes/rbd/README.md#how-to-use-it
* **pool**: string: The rados pool name. Default is rbd. More info: https://examples.k8s.io/volumes/rbd/README.md#how-to-use-it
* **readOnly**: bool: ReadOnly here will force the ReadOnly setting in VolumeMounts. Defaults to false. More info: https://examples.k8s.io/volumes/rbd/README.md#how-to-use-it
* **secretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **user**: string: The rados user name. Default is admin. More info: https://examples.k8s.io/volumes/rbd/README.md#how-to-use-it

## IoK8SApiCoreV1ScaleIOPersistentVolumeSource
### Properties
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs". Default is "xfs"
* **gateway**: string (Required): The host address of the ScaleIO API Gateway.
* **protectionDomain**: string: The name of the ScaleIO Protection Domain for the configured storage.
* **readOnly**: bool: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts.
* **secretRef**: [IoK8SApiCoreV1SecretReference](#iok8sapicorev1secretreference) (Required): SecretReference represents a Secret Reference. It has enough information to retrieve secret in any namespace
* **sslEnabled**: bool: Flag to enable/disable SSL communication with Gateway, default false
* **storageMode**: string: Indicates whether the storage for a volume should be ThickProvisioned or ThinProvisioned. Default is ThinProvisioned.
* **storagePool**: string: The ScaleIO Storage Pool associated with the protection domain.
* **system**: string (Required): The name of the storage system as configured in ScaleIO.
* **volumeName**: string: The name of a volume already created in the ScaleIO system that is associated with this volume source.

## IoK8SApiCoreV1StorageOSPersistentVolumeSource
### Properties
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified.
* **readOnly**: bool: Defaults to false (read/write). ReadOnly here will force the ReadOnly setting in VolumeMounts.
* **secretRef**: [IoK8SApiCoreV1ObjectReference](#iok8sapicorev1objectreference): ObjectReference contains enough information to let you inspect or modify the referred object.
* **volumeName**: string: VolumeName is the human-readable name of the StorageOS volume.  Volume names are only unique within a namespace.
* **volumeNamespace**: string: VolumeNamespace specifies the scope of the volume within StorageOS.  If no namespace is specified then the Pod's namespace will be used.  This allows the Kubernetes name scoping to be mirrored within StorageOS for tighter integration. Set VolumeName to any name to override the default behaviour. Set to "default" if you are not using namespaces within StorageOS. Namespaces that do not pre-exist within StorageOS will be created.

## IoK8SApiCoreV1VsphereVirtualDiskVolumeSource
### Properties
* **fsType**: string: Filesystem type to mount. Must be a filesystem type supported by the host operating system. Ex. "ext4", "xfs", "ntfs". Implicitly inferred to be "ext4" if unspecified.
* **storagePolicyID**: string: Storage Policy Based Management (SPBM) profile ID associated with the StoragePolicyName.
* **storagePolicyName**: string: Storage Policy Based Management (SPBM) profile name.
* **volumePath**: string (Required): Path that identifies vSphere volume vmdk

## IoK8SApiStorageV1Beta1VolumeAttachmentStatus
### Properties
* **attached**: bool (Required): Indicates the volume is successfully attached. This field must only be set by the entity completing the attach operation, i.e. the external-attacher.
* **attachError**: [IoK8SApiStorageV1Beta1VolumeError](#iok8sapistoragev1beta1volumeerror): VolumeError captures an error encountered during a volume operation.
* **attachmentMetadata**: [IoK8SApiStorageV1Beta1VolumeAttachmentStatusAttachmentMetadata](#iok8sapistoragev1beta1volumeattachmentstatusattachmentmetadata): Upon successful attach, this field is populated with any information returned by the attach operation that must be passed into subsequent WaitForAttach or Mount calls. This field must only be set by the entity completing the attach operation, i.e. the external-attacher.
* **detachError**: [IoK8SApiStorageV1Beta1VolumeError](#iok8sapistoragev1beta1volumeerror): VolumeError captures an error encountered during a volume operation.

## IoK8SApiStorageV1Beta1VolumeError
### Properties
* **message**: string: String detailing the error encountered during Attach or Detach operation. This string may be logged, so it should not contain sensitive information.
* **time**: string: Time is a wrapper around time.Time which supports correct marshaling to YAML and JSON.  Wrappers are provided for many of the factory methods that the time package offers.

## IoK8SApiStorageV1Beta1VolumeAttachmentStatusAttachmentMetadata
### Properties
### Additional Properties
* **Additional Properties Type**: string

