
@sys.description('this is basicStorage')
//@[69:69]         "description": "this is basicStorage"
resource basicStorage 'Microsoft.Storage/storageAccounts@2019-06-01' = {
//@[59:71]     "basicStorage": {
  name: 'basicblobs'
  location: 'westus'
//@[63:63]       "location": "westus",
  kind: 'BlobStorage'
//@[64:64]       "kind": "BlobStorage",
  sku: {
//@[65:67]       "sku": {
    name: 'Standard_GRS'
//@[66:66]         "name": "Standard_GRS"
  }
}

@sys.description('this is dnsZone')
//@[78:78]         "description": "this is dnsZone"
resource dnsZone 'Microsoft.Network/dnszones@2018-05-01' = {
//@[72:80]     "dnsZone": {
  name: 'myZone'
  location: 'global'
//@[76:76]       "location": "global",
}

resource myStorageAccount 'Microsoft.Storage/storageAccounts@2017-10-01' = {
//@[81:105]     "myStorageAccount": {
  name: 'myencryptedone'
  location: 'eastus2'
//@[85:85]       "location": "eastus2",
  properties: {
//@[86:100]       "properties": {
    supportsHttpsTrafficOnly: true
//@[87:87]         "supportsHttpsTrafficOnly": true,
    accessTier: 'Hot'
//@[88:88]         "accessTier": "Hot",
    encryption: {
//@[89:99]         "encryption": {
      keySource: 'Microsoft.Storage'
//@[90:90]           "keySource": "Microsoft.Storage",
      services: {
//@[91:98]           "services": {
        blob: {
//@[92:94]             "blob": {
          enabled: true
//@[93:93]               "enabled": true
        }
        file: {
//@[95:97]             "file": {
          enabled: true
//@[96:96]               "enabled": true
        }
      }
    }
  }
  kind: 'StorageV2'
//@[101:101]       "kind": "StorageV2",
  sku: {
//@[102:104]       "sku": {
    name: 'Standard_LRS'
//@[103:103]         "name": "Standard_LRS"
  }
}

resource withExpressions 'Microsoft.Storage/storageAccounts@2017-10-01' = {
//@[106:133]     "withExpressions": {
  name: 'myencryptedone2'
  location: 'eastus2'
//@[110:110]       "location": "eastus2",
  properties: {
//@[111:125]       "properties": {
    supportsHttpsTrafficOnly: !false
//@[112:112]         "supportsHttpsTrafficOnly": "[not(false())]",
    accessTier: true ? 'Hot' : 'Cold'
//@[113:113]         "accessTier": "[if(true(), 'Hot', 'Cold')]",
    encryption: {
//@[114:124]         "encryption": {
      keySource: 'Microsoft.Storage'
//@[115:115]           "keySource": "Microsoft.Storage",
      services: {
//@[116:123]           "services": {
        blob: {
//@[117:119]             "blob": {
          enabled: true || false
//@[118:118]               "enabled": "[or(true(), false())]"
        }
        file: {
//@[120:122]             "file": {
          enabled: true
//@[121:121]               "enabled": true
        }
      }
    }
  }
  kind: 'StorageV2'
//@[126:126]       "kind": "StorageV2",
  sku: {
//@[127:129]       "sku": {
    name: 'Standard_LRS'
//@[128:128]         "name": "Standard_LRS"
  }
  dependsOn: [
    myStorageAccount
  ]
}

param applicationName string = 'to-do-app${uniqueString(resourceGroup().id)}'
//@[13:16]     "applicationName": {
var hostingPlanName = applicationName // why not just use the param directly?
//@[35:35]     "hostingPlanName": "[parameters('applicationName')]",

param appServicePlanTier string
//@[17:19]     "appServicePlanTier": {
param appServicePlanInstances int
//@[20:22]     "appServicePlanInstances": {

var location = resourceGroup().location
//@[36:36]     "location": "[resourceGroup().location]",

resource farm 'Microsoft.Web/serverFarms@2019-08-01' = {
//@[134:146]     "farm": {
  // dependsOn: resourceId('Microsoft.DocumentDB/databaseAccounts', cosmosAccountName)
  name: hostingPlanName
  location: location
//@[138:138]       "location": "[variables('location')]",
  sku: {
//@[139:142]       "sku": {
    name: appServicePlanTier
//@[140:140]         "name": "[parameters('appServicePlanTier')]",
    capacity: appServicePlanInstances
//@[141:141]         "capacity": "[parameters('appServicePlanInstances')]"
  }
  properties: {
//@[143:145]       "properties": {
    name: hostingPlanName // just hostingPlanName results in an error
//@[144:144]         "name": "[variables('hostingPlanName')]"
  }
}

