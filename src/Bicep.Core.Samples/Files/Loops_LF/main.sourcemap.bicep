param name string
//@[13:15]     "name": {
param accounts array
//@[16:18]     "accounts": {
param index int
//@[19:21]     "index": {

// single resource
resource singleResource 'Microsoft.Storage/storageAccounts@2019-06-01' = {
//@[45:54]     "singleResource": {
  name: '${name}single-resource-name'
  location: resourceGroup().location
//@[49:49]       "location": "[resourceGroup().location]",
  kind: 'StorageV2'
//@[50:50]       "kind": "StorageV2",
  sku: {
//@[51:53]       "sku": {
    name: 'Standard_LRS'
//@[52:52]         "name": "Standard_LRS"
  }
}

// extension of single resource
resource singleResourceExtension 'Microsoft.Authorization/locks@2016-09-01' = {
//@[55:66]     "singleResourceExtension": {
  scope: singleResource
  name: 'single-resource-lock'
  properties: {
//@[60:62]       "properties": {
    level: 'CanNotDelete'
//@[61:61]         "level": "CanNotDelete"
  }
}

// single resource cascade extension
resource singleResourceCascadeExtension 'Microsoft.Authorization/locks@2016-09-01' = {
//@[67:78]     "singleResourceCascadeExtension": {
  scope: singleResourceExtension
  name: 'single-resource-cascade-extension'
  properties: {
//@[72:74]       "properties": {
    level: 'CanNotDelete'
//@[73:73]         "level": "CanNotDelete"
  }
}

// resource collection
@batchSize(42)
resource storageAccounts 'Microsoft.Storage/storageAccounts@2019-06-01' = [for account in accounts: {
//@[79:97]     "storageAccounts": {
  name: '${name}-collection-${account.name}'
  location: account.location
//@[89:89]       "location": "[parameters('accounts')[copyIndex()].location]",
  kind: 'StorageV2'
//@[90:90]       "kind": "StorageV2",
  sku: {
//@[91:93]       "sku": {
    name: 'Standard_LRS'
//@[92:92]         "name": "Standard_LRS"
  }
  dependsOn: [
    singleResource
  ]
}]

// extension of a single resource in a collection
resource extendSingleResourceInCollection 'Microsoft.Authorization/locks@2016-09-01' = {
//@[98:109]     "extendSingleResourceInCollection": {
  name: 'one-resource-collection-item-lock'
  properties: {
//@[103:105]       "properties": {
    level: 'ReadOnly'
//@[104:104]         "level": "ReadOnly"
  }
  scope: storageAccounts[index % 2]
}

// collection of extensions
resource extensionCollection 'Microsoft.Authorization/locks@2016-09-01' = [for i in range(0,1): {
//@[110:125]     "extensionCollection": {
  name: 'lock-${i}'
  properties: {
//@[119:121]       "properties": {
    level: i == 0 ? 'CanNotDelete' : 'ReadOnly'
//@[120:120]         "level": "[if(equals(range(0, 1)[copyIndex()], 0), 'CanNotDelete', 'ReadOnly')]"
  }
  scope: singleResource
}]

// cascade extend the extension
@batchSize(1)
resource lockTheLocks 'Microsoft.Authorization/locks@2016-09-01' = [for i in range(0,1): {
//@[126:143]     "lockTheLocks": {
  name: 'lock-the-lock-${i}'
  properties: {
//@[137:139]       "properties": {
    level: i == 0 ? 'CanNotDelete' : 'ReadOnly'
//@[138:138]         "level": "[if(equals(range(0, 1)[copyIndex()], 0), 'CanNotDelete', 'ReadOnly')]"
  }
  scope: extensionCollection[i]
}]

