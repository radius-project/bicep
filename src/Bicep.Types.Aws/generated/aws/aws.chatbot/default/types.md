# AWS.Chatbot @ default

## Resource AWS.Chatbot/SlackChannelConfiguration@default
* **Valid Scope(s)**: Unknown
### Properties
* **properties**: [AWS.Chatbot/SlackChannelConfigurationProperties](#awschatbotslackchannelconfigurationproperties) (Required): properties of the resource

## AWS.Chatbot/SlackChannelConfigurationProperties
### Properties
* **Arn**: string (ReadOnly): Amazon Resource Name (ARN) of the configuration
* **ConfigurationName**: string (Required): The name of the configuration
* **GuardrailPolicies**: string[]: The list of IAM policy ARNs that are applied as channel guardrails. The AWS managed 'AdministratorAccess' policy is applied as a default if this is not set.
* **IamRoleArn**: string (Required): The ARN of the IAM role that defines the permissions for AWS Chatbot
* **LoggingLevel**: string: Specifies the logging level for this configuration:ERROR,INFO or NONE. This property affects the log entries pushed to Amazon CloudWatch logs
* **SlackChannelId**: string (Required): The id of the Slack channel
* **SlackWorkspaceId**: string (Required): The id of the Slack workspace
* **SnsTopicArns**: string[]: ARNs of SNS topics which delivers notifications to AWS Chatbot, for example CloudWatch alarm notifications.
* **UserRoleRequired**: bool: Enables use of a user role requirement in your chat configuration

