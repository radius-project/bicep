param deployTimeParam string = 'steve'
//@[13:16]     "deployTimeParam": {
var deployTimeVar = 'nigel'
//@[19:19]     "deployTimeVar": "nigel",
var dependentVar = {
//@[20:25]     "dependentVar": {
  dependencies: [
//@[21:24]       "dependencies": [
    deployTimeVar
//@[22:22]         "[variables('deployTimeVar')]",
    deployTimeParam
//@[23:23]         "[parameters('deployTimeParam')]"
  ]
}

var resourceDependency = {
  dependenciesA: [
//@[47:53]           "dependenciesA": [
    resA.id
//@[48:48]             "[resourceInfo('resA').id]",
    resA.name
//@[49:49]             "[resourceInfo('resA').name]",
    resA.type
//@[50:50]             "[resourceInfo('resA').type]",
    resA.properties.deployTime
//@[51:51]             "[reference('resA').deployTime]",
    resA.properties.eTag
//@[52:52]             "[reference('resA').eTag]"
  ]
}

output resourceAType string = resA.type
//@[94:97]     "resourceAType": {
resource resA 'My.Rp/myResourceType@2020-01-01' = {
//@[32:40]     "resA": {
  name: 'resA'
  properties: {
//@[36:39]       "properties": {
    deployTime: dependentVar
//@[37:37]         "deployTime": "[variables('dependentVar')]",
    eTag: '1234'
//@[38:38]         "eTag": "1234"
  }
}

output resourceBId string = resB.id
//@[98:101]     "resourceBId": {
resource resB 'My.Rp/myResourceType@2020-01-01' = {
//@[41:59]     "resB": {
  name: 'resB'
  properties: {
//@[45:55]       "properties": {
    dependencies: resourceDependency
//@[46:54]         "dependencies": {
  }
}

var resourceIds = {
//@[26:29]     "resourceIds": {
  a: resA.id
//@[27:27]       "a": "[resourceInfo('resA').id]",
  b: resB.id
//@[28:28]       "b": "[resourceInfo('resB').id]"
}

resource resC 'My.Rp/myResourceType@2020-01-01' = {
//@[60:71]     "resC": {
  name: 'resC'
  properties: {
//@[64:66]       "properties": {
    resourceIds: resourceIds
//@[65:65]         "resourceIds": "[variables('resourceIds')]"
  }
}

resource resD 'My.Rp/myResourceType/childType@2020-01-01' = {
//@[72:80]     "resD": {
  name: '${resC.name}/resD'
  properties: {
//@[76:76]       "properties": {},
  }
}

resource resE 'My.Rp/myResourceType/childType@2020-01-01' = {
//@[81:91]     "resE": {
  name: 'resC/resD'
  properties: {
//@[85:87]       "properties": {
    resDRef: resD.id
//@[86:86]         "resDRef": "[resourceInfo('resD').id]"
  }
}

output resourceCProperties object = resC.properties
//@[102:105]     "resourceCProperties": {

