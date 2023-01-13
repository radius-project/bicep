# AWS.SageMaker @ default

## Resource AWS.SageMaker/AppImageConfig@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/AppImageConfigProperties](#awssagemakerappimageconfigproperties) (Required): properties of the resource

## Resource AWS.SageMaker/Device@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/DeviceProperties](#awssagemakerdeviceproperties) (Required): properties of the resource

## Resource AWS.SageMaker/DeviceFleet@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/DeviceFleetProperties](#awssagemakerdevicefleetproperties) (Required): properties of the resource

## Resource AWS.SageMaker/Domain@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/DomainProperties](#awssagemakerdomainproperties) (Required): properties of the resource

## Resource AWS.SageMaker/Image@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/ImageProperties](#awssagemakerimageproperties) (Required): properties of the resource

## Resource AWS.SageMaker/ModelPackage@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/ModelPackageProperties](#awssagemakermodelpackageproperties): properties of the resource

## Resource AWS.SageMaker/ModelPackageGroup@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/ModelPackageGroupProperties](#awssagemakermodelpackagegroupproperties) (Required): properties of the resource

## Resource AWS.SageMaker/MonitoringSchedule@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/MonitoringScheduleProperties](#awssagemakermonitoringscheduleproperties) (Required): properties of the resource

## Resource AWS.SageMaker/Pipeline@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/PipelineProperties](#awssagemakerpipelineproperties) (Required): properties of the resource

## Resource AWS.SageMaker/Project@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/ProjectProperties](#awssagemakerprojectproperties) (Required): properties of the resource

## Resource AWS.SageMaker/UserProfile@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.SageMaker/UserProfileProperties](#awssagemakeruserprofileproperties) (Required): properties of the resource

