# AWS.LakeFormation @ default

## Resource AWS.LakeFormation/Tag@default
* **Valid Scope(s)**: Unknown
### Properties
* **name**: string: the resource name
* **properties**: [AWS.LakeFormation/TagProperties](#awslakeformationtagproperties) (Required): properties of the resource

## AWS.LakeFormation/TagProperties
### Properties
* **CatalogId**: [CatalogIdString](#catalogidstring): The identifier for the Data Catalog. By default, the account ID. The Data Catalog is the persistent metadata store. It contains database definitions, table definitions, and other control information to manage your Lake Formation environment.
* **TagKey**: [LFTagKey](#lftagkey) (Required): The key-name for the LF-tag.
* **TagValues**: [TagValueList](#tagvaluelist) (Required): A list of possible values an attribute can take.

## CatalogIdString
### Properties

## LFTagKey
### Properties

## TagValueList
### Properties

