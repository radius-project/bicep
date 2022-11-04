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
resource storageAccounts 'Microsoft.Storage/storageAccounts@2019-06-01' = [for (account, index) in accounts: {
//@[79:97]     "storageAccounts": {
  name: '${name}-collection-${account.name}-${index}'
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
resource extensionCollection 'Microsoft.Authorization/locks@2016-09-01' = [for (i, i2) in range(0,1): {
//@[110:125]     "extensionCollection": {
  name: 'lock-${i}-${i2}'
  properties: {
//@[119:121]       "properties": {
    level: i == 0 && i2 == 0 ? 'CanNotDelete' : 'ReadOnly'
//@[120:120]         "level": "[if(and(equals(range(0, 1)[copyIndex()], 0), equals(copyIndex(), 0)), 'CanNotDelete', 'ReadOnly')]"
  }
  scope: singleResource
}]

// cascade extend the extension
@batchSize(1)
resource lockTheLocks 'Microsoft.Authorization/locks@2016-09-01' = [for (i, i2) in range(0,1): {
//@[126:143]     "lockTheLocks": {
  name: 'lock-the-lock-${i}-${i2}'
  properties: {
//@[137:139]       "properties": {
    level: i == 0 && i2 == 0 ? 'CanNotDelete' : 'ReadOnly'
//@[138:138]         "level": "[if(and(equals(range(0, 1)[copyIndex()], 0), equals(copyIndex(), 0)), 'CanNotDelete', 'ReadOnly')]"
  }
  scope: extensionCollection[i2]
}]

// special case property access
output indexedCollectionBlobEndpoint string = storageAccounts[index].properties.primaryEndpoints.blob
//@[697:700]     "indexedCollectionBlobEndpoint": {
output indexedCollectionName string = storageAccounts[index].name
//@[701:704]     "indexedCollectionName": {
output indexedCollectionId string = storageAccounts[index].id
//@[705:708]     "indexedCollectionId": {
output indexedCollectionType string = storageAccounts[index].type
//@[709:712]     "indexedCollectionType": {
output indexedCollectionVersion string = storageAccounts[index].apiVersion
//@[713:716]     "indexedCollectionVersion": {

// general case property access
output indexedCollectionIdentity object = storageAccounts[index].identity
//@[717:720]     "indexedCollectionIdentity": {

// indexed access of two properties
output indexedEndpointPair object = {
//@[721:727]     "indexedEndpointPair": {
  primary: storageAccounts[index].properties.primaryEndpoints.blob
//@[724:724]         "primary": "[reference(format('storageAccounts[{0}]', parameters('index'))).primaryEndpoints.blob]",
  secondary: storageAccounts[index + 1].properties.secondaryEndpoints.blob
//@[725:725]         "secondary": "[reference(format('storageAccounts[{0}]', add(parameters('index'), 1))).secondaryEndpoints.blob]"
}

// nested indexer?
output indexViaReference string = storageAccounts[int(storageAccounts[index].properties.creationTime)].properties.accessTier
//@[728:731]     "indexViaReference": {

// dependency on a resource collection
resource storageAccounts2 'Microsoft.Storage/storageAccounts@2019-06-01' = [for (account, idx) in accounts: {
//@[144:160]     "storageAccounts2": {
  name: '${name}-collection-${account.name}-${idx}'
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
resource firstSet 'Microsoft.Storage/storageAccounts@2019-06-01' = [for (i,ii) in range(0, length(accounts)): {
//@[161:174]     "firstSet": {
  name: '${name}-set1-${i}-${ii}'
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

resource secondSet 'Microsoft.Storage/storageAccounts@2019-06-01' = [for (i,iii) in range(0, length(accounts)): {
//@[175:191]     "secondSet": {
  name: '${name}-set2-${i}-${iii}'
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
    firstSet[iii]
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

resource vnets 'Microsoft.Network/virtualNetworks@2020-06-01' = [for (vnetConfig, index) in vnetConfigurations: {
//@[205:214]     "vnets": {
  name: '${vnetConfig.name}-${index}'
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
//@[413:453]     "singleModule": {
  name: 'test'
//@[416:416]       "name": "test",
  params: {
    myInput: 'hello'
//@[424:424]             "value": "hello"
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
module moduleCollectionWithSingleDependency 'passthrough.bicep' = [for (moduleName, moduleIndex) in moduleSetup: {
//@[454:504]     "moduleCollectionWithSingleDependency": {
  name: concat(moduleName, moduleIndex)
//@[463:463]       "name": "[concat(variables('moduleSetup')[copyIndex()], copyIndex())]",
  params: {
    myInput: 'in-${moduleName}-${moduleIndex}'
//@[471:471]             "value": "[format('in-{0}-{1}', variables('moduleSetup')[copyIndex()], copyIndex())]"
  }
  dependsOn: [
    singleModule
    singleResource
  ]
}]

// another module collection with dependency on another module collection
module moduleCollectionWithCollectionDependencies 'passthrough.bicep' = [for (moduleName, moduleIndex) in moduleSetup: {
//@[505:553]     "moduleCollectionWithCollectionDependencies": {
  name: concat(moduleName, moduleIndex)
//@[512:512]       "name": "[concat(variables('moduleSetup')[copyIndex()], copyIndex())]",
  params: {
    myInput: 'in-${moduleName}-${moduleIndex}'
//@[520:520]             "value": "[format('in-{0}-{1}', variables('moduleSetup')[copyIndex()], copyIndex())]"
  }
  dependsOn: [
    storageAccounts
    moduleCollectionWithSingleDependency
  ]
}]

module singleModuleWithIndexedDependencies 'passthrough.bicep' = {
//@[554:599]     "singleModuleWithIndexedDependencies": {
  name: 'hello'
//@[557:557]       "name": "hello",
  params: {
    myInput: concat(moduleCollectionWithCollectionDependencies[index].outputs.myOutput, storageAccounts[index * 3].properties.accessTier)
//@[565:565]             "value": "[concat(reference(format('moduleCollectionWithCollectionDependencies[{0}]', parameters('index'))).outputs.myOutput.value, reference(format('storageAccounts[{0}]', mul(parameters('index'), 3))).accessTier)]"
  }
  dependsOn: [
    storageAccounts2[index - 10]
  ]
}

module moduleCollectionWithIndexedDependencies 'passthrough.bicep' = [for (moduleName, moduleIndex) in moduleSetup: {
//@[600:649]     "moduleCollectionWithIndexedDependencies": {
  name: concat(moduleName, moduleIndex)
//@[607:607]       "name": "[concat(variables('moduleSetup')[copyIndex()], copyIndex())]",
  params: {
    myInput: '${moduleCollectionWithCollectionDependencies[index].outputs.myOutput} - ${storageAccounts[index * 3].properties.accessTier} - ${moduleName} - ${moduleIndex}'
//@[615:615]             "value": "[format('{0} - {1} - {2} - {3}', reference(format('moduleCollectionWithCollectionDependencies[{0}]', parameters('index'))).outputs.myOutput.value, reference(format('storageAccounts[{0}]', mul(parameters('index'), 3))).accessTier, variables('moduleSetup')[copyIndex()], copyIndex())]"
  }
  dependsOn: [
    storageAccounts2[index - 9]
  ]
}]

output indexedModulesName string = moduleCollectionWithSingleDependency[index].name
//@[732:735]     "indexedModulesName": {
output indexedModuleOutput string = moduleCollectionWithSingleDependency[index * 1].outputs.myOutput
//@[736:739]     "indexedModuleOutput": {

// resource collection
resource existingStorageAccounts 'Microsoft.Storage/storageAccounts@2019-06-01' existing = [for (account, i) in accounts: {
//@[250:259]     "existingStorageAccounts": {
  name: '${name}-existing-${account.name}-${i}'
}]

output existingIndexedResourceName string = existingStorageAccounts[index * 0].name
//@[740:743]     "existingIndexedResourceName": {
output existingIndexedResourceId string = existingStorageAccounts[index * 1].id
//@[744:747]     "existingIndexedResourceId": {
output existingIndexedResourceType string = existingStorageAccounts[index+2].type
//@[748:751]     "existingIndexedResourceType": {
output existingIndexedResourceApiVersion string = existingStorageAccounts[index-7].apiVersion
//@[752:755]     "existingIndexedResourceApiVersion": {
output existingIndexedResourceLocation string = existingStorageAccounts[index/2].location
//@[756:759]     "existingIndexedResourceLocation": {
output existingIndexedResourceAccessTier string = existingStorageAccounts[index%3].properties.accessTier
//@[760:763]     "existingIndexedResourceAccessTier": {

resource duplicatedNames 'Microsoft.Network/dnsZones@2018-05-01' = [for (zone,i) in []: {
//@[260:269]     "duplicatedNames": {
  name: 'no loop variable'
  location: 'eastus'
//@[268:268]       "location": "eastus"
}]

// reference to a resource collection whose name expression does not reference any loop variables
resource referenceToDuplicateNames 'Microsoft.Network/dnsZones@2018-05-01' = [for (zone,i) in []: {
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

module apim 'passthrough.bicep' = [for (region, i) in regions: {
//@[650:694]     "apim": {
  name: 'apim-${region}-${name}-${i}'
//@[657:657]       "name": "[format('apim-{0}-{1}-{2}', variables('regions')[copyIndex()], parameters('name'), copyIndex())]",
  params: {
    myInput: region
//@[665:665]             "value": "[variables('regions')[copyIndex()]]"
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
          backends: [for (index,i) in range(0, length(regions)): {
//@[294:305]                   "name": "backends",
            // we cannot codegen index correctly because the generated dependsOn property
            // would be outside of the scope of the property loop
            // as a result, this will generate a dependency on the entire collection
            address: apim[index + i].outputs.myOutput
//@[298:298]                     "address": "[reference(format('apim[{0}]', add(range(0, length(variables('regions')))[copyIndex('backends')], copyIndex('backends')))).outputs.myOutput.value]",
            backendHostHeader: apim[index + i].outputs.myOutput
//@[299:299]                     "backendHostHeader": "[reference(format('apim[{0}]', add(range(0, length(variables('regions')))[copyIndex('backends')], copyIndex('backends')))).outputs.myOutput.value]",
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

resource indexedModuleCollectionDependency 'Microsoft.Network/frontDoors@2020-05-01' = [for (index, i) in range(0, length(regions)): {
//@[315:347]     "indexedModuleCollectionDependency": {
  name: '${name}-${index}-${i}'
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
              address: apim[index+i].outputs.myOutput
//@[331:331]                   "address": "[reference(format('apim[{0}]', add(range(0, length(variables('regions')))[copyIndex()], copyIndex()))).outputs.myOutput.value]",
              backendHostHeader: apim[index+i].outputs.myOutput
//@[332:332]                   "backendHostHeader": "[reference(format('apim[{0}]', add(range(0, length(variables('regions')))[copyIndex()], copyIndex()))).outputs.myOutput.value]",
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

resource indexedResourceCollectionDependency 'Microsoft.Network/frontDoors@2020-05-01' = [for (index,i) in range(0, length(accounts)): {
//@[380:412]     "indexedResourceCollectionDependency": {
  name: '${name}-${index}-${i}'
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
              address: storageAccounts[index+i].properties.primaryEndpoints.internetEndpoints.web
//@[396:396]                   "address": "[reference(format('storageAccounts[{0}]', add(range(0, length(parameters('accounts')))[copyIndex()], copyIndex()))).primaryEndpoints.internetEndpoints.web]",
              backendHostHeader: storageAccounts[index+i].properties.primaryEndpoints.internetEndpoints.web
//@[397:397]                   "backendHostHeader": "[reference(format('storageAccounts[{0}]', add(range(0, length(parameters('accounts')))[copyIndex()], copyIndex()))).primaryEndpoints.internetEndpoints.web]",
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

