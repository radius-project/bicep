# AWS.ServiceCatalogAppRegistry @ default

## Resource AWS.ServiceCatalogAppRegistry/Application@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.ServiceCatalogAppRegistry/ApplicationProperties](#awsservicecatalogappregistryapplicationproperties) (Required): properties of the resource

## Resource AWS.ServiceCatalogAppRegistry/AttributeGroup@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.ServiceCatalogAppRegistry/AttributeGroupProperties](#awsservicecatalogappregistryattributegroupproperties) (Required): properties of the resource

## AWS.ServiceCatalogAppRegistry/ApplicationProperties
### Properties
* **Arn**: string (ReadOnly)
* **Description**: string: The description of the application. 
* **Id**: string (ReadOnly, Identifier)
* **Name**: string (Required): The name of the application. 
* **Tags**: [Tags](#tags)

## Tags
### Properties

## AWS.ServiceCatalogAppRegistry/AttributeGroupProperties
### Properties
* **Arn**: string (ReadOnly)
* **Attributes**: [AttributeGroup_Attributes](#attributegroupattributes) (Required)
* **Description**: string: The description of the attribute group. 
* **Id**: string (ReadOnly, Identifier)
* **Name**: string (Required): The name of the attribute group. 
* **Tags**: [Tags](#tags)

## AttributeGroup_Attributes
### Properties

## Tags
### Properties