// special case property access
output indexedCollectionBlobEndpoint string = storageAccounts[index].properties.primaryEndpoints.blob
//@[811:814]     "indexedCollectionBlobEndpoint": {
output indexedCollectionName string = storageAccounts[index].name
//@[815:818]     "indexedCollectionName": {
output indexedCollectionId string = storageAccounts[index].id
//@[819:822]     "indexedCollectionId": {
output indexedCollectionType string = storageAccounts[index].type
//@[823:826]     "indexedCollectionType": {
output indexedCollectionVersion string = storageAccounts[index].apiVersion
//@[827:830]     "indexedCollectionVersion": {

// general case property access
output indexedCollectionIdentity object = storageAccounts[index].identity
//@[831:834]     "indexedCollectionIdentity": {

// indexed access of two properties
output indexedEndpointPair object = {
//@[835:841]     "indexedEndpointPair": {
  primary: storageAccounts[index].properties.primaryEndpoints.blob
//@[838:838]         "primary": "[reference(format('storageAccounts[{0}]', parameters('index'))).primaryEndpoints.blob]",
  secondary: storageAccounts[index + 1].properties.secondaryEndpoints.blob
//@[839:839]         "secondary": "[reference(format('storageAccounts[{0}]', add(parameters('index'), 1))).secondaryEndpoints.blob]"
}

// nested indexer?
output indexViaReference string = storageAccounts[int(storageAccounts[index].properties.creationTime)].properties.accessTier
//@[842:845]     "indexViaReference": {

// dependency on a resource collection
resource storageAccounts2 'Microsoft.Storage/storageAccounts@2019-06-01' = [for account in accounts: {
//@[144:160]     "storageAccounts2": {
  name: '${name}-collection-${account.name}'
  location: account.location
//@[152:152]       "location": "[parameters('accounts')[copyIndex()].location]",
  kind: 'StorageV2'
//@[153:153]       "kind": "StorageV2",
  sku: {
//@[154:156]       "sku": {
    name: 'Standard_LRS'
//@[155:155]         "name": "Standard_LRS"
  }
  dependsOn: [
    storageAccounts
  ]
}]

// one-to-one paired dependencies
resource firstSet 'Microsoft.Storage/storageAccounts@2019-06-01' = [for i in range(0, length(accounts)): {
//@[161:174]     "firstSet": {
  name: '${name}-set1-${i}'
  location: resourceGroup().location
//@[169:169]       "location": "[resourceGroup().location]",
  kind: 'StorageV2'
//@[170:170]       "kind": "StorageV2",
  sku: {
//@[171:173]       "sku": {
    name: 'Standard_LRS'
//@[172:172]         "name": "Standard_LRS"
  }
}]

resource secondSet 'Microsoft.Storage/storageAccounts@2019-06-01' = [for i in range(0, length(accounts)): {
//@[175:191]     "secondSet": {
  name: '${name}-set2-${i}'
  location: resourceGroup().location
//@[183:183]       "location": "[resourceGroup().location]",
  kind: 'StorageV2'
//@[184:184]       "kind": "StorageV2",
  sku: {
//@[185:187]       "sku": {
    name: 'Standard_LRS'
//@[186:186]         "name": "Standard_LRS"
  }
  dependsOn: [
    firstSet[i]
  ]
}]

// depending on collection and one resource in the collection optimizes the latter part away
resource anotherSingleResource 'Microsoft.Storage/storageAccounts@2019-06-01' = {
//@[192:204]     "anotherSingleResource": {
  name: '${name}single-resource-name'
  location: resourceGroup().location
//@[196:196]       "location": "[resourceGroup().location]",
  kind: 'StorageV2'
//@[197:197]       "kind": "StorageV2",
  sku: {
//@[198:200]       "sku": {
    name: 'Standard_LRS'
//@[199:199]         "name": "Standard_LRS"
  }
  dependsOn: [
    secondSet
    secondSet[0]
  ]
}

// vnets
var vnetConfigurations = [
//@[24:33]     "vnetConfigurations": [
  {
    name: 'one'
//@[26:26]         "name": "one",
    location: resourceGroup().location
//@[27:27]         "location": "[resourceGroup().location]"
  }
  {
    name: 'two'
//@[30:30]         "name": "two",
    location: 'westus'
//@[31:31]         "location": "westus"
  }
]

