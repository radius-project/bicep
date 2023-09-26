# AWS.GameLift @ default

## Resource AWS.GameLift/Alias@default
* **Valid Scope(s)**: Unknown
### Properties
* **alias**: string (Required): the resource alias
* **name**: string: the resource name
* **properties**: [AWS.GameLift/AliasProperties](#awsgameliftaliasproperties) (Required): properties of the resource

## Resource AWS.GameLift/Build@default
* **Valid Scope(s)**: Unknown
### Properties
* **alias**: string (Required): the resource alias
* **name**: string: the resource name
* **properties**: [AWS.GameLift/BuildProperties](#awsgameliftbuildproperties): properties of the resource

## Resource AWS.GameLift/Fleet@default
* **Valid Scope(s)**: Unknown
### Properties
* **alias**: string (Required): the resource alias
* **name**: string: the resource name
* **properties**: [AWS.GameLift/FleetProperties](#awsgameliftfleetproperties) (Required): properties of the resource

## Resource AWS.GameLift/GameServerGroup@default
* **Valid Scope(s)**: Unknown
### Properties
* **alias**: string (Required): the resource alias
* **name**: string: the resource name
* **properties**: [AWS.GameLift/GameServerGroupProperties](#awsgameliftgameservergroupproperties) (Required): properties of the resource

## Resource AWS.GameLift/Location@default
* **Valid Scope(s)**: Unknown
### Properties
* **alias**: string (Required): the resource alias
* **name**: string: the resource name
* **properties**: [AWS.GameLift/LocationProperties](#awsgameliftlocationproperties) (Required): properties of the resource

## AWS.GameLift/AliasProperties
### Properties
* **AliasId**: string (ReadOnly, Identifier): Unique alias ID
* **Description**: string: A human-readable description of the alias.
* **Name**: string (Required): A descriptive label that is associated with an alias. Alias names do not need to be unique.
* **RoutingStrategy**: [RoutingStrategy](#routingstrategy) (Required): A routing configuration that specifies where traffic is directed for this alias, such as to a fleet or to a message.

## RoutingStrategy
### Properties
* **FleetId**: string: A unique identifier for a fleet that the alias points to. If you specify SIMPLE for the Type property, you must specify this property.
* **Message**: string: The message text to be used with a terminal routing strategy. If you specify TERMINAL for the Type property, you must specify this property.
* **Type**: string (Required): Simple routing strategy. The alias resolves to one specific fleet. Use this type when routing to active fleets.

## AWS.GameLift/BuildProperties
### Properties
* **BuildId**: string (ReadOnly, Identifier): A unique identifier for a build to be deployed on the new fleet. If you are deploying the fleet with a custom game build, you must specify this property. The build must have been successfully uploaded to Amazon GameLift and be in a READY status. This fleet setting cannot be changed once the fleet is created.
* **Name**: string: A descriptive label that is associated with a build. Build names do not need to be unique.
* **OperatingSystem**: string: The operating system that the game server binaries are built to run on. This value determines the type of fleet resources that you can use for this build. If your game build contains multiple executables, they all must run on the same operating system. If an operating system is not specified when creating a build, Amazon GameLift uses the default value (WINDOWS_2012). This value cannot be changed later.
* **ServerSdkVersion**: string (WriteOnly): A server SDK version you used when integrating your game server build with Amazon GameLift. By default Amazon GameLift sets this value to 4.0.2.
* **StorageLocation**: [StorageLocation](#storagelocation) (WriteOnly): Information indicating where your game build files are stored. Use this parameter only when creating a build with files stored in an Amazon S3 bucket that you own. The storage location must specify an Amazon S3 bucket name and key. The location must also specify a role ARN that you set up to allow Amazon GameLift to access your Amazon S3 bucket. The S3 bucket and your new build must be in the same Region.
* **Version**: string: Version information that is associated with this build. Version strings do not need to be unique.

## StorageLocation
### Properties
* **Bucket**: string (Required): An Amazon S3 bucket identifier. This is the name of the S3 bucket.
* **Key**: string (Required): The name of the zip file that contains the build files or script files.
* **ObjectVersion**: string: The version of the file, if object versioning is turned on for the bucket. Amazon GameLift uses this information when retrieving files from your S3 bucket. To retrieve a specific version of the file, provide an object version. To retrieve the latest version of the file, do not set this parameter.
* **RoleArn**: string (Required): The Amazon Resource Name (ARN) for an IAM role that allows Amazon GameLift to access the S3 bucket.

## AWS.GameLift/FleetProperties
### Properties
* **AnywhereConfiguration**: [AnywhereConfiguration](#anywhereconfiguration): Configuration for Anywhere fleet.
* **BuildId**: string: A unique identifier for a build to be deployed on the new fleet. If you are deploying the fleet with a custom game build, you must specify this property. The build must have been successfully uploaded to Amazon GameLift and be in a READY status. This fleet setting cannot be changed once the fleet is created.
* **CertificateConfiguration**: [CertificateConfiguration](#certificateconfiguration): Indicates whether to generate a TLS/SSL certificate for the new fleet. TLS certificates are used for encrypting traffic between game clients and game servers running on GameLift. If this parameter is not set, certificate generation is disabled. This fleet setting cannot be changed once the fleet is created.
* **ComputeType**: string: ComputeType to differentiate EC2 hardware managed by GameLift and Anywhere hardware managed by the customer.
* **Description**: string: A human-readable description of a fleet.
* **DesiredEC2Instances**: int: [DEPRECATED] The number of EC2 instances that you want this fleet to host. When creating a new fleet, GameLift automatically sets this value to "1" and initiates a single instance. Once the fleet is active, update this value to trigger GameLift to add or remove instances from the fleet.
* **EC2InboundPermissions**: [IpPermission](#ippermission)[]: A range of IP addresses and port settings that allow inbound traffic to connect to server processes on an Amazon GameLift server.
* **EC2InstanceType**: string: The name of an EC2 instance type that is supported in Amazon GameLift. A fleet instance type determines the computing resources of each instance in the fleet, including CPU, memory, storage, and networking capacity. Amazon GameLift supports the following EC2 instance types. See Amazon EC2 Instance Types for detailed descriptions.
* **FleetId**: string (ReadOnly, Identifier): Unique fleet ID
* **FleetType**: string: Indicates whether to use On-Demand instances or Spot instances for this fleet. If empty, the default is ON_DEMAND. Both categories of instances use identical hardware and configurations based on the instance type selected for this fleet.
* **InstanceRoleARN**: string: A unique identifier for an AWS IAM role that manages access to your AWS services. With an instance role ARN set, any application that runs on an instance in this fleet can assume the role, including install scripts, server processes, and daemons (background processes). Create a role or look up a role's ARN from the IAM dashboard in the AWS Management Console.
* **Locations**: [LocationConfiguration](#locationconfiguration)[]
* **LogPaths**: string[]: This parameter is no longer used. When hosting a custom game build, specify where Amazon GameLift should store log files using the Amazon GameLift server API call ProcessReady()
* **MaxSize**: int: [DEPRECATED] The maximum value that is allowed for the fleet's instance count. When creating a new fleet, GameLift automatically sets this value to "1". Once the fleet is active, you can change this value.
* **MetricGroups**: string[]: The name of an Amazon CloudWatch metric group. A metric group aggregates the metrics for all fleets in the group. Specify a string containing the metric group name. You can use an existing name or use a new name to create a new metric group. Currently, this parameter can have only one string.
* **MinSize**: int: [DEPRECATED] The minimum value allowed for the fleet's instance count. When creating a new fleet, GameLift automatically sets this value to "0". After the fleet is active, you can change this value.
* **Name**: string (Required): A descriptive label that is associated with a fleet. Fleet names do not need to be unique.
* **NewGameSessionProtectionPolicy**: string: A game session protection policy to apply to all game sessions hosted on instances in this fleet. When protected, active game sessions cannot be terminated during a scale-down event. If this parameter is not set, instances in this fleet default to no protection. You can change a fleet's protection policy to affect future game sessions on the fleet. You can also set protection for individual game sessions.
* **PeerVpcAwsAccountId**: string: A unique identifier for the AWS account with the VPC that you want to peer your Amazon GameLift fleet with. You can find your account ID in the AWS Management Console under account settings.
* **PeerVpcId**: string: A unique identifier for a VPC with resources to be accessed by your Amazon GameLift fleet. The VPC must be in the same Region as your fleet. To look up a VPC ID, use the VPC Dashboard in the AWS Management Console.
* **ResourceCreationLimitPolicy**: [ResourceCreationLimitPolicy](#resourcecreationlimitpolicy): A policy that limits the number of game sessions an individual player can create over a span of time for this fleet.
* **RuntimeConfiguration**: [RuntimeConfiguration](#runtimeconfiguration): Instructions for launching server processes on each instance in the fleet. Server processes run either a custom game build executable or a Realtime script. The runtime configuration defines the server executables or launch script file, launch parameters, and the number of processes to run concurrently on each instance. When creating a fleet, the runtime configuration must have at least one server process configuration; otherwise the request fails with an invalid request exception.

This parameter is required unless the parameters ServerLaunchPath and ServerLaunchParameters are defined. Runtime configuration has replaced these parameters, but fleets that use them will continue to work.
* **ScriptId**: string: A unique identifier for a Realtime script to be deployed on a new Realtime Servers fleet. The script must have been successfully uploaded to Amazon GameLift. This fleet setting cannot be changed once the fleet is created.

Note: It is not currently possible to use the !Ref command to reference a script created with a CloudFormation template for the fleet property ScriptId. Instead, use Fn::GetAtt Script.Arn or Fn::GetAtt Script.Id to retrieve either of these properties as input for ScriptId. Alternatively, enter a ScriptId string manually.
* **ServerLaunchParameters**: string: This parameter is no longer used but is retained for backward compatibility. Instead, specify server launch parameters in the RuntimeConfiguration parameter. A request must specify either a runtime configuration or values for both ServerLaunchParameters and ServerLaunchPath.
* **ServerLaunchPath**: string: This parameter is no longer used. Instead, specify a server launch path using the RuntimeConfiguration parameter. Requests that specify a server launch path and launch parameters instead of a runtime configuration will continue to work.

## AnywhereConfiguration
### Properties
* **Cost**: string (Required): Cost of compute can be specified on Anywhere Fleets to prioritize placement across Queue destinations based on Cost.

## CertificateConfiguration
### Properties
* **CertificateType**: string (Required)

## IpPermission
### Properties
* **FromPort**: int (Required): A starting value for a range of allowed port numbers.
* **IpRange**: string (Required): A range of allowed IP addresses. This value must be expressed in CIDR notation. Example: "000.000.000.000/[subnet mask]" or optionally the shortened version "0.0.0.0/[subnet mask]".
* **Protocol**: string (Required): The network communication protocol used by the fleet.
* **ToPort**: int (Required): An ending value for a range of allowed port numbers. Port numbers are end-inclusive. This value must be higher than FromPort.

## LocationConfiguration
### Properties
* **Location**: string (Required)
* **LocationCapacity**: [LocationCapacity](#locationcapacity)

## LocationCapacity
### Properties
* **DesiredEC2Instances**: int (Required): The number of EC2 instances you want to maintain in the specified fleet location. This value must fall between the minimum and maximum size limits.
* **MaxSize**: int (Required): The maximum value that is allowed for the fleet's instance count for a location. When creating a new fleet, GameLift automatically sets this value to "1". Once the fleet is active, you can change this value.
* **MinSize**: int (Required): The minimum value allowed for the fleet's instance count for a location. When creating a new fleet, GameLift automatically sets this value to "0". After the fleet is active, you can change this value.

## ResourceCreationLimitPolicy
### Properties
* **NewGameSessionsPerCreator**: int: The maximum number of game sessions that an individual can create during the policy period.
* **PolicyPeriodInMinutes**: int: The time span used in evaluating the resource creation limit policy.

## RuntimeConfiguration
### Properties
* **GameSessionActivationTimeoutSeconds**: int: The maximum amount of time (in seconds) that a game session can remain in status ACTIVATING. If the game session is not active before the timeout, activation is terminated and the game session status is changed to TERMINATED.
* **MaxConcurrentGameSessionActivations**: int: The maximum number of game sessions with status ACTIVATING to allow on an instance simultaneously. This setting limits the amount of instance resources that can be used for new game activations at any one time.
* **ServerProcesses**: [ServerProcess](#serverprocess)[]: A collection of server process configurations that describe which server processes to run on each instance in a fleet.

## ServerProcess
### Properties
* **ConcurrentExecutions**: int (Required): The number of server processes that use this configuration to run concurrently on an instance.
* **LaunchPath**: string (Required): The location of the server executable in a custom game build or the name of the Realtime script file that contains the Init() function. Game builds and Realtime scripts are installed on instances at the root:

Windows (for custom game builds only): C:\game. Example: "C:\game\MyGame\server.exe"

Linux: /local/game. Examples: "/local/game/MyGame/server.exe" or "/local/game/MyRealtimeScript.js"
* **Parameters**: string: An optional list of parameters to pass to the server executable or Realtime script on launch.

## AWS.GameLift/GameServerGroupProperties
### Properties
* **AutoScalingGroupArn**: string (ReadOnly): A generated unique ID for the EC2 Auto Scaling group that is associated with this game server group.
* **AutoScalingPolicy**: [AutoScalingPolicy](#autoscalingpolicy) (WriteOnly): Configuration settings to define a scaling policy for the Auto Scaling group that is optimized for game hosting
* **BalancingStrategy**: string: The fallback balancing method to use for the game server group when Spot Instances in a Region become unavailable or are not viable for game hosting.
* **DeleteOption**: string (WriteOnly): The type of delete to perform.
* **GameServerGroupArn**: string (ReadOnly, Identifier): A generated unique ID for the game server group.
* **GameServerGroupName**: string (Required): An identifier for the new game server group.
* **GameServerProtectionPolicy**: string: A flag that indicates whether instances in the game server group are protected from early termination.
* **InstanceDefinitions**: [InstanceDefinition](#instancedefinition)[] (Required): A set of EC2 instance types to use when creating instances in the group.
* **LaunchTemplate**: [LaunchTemplate](#launchtemplate) (WriteOnly): The EC2 launch template that contains configuration settings and game server code to be deployed to all instances in the game server group.
* **MaxSize**: int (WriteOnly): The maximum number of instances allowed in the EC2 Auto Scaling group.
* **MinSize**: int (WriteOnly): The minimum number of instances allowed in the EC2 Auto Scaling group.
* **RoleArn**: string (Required): The Amazon Resource Name (ARN) for an IAM role that allows Amazon GameLift to access your EC2 Auto Scaling groups.
* **Tags**: [Tag](#tag)[] (WriteOnly): A list of labels to assign to the new game server group resource.
* **VpcSubnets**: string[] (WriteOnly): A list of virtual private cloud (VPC) subnets to use with instances in the game server group.

## AutoScalingPolicy
### Properties
* **EstimatedInstanceWarmup**: int
* **TargetTrackingConfiguration**: [TargetTrackingConfiguration](#targettrackingconfiguration) (Required)

## TargetTrackingConfiguration
### Properties
* **TargetValue**: int (Required)

## InstanceDefinition
### Properties
* **InstanceType**: string (Required)
* **WeightedCapacity**: string

## LaunchTemplate
### Properties
* **LaunchTemplateId**: string
* **LaunchTemplateName**: string
* **Version**: string

## Tag
### Properties
* **Key**: string: The key for a developer-defined key:value pair for tagging an AWS resource.
* **Value**: string: The value for a developer-defined key:value pair for tagging an AWS resource.

## AWS.GameLift/LocationProperties
### Properties
* **LocationArn**: string (ReadOnly)
* **LocationName**: string (Required, Identifier)
* **Tags**: [Tag](#tag)[]: An array of key-value pairs to apply to this resource.

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 128 Unicode characters in length.
* **Value**: string (Required): The value for the tag. You can specify a value that is 0 to 256 Unicode characters in length.

