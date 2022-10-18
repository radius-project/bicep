# AWS.ApiGatewayV2 @ default

## Resource AWS.ApiGatewayV2/VpcLink@default
* **Valid Scope(s)**: Unknown
### Properties
* **properties**: [AWS.ApiGatewayV2/VpcLinkProperties](#awsapigatewayv2vpclinkproperties) (Required): properties of the resource

## AWS.ApiGatewayV2/VpcLinkProperties
### Properties
* **Name**: string (Required)
* **SecurityGroupIds**: string[]
* **SubnetIds**: string[] (Required)
* **Tags**: [VpcLink_Tags](#vpclinktags): This resource type use map for Tags, suggest to use List of Tag
* **VpcLinkId**: string (ReadOnly, Identifier)

## VpcLink_Tags
### Properties