resource vnets 'Microsoft.Network/virtualNetworks@2020-06-01' = [for vnetConfig in vnetConfigurations: {
//@[205:214]     "vnets": {
  name: vnetConfig.name
  location: vnetConfig.location
//@[213:213]       "location": "[variables('vnetConfigurations')[copyIndex()].location]"
}]

// implicit dependency on single resource from a resource collection
resource implicitDependencyOnSingleResourceByIndex 'Microsoft.Network/dnsZones@2018-05-01' = {
//@[215:230]     "implicitDependencyOnSingleResourceByIndex": {
  name: 'test'
  location: 'global'
//@[219:219]       "location": "global",
  properties: {
//@[220:226]       "properties": {
    resolutionVirtualNetworks: [
//@[221:225]         "resolutionVirtualNetworks": [
      {
        id: vnets[index+1].id
//@[223:223]             "id": "[resourceInfo(format('vnets[{0}]', add(parameters('index'), 1))).id]"
      }
    ]
  }
}

// implicit and explicit dependency combined
resource combinedDependencies 'Microsoft.Network/dnsZones@2018-05-01' = {
//@[231:249]     "combinedDependencies": {
  name: 'test2'
  location: 'global'
//@[235:235]       "location": "global",
  properties: {
//@[236:245]       "properties": {
    resolutionVirtualNetworks: [
//@[237:244]         "resolutionVirtualNetworks": [
      {
        id: vnets[index-1].id
//@[239:239]             "id": "[resourceInfo(format('vnets[{0}]', sub(parameters('index'), 1))).id]"
      }
      {
        id: vnets[index * 2].id
//@[242:242]             "id": "[resourceInfo(format('vnets[{0}]', mul(parameters('index'), 2))).id]"
      }
    ]
  }
  dependsOn: [
    vnets
  ]
}

// single module
module singleModule 'passthrough.bicep' = {
//@[435:475]     "singleModule": {
  name: 'test'
//@[438:438]       "name": "test",
  params: {
    myInput: 'hello'
//@[446:446]             "value": "hello"
  }
}

var moduleSetup = [
//@[34:38]     "moduleSetup": [
  'one'
//@[35:35]       "one",
  'two'
//@[36:36]       "two",
  'three'
//@[37:37]       "three"
]

// module collection plus explicit dependency on single module
@sys.batchSize(3)
module moduleCollectionWithSingleDependency 'passthrough.bicep' = [for moduleName in moduleSetup: {
//@[476:526]     "moduleCollectionWithSingleDependency": {
  name: moduleName
//@[485:485]       "name": "[variables('moduleSetup')[copyIndex()]]",
  params: {
    myInput: 'in-${moduleName}'
//@[493:493]             "value": "[format('in-{0}', variables('moduleSetup')[copyIndex()])]"
  }
  dependsOn: [
    singleModule
    singleResource
  ]
}]

// another module collection with dependency on another module collection
module moduleCollectionWithCollectionDependencies 'passthrough.bicep' = [for moduleName in moduleSetup: {
//@[527:575]     "moduleCollectionWithCollectionDependencies": {
  name: moduleName
//@[534:534]       "name": "[variables('moduleSetup')[copyIndex()]]",
  params: {
    myInput: 'in-${moduleName}'
//@[542:542]             "value": "[format('in-{0}', variables('moduleSetup')[copyIndex()])]"
  }
  dependsOn: [
    storageAccounts
    moduleCollectionWithSingleDependency
  ]
}]

module singleModuleWithIndexedDependencies 'passthrough.bicep' = {
//@[576:621]     "singleModuleWithIndexedDependencies": {
  name: 'hello'
//@[579:579]       "name": "hello",
  params: {
    myInput: concat(moduleCollectionWithCollectionDependencies[index].outputs.myOutput, storageAccounts[index * 3].properties.accessTier)
//@[587:587]             "value": "[concat(reference(format('moduleCollectionWithCollectionDependencies[{0}]', parameters('index'))).outputs.myOutput.value, reference(format('storageAccounts[{0}]', mul(parameters('index'), 3))).accessTier)]"
  }
  dependsOn: [
    storageAccounts2[index - 10]
  ]
}