var cosmosDbResourceId = resourceId('Microsoft.DocumentDB/databaseAccounts', cosmosDb.account)
//@[37:37]     "cosmosDbResourceId": "[resourceId('Microsoft.DocumentDB/databaseAccounts', parameters('cosmosDb').account)]",
var cosmosDbRef = reference(cosmosDbResourceId).documentEndpoint

// this variable is not accessed anywhere in this template and depends on a run-time reference
// it should not be present at all in the template output as there is nowhere logical to put it
var cosmosDbEndpoint = cosmosDbRef.documentEndpoint

param webSiteName string
//@[23:25]     "webSiteName": {
param cosmosDb object
//@[26:28]     "cosmosDb": {
resource site 'Microsoft.Web/sites@2019-08-01' = {
//@[147:174]     "site": {
  name: webSiteName
  location: location
//@[151:151]       "location": "[variables('location')]",
  properties: {
//@[152:173]       "properties": {
    // not yet supported // serverFarmId: farm.id
    siteConfig: {
//@[153:172]         "siteConfig": {
      appSettings: [
//@[154:171]           "appSettings": [
        {
          name: 'CosmosDb:Account'
//@[156:156]               "name": "CosmosDb:Account",
          value: reference(cosmosDbResourceId).documentEndpoint
//@[157:157]               "value": "[reference(variables('cosmosDbResourceId')).documentEndpoint]"
        }
        {
          name: 'CosmosDb:Key'
//@[160:160]               "name": "CosmosDb:Key",
          value: listKeys(cosmosDbResourceId, '2020-04-01').primaryMasterKey
//@[161:161]               "value": "[listKeys(variables('cosmosDbResourceId'), '2020-04-01').primaryMasterKey]"
        }
        {
          name: 'CosmosDb:DatabaseName'
//@[164:164]               "name": "CosmosDb:DatabaseName",
          value: cosmosDb.databaseName
//@[165:165]               "value": "[parameters('cosmosDb').databaseName]"
        }
        {
          name: 'CosmosDb:ContainerName'
//@[168:168]               "name": "CosmosDb:ContainerName",
          value: cosmosDb.containerName
//@[169:169]               "value": "[parameters('cosmosDb').containerName]"
        }
      ]
    }
  }
}

var _siteApiVersion = site.apiVersion
//@[38:38]     "_siteApiVersion": "[resourceInfo('site').apiVersion]",
var _siteType = site.type
//@[39:39]     "_siteType": "[resourceInfo('site').type]",

output siteApiVersion string = site.apiVersion
//@[621:624]     "siteApiVersion": {
output siteType string = site.type
//@[625:628]     "siteType": {

resource nested 'Microsoft.Resources/deployments@2019-10-01' = {
//@[175:187]     "nested": {
  name: 'nestedTemplate1'
  properties: {
//@[179:186]       "properties": {
    mode: 'Incremental'
//@[180:180]         "mode": "Incremental",
    template: {
//@[181:185]         "template": {
      // string key value
      '$schema': 'https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#'
//@[182:182]           "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
      contentVersion: '1.0.0.0'
//@[183:183]           "contentVersion": "1.0.0.0",
      resources: [
//@[184:184]           "resources": []
      ]
    }
  }
}

// should be able to access the read only properties
resource accessingReadOnlyProperties 'Microsoft.Foo/foos@2019-10-01' = {
//@[188:202]     "accessingReadOnlyProperties": {
  name: 'nestedTemplate1'
  properties: {
//@[192:198]       "properties": {
    otherId: nested.id
//@[193:193]         "otherId": "[resourceInfo('nested').id]",
    otherName: nested.name
//@[194:194]         "otherName": "[resourceInfo('nested').name]",
    otherVersion: nested.apiVersion
//@[195:195]         "otherVersion": "[resourceInfo('nested').apiVersion]",
    otherType: nested.type
//@[196:196]         "otherType": "[resourceInfo('nested').type]",

    otherThings: nested.properties.mode
//@[197:197]         "otherThings": "[reference('nested').mode]"
  }
}

