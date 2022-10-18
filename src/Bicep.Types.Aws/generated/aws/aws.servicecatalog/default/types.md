# AWS.ServiceCatalog @ default

## Resource AWS.ServiceCatalog/CloudFormationProvisionedProduct@default
* **Valid Scope(s)**: Unknown
### Properties
* **properties**: [AWS.ServiceCatalog/CloudFormationProvisionedProductProperties](#awsservicecatalogcloudformationprovisionedproductproperties): properties of the resource

## Resource AWS.ServiceCatalog/ServiceAction@default
* **Valid Scope(s)**: Unknown
### Properties
* **properties**: [AWS.ServiceCatalog/ServiceActionProperties](#awsservicecatalogserviceactionproperties) (Required): properties of the resource

## AWS.ServiceCatalog/CloudFormationProvisionedProductProperties
### Properties
* **AcceptLanguage**: string
* **CloudformationStackArn**: string (ReadOnly)
* **NotificationArns**: string[]
* **Outputs**: [CloudFormationProvisionedProduct_Outputs](#cloudformationprovisionedproductoutputs) (ReadOnly): List of key-value pair outputs.
* **PathId**: string
* **PathName**: string
* **ProductId**: string
* **ProductName**: string
* **ProvisionedProductId**: string (ReadOnly)
* **ProvisionedProductName**: string
* **ProvisioningArtifactId**: string
* **ProvisioningArtifactName**: string
* **ProvisioningParameters**: [ProvisioningParameter](#provisioningparameter)[]
* **ProvisioningPreferences**: [ProvisioningPreferences](#provisioningpreferences)
* **RecordId**: string (ReadOnly)
* **Tags**: [Tag](#tag)[]

## CloudFormationProvisionedProduct_Outputs
### Properties

## ProvisioningParameter
### Properties
* **Key**: string (Required)
* **Value**: string (Required)

## ProvisioningPreferences
### Properties
* **StackSetAccounts**: string[]
* **StackSetFailureToleranceCount**: int
* **StackSetFailureTolerancePercentage**: int
* **StackSetMaxConcurrencyCount**: int
* **StackSetMaxConcurrencyPercentage**: int
* **StackSetOperationType**: string
* **StackSetRegions**: string[]

## Tag
### Properties
* **Key**: string (Required)
* **Value**: string (Required)

## AWS.ServiceCatalog/ServiceActionProperties
### Properties
* **AcceptLanguage**: string (WriteOnly)
* **Definition**: [DefinitionParameter](#definitionparameter)[] (Required)
* **DefinitionType**: string (Required)
* **Description**: string
* **Id**: string (ReadOnly)
* **Name**: string (Required)

## DefinitionParameter
### Properties
* **Key**: string (Required)
* **Value**: string (Required)