module moduleCollectionWithIndexedDependencies 'passthrough.bicep' = [for moduleName in moduleSetup: {
//@[622:671]     "moduleCollectionWithIndexedDependencies": {
  name: moduleName
//@[629:629]       "name": "[variables('moduleSetup')[copyIndex()]]",
  params: {
    myInput: '${moduleCollectionWithCollectionDependencies[index].outputs.myOutput} - ${storageAccounts[index * 3].properties.accessTier} - ${moduleName}'
//@[637:637]             "value": "[format('{0} - {1} - {2}', reference(format('moduleCollectionWithCollectionDependencies[{0}]', parameters('index'))).outputs.myOutput.value, reference(format('storageAccounts[{0}]', mul(parameters('index'), 3))).accessTier, variables('moduleSetup')[copyIndex()])]"
  }
  dependsOn: [
    storageAccounts2[index - 9]
  ]
}]

output indexedModulesName string = moduleCollectionWithSingleDependency[index].name
//@[846:849]     "indexedModulesName": {
output indexedModuleOutput string = moduleCollectionWithSingleDependency[index * 1].outputs.myOutput
//@[850:853]     "indexedModuleOutput": {

// resource collection
resource existingStorageAccounts 'Microsoft.Storage/storageAccounts@2019-06-01' existing = [for account in accounts: {
//@[250:259]     "existingStorageAccounts": {
  name: '${name}-existing-${account.name}'
}]

output existingIndexedResourceName string = existingStorageAccounts[index * 0].name
//@[854:857]     "existingIndexedResourceName": {
output existingIndexedResourceId string = existingStorageAccounts[index * 1].id
//@[858:861]     "existingIndexedResourceId": {
output existingIndexedResourceType string = existingStorageAccounts[index+2].type
//@[862:865]     "existingIndexedResourceType": {
output existingIndexedResourceApiVersion string = existingStorageAccounts[index-7].apiVersion
//@[866:869]     "existingIndexedResourceApiVersion": {
output existingIndexedResourceLocation string = existingStorageAccounts[index/2].location
//@[870:873]     "existingIndexedResourceLocation": {
output existingIndexedResourceAccessTier string = existingStorageAccounts[index%3].properties.accessTier
//@[874:877]     "existingIndexedResourceAccessTier": {

resource duplicatedNames 'Microsoft.Network/dnsZones@2018-05-01' = [for zone in []: {
//@[260:269]     "duplicatedNames": {
  name: 'no loop variable'
  location: 'eastus'
//@[268:268]       "location": "eastus"
}]

// reference to a resource collection whose name expression does not reference any loop variables
resource referenceToDuplicateNames 'Microsoft.Network/dnsZones@2018-05-01' = [for zone in []: {
//@[270:282]     "referenceToDuplicateNames": {
  name: 'no loop variable 2'
  location: 'eastus'
//@[278:278]       "location": "eastus",
  dependsOn: [
    duplicatedNames[index]
  ]
}]

var regions = [
//@[39:42]     "regions": [
  'eastus'
//@[40:40]       "eastus",
  'westus'
//@[41:41]       "westus"
]

module apim 'passthrough.bicep' = [for region in regions: {
//@[672:716]     "apim": {
  name: 'apim-${region}-${name}'
//@[679:679]       "name": "[format('apim-{0}-{1}', variables('regions')[copyIndex()], parameters('name'))]",
  params: {
    myInput: region
//@[687:687]             "value": "[variables('regions')[copyIndex()]]"
  }
}]