resource resourceA 'My.Rp/typeA@2020-01-01' = {
//@[203:207]     "resourceA": {
  name: 'resourceA'
}

resource resourceB 'My.Rp/typeA/typeB@2020-01-01' = {
//@[208:215]     "resourceB": {
  name: '${resourceA.name}/myName'
}

resource resourceC 'My.Rp/typeA/typeB@2020-01-01' = {
//@[216:231]     "resourceC": {
  name: '${resourceA.name}/myName'
  properties: {
//@[220:226]       "properties": {
    aId: resourceA.id
//@[221:221]         "aId": "[resourceInfo('resourceA').id]",
    aType: resourceA.type
//@[222:222]         "aType": "[resourceInfo('resourceA').type]",
    aName: resourceA.name
//@[223:223]         "aName": "[resourceInfo('resourceA').name]",
    aApiVersion: resourceA.apiVersion
//@[224:224]         "aApiVersion": "[resourceInfo('resourceA').apiVersion]",
    bProperties: resourceB.properties
//@[225:225]         "bProperties": "[reference('resourceB')]"
  }
}

var varARuntime = {
  bId: resourceB.id
//@[239:239]             "bId": "[resourceInfo('resourceB').id]",
  bType: resourceB.type
//@[240:240]             "bType": "[resourceInfo('resourceB').type]",
  bName: resourceB.name
//@[241:241]             "bName": "[resourceInfo('resourceB').name]",
  bApiVersion: resourceB.apiVersion
//@[242:242]             "bApiVersion": "[resourceInfo('resourceB').apiVersion]",
  aKind: resourceA.kind
//@[243:243]             "aKind": "[reference('resourceA', '2020-01-01', 'full').kind]"
}

var varBRuntime = [
  varARuntime
//@[238:244]             "bId": "[resourceInfo('resourceB').id]",
]

var resourceCRef = {
//@[40:42]     "resourceCRef": {
  id: resourceC.id
//@[41:41]       "id": "[resourceInfo('resourceC').id]"
}
var setResourceCRef = true
//@[43:43]     "setResourceCRef": true,

resource resourceD 'My.Rp/typeD@2020-01-01' = {
//@[232:253]     "resourceD": {
  name: 'constant'
  properties: {
//@[236:247]       "properties": {
    runtime: varBRuntime
//@[237:245]         "runtime": [
    // repro for https://github.com/Azure/bicep/issues/316
    repro316: setResourceCRef ? resourceCRef : null
//@[246:246]         "repro316": "[if(variables('setResourceCRef'), variables('resourceCRef'), null())]"
  }
}

var myInterpKey = 'abc'
//@[44:44]     "myInterpKey": "abc",
resource resourceWithInterp 'My.Rp/interp@2020-01-01' = {
//@[254:263]     "resourceWithInterp": {
  name: 'interpTest'
  properties: {
//@[258:262]       "properties": {
    '${myInterpKey}': 1
//@[259:259]         "[format('{0}', variables('myInterpKey'))]": 1,
    'abc${myInterpKey}def': 2
//@[260:260]         "[format('abc{0}def', variables('myInterpKey'))]": 2,
    '${myInterpKey}abc${myInterpKey}': 3
//@[261:261]         "[format('{0}abc{1}', variables('myInterpKey'), variables('myInterpKey'))]": 3
  }
}

resource resourceWithEscaping 'My.Rp/mockResource@2020-01-01' = {
//@[264:271]     "resourceWithEscaping": {
  name: 'test'
  properties: {
//@[268:270]       "properties": {
    // both key and value should be escaped in template output
    '[resourceGroup().location]': '[resourceGroup().location]'
//@[269:269]         "[[resourceGroup().location]": "[[resourceGroup().location]"
  }
}

param shouldDeployVm bool = true
//@[29:32]     "shouldDeployVm": {

@sys.description('this is vmWithCondition')
//@[286:286]         "description": "this is vmWithCondition"
resource vmWithCondition 'Microsoft.Compute/virtualMachines@2020-06-01' = if (shouldDeployVm) {
//@[272:288]     "vmWithCondition": {
  name: 'vmName'
  location: 'westus'
//@[277:277]       "location": "westus",
  properties: {
//@[278:284]       "properties": {
    osProfile: {
//@[279:283]         "osProfile": {
      windowsConfiguration: {
//@[280:282]           "windowsConfiguration": {
        enableAutomaticUpdates: true
//@[281:281]             "enableAutomaticUpdates": true
      }
    }
  }
}

