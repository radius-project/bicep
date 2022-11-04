
@sys.description('this is deployTimeSuffix param')
//@[17:17]         "description": "this is deployTimeSuffix param"
param deployTimeSuffix string = newGuid()
//@[13:19]     "deployTimeSuffix": {

@sys.description('this module a')
//@[204:204]         "description": "this module a"
module modATest './modulea.bicep' = {
//@[112:206]     "modATest": {
  name: 'modATest'
//@[115:115]       "name": "modATest",
  params: {
    stringParamB: 'hello!'
//@[123:123]             "value": "hello!"
    objParam: {
//@[126:128]             "value": {
      a: 'b'
//@[127:127]               "a": "b"
    }
    arrayParam: [
//@[131:136]             "value": [
      {
        a: 'b'
//@[133:133]                 "a": "b"
      }
      'abc'
//@[135:135]               "abc"
    ]
  }
}


@sys.description('this module b')
//@[255:255]         "description": "this module b"
module modB './child/moduleb.bicep' = {
//@[207:257]     "modB": {
  name: 'modB'
//@[210:210]       "name": "modB",
  params: {
    location: 'West US'
//@[218:218]             "value": "West US"
  }
}

@sys.description('this is just module b with a condition')
//@[307:307]         "description": "this is just module b with a condition"
module modBWithCondition './child/moduleb.bicep' = if (1 + 1 == 2) {
//@[258:309]     "modBWithCondition": {
  name: 'modBWithCondition'
//@[262:262]       "name": "modBWithCondition",
  params: {
    location: 'East US'
//@[270:270]             "value": "East US"
  }
}

module modC './child/modulec.json' = {
//@[310:349]     "modC": {
  name: 'modC'
//@[313:313]       "name": "modC",
  params: {
    location: 'West US'
//@[321:321]             "value": "West US"
  }
}

module modCWithCondition './child/modulec.json' = if (2 - 1 == 1) {
//@[350:390]     "modCWithCondition": {
  name: 'modCWithCondition'
//@[354:354]       "name": "modCWithCondition",
  params: {
    location: 'East US'
//@[362:362]             "value": "East US"
  }
}

module optionalWithNoParams1 './child/optionalParams.bicep'= {
//@[391:450]     "optionalWithNoParams1": {
  name: 'optionalWithNoParams1'
//@[394:394]       "name": "optionalWithNoParams1",
}

module optionalWithNoParams2 './child/optionalParams.bicep'= {
//@[451:511]     "optionalWithNoParams2": {
  name: 'optionalWithNoParams2'
//@[454:454]       "name": "optionalWithNoParams2",
  params: {
  }
}

module optionalWithAllParams './child/optionalParams.bicep'= {
//@[512:585]     "optionalWithAllParams": {
  name: 'optionalWithNoParams3'
//@[515:515]       "name": "optionalWithNoParams3",
  params: {
    optionalString: 'abc'
//@[523:523]             "value": "abc"
    optionalInt: 42
//@[526:526]             "value": 42
    optionalObj: { }
//@[529:529]             "value": {}
    optionalArray: [ ]
//@[532:532]             "value": []
  }
}

resource resWithDependencies 'Mock.Rp/mockResource@2020-01-01' = {
//@[60:74]     "resWithDependencies": {
  name: 'harry'
  properties: {
//@[64:68]       "properties": {
    modADep: modATest.outputs.stringOutputA
//@[65:65]         "modADep": "[reference('modATest').outputs.stringOutputA.value]",
    modBDep: modB.outputs.myResourceId
//@[66:66]         "modBDep": "[reference('modB').outputs.myResourceId.value]",
    modCDep: modC.outputs.myResourceId
//@[67:67]         "modCDep": "[reference('modC').outputs.myResourceId.value]"
  }
}

module optionalWithAllParamsAndManualDependency './child/optionalParams.bicep'= {
//@[586:663]     "optionalWithAllParamsAndManualDependency": {
  name: 'optionalWithAllParamsAndManualDependency'
//@[589:589]       "name": "optionalWithAllParamsAndManualDependency",
  params: {
    optionalString: 'abc'
//@[597:597]             "value": "abc"
    optionalInt: 42
//@[600:600]             "value": 42
    optionalObj: { }
//@[603:603]             "value": {}
    optionalArray: [ ]
//@[606:606]             "value": []
  }
  dependsOn: [
    resWithDependencies
    optionalWithAllParams
  ]
}

module optionalWithImplicitDependency './child/optionalParams.bicep'= {
//@[664:741]     "optionalWithImplicitDependency": {
  name: 'optionalWithImplicitDependency'
//@[667:667]       "name": "optionalWithImplicitDependency",
  params: {
    optionalString: concat(resWithDependencies.id, optionalWithAllParamsAndManualDependency.name)
//@[675:675]             "value": "[concat(resourceInfo('resWithDependencies').id, 'optionalWithAllParamsAndManualDependency')]"
    optionalInt: 42
//@[678:678]             "value": 42
    optionalObj: { }
//@[681:681]             "value": {}
    optionalArray: [ ]
//@[684:684]             "value": []
  }
}