resource propertyLoopDependencyOnModuleCollection 'Microsoft.Network/frontDoors@2020-05-01' = {
//@[283:314]     "propertyLoopDependencyOnModuleCollection": {
  name: name
  location: 'Global'
//@[287:287]       "location": "Global",
  properties: {
//@[288:310]       "properties": {
    backendPools: [
//@[289:309]         "backendPools": [
      {
        name: 'BackendAPIMs'
//@[291:291]             "name": "BackendAPIMs",
        properties: {
//@[292:307]             "properties": {
          backends: [for index in range(0, length(regions)): {
//@[294:305]                   "name": "backends",
            // we cannot codegen index correctly because the generated dependsOn property
            // would be outside of the scope of the property loop
            // as a result, this will generate a dependency on the entire collection
            address: apim[index].outputs.myOutput
//@[298:298]                     "address": "[reference(format('apim[{0}]', range(0, length(variables('regions')))[copyIndex('backends')])).outputs.myOutput.value]",
            backendHostHeader: apim[index].outputs.myOutput
//@[299:299]                     "backendHostHeader": "[reference(format('apim[{0}]', range(0, length(variables('regions')))[copyIndex('backends')])).outputs.myOutput.value]",
            httpPort: 80
//@[300:300]                     "httpPort": 80,
            httpsPort: 443
//@[301:301]                     "httpsPort": 443,
            priority: 1
//@[302:302]                     "priority": 1,
            weight: 50
//@[303:303]                     "weight": 50
          }]
        }
      }
    ]
  }
}

resource indexedModuleCollectionDependency 'Microsoft.Network/frontDoors@2020-05-01' = [for index in range(0, length(regions)): {
//@[315:347]     "indexedModuleCollectionDependency": {
  name: '${name}-${index}'
  location: 'Global'
//@[323:323]       "location": "Global",
  properties: {
//@[324:342]       "properties": {
    backendPools: [
//@[325:341]         "backendPools": [
      {
        name: 'BackendAPIMs'
//@[327:327]             "name": "BackendAPIMs",
        properties: {
//@[328:339]             "properties": {
          backends: [
//@[329:338]               "backends": [
            {
              // this indexed dependency on a module collection will be generated correctly because
              // copyIndex() can be invoked in the generated dependsOn
              address: apim[index].outputs.myOutput
//@[331:331]                   "address": "[reference(format('apim[{0}]', range(0, length(variables('regions')))[copyIndex()])).outputs.myOutput.value]",
              backendHostHeader: apim[index].outputs.myOutput
//@[332:332]                   "backendHostHeader": "[reference(format('apim[{0}]', range(0, length(variables('regions')))[copyIndex()])).outputs.myOutput.value]",
              httpPort: 80
//@[333:333]                   "httpPort": 80,
              httpsPort: 443
//@[334:334]                   "httpsPort": 443,
              priority: 1
//@[335:335]                   "priority": 1,
              weight: 50
//@[336:336]                   "weight": 50
            }
          ]
        }
      }
    ]
  }
}]

resource propertyLoopDependencyOnResourceCollection 'Microsoft.Network/frontDoors@2020-05-01' = {
//@[348:379]     "propertyLoopDependencyOnResourceCollection": {
  name: name
  location: 'Global'
//@[352:352]       "location": "Global",
  properties: {
//@[353:375]       "properties": {
    backendPools: [
//@[354:374]         "backendPools": [
      {
        name: 'BackendAPIMs'
//@[356:356]             "name": "BackendAPIMs",
        properties: {
//@[357:372]             "properties": {
          backends: [for index in range(0, length(accounts)): {
//@[359:370]                   "name": "backends",
            // we cannot codegen index correctly because the generated dependsOn property
            // would be outside of the scope of the property loop
            // as a result, this will generate a dependency on the entire collection
            address: storageAccounts[index].properties.primaryEndpoints.internetEndpoints.web
//@[363:363]                     "address": "[reference(format('storageAccounts[{0}]', range(0, length(parameters('accounts')))[copyIndex('backends')])).primaryEndpoints.internetEndpoints.web]",
            backendHostHeader: storageAccounts[index].properties.primaryEndpoints.internetEndpoints.web
//@[364:364]                     "backendHostHeader": "[reference(format('storageAccounts[{0}]', range(0, length(parameters('accounts')))[copyIndex('backends')])).primaryEndpoints.internetEndpoints.web]",
            httpPort: 80
//@[365:365]                     "httpPort": 80,
            httpsPort: 443
//@[366:366]                     "httpsPort": 443,
            priority: 1
//@[367:367]                     "priority": 1,
            weight: 50
//@[368:368]                     "weight": 50
          }]
        }
      }
    ]
  }
}

