targetScope='tenant'

var deploymentLocation = deployment().location
//@[13:13]     "deploymentLocation": "[deployment().location]",

var scopesWithArmRepresentation = {
//@[14:16]     "scopesWithArmRepresentation": {
  tenant: tenant()
//@[15:15]       "tenant": "[tenant()]"
}