resource extension1 'My.Rp/extensionResource@2020-12-01' = {
//@[289:297]     "extension1": {
  name: 'extension'
  scope: vmWithCondition
}

resource extension2 'My.Rp/extensionResource@2020-12-01' = {
//@[298:306]     "extension2": {
  name: 'extension'
  scope: extension1
}

resource extensionDependencies 'My.Rp/mockResource@2020-01-01' = {
//@[307:324]     "extensionDependencies": {
  name: 'extensionDependencies'
  properties: {
//@[311:318]       "properties": {
    res1: vmWithCondition.id
//@[312:312]         "res1": "[resourceInfo('vmWithCondition').id]",
    res1runtime: vmWithCondition.properties.something
//@[313:313]         "res1runtime": "[reference('vmWithCondition').something]",
    res2: extension1.id
//@[314:314]         "res2": "[resourceInfo('extension1').id]",
    res2runtime: extension1.properties.something
//@[315:315]         "res2runtime": "[reference('extension1').something]",
    res3: extension2.id
//@[316:316]         "res3": "[resourceInfo('extension2').id]",
    res3runtime: extension2.properties.something
//@[317:317]         "res3runtime": "[reference('extension2').something]"
  }
}

@sys.description('this is existing1')
//@[335:335]         "description": "this is existing1"
resource existing1 'Mock.Rp/existingExtensionResource@2020-01-01' existing = {
//@[325:337]     "existing1": {
  name: 'existing1'
  scope: extension1
}

resource existing2 'Mock.Rp/existingExtensionResource@2020-01-01' existing = {
//@[338:347]     "existing2": {
  name: 'existing2'
  scope: existing1
}

resource extension3 'My.Rp/extensionResource@2020-12-01' = {
//@[348:356]     "extension3": {
  name: 'extension3'
  scope: existing1
}

/*
  valid loop cases
*/ 
var storageAccounts = [
//@[45:54]     "storageAccounts": [
  {
    name: 'one'
//@[47:47]         "name": "one",
    location: 'eastus2'
//@[48:48]         "location": "eastus2"
  }
  {
    name: 'two'
//@[51:51]         "name": "two",
    location: 'westus'
//@[52:52]         "location": "westus"
  }
]

// just a storage account loop
@sys.description('this is just a storage account loop')
//@[371:371]         "description": "this is just a storage account loop"
resource storageResources 'Microsoft.Storage/storageAccounts@2019-06-01' = [for account in storageAccounts: {
//@[357:373]     "storageResources": {
  name: account.name
  location: account.location
//@[365:365]       "location": "[variables('storageAccounts')[copyIndex()].location]",
  sku: {
//@[366:368]       "sku": {
    name: 'Standard_LRS'
//@[367:367]         "name": "Standard_LRS"
  }
  kind: 'StorageV2'
//@[369:369]       "kind": "StorageV2",
}]

// storage account loop with index
@sys.description('this is just a storage account loop with index')
//@[388:388]         "description": "this is just a storage account loop with index"
resource storageResourcesWithIndex 'Microsoft.Storage/storageAccounts@2019-06-01' = [for (account, i) in storageAccounts: {
//@[374:390]     "storageResourcesWithIndex": {
  name: '${account.name}${i}'
  location: account.location
//@[382:382]       "location": "[variables('storageAccounts')[copyIndex()].location]",
  sku: {
//@[383:385]       "sku": {
    name: 'Standard_LRS'
//@[384:384]         "name": "Standard_LRS"
  }
  kind: 'StorageV2'
//@[386:386]       "kind": "StorageV2",
}]

// basic nested loop
@sys.description('this is just a basic nested loop')
//@[411:411]         "description": "this is just a basic nested loop"
resource vnet 'Microsoft.Network/virtualNetworks@2020-06-01' = [for i in range(0, 3): {
//@[391:413]     "vnet": {
  name: 'vnet-${i}'
  properties: {
//@[399:409]       "properties": {
    subnets: [for j in range(0, 4): {
//@[401:407]             "name": "subnets",
      // #completionTest(0,1,2,3,4,5) -> subnetIdAndProperties
     
      // #completionTest(6) -> subnetIdAndPropertiesNoColon
      name: 'subnet-${i}-${j}'
//@[405:405]               "name": "[format('subnet-{0}-{1}', range(0, 3)[copyIndex()], range(0, 4)[copyIndex('subnets')])]"
    }]
  }
}]