resource indexedResourceCollectionDependency 'Microsoft.Network/frontDoors@2020-05-01' = [for index in range(0, length(accounts)): {
//@[380:412]     "indexedResourceCollectionDependency": {
  name: '${name}-${index}'
  location: 'Global'
//@[388:388]       "location": "Global",
  properties: {
//@[389:407]       "properties": {
    backendPools: [
//@[390:406]         "backendPools": [
      {
        name: 'BackendAPIMs'
//@[392:392]             "name": "BackendAPIMs",
        properties: {
//@[393:404]             "properties": {
          backends: [
//@[394:403]               "backends": [
            {
              // this indexed dependency on a module collection will be generated correctly because
              // copyIndex() can be invoked in the generated dependsOn
              address: storageAccounts[index].properties.primaryEndpoints.internetEndpoints.web
//@[396:396]                   "address": "[reference(format('storageAccounts[{0}]', range(0, length(parameters('accounts')))[copyIndex()])).primaryEndpoints.internetEndpoints.web]",
              backendHostHeader: storageAccounts[index].properties.primaryEndpoints.internetEndpoints.web
//@[397:397]                   "backendHostHeader": "[reference(format('storageAccounts[{0}]', range(0, length(parameters('accounts')))[copyIndex()])).primaryEndpoints.internetEndpoints.web]",
              httpPort: 80
//@[398:398]                   "httpPort": 80,
              httpsPort: 443
//@[399:399]                   "httpsPort": 443,
              priority: 1
//@[400:400]                   "priority": 1,
              weight: 50
//@[401:401]                   "weight": 50
            }
          ]
        }
      }
    ]
  }
}]

resource filteredZones 'Microsoft.Network/dnsZones@2018-05-01' = [for i in range(0,10): if(i % 3 == 0) {
//@[413:423]     "filteredZones": {
  name: 'zone${i}'
  location: resourceGroup().location
//@[422:422]       "location": "[resourceGroup().location]"
}]

module filteredModules 'passthrough.bicep' = [for i in range(0,6): if(i % 2 == 0) {
//@[717:762]     "filteredModules": {
  name: 'stuff${i}'
//@[725:725]       "name": "[format('stuff{0}', range(0, 6)[copyIndex()])]",
  params: {
    myInput: 'script-${i}'
//@[733:733]             "value": "[format('script-{0}', range(0, 6)[copyIndex()])]"
  }
}]

resource filteredIndexedZones 'Microsoft.Network/dnsZones@2018-05-01' = [for (account, i) in accounts: if(account.enabled) {
//@[424:434]     "filteredIndexedZones": {
  name: 'indexedZone-${account.name}-${i}'
  location: account.location
//@[433:433]       "location": "[parameters('accounts')[copyIndex()].location]"
}]

output lastNameServers array = filteredIndexedZones[length(accounts) - 1].properties.nameServers
//@[878:881]     "lastNameServers": {

module filteredIndexedModules 'passthrough.bicep' = [for (account, i) in accounts: if(account.enabled) {
//@[763:808]     "filteredIndexedModules": {
  name: 'stuff-${i}'
//@[771:771]       "name": "[format('stuff-{0}', copyIndex())]",
  params: {
    myInput: 'script-${account.name}-${i}'
//@[779:779]             "value": "[format('script-{0}-{1}', parameters('accounts')[copyIndex()].name, copyIndex())]"
  }
}]

output lastModuleOutput string = filteredIndexedModules[length(accounts) - 1].outputs.myOutput
//@[882:885]     "lastModuleOutput": {

