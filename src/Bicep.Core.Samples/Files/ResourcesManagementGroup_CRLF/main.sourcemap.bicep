targetScope = 'managementGroup'

param ownerPrincipalId string
//@[13:15]     "ownerPrincipalId": {

param contributorPrincipals array
//@[16:18]     "contributorPrincipals": {
param readerPrincipals array
//@[19:21]     "readerPrincipals": {

resource owner 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
//@[24:32]     "owner": {
  name: guid('owner', ownerPrincipalId)
  properties: {
//@[28:31]       "properties": {
    principalId: ownerPrincipalId
//@[29:29]         "principalId": "[parameters('ownerPrincipalId')]",
    roleDefinitionId: '8e3af657-a8ff-443c-a75c-2fe8c4bcb635'
//@[30:30]         "roleDefinitionId": "8e3af657-a8ff-443c-a75c-2fe8c4bcb635"
  }
}

resource contributors 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = [for contributor in contributorPrincipals: {
//@[33:48]     "contributors": {
  name: guid('contributor', contributor)
  properties: {
//@[41:44]       "properties": {
    principalId: contributor
//@[42:42]         "principalId": "[parameters('contributorPrincipals')[copyIndex()]]",
    roleDefinitionId: 'b24988ac-6180-42a0-ab88-20f7382dd24c'
//@[43:43]         "roleDefinitionId": "b24988ac-6180-42a0-ab88-20f7382dd24c"
  }
  dependsOn: [
    owner
  ]
}]

resource readers 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = [for reader in readerPrincipals: {
//@[49:65]     "readers": {
  name: guid('reader', reader)
  properties: {
//@[57:60]       "properties": {
    principalId: reader
//@[58:58]         "principalId": "[parameters('readerPrincipals')[copyIndex()]]",
    roleDefinitionId: 'b24988ac-6180-42a0-ab88-20f7382dd24c'
//@[59:59]         "roleDefinitionId": "b24988ac-6180-42a0-ab88-20f7382dd24c"
  }
  dependsOn: [
    owner
    contributors[0]
  ]
}]

resource single_mg 'Microsoft.Management/managementGroups@2020-05-01' = {
//@[66:71]     "single_mg": {
  scope: tenant()
  name: 'one-mg'
}

// Blueprints are read-only at tenant Scope, but it's a convenient example to use to validate this.
resource tenant_blueprint 'Microsoft.Blueprint/blueprints@2018-11-01-preview' = {
//@[72:78]     "tenant_blueprint": {
  name: 'tenant-blueprint'
  properties: {}
//@[77:77]       "properties": {}
  scope: tenant()
}

resource mg_blueprint 'Microsoft.Blueprint/blueprints@2018-11-01-preview' = {
//@[79:84]     "mg_blueprint": {
  name: 'mg-blueprint'
  properties: {}
//@[83:83]       "properties": {}
}

