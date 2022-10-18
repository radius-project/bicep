# AWS.RefactorSpaces @ default

## Resource AWS.RefactorSpaces/Route@default
* **Valid Scope(s)**: Unknown
### Properties
* **properties**: [AWS.RefactorSpaces/RouteProperties](#awsrefactorspacesrouteproperties) (Required): properties of the resource

## AWS.RefactorSpaces/RouteProperties
### Properties
* **ApplicationIdentifier**: string (Required, Identifier)
* **Arn**: string (ReadOnly)
* **DefaultRoute**: [DefaultRouteInput](#defaultrouteinput) (WriteOnly)
* **EnvironmentIdentifier**: string (Required, Identifier)
* **PathResourceToId**: string (ReadOnly)
* **RouteIdentifier**: string (ReadOnly, Identifier)
* **RouteType**: [RouteType](#routetype) (WriteOnly)
* **ServiceIdentifier**: string (Required, WriteOnly)
* **Tags**: [Tag](#tag)[]: Metadata that you can assign to help organize the frameworks that you create. Each tag is a key-value pair.
* **UriPathRoute**: [UriPathRouteInput](#uripathrouteinput) (WriteOnly)

## DefaultRouteInput
### Properties
* **ActivationState**: [RouteActivationState](#routeactivationstate) (Required)

## RouteActivationState
### Properties

## RouteType
### Properties

## Tag
### Properties
* **Key**: string (Required): A string used to identify this tag
* **Value**: string (Required): A string containing the value for the tag

## UriPathRouteInput
### Properties
* **ActivationState**: [RouteActivationState](#routeactivationstate) (Required)
* **IncludeChildPaths**: bool
* **Methods**: [Method](#method)[]
* **SourcePath**: string

## Method
### Properties