module moduleWithCalculatedName './child/optionalParams.bicep'= {
//@[742:819]     "moduleWithCalculatedName": {
  name: '${optionalWithAllParamsAndManualDependency.name}${deployTimeSuffix}'
//@[745:745]       "name": "[format('{0}{1}', 'optionalWithAllParamsAndManualDependency', parameters('deployTimeSuffix'))]",
  params: {
    optionalString: concat(resWithDependencies.id, optionalWithAllParamsAndManualDependency.name)
//@[753:753]             "value": "[concat(resourceInfo('resWithDependencies').id, 'optionalWithAllParamsAndManualDependency')]"
    optionalInt: 42
//@[756:756]             "value": 42
    optionalObj: { }
//@[759:759]             "value": {}
    optionalArray: [ ]
//@[762:762]             "value": []
  }
}

resource resWithCalculatedNameDependencies 'Mock.Rp/mockResource@2020-01-01' = {
//@[75:86]     "resWithCalculatedNameDependencies": {
  name: '${optionalWithAllParamsAndManualDependency.name}${deployTimeSuffix}'
  properties: {
//@[79:81]       "properties": {
    modADep: moduleWithCalculatedName.outputs.outputObj
//@[80:80]         "modADep": "[reference('moduleWithCalculatedName').outputs.outputObj.value]"
  }
}

output stringOutputA string = modATest.outputs.stringOutputA
//@[2060:2063]     "stringOutputA": {
output stringOutputB string = modATest.outputs.stringOutputB
//@[2064:2067]     "stringOutputB": {
output objOutput object = modATest.outputs.objOutput
//@[2068:2071]     "objOutput": {
output arrayOutput array = modATest.outputs.arrayOutput
//@[2072:2075]     "arrayOutput": {
output modCalculatedNameOutput object = moduleWithCalculatedName.outputs.outputObj
//@[2076:2079]     "modCalculatedNameOutput": {

/*
  valid loop cases
*/

@sys.description('this is myModules')
var myModules = [
//@[22:31]     "myModules": [
  {
    name: 'one'
//@[24:24]         "name": "one",
    location: 'eastus2'
//@[25:25]         "location": "eastus2"
  }
  {
    name: 'two'
//@[28:28]         "name": "two",
    location: 'westus'
//@[29:29]         "location": "westus"
  }
]

var emptyArray = []
//@[32:32]     "emptyArray": [],

// simple module loop
module storageResources 'modulea.bicep' = [for module in myModules: {
//@[820:908]     "storageResources": {
  name: module.name
//@[827:827]       "name": "[variables('myModules')[copyIndex()].name]",
  params: {
    arrayParam: []
//@[835:835]             "value": []
    objParam: module
//@[838:838]             "value": "[variables('myModules')[copyIndex()]]"
    stringParamB: module.location
//@[841:841]             "value": "[variables('myModules')[copyIndex()].location]"
  }
}]

// simple indexed module loop
module storageResourcesWithIndex 'modulea.bicep' = [for (module, i) in myModules: {
//@[909:1002]     "storageResourcesWithIndex": {
  name: module.name
//@[916:916]       "name": "[variables('myModules')[copyIndex()].name]",
  params: {
    arrayParam: [
//@[924:926]             "value": [
      i + 1
//@[925:925]               "[add(copyIndex(), 1)]"
    ]
    objParam: module
//@[929:929]             "value": "[variables('myModules')[copyIndex()]]"
    stringParamB: module.location
//@[932:932]             "value": "[variables('myModules')[copyIndex()].location]"
    stringParamA: concat('a', i)
//@[935:935]             "value": "[concat('a', copyIndex())]"
  }
}]

// nested module loop
module nestedModuleLoop 'modulea.bicep' = [for module in myModules: {
//@[1003:1097]     "nestedModuleLoop": {
  name: module.name
//@[1010:1010]       "name": "[variables('myModules')[copyIndex()].name]",
  params: {
    arrayParam: [for i in range(0,3): concat('test-', i, '-', module.name)]
//@[1019:1023]                 "name": "value",
    objParam: module
//@[1027:1027]             "value": "[variables('myModules')[copyIndex()]]"
    stringParamB: module.location
//@[1030:1030]             "value": "[variables('myModules')[copyIndex()].location]"
  }
}]