## AWS.SageMaker/AppImageConfigProperties
### Properties
* **AppImageConfigArn**: string (ReadOnly): The Amazon Resource Name (ARN) of the AppImageConfig.
* **AppImageConfigName**: string (Required, Identifier): The Name of the AppImageConfig.
* **KernelGatewayImageConfig**: [KernelGatewayImageConfig](#kernelgatewayimageconfig): The KernelGatewayImageConfig.
* **Tags**: [Tag](#tag)[] (WriteOnly): A list of tags to apply to the AppImageConfig.

## KernelGatewayImageConfig
### Properties
* **FileSystemConfig**: [FileSystemConfig](#filesystemconfig): The Amazon Elastic File System (EFS) storage configuration for a SageMaker image.
* **KernelSpecs**: [KernelSpec](#kernelspec)[] (Required): The specification of the Jupyter kernels in the image.

## FileSystemConfig
### Properties
* **DefaultGid**: int: The default POSIX group ID (GID). If not specified, defaults to 100.
* **DefaultUid**: int: The default POSIX user ID (UID). If not specified, defaults to 1000.
* **MountPath**: string: The path within the image to mount the user's EFS home directory. The directory should be empty. If not specified, defaults to /home/sagemaker-user.

## KernelSpec
### Properties
* **DisplayName**: string: The display name of the kernel.
* **Name**: string (Required): The name of the kernel.

## Tag
### Properties
* **Key**: string (Required)
* **Value**: string (Required)

## AWS.SageMaker/DeviceProperties
### Properties
* **Device**: [Device](#device): The Edge Device you want to register against a device fleet
* **DeviceFleetName**: string (Required): The name of the edge device fleet
* **Tags**: [Tag](#tag)[]: Associate tags with the resource

## Device
### Properties
* **Description**: string: Description of the device
* **DeviceName**: string (Required): The name of the device
* **IotThingName**: string: AWS Internet of Things (IoT) object name.

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 
* **Value**: string (Required): The key value of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 

## AWS.SageMaker/DeviceFleetProperties
### Properties
* **Description**: string: Description for the edge device fleet
* **DeviceFleetName**: string (Required, Identifier): The name of the edge device fleet
* **OutputConfig**: [EdgeOutputConfig](#edgeoutputconfig) (Required): S3 bucket and an ecryption key id (if available) to store outputs for the fleet
* **RoleArn**: string (Required): Role associated with the device fleet
* **Tags**: [Tag](#tag)[]: Associate tags with the resource

## EdgeOutputConfig
### Properties
* **KmsKeyId**: string: The KMS key id used for encryption on the S3 bucket
* **S3OutputLocation**: string (Required): The Amazon Simple Storage (S3) bucket URI

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 
* **Value**: string (Required): The key value of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 

## AWS.SageMaker/DomainProperties
### Properties
* **AppNetworkAccessType**: string: Specifies the VPC used for non-EFS traffic. The default value is PublicInternetOnly.
* **AppSecurityGroupManagement**: string: The entity that creates and manages the required security groups for inter-app communication in VPCOnly mode. Required when CreateDomain.AppNetworkAccessType is VPCOnly and DomainSettings.RStudioServerProDomainSettings.DomainExecutionRoleArn is provided.
* **AuthMode**: string (Required): The mode of authentication that members use to access the domain.
* **DefaultUserSettings**: [UserSettings](#usersettings) (Required): The default user settings.
* **DomainArn**: string (ReadOnly): The Amazon Resource Name (ARN) of the created domain.
* **DomainId**: string (ReadOnly, Identifier): The domain name.
* **DomainName**: string (Required): A name for the domain.
* **DomainSettings**: [DomainSettings](#domainsettings)
* **HomeEfsFileSystemId**: string (ReadOnly): The ID of the Amazon Elastic File System (EFS) managed by this Domain.
* **KmsKeyId**: string: SageMaker uses AWS KMS to encrypt the EFS volume attached to the domain with an AWS managed customer master key (CMK) by default.
* **SecurityGroupIdForDomainBoundary**: string (ReadOnly): The ID of the security group that authorizes traffic between the RSessionGateway apps and the RStudioServerPro app.
* **SingleSignOnManagedApplicationInstanceId**: string (ReadOnly): The SSO managed application instance ID.
* **SubnetIds**: string[] (Required): The VPC subnets that Studio uses for communication.
* **Tags**: [Tag](#tag)[] (WriteOnly): A list of tags to apply to the user profile.
* **Url**: string (ReadOnly): The URL to the created domain.
* **VpcId**: string (Required): The ID of the Amazon Virtual Private Cloud (VPC) that Studio uses for communication.

## UserSettings
### Properties
* **ExecutionRole**: string: The user profile Amazon Resource Name (ARN).
* **JupyterServerAppSettings**: [JupyterServerAppSettings](#jupyterserverappsettings): The Jupyter server's app settings.
* **KernelGatewayAppSettings**: [KernelGatewayAppSettings](#kernelgatewayappsettings): The kernel gateway app settings.
* **RSessionAppSettings**: [RSessionAppSettings](#rsessionappsettings)
* **RStudioServerProAppSettings**: [RStudioServerProAppSettings](#rstudioserverproappsettings)
* **SecurityGroups**: string[]: The security groups for the Amazon Virtual Private Cloud (VPC) that Studio uses for communication.
* **SharingSettings**: [SharingSettings](#sharingsettings): The sharing settings.

## JupyterServerAppSettings
### Properties
* **DefaultResourceSpec**: [ResourceSpec](#resourcespec)

## ResourceSpec
### Properties
* **InstanceType**: string: The instance type that the image version runs on.
* **LifecycleConfigArn**: string: The Amazon Resource Name (ARN) of the Lifecycle Configuration to attach to the Resource.
* **SageMakerImageArn**: string: The Amazon Resource Name (ARN) of the SageMaker image that the image version belongs to.
* **SageMakerImageVersionArn**: string: The Amazon Resource Name (ARN) of the image version created on the instance.

## KernelGatewayAppSettings
### Properties
* **CustomImages**: [CustomImage](#customimage)[]: A list of custom SageMaker images that are configured to run as a KernelGateway app.
* **DefaultResourceSpec**: [ResourceSpec](#resourcespec): The default instance type and the Amazon Resource Name (ARN) of the default SageMaker image used by the KernelGateway app.

## CustomImage
### Properties
* **AppImageConfigName**: string (Required): The Name of the AppImageConfig.
* **ImageName**: string (Required): The name of the CustomImage. Must be unique to your account.
* **ImageVersionNumber**: int: The version number of the CustomImage.

## RSessionAppSettings
### Properties
* **CustomImages**: [CustomImage](#customimage)[]: A list of custom SageMaker images that are configured to run as a KernelGateway app.
* **DefaultResourceSpec**: [ResourceSpec](#resourcespec)

## RStudioServerProAppSettings
### Properties
* **AccessStatus**: string: Indicates whether the current user has access to the RStudioServerPro app.
* **UserGroup**: string: The level of permissions that the user has within the RStudioServerPro app. This value defaults to User. The Admin value allows the user access to the RStudio Administrative Dashboard.

## SharingSettings
### Properties
* **NotebookOutputOption**: string: Whether to include the notebook cell output when sharing the notebook. The default is Disabled.
* **S3KmsKeyId**: string: When NotebookOutputOption is Allowed, the AWS Key Management Service (KMS) encryption key ID used to encrypt the notebook cell output in the Amazon S3 bucket.
* **S3OutputPath**: string: When NotebookOutputOption is Allowed, the Amazon S3 bucket used to store the shared notebook snapshots.

## DomainSettings
### Properties
* **RStudioServerProDomainSettings**: [RStudioServerProDomainSettings](#rstudioserverprodomainsettings)
* **SecurityGroupIds**: string[]: The security groups for the Amazon Virtual Private Cloud that the Domain uses for communication between Domain-level apps and user apps.

## RStudioServerProDomainSettings
### Properties
* **DefaultResourceSpec**: [ResourceSpec](#resourcespec)
* **DomainExecutionRoleArn**: string (Required): The ARN of the execution role for the RStudioServerPro Domain-level app.
* **RStudioConnectUrl**: string: A URL pointing to an RStudio Connect server.
* **RStudioPackageManagerUrl**: string: A URL pointing to an RStudio Package Manager server.

## Tag
### Properties
* **Key**: string (Required)
* **Value**: string (Required)

## AWS.SageMaker/ImageProperties
### Properties
* **ImageArn**: string (ReadOnly, Identifier)
* **ImageDescription**: string
* **ImageDisplayName**: string
* **ImageName**: string (Required)
* **ImageRoleArn**: string (Required)
* **Tags**: [Tag](#tag)[]: An array of key-value pairs to apply to this resource.

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 
* **Value**: string (Required): The value for the tag. You can specify a value that is 1 to 255 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 

## AWS.SageMaker/ModelPackageProperties
### Properties
* **AdditionalInferenceSpecificationDefinition**: [AdditionalInferenceSpecificationDefinition](#additionalinferencespecificationdefinition)
* **AdditionalInferenceSpecifications**: [AdditionalInferenceSpecificationDefinition](#additionalinferencespecificationdefinition)[]
* **AdditionalInferenceSpecificationsToAdd**: [AdditionalInferenceSpecificationDefinition](#additionalinferencespecificationdefinition)[]
* **ApprovalDescription**: string
* **CertifyForMarketplace**: bool
* **ClientToken**: string
* **CreatedBy**: [ModelPackage_CreatedBy](#modelpackagecreatedby)
* **CreationTime**: string (ReadOnly)
* **CustomerMetadataProperties**: [CustomerMetadataProperties](#customermetadataproperties)
* **Domain**: string
* **DriftCheckBaselines**: [DriftCheckBaselines](#driftcheckbaselines)
* **Environment**: [Environment](#environment)
* **InferenceSpecification**: [InferenceSpecification](#inferencespecification)
* **LastModifiedBy**: [ModelPackage_LastModifiedBy](#modelpackagelastmodifiedby)
* **LastModifiedTime**: string
* **MetadataProperties**: [MetadataProperties](#metadataproperties)
* **ModelApprovalStatus**: string
* **ModelMetrics**: [ModelMetrics](#modelmetrics)
* **ModelPackageArn**: string (ReadOnly, Identifier)
* **ModelPackageDescription**: string
* **ModelPackageGroupName**: string
* **ModelPackageName**: string
* **ModelPackageStatus**: string (ReadOnly)
* **ModelPackageStatusDetails**: [ModelPackageStatusDetails](#modelpackagestatusdetails)
* **ModelPackageStatusItem**: [ModelPackageStatusItem](#modelpackagestatusitem)
* **ModelPackageVersion**: int
* **SamplePayloadUrl**: string
* **SourceAlgorithmSpecification**: [SourceAlgorithmSpecification](#sourcealgorithmspecification)
* **Tag**: [Tag](#tag)
* **Tags**: [Tag](#tag)[]: An array of key-value pairs to apply to this resource.
* **Task**: string
* **ValidationSpecification**: [ValidationSpecification](#validationspecification)

## AdditionalInferenceSpecificationDefinition
### Properties
* **Containers**: [ModelPackageContainerDefinition](#modelpackagecontainerdefinition)[] (Required): The Amazon ECR registry path of the Docker image that contains the inference code.
* **Description**: string: A description of the additional Inference specification.
* **Name**: string (Required): A unique name to identify the additional inference specification. The name must be unique within the list of your additional inference specifications for a particular model package.
* **SupportedContentTypes**: string[]: The supported MIME types for the input data.
* **SupportedRealtimeInferenceInstanceTypes**: string[]: A list of the instance types that are used to generate inferences in real-time
* **SupportedResponseMIMETypes**: string[]: The supported MIME types for the output data.
* **SupportedTransformInstanceTypes**: string[]: A list of the instance types on which a transformation job can be run or on which an endpoint can be deployed.

## ModelPackageContainerDefinition
### Properties
* **ContainerHostname**: string: The DNS host name for the Docker container.
* **Environment**: [Environment](#environment)
* **Framework**: string: The machine learning framework of the model package container image.
* **FrameworkVersion**: string: The framework version of the Model Package Container Image.
* **Image**: string (Required): The Amazon EC2 Container Registry (Amazon ECR) path where inference code is stored.
* **ImageDigest**: string: An MD5 hash of the training algorithm that identifies the Docker image used for training.
* **ModelDataUrl**: string: A structure with Model Input details.
* **ModelInput**: [ModelPackage_ModelInput](#modelpackagemodelinput)
* **NearestModelName**: string: The name of a pre-trained machine learning benchmarked by Amazon SageMaker Inference Recommender model that matches your model.
* **ProductId**: string: The AWS Marketplace product ID of the model package.

## Environment
### Properties

## ModelPackage_ModelInput
### Properties
* **DataInputConfig**: string (Required): The input configuration object for the model.

## ModelPackage_CreatedBy
### Properties

## CustomerMetadataProperties
### Properties

## DriftCheckBaselines
### Properties
* **Bias**: [DriftCheckBias](#driftcheckbias)
* **Explainability**: [DriftCheckExplainability](#driftcheckexplainability)
* **ModelDataQuality**: [DriftCheckModelDataQuality](#driftcheckmodeldataquality)
* **ModelQuality**: [DriftCheckModelQuality](#driftcheckmodelquality)

## DriftCheckBias
### Properties
* **ConfigFile**: [FileSource](#filesource)
* **PostTrainingConstraints**: [MetricsSource](#metricssource)
* **PreTrainingConstraints**: [MetricsSource](#metricssource)

## FileSource
### Properties
* **ContentDigest**: string: The digest of the file source.
* **ContentType**: string: The type of content stored in the file source.
* **S3Uri**: string (Required): The Amazon S3 URI for the file source.

## MetricsSource
### Properties
* **ContentDigest**: string: The digest of the metric source.
* **ContentType**: string (Required): The type of content stored in the metric source.
* **S3Uri**: string (Required): The Amazon S3 URI for the metric source.

## DriftCheckExplainability
### Properties
* **ConfigFile**: [FileSource](#filesource)
* **Constraints**: [MetricsSource](#metricssource)

## DriftCheckModelDataQuality
### Properties
* **Constraints**: [MetricsSource](#metricssource)
* **Statistics**: [MetricsSource](#metricssource)

## DriftCheckModelQuality
### Properties
* **Constraints**: [MetricsSource](#metricssource)
* **Statistics**: [MetricsSource](#metricssource)

## InferenceSpecification
### Properties
* **Containers**: [ModelPackageContainerDefinition](#modelpackagecontainerdefinition)[] (Required): The Amazon ECR registry path of the Docker image that contains the inference code.
* **SupportedContentTypes**: string[] (Required): The supported MIME types for the input data.
* **SupportedRealtimeInferenceInstanceTypes**: string[]: A list of the instance types that are used to generate inferences in real-time
* **SupportedResponseMIMETypes**: string[] (Required): The supported MIME types for the output data.
* **SupportedTransformInstanceTypes**: string[]: A list of the instance types on which a transformation job can be run or on which an endpoint can be deployed.

## ModelPackage_LastModifiedBy
### Properties

## MetadataProperties
### Properties
* **CommitId**: string: The commit ID.
* **GeneratedBy**: string: The entity this entity was generated by.
* **ProjectId**: string: The project ID metadata.
* **Repository**: string: The repository metadata.

## ModelMetrics
### Properties
* **Bias**: [Bias](#bias)
* **Explainability**: [Explainability](#explainability)
* **ModelDataQuality**: [ModelDataQuality](#modeldataquality)
* **ModelQuality**: [ModelQuality](#modelquality)

## Bias
### Properties
* **PostTrainingReport**: [MetricsSource](#metricssource)
* **PreTrainingReport**: [MetricsSource](#metricssource)
* **Report**: [MetricsSource](#metricssource)

## Explainability
### Properties
* **Report**: [MetricsSource](#metricssource)

## ModelDataQuality
### Properties
* **Constraints**: [MetricsSource](#metricssource)
* **Statistics**: [MetricsSource](#metricssource)

## ModelQuality
### Properties
* **Constraints**: [MetricsSource](#metricssource)
* **Statistics**: [MetricsSource](#metricssource)

## ModelPackageStatusDetails
### Properties
* **ImageScanStatuses**: [ModelPackageStatusItem](#modelpackagestatusitem)[]
* **ValidationStatuses**: [ModelPackageStatusItem](#modelpackagestatusitem)[] (Required)

## ModelPackageStatusItem
### Properties
* **FailureReason**: string: If the overall status is Failed, the reason for the failure.
* **Name**: string (Required): The name of the model package for which the overall status is being reported.
* **Status**: string (Required): The current status.

## SourceAlgorithmSpecification
### Properties
* **SourceAlgorithms**: [SourceAlgorithm](#sourcealgorithm)[] (Required): A list of algorithms that were used to create a model package.

## SourceAlgorithm
### Properties
* **AlgorithmName**: string (Required): The name of an algorithm that was used to create the model package. The algorithm must be either an algorithm resource in your Amazon SageMaker account or an algorithm in AWS Marketplace that you are subscribed to.
* **ModelDataUrl**: string: The Amazon S3 path where the model artifacts, which result from model training, are stored. This path must point to a single gzip compressed tar archive (.tar.gz suffix).

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -.
* **Value**: string (Required): The value for the tag. You can specify a value that is 1 to 255 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -.

## ValidationSpecification
### Properties
* **ValidationProfiles**: [ValidationProfile](#validationprofile)[] (Required)
* **ValidationRole**: string (Required): The IAM roles to be used for the validation of the model package.

## ValidationProfile
### Properties
* **ProfileName**: string (Required): The name of the profile for the model package.
* **TransformJobDefinition**: [TransformJobDefinition](#transformjobdefinition) (Required)

## TransformJobDefinition
### Properties
* **BatchStrategy**: string: A string that determines the number of records included in a single mini-batch.
* **Environment**: [Environment](#environment)
* **MaxConcurrentTransforms**: int: The maximum number of parallel requests that can be sent to each instance in a transform job. The default value is 1.
* **MaxPayloadInMB**: int: The maximum payload size allowed, in MB. A payload is the data portion of a record (without metadata).
* **TransformInput**: [TransformInput](#transforminput) (Required)
* **TransformOutput**: [TransformOutput](#transformoutput) (Required)
* **TransformResources**: [TransformResources](#transformresources) (Required)

## TransformInput
### Properties
* **CompressionType**: string: If your transform data is compressed, specify the compression type. Amazon SageMaker automatically decompresses the data for the transform job accordingly. The default value is None.
* **ContentType**: string: The multipurpose internet mail extension (MIME) type of the data. Amazon SageMaker uses the MIME type with each http call to transfer data to the transform job.
* **DataSource**: [DataSource](#datasource) (Required)
* **SplitType**: string: The method to use to split the transform job's data files into smaller batches. 

## DataSource
### Properties
* **S3DataSource**: [S3DataSource](#s3datasource) (Required)

## S3DataSource
### Properties
* **S3DataType**: string (Required): The S3 Data Source Type
* **S3Uri**: string (Required): Depending on the value specified for the S3DataType, identifies either a key name prefix or a manifest.

## TransformOutput
### Properties
* **Accept**: string: The MIME type used to specify the output data. Amazon SageMaker uses the MIME type with each http call to transfer data from the transform job.
* **AssembleWith**: string: Defines how to assemble the results of the transform job as a single S3 object.
* **KmsKeyId**: string: The AWS Key Management Service (AWS KMS) key that Amazon SageMaker uses to encrypt the model artifacts at rest using Amazon S3 server-side encryption.
* **S3OutputPath**: string (Required): The Amazon S3 path where you want Amazon SageMaker to store the results of the transform job.

## TransformResources
### Properties
* **InstanceCount**: int (Required): The number of ML compute instances to use in the transform job. For distributed transform jobs, specify a value greater than 1. The default value is 1.
* **InstanceType**: string (Required): The ML compute instance type for the transform job.
* **VolumeKmsKeyId**: string: The AWS Key Management Service (AWS KMS) key that Amazon SageMaker uses to encrypt model data on the storage volume attached to the ML compute instance(s) that run the batch transform job.

## AWS.SageMaker/ModelPackageGroupProperties
### Properties
* **CreationTime**: string (ReadOnly): The time at which the model package group was created.
* **ModelPackageGroupArn**: string (ReadOnly, Identifier)
* **ModelPackageGroupDescription**: string
* **ModelPackageGroupName**: string (Required)
* **ModelPackageGroupPolicy**: [ModelPackageGroup_ModelPackageGroupPolicy](#modelpackagegroupmodelpackagegrouppolicy) | string
* **ModelPackageGroupStatus**: string (ReadOnly): The status of a modelpackage group job.
* **Tags**: [Tag](#tag)[]: An array of key-value pairs to apply to this resource.

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 
* **Value**: string (Required): The value for the tag. You can specify a value that is 1 to 255 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 

## AWS.SageMaker/MonitoringScheduleProperties
### Properties
* **CreationTime**: string (ReadOnly): The time at which the schedule was created.
* **EndpointName**: string
* **FailureReason**: string: Contains the reason a monitoring job failed, if it failed.
* **LastModifiedTime**: string (ReadOnly): A timestamp that indicates the last time the monitoring job was modified.
* **LastMonitoringExecutionSummary**: [MonitoringExecutionSummary](#monitoringexecutionsummary): Describes metadata on the last execution to run, if there was one.
* **MonitoringScheduleArn**: string (ReadOnly, Identifier): The Amazon Resource Name (ARN) of the monitoring schedule.
* **MonitoringScheduleConfig**: [MonitoringScheduleConfig](#monitoringscheduleconfig) (Required)
* **MonitoringScheduleName**: string (Required)
* **MonitoringScheduleStatus**: string: The status of a schedule job.
* **Tags**: [Tag](#tag)[]: An array of key-value pairs to apply to this resource.

## MonitoringExecutionSummary
### Properties
* **CreationTime**: string (Required): The time at which the monitoring job was created.
* **EndpointName**: string
* **FailureReason**: string: Contains the reason a monitoring job failed, if it failed.
* **LastModifiedTime**: string (Required): A timestamp that indicates the last time the monitoring job was modified.
* **MonitoringExecutionStatus**: string (Required): The status of the monitoring job.
* **MonitoringScheduleName**: string (Required)
* **ProcessingJobArn**: string: The Amazon Resource Name (ARN) of the monitoring job.
* **ScheduledTime**: string (Required): The time the monitoring job was scheduled.

## MonitoringScheduleConfig
### Properties
* **MonitoringJobDefinition**: [MonitoringJobDefinition](#monitoringjobdefinition)
* **MonitoringJobDefinitionName**: string: Name of the job definition
* **MonitoringType**: string
* **ScheduleConfig**: [ScheduleConfig](#scheduleconfig)

## MonitoringJobDefinition
### Properties
* **BaselineConfig**: [BaselineConfig](#baselineconfig)
* **Environment**: [MonitoringSchedule_Environment](#monitoringscheduleenvironment): Sets the environment variables in the Docker container
* **MonitoringAppSpecification**: [MonitoringAppSpecification](#monitoringappspecification) (Required)
* **MonitoringInputs**: [MonitoringInput](#monitoringinput)[] (Required)
* **MonitoringOutputConfig**: [MonitoringOutputConfig](#monitoringoutputconfig) (Required)
* **MonitoringResources**: [MonitoringResources](#monitoringresources) (Required)
* **NetworkConfig**: [NetworkConfig](#networkconfig)
* **RoleArn**: string (Required): The Amazon Resource Name (ARN) of an IAM role that Amazon SageMaker can assume to perform tasks on your behalf.
* **StoppingCondition**: [StoppingCondition](#stoppingcondition)

## BaselineConfig
### Properties
* **ConstraintsResource**: [ConstraintsResource](#constraintsresource)
* **StatisticsResource**: [StatisticsResource](#statisticsresource)

## ConstraintsResource
### Properties
* **S3Uri**: string: The Amazon S3 URI for baseline constraint file in Amazon S3 that the current monitoring job should validated against.

## StatisticsResource
### Properties
* **S3Uri**: string: The Amazon S3 URI for the baseline statistics file in Amazon S3 that the current monitoring job should be validated against.

## MonitoringSchedule_Environment
### Properties

## MonitoringAppSpecification
### Properties
* **ContainerArguments**: string[]: An array of arguments for the container used to run the monitoring job.
* **ContainerEntrypoint**: string[]: Specifies the entrypoint for a container used to run the monitoring job.
* **ImageUri**: string (Required): The container image to be run by the monitoring job.
* **PostAnalyticsProcessorSourceUri**: string: An Amazon S3 URI to a script that is called after analysis has been performed. Applicable only for the built-in (first party) containers.
* **RecordPreprocessorSourceUri**: string: An Amazon S3 URI to a script that is called per row prior to running analysis. It can base64 decode the payload and convert it into a flatted json so that the built-in container can use the converted data. Applicable only for the built-in (first party) containers

## MonitoringInput
### Properties
* **BatchTransformInput**: [BatchTransformInput](#batchtransforminput)
* **EndpointInput**: [EndpointInput](#endpointinput)

## BatchTransformInput
### Properties
* **DataCapturedDestinationS3Uri**: string (Required): A URI that identifies the Amazon S3 storage location where Batch Transform Job captures data.
* **DatasetFormat**: [DatasetFormat](#datasetformat) (Required)
* **LocalPath**: string (Required): Path to the filesystem where the endpoint data is available to the container.
* **S3DataDistributionType**: string: Whether input data distributed in Amazon S3 is fully replicated or sharded by an S3 key. Defauts to FullyReplicated
* **S3InputMode**: string: Whether the Pipe or File is used as the input mode for transfering data for the monitoring job. Pipe mode is recommended for large datasets. File mode is useful for small files that fit in memory. Defaults to File.

## DatasetFormat
### Properties
* **Csv**: [Csv](#csv)
* **Json**: [Json](#json)
* **Parquet**: bool

## Csv
### Properties
* **Header**: bool: A boolean flag indicating if given CSV has header

## Json
### Properties
* **Line**: bool: A boolean flag indicating if it is JSON line format

## EndpointInput
### Properties
* **EndpointName**: string (Required)
* **LocalPath**: string (Required): Path to the filesystem where the endpoint data is available to the container.
* **S3DataDistributionType**: string: Whether input data distributed in Amazon S3 is fully replicated or sharded by an S3 key. Defauts to FullyReplicated
* **S3InputMode**: string: Whether the Pipe or File is used as the input mode for transfering data for the monitoring job. Pipe mode is recommended for large datasets. File mode is useful for small files that fit in memory. Defaults to File.

## MonitoringOutputConfig
### Properties
* **KmsKeyId**: string: The AWS Key Management Service (AWS KMS) key that Amazon SageMaker uses to encrypt the model artifacts at rest using Amazon S3 server-side encryption.
* **MonitoringOutputs**: [MonitoringOutput](#monitoringoutput)[] (Required): Monitoring outputs for monitoring jobs. This is where the output of the periodic monitoring jobs is uploaded.

## MonitoringOutput
### Properties
* **S3Output**: [S3Output](#s3output) (Required)

## S3Output
### Properties
* **LocalPath**: string (Required): The local path to the Amazon S3 storage location where Amazon SageMaker saves the results of a monitoring job. LocalPath is an absolute path for the output data.
* **S3UploadMode**: string: Whether to upload the results of the monitoring job continuously or after the job completes.
* **S3Uri**: string (Required): A URI that identifies the Amazon S3 storage location where Amazon SageMaker saves the results of a monitoring job.

## MonitoringResources
### Properties
* **ClusterConfig**: [ClusterConfig](#clusterconfig) (Required)

## ClusterConfig
### Properties
* **InstanceCount**: int (Required): The number of ML compute instances to use in the model monitoring job. For distributed processing jobs, specify a value greater than 1. The default value is 1.
* **InstanceType**: string (Required): The ML compute instance type for the processing job.
* **VolumeKmsKeyId**: string: The AWS Key Management Service (AWS KMS) key that Amazon SageMaker uses to encrypt data on the storage volume attached to the ML compute instance(s) that run the model monitoring job.
* **VolumeSizeInGB**: int (Required): The size of the ML storage volume, in gigabytes, that you want to provision. You must specify sufficient ML storage for your scenario.

## NetworkConfig
### Properties
* **EnableInterContainerTrafficEncryption**: bool: Whether to encrypt all communications between distributed processing jobs. Choose True to encrypt communications. Encryption provides greater security for distributed processing jobs, but the processing might take longer.
* **EnableNetworkIsolation**: bool: Whether to allow inbound and outbound network calls to and from the containers used for the processing job.
* **VpcConfig**: [VpcConfig](#vpcconfig)

## VpcConfig
### Properties
* **SecurityGroupIds**: string[] (Required): The VPC security group IDs, in the form sg-xxxxxxxx. Specify the security groups for the VPC that is specified in the Subnets field.
* **Subnets**: string[] (Required): The ID of the subnets in the VPC to which you want to connect to your monitoring jobs.

## StoppingCondition
### Properties
* **MaxRuntimeInSeconds**: int (Required): The maximum runtime allowed in seconds.

## ScheduleConfig
### Properties
* **ScheduleExpression**: string (Required): A cron expression that describes details about the monitoring schedule.

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 
* **Value**: string (Required): The value for the tag. You can specify a value that is 1 to 255 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 

## AWS.SageMaker/PipelineProperties
### Properties
* **ParallelismConfiguration**: [Pipeline_ParallelismConfiguration](#pipelineparallelismconfiguration)
* **PipelineDefinition**: [Pipeline_PipelineDefinition](#pipelinepipelinedefinition) (Required)
* **PipelineDescription**: string: The description of the Pipeline.
* **PipelineDisplayName**: string: The display name of the Pipeline.
* **PipelineName**: string (Required, Identifier): The name of the Pipeline.
* **RoleArn**: string (Required): Role Arn
* **Tags**: [Tag](#tag)[]

## Pipeline_ParallelismConfiguration
### Properties
* **MaxParallelExecutionSteps**: int (Required): Maximum parallel execution steps

## Pipeline_PipelineDefinition
### Properties

## Tag
### Properties
* **Key**: string (Required)
* **Value**: string (Required)

## AWS.SageMaker/ProjectProperties
### Properties
* **CreationTime**: string (ReadOnly): The time at which the project was created.
* **ProjectArn**: string (ReadOnly, Identifier)
* **ProjectDescription**: string
* **ProjectId**: string (ReadOnly)
* **ProjectName**: string (Required)
* **ProjectStatus**: string (ReadOnly): The status of a project.
* **ServiceCatalogProvisionedProductDetails**: [Project_ServiceCatalogProvisionedProductDetails](#projectservicecatalogprovisionedproductdetails) (ReadOnly): Provisioned ServiceCatalog  Details
* **ServiceCatalogProvisioningDetails**: [Project_ServiceCatalogProvisioningDetails](#projectservicecatalogprovisioningdetails) (Required): Input ServiceCatalog Provisioning Details
* **Tags**: [Tag](#tag)[]: An array of key-value pairs to apply to this resource.

## Project_ServiceCatalogProvisionedProductDetails
### Properties
* **ProvisionedProductId**: string
* **ProvisionedProductStatusMessage**: string

## Project_ServiceCatalogProvisioningDetails
### Properties
* **PathId**: string
* **ProductId**: string (Required)
* **ProvisioningArtifactId**: string
* **ProvisioningParameters**: [ProvisioningParameter](#provisioningparameter)[]: Parameters specified by the administrator that are required for provisioning the product.

## ProvisioningParameter
### Properties
* **Key**: string (Required): The parameter key.
* **Value**: string (Required): The parameter value.

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 
* **Value**: string (Required): The value for the tag. You can specify a value that is 1 to 255 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 

## AWS.SageMaker/UserProfileProperties
### Properties
* **DomainId**: string (Required, Identifier): The ID of the associated Domain.
* **SingleSignOnUserIdentifier**: string: A specifier for the type of value specified in SingleSignOnUserValue. Currently, the only supported value is "UserName". If the Domain's AuthMode is SSO, this field is required. If the Domain's AuthMode is not SSO, this field cannot be specified.
* **SingleSignOnUserValue**: string: The username of the associated AWS Single Sign-On User for this UserProfile. If the Domain's AuthMode is SSO, this field is required, and must match a valid username of a user in your directory. If the Domain's AuthMode is not SSO, this field cannot be specified.
* **Tags**: [Tag](#tag)[] (WriteOnly): A list of tags to apply to the user profile.
* **UserProfileArn**: string (ReadOnly): The user profile Amazon Resource Name (ARN).
* **UserProfileName**: string (Required, Identifier): A name for the UserProfile.
* **UserSettings**: [UserSettings](#usersettings): A collection of settings.

## Tag
### Properties
* **Key**: string (Required)
* **Value**: string (Required)

## UserSettings
### Properties
* **ExecutionRole**: string: The user profile Amazon Resource Name (ARN).
* **JupyterServerAppSettings**: [JupyterServerAppSettings](#jupyterserverappsettings): The Jupyter server's app settings.
* **KernelGatewayAppSettings**: [KernelGatewayAppSettings](#kernelgatewayappsettings): The kernel gateway app settings.
* **RStudioServerProAppSettings**: [RStudioServerProAppSettings](#rstudioserverproappsettings)
* **SecurityGroups**: string[]: The security groups for the Amazon Virtual Private Cloud (VPC) that Studio uses for communication.
* **SharingSettings**: [SharingSettings](#sharingsettings): The sharing settings.

## JupyterServerAppSettings
### Properties
* **DefaultResourceSpec**: [ResourceSpec](#resourcespec)

## ResourceSpec
### Properties
* **InstanceType**: string: The instance type that the image version runs on.
* **SageMakerImageArn**: string: The ARN of the SageMaker image that the image version belongs to.
* **SageMakerImageVersionArn**: string: The ARN of the image version created on the instance.

## KernelGatewayAppSettings
### Properties
* **CustomImages**: [CustomImage](#customimage)[]: A list of custom SageMaker images that are configured to run as a KernelGateway app.
* **DefaultResourceSpec**: [ResourceSpec](#resourcespec): The default instance type and the Amazon Resource Name (ARN) of the default SageMaker image used by the KernelGateway app.

## CustomImage
### Properties
* **AppImageConfigName**: string (Required): The Name of the AppImageConfig.
* **ImageName**: string (Required): The name of the CustomImage. Must be unique to your account.
* **ImageVersionNumber**: int: The version number of the CustomImage.

## RStudioServerProAppSettings
### Properties
* **AccessStatus**: string: Indicates whether the current user has access to the RStudioServerPro app.
* **UserGroup**: string: The level of permissions that the user has within the RStudioServerPro app. This value defaults to User. The Admin value allows the user access to the RStudio Administrative Dashboard.

## SharingSettings
### Properties
* **NotebookOutputOption**: string: Whether to include the notebook cell output when sharing the notebook. The default is Disabled.
* **S3KmsKeyId**: string: When NotebookOutputOption is Allowed, the AWS Key Management Service (KMS) encryption key ID used to encrypt the notebook cell output in the Amazon S3 bucket.
* **S3OutputPath**: string: When NotebookOutputOption is Allowed, the Amazon S3 bucket used to store the shared notebook snapshots.