// duplicate identifiers within the loop are allowed
resource duplicateIdentifiersWithinLoop 'Microsoft.Network/virtualNetworks@2020-06-01' = [for i in range(0, 3): {
//@[414:433]     "duplicateIdentifiersWithinLoop": {
  name: 'vnet-${i}'
  properties: {
//@[422:432]       "properties": {
    subnets: [for i in range(0, 4): {
//@[424:430]             "name": "subnets",
      name: 'subnet-${i}-${i}'
//@[428:428]               "name": "[format('subnet-{0}-{1}', range(0, 4)[copyIndex('subnets')], range(0, 4)[copyIndex('subnets')])]"
    }]
  }
}]

// duplicate identifers in global and single loop scope are allowed (inner variable hides the outer)
var canHaveDuplicatesAcrossScopes = 'hello'
//@[55:55]     "canHaveDuplicatesAcrossScopes": "hello",
resource duplicateInGlobalAndOneLoop 'Microsoft.Network/virtualNetworks@2020-06-01' = [for canHaveDuplicatesAcrossScopes in range(0, 3): {
//@[434:453]     "duplicateInGlobalAndOneLoop": {
  name: 'vnet-${canHaveDuplicatesAcrossScopes}'
  properties: {
//@[442:452]       "properties": {
    subnets: [for i in range(0, 4): {
//@[444:450]             "name": "subnets",
      name: 'subnet-${i}-${i}'
//@[448:448]               "name": "[format('subnet-{0}-{1}', range(0, 4)[copyIndex('subnets')], range(0, 4)[copyIndex('subnets')])]"
    }]
  }
}]

// duplicate in global and multiple loop scopes are allowed (inner hides the outer)
var duplicatesEverywhere = 'hello'
//@[56:56]     "duplicatesEverywhere": "hello"
resource duplicateInGlobalAndTwoLoops 'Microsoft.Network/virtualNetworks@2020-06-01' = [for duplicatesEverywhere in range(0, 3): {
//@[454:473]     "duplicateInGlobalAndTwoLoops": {
  name: 'vnet-${duplicatesEverywhere}'
  properties: {
//@[462:472]       "properties": {
    subnets: [for duplicatesEverywhere in range(0, 4): {
//@[464:470]             "name": "subnets",
      name: 'subnet-${duplicatesEverywhere}'
//@[468:468]               "name": "[format('subnet-{0}', range(0, 4)[copyIndex('subnets')])]"
    }]
  }
}]

/*
  Scope values created via array access on a resource collection
*/
resource dnsZones 'Microsoft.Network/dnsZones@2018-05-01' = [for zone in range(0,4): {
//@[474:483]     "dnsZones": {
  name: 'zone${zone}'
  location: 'global'
//@[482:482]       "location": "global"
}]

resource locksOnZones 'Microsoft.Authorization/locks@2016-09-01' = [for lock in range(0,2): {
//@[484:499]     "locksOnZones": {
  name: 'lock${lock}'
  properties: {
//@[493:495]       "properties": {
    level: 'CanNotDelete'
//@[494:494]         "level": "CanNotDelete"
  }
  scope: dnsZones[lock]
}]

resource moreLocksOnZones 'Microsoft.Authorization/locks@2016-09-01' = [for (lock, i) in range(0,3): {
//@[500:515]     "moreLocksOnZones": {
  name: 'another${i}'
  properties: {
//@[509:511]       "properties": {
    level: 'ReadOnly'
//@[510:510]         "level": "ReadOnly"
  }
  scope: dnsZones[i]
}]

resource singleLockOnFirstZone 'Microsoft.Authorization/locks@2016-09-01' = {
//@[516:527]     "singleLockOnFirstZone": {
  name: 'single-lock'
  properties: {
//@[521:523]       "properties": {
    level: 'ReadOnly'
//@[522:522]         "level": "ReadOnly"
  }
  scope: dnsZones[0]
}


