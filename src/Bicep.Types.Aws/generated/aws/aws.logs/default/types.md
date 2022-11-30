# AWS.Logs @ default

## Resource AWS.Logs/Destination@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.Logs/DestinationProperties](#awslogsdestinationproperties) (Required): properties of the resource

## Resource AWS.Logs/LogGroup@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.Logs/LogGroupProperties](#awslogsloggroupproperties): properties of the resource

## Resource AWS.Logs/MetricFilter@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.Logs/MetricFilterProperties](#awslogsmetricfilterproperties) (Required): properties of the resource

## Resource AWS.Logs/QueryDefinition@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.Logs/QueryDefinitionProperties](#awslogsquerydefinitionproperties) (Required): properties of the resource

## Resource AWS.Logs/ResourcePolicy@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.Logs/ResourcePolicyProperties](#awslogsresourcepolicyproperties) (Required): properties of the resource

## AWS.Logs/DestinationProperties
### Properties
* **Arn**: string (ReadOnly)
* **DestinationName**: string (Required): The name of the destination resource
* **DestinationPolicy**: string: An IAM policy document that governs which AWS accounts can create subscription filters against this destination.
* **RoleArn**: string (Required): The ARN of an IAM role that permits CloudWatch Logs to send data to the specified AWS resource
* **TargetArn**: string (Required): The ARN of the physical target where the log events are delivered (for example, a Kinesis stream)

## AWS.Logs/LogGroupProperties
### Properties
* **Arn**: string (ReadOnly): The CloudWatch log group ARN.
* **KmsKeyId**: string: The Amazon Resource Name (ARN) of the CMK to use when encrypting log data.
* **LogGroupName**: string: The name of the log group. If you don't specify a name, AWS CloudFormation generates a unique ID for the log group.
* **RetentionInDays**: int: The number of days to retain the log events in the specified log group. Possible values are: 1, 3, 5, 7, 14, 30, 60, 90, 120, 150, 180, 365, 400, 545, 731, 1827, and 3653.
* **Tags**: [Tag](#tag)[]: An array of key-value pairs to apply to this resource.

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 128 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., :, /, =, +, - and @.
* **Value**: string (Required): The value for the tag. You can specify a value that is 0 to 256 Unicode characters in length. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., :, /, =, +, - and @.

## AWS.Logs/MetricFilterProperties
### Properties
* **FilterName**: string: A name for the metric filter.
* **FilterPattern**: string (Required): Pattern that Logs follows to interpret each entry in a log.
* **LogGroupName**: string (Required): Existing log group that you want to associate with this filter.
* **MetricTransformations**: [MetricTransformation](#metrictransformation)[] (Required): A collection of information that defines how metric data gets emitted.

## MetricTransformation
### Properties
* **DefaultValue**: int: The value to emit when a filter pattern does not match a log event. This value can be null.
* **Dimensions**: [Dimension](#dimension)[]: Dimensions are the key-value pairs that further define a metric
* **MetricName**: string (Required): The name of the CloudWatch metric. Metric name must be in ASCII format.
* **MetricNamespace**: string (Required): The namespace of the CloudWatch metric.
* **MetricValue**: string (Required): The value to publish to the CloudWatch metric when a filter pattern matches a log event.
* **Unit**: string: The unit to assign to the metric. If you omit this, the unit is set as None.

## Dimension
### Properties
* **Key**: string (Required): The key of the dimension. Maximum length of 255.
* **Value**: string (Required): The value of the dimension. Maximum length of 255.

## AWS.Logs/QueryDefinitionProperties
### Properties
* **LogGroupNames**: [LogGroup](#loggroup)[]: Optionally define specific log groups as part of your query definition
* **Name**: string (Required): A name for the saved query definition
* **QueryDefinitionId**: string (ReadOnly): Unique identifier of a query definition
* **QueryString**: string (Required): The query string to use for this definition

## LogGroup
### Properties

## AWS.Logs/ResourcePolicyProperties
### Properties
* **PolicyDocument**: string (Required): The policy document
* **PolicyName**: string (Required): A name for resource policy

