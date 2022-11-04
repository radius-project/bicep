targetScope='managementGroup'

var deploymentLocation = deployment().location
//@[13:13]     "deploymentLocation": "[deployment().location]",

var scopesWithArmRepresentation = {
//@[14:17]     "scopesWithArmRepresentation": {
  tenant: tenant()
//@[15:15]       "tenant": "[tenant()]",
  managementGroup: managementGroup()
//@[16:16]       "managementGroup": "[managementGroup()]"
}