resource p1_vnet 'Microsoft.Network/virtualNetworks@2020-06-01' = {
//@[528:540]     "p1_vnet": {
  location: resourceGroup().location
//@[532:532]       "location": "[resourceGroup().location]",
  name: 'myVnet'
  properties: {
//@[533:539]       "properties": {
    addressSpace: {
//@[534:538]         "addressSpace": {
      addressPrefixes: [
//@[535:537]           "addressPrefixes": [
        '10.0.0.0/20'
//@[536:536]             "10.0.0.0/20"
      ]
    }
  }
}

resource p1_subnet1 'Microsoft.Network/virtualNetworks/subnets@2020-06-01' = {
//@[541:551]     "p1_subnet1": {
  parent: p1_vnet
  name: 'subnet1'
  properties: {
//@[545:547]       "properties": {
    addressPrefix: '10.0.0.0/24'
//@[546:546]         "addressPrefix": "10.0.0.0/24"
  }
}

resource p1_subnet2 'Microsoft.Network/virtualNetworks/subnets@2020-06-01' = {
//@[552:562]     "p1_subnet2": {
  parent: p1_vnet
  name: 'subnet2'
  properties: {
//@[556:558]       "properties": {
    addressPrefix: '10.0.1.0/24'
//@[557:557]         "addressPrefix": "10.0.1.0/24"
  }
}

output p1_subnet1prefix string = p1_subnet1.properties.addressPrefix
//@[629:632]     "p1_subnet1prefix": {
output p1_subnet1name string = p1_subnet1.name
//@[633:636]     "p1_subnet1name": {
output p1_subnet1type string = p1_subnet1.type
//@[637:640]     "p1_subnet1type": {
output p1_subnet1id string = p1_subnet1.id
//@[641:644]     "p1_subnet1id": {

// parent property with extension resource
resource p2_res1 'Microsoft.Rp1/resource1@2020-06-01' = {
//@[563:567]     "p2_res1": {
  name: 'res1'
}

resource p2_res1child 'Microsoft.Rp1/resource1/child1@2020-06-01' = {
//@[568:575]     "p2_res1child": {
  parent: p2_res1
  name: 'child1'
}

resource p2_res2 'Microsoft.Rp2/resource2@2020-06-01' = {
//@[576:584]     "p2_res2": {
  scope: p2_res1child
  name: 'res2'
}

resource p2_res2child 'Microsoft.Rp2/resource2/child2@2020-06-01' = {
//@[585:593]     "p2_res2child": {
  parent: p2_res2
  name: 'child2'
}

output p2_res2childprop string = p2_res2child.properties.someProp
//@[645:648]     "p2_res2childprop": {
output p2_res2childname string = p2_res2child.name
//@[649:652]     "p2_res2childname": {
output p2_res2childtype string = p2_res2child.type
//@[653:656]     "p2_res2childtype": {
output p2_res2childid string = p2_res2child.id
//@[657:660]     "p2_res2childid": {

// parent property with 'existing' resource
resource p3_res1 'Microsoft.Rp1/resource1@2020-06-01' existing = {
//@[594:599]     "p3_res1": {
  name: 'res1'
}

resource p3_child1 'Microsoft.Rp1/resource1/child1@2020-06-01' = {
//@[600:604]     "p3_child1": {
  parent: p3_res1
  name: 'child1'
}

output p3_res1childprop string = p3_child1.properties.someProp
//@[661:664]     "p3_res1childprop": {
output p3_res1childname string = p3_child1.name
//@[665:668]     "p3_res1childname": {
output p3_res1childtype string = p3_child1.type
//@[669:672]     "p3_res1childtype": {
output p3_res1childid string = p3_child1.id
//@[673:676]     "p3_res1childid": {

// parent & child with 'existing'
resource p4_res1 'Microsoft.Rp1/resource1@2020-06-01' existing = {
//@[605:611]     "p4_res1": {
  scope: tenant()
  name: 'res1'
}

resource p4_child1 'Microsoft.Rp1/resource1/child1@2020-06-01' existing = {
//@[612:618]     "p4_child1": {
  parent: p4_res1
  name: 'child1'
}

output p4_res1childprop string = p4_child1.properties.someProp
//@[677:680]     "p4_res1childprop": {
output p4_res1childname string = p4_child1.name
//@[681:684]     "p4_res1childname": {
output p4_res1childtype string = p4_child1.type
//@[685:688]     "p4_res1childtype": {
output p4_res1childid string = p4_child1.id
//@[689:692]     "p4_res1childid": {