// duplicate identifiers across scopes are allowed (inner hides the outer)
module duplicateIdentifiersWithinLoop 'modulea.bicep' = [for x in emptyArray:{
//@[1098:1195]     "duplicateIdentifiersWithinLoop": {
  name: 'hello-${x}'
//@[1105:1105]       "name": "[format('hello-{0}', variables('emptyArray')[copyIndex()])]",
  params: {
    objParam: {}
//@[1113:1113]             "value": {}
    stringParamA: 'test'
//@[1116:1116]             "value": "test"
    stringParamB: 'test'
//@[1119:1119]             "value": "test"
    arrayParam: [for x in emptyArray: x]
//@[1123:1127]                 "name": "value",
  }
}]

// duplicate identifiers across scopes are allowed (inner hides the outer)
var duplicateAcrossScopes = 'hello'
//@[33:33]     "duplicateAcrossScopes": "hello",
module duplicateInGlobalAndOneLoop 'modulea.bicep' = [for duplicateAcrossScopes in []: {
//@[1196:1293]     "duplicateInGlobalAndOneLoop": {
  name: 'hello-${duplicateAcrossScopes}'
//@[1203:1203]       "name": "[format('hello-{0}', createArray()[copyIndex()])]",
  params: {
    objParam: {}
//@[1211:1211]             "value": {}
    stringParamA: 'test'
//@[1214:1214]             "value": "test"
    stringParamB: 'test'
//@[1217:1217]             "value": "test"
    arrayParam: [for x in emptyArray: x]
//@[1221:1225]                 "name": "value",
  }
}]

var someDuplicate = true
//@[34:34]     "someDuplicate": true,
var otherDuplicate = false
//@[35:35]     "otherDuplicate": false,
module duplicatesEverywhere 'modulea.bicep' = [for someDuplicate in []: {
//@[1294:1388]     "duplicatesEverywhere": {
  name: 'hello-${someDuplicate}'
//@[1301:1301]       "name": "[format('hello-{0}', createArray()[copyIndex()])]",
  params: {
    objParam: {}
//@[1309:1309]             "value": {}
    stringParamB: 'test'
//@[1312:1312]             "value": "test"
    arrayParam: [for otherDuplicate in emptyArray: '${someDuplicate}-${otherDuplicate}']
//@[1316:1320]                 "name": "value",
  }
}]

module propertyLoopInsideParameterValue 'modulea.bicep' = {
//@[1389:1512]     "propertyLoopInsideParameterValue": {
  name: 'propertyLoopInsideParameterValue'
//@[1392:1392]       "name": "propertyLoopInsideParameterValue",
  params: {
    objParam: {
//@[1400:1429]             "value": {
      a: [for i in range(0,10): i]
//@[1402:1406]                   "name": "a",
      b: [for i in range(1,2): i]
//@[1407:1411]                   "name": "b",
      c: {
//@[1420:1428]               "c": {
        d: [for j in range(2,3): j]
//@[1422:1426]                     "name": "d",
      }
      e: [for k in range(4,4): {
//@[1412:1418]                   "name": "e",
        f: k
//@[1416:1416]                     "f": "[range(4, 4)[copyIndex('e')]]"
      }]
    }
    stringParamB: ''
//@[1432:1432]             "value": ""
    arrayParam: [
//@[1435:1445]             "value": [
      {
        e: [for j in range(7,7): j]
//@[1438:1442]                     "name": "e",
      }
    ]
  }
}

module propertyLoopInsideParameterValueWithIndexes 'modulea.bicep' = {
//@[1513:1637]     "propertyLoopInsideParameterValueWithIndexes": {
  name: 'propertyLoopInsideParameterValueWithIndexes'
//@[1516:1516]       "name": "propertyLoopInsideParameterValueWithIndexes",
  params: {
    objParam: {
//@[1524:1554]             "value": {
      a: [for (i, i2) in range(0,10): i + i2]
//@[1526:1530]                   "name": "a",
      b: [for (i, i2) in range(1,2): i / i2]
//@[1531:1535]                   "name": "b",
      c: {
//@[1545:1553]               "c": {
        d: [for (j, j2) in range(2,3): j * j2]
//@[1547:1551]                     "name": "d",
      }
      e: [for (k, k2) in range(4,4): {
//@[1536:1543]                   "name": "e",
        f: k
//@[1540:1540]                     "f": "[range(4, 4)[copyIndex('e')]]",
        g: k2
//@[1541:1541]                     "g": "[copyIndex('e')]"
      }]
    }
    stringParamB: ''
//@[1557:1557]             "value": ""
    arrayParam: [
//@[1560:1570]             "value": [
      {
        e: [for j in range(7,7): j]
//@[1563:1567]                     "name": "e",
      }
    ]
  }
}

