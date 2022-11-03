targetScope='subscription'

var deploymentLocation = deployment().location
//@[13:13]     "deploymentLocation": "[deployment().location]",

var scopesWithArmRepresentation = {
//@[14:17]     "scopesWithArmRepresentation": {
  tenant: tenant()
//@[15:15]       "tenant": "[tenant()]",
  subscription: subscription()
//@[16:16]       "subscription": "[subscription()]"
}