module propertyLoopInsideParameterValueInsideModuleLoop 'modulea.bicep' = [for thing in range(0,1): {
//@[1638:1765]     "propertyLoopInsideParameterValueInsideModuleLoop": {
  name: 'propertyLoopInsideParameterValueInsideModuleLoop'
//@[1645:1645]       "name": "propertyLoopInsideParameterValueInsideModuleLoop",
  params: {
    objParam: {
//@[1653:1682]             "value": {
      a: [for i in range(0,10): i + thing]
//@[1655:1659]                   "name": "a",
      b: [for i in range(1,2): i * thing]
//@[1660:1664]                   "name": "b",
      c: {
//@[1673:1681]               "c": {
        d: [for j in range(2,3): j]
//@[1675:1679]                     "name": "d",
      }
      e: [for k in range(4,4): {
//@[1665:1671]                   "name": "e",
        f: k - thing
//@[1669:1669]                     "f": "[sub(range(4, 4)[copyIndex('e')], range(0, 1)[copyIndex()])]"
      }]
    }
    stringParamB: ''
//@[1685:1685]             "value": ""
    arrayParam: [
//@[1688:1698]             "value": [
      {
        e: [for j in range(7,7): j % thing]
//@[1691:1695]                     "name": "e",
      }
    ]
  }
}]


// BEGIN: Key Vault Secret Reference

resource kv 'Microsoft.KeyVault/vaults@2019-09-01' existing = {
//@[87:92]     "kv": {
  name: 'testkeyvault'
}

module secureModule1 'child/secureParams.bicep' = {
//@[1766:1824]     "secureModule1": {
  name: 'secureModule1'
//@[1769:1769]       "name": "secureModule1",
  params: {
    secureStringParam1: kv.getSecret('mySecret')
    secureStringParam2: kv.getSecret('mySecret','secretVersion')
  }
}

resource scopedKv 'Microsoft.KeyVault/vaults@2019-09-01' existing = {
//@[93:99]     "scopedKv": {
  name: 'testkeyvault'
  scope: resourceGroup('otherGroup')
}

module secureModule2 'child/secureParams.bicep' = {
//@[1825:1883]     "secureModule2": {
  name: 'secureModule2'
//@[1828:1828]       "name": "secureModule2",
  params: {
    secureStringParam1: scopedKv.getSecret('mySecret')
    secureStringParam2: scopedKv.getSecret('mySecret','secretVersion')
  }
}

//looped module with looped existing resource (Issue #2862)
var vaults = [
//@[36:47]     "vaults": [
  {
    vaultName: 'test-1-kv'
//@[38:38]         "vaultName": "test-1-kv",
    vaultRG: 'test-1-rg'
//@[39:39]         "vaultRG": "test-1-rg",
    vaultSub: 'abcd-efgh'
//@[40:40]         "vaultSub": "abcd-efgh"
  }
  {
    vaultName: 'test-2-kv'
//@[43:43]         "vaultName": "test-2-kv",
    vaultRG: 'test-2-rg'
//@[44:44]         "vaultRG": "test-2-rg",
    vaultSub: 'ijkl-1adg1'
//@[45:45]         "vaultSub": "ijkl-1adg1"
  }
]
var secrets = [
//@[48:57]     "secrets": [
  {
    name: 'secret01'
//@[50:50]         "name": "secret01",
    version: 'versionA'
//@[51:51]         "version": "versionA"
  }
  {
    name: 'secret02'
//@[54:54]         "name": "secret02",
    version: 'versionB'
//@[55:55]         "version": "versionB"
  }
]

resource loopedKv 'Microsoft.KeyVault/vaults@2019-09-01' existing = [for vault in vaults: {
//@[100:111]     "loopedKv": {
  name: vault.vaultName
  scope: resourceGroup(vault.vaultSub, vault.vaultRG)
}]

module secureModuleLooped 'child/secureParams.bicep' = [for (secret, i) in secrets: {
//@[1884:1946]     "secureModuleLooped": {
  name: 'secureModuleLooped-${i}'
//@[1891:1891]       "name": "[format('secureModuleLooped-{0}', copyIndex())]",
  params: {
    secureStringParam1: loopedKv[i].getSecret(secret.name)
    secureStringParam2: loopedKv[i].getSecret(secret.name, secret.version)
  }
}]


// END: Key Vault Secret Reference

module withSpace 'module with space.bicep' = {
//@[1947:1983]     "withSpace": {
  name: 'withSpace'
//@[1950:1950]       "name": "withSpace",
}

module folderWithSpace 'child/folder with space/child with space.bicep' = {
//@[1984:2020]     "folderWithSpace": {
  name: 'childWithSpace'
//@[1987:1987]       "name": "childWithSpace",
}

module withSeparateConfig './child/folder with separate config/moduleWithAzImport.bicep' = {
//@[2021:2057]     "withSeparateConfig": {
  name: 'withSeparateConfig'
//@[2024:2024]       "name": "withSeparateConfig",
}

