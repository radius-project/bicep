targetScope = 'subscription'

resource rg 'Microsoft.Resources/resourceGroups@2020-06-01' = {
//@[35:40]     "rg": {
  name: 'adotfrank-rg'
  location: deployment().location
//@[39:39]       "location": "[deployment().location]"
}

module appPlanDeploy 'br:mock-registry-one.invalid/demo/plan:v2' = {
//@[41:103]     "appPlanDeploy": {
  name: 'planDeploy'
//@[44:44]       "name": "planDeploy",
  scope: rg
  params: {
    namePrefix: 'hello'
//@[53:53]             "value": "hello"
  }
}

module appPlanDeploy2 'br/mock-registry-one:demo/plan:v2' = {
//@[104:166]     "appPlanDeploy2": {
  name: 'planDeploy2'
//@[107:107]       "name": "planDeploy2",
  scope: rg
  params: {
    namePrefix: 'hello'
//@[116:116]             "value": "hello"
  }
}

var websites = [
//@[13:22]     "websites": [
  {
    name: 'fancy'
//@[15:15]         "name": "fancy",
    tag: 'latest'
//@[16:16]         "tag": "latest"
  }
  {
    name: 'plain'
//@[19:19]         "name": "plain",
    tag: 'plain-text'
//@[20:20]         "tag": "plain-text"
  }
]

module siteDeploy 'br:mock-registry-two.invalid/demo/site:v3' = [for site in websites: {
//@[167:269]     "siteDeploy": {
  name: '${site.name}siteDeploy'
//@[174:174]       "name": "[format('{0}siteDeploy', variables('websites')[copyIndex()].name)]",
  scope: rg
  params: {
    appPlanId: appPlanDeploy.outputs.planId
//@[183:183]             "value": "[reference('appPlanDeploy').outputs.planId.value]"
    namePrefix: site.name
//@[186:186]             "value": "[variables('websites')[copyIndex()].name]"
    dockerImage: 'nginxdemos/hello'
//@[189:189]             "value": "nginxdemos/hello"
    dockerImageTag: site.tag
//@[192:192]             "value": "[variables('websites')[copyIndex()].tag]"
  }
}]

module siteDeploy2 'br/demo-two:site:v3' = [for site in websites: {
//@[270:372]     "siteDeploy2": {
  name: '${site.name}siteDeploy2'
//@[277:277]       "name": "[format('{0}siteDeploy2', variables('websites')[copyIndex()].name)]",
  scope: rg
  params: {
    appPlanId: appPlanDeploy.outputs.planId
//@[286:286]             "value": "[reference('appPlanDeploy').outputs.planId.value]"
    namePrefix: site.name
//@[289:289]             "value": "[variables('websites')[copyIndex()].name]"
    dockerImage: 'nginxdemos/hello'
//@[292:292]             "value": "nginxdemos/hello"
    dockerImageTag: site.tag
//@[295:295]             "value": "[variables('websites')[copyIndex()].tag]"
  }
}]

module storageDeploy 'ts:00000000-0000-0000-0000-000000000000/test-rg/storage-spec:1.0' = {
//@[373:395]     "storageDeploy": {
  name: 'storageDeploy'
//@[376:376]       "name": "storageDeploy",
  scope: rg
  params: {
    location: 'eastus'
//@[385:385]             "value": "eastus"
  }
}

module storageDeploy2 'ts/mySpecRG:storage-spec:1.0' = {
//@[396:418]     "storageDeploy2": {
  name: 'storageDeploy2'
//@[399:399]       "name": "storageDeploy2",
  scope: rg
  params: {
    location: 'eastus'
//@[408:408]             "value": "eastus"
  }
}

var vnets = [
//@[23:32]     "vnets": [
  {
    name: 'vnet1'
//@[25:25]         "name": "vnet1",
    subnetName: 'subnet1.1'
//@[26:26]         "subnetName": "subnet1.1"
  }
  {
    name: 'vnet2'
//@[29:29]         "name": "vnet2",
    subnetName: 'subnet2.1'
//@[30:30]         "subnetName": "subnet2.1"
  }
]

module vnetDeploy 'ts:11111111-1111-1111-1111-111111111111/prod-rg/vnet-spec:v2' = [for vnet in vnets: {
//@[419:448]     "vnetDeploy": {
  name: '${vnet.name}Deploy'
//@[426:426]       "name": "[format('{0}Deploy', variables('vnets')[copyIndex()].name)]",
  scope: rg
  params: {
    vnetName: vnet.name
//@[435:435]             "value": "[variables('vnets')[copyIndex()].name]"
    subnetName: vnet.subnetName
//@[438:438]             "value": "[variables('vnets')[copyIndex()].subnetName]"
  }
}]

output siteUrls array = [for (site, i) in websites: siteDeploy[i].outputs.siteUrl]
//@[676:682]     "siteUrls": {

module passthroughPort 'br:localhost:5000/passthrough/port:v1' = {
//@[449:493]     "passthroughPort": {
  scope: rg
  name: 'port'
//@[452:452]       "name": "port",
  params: {
    port: 'test'
//@[461:461]             "value": "test"
  }
}

module ipv4 'br:127.0.0.1/passthrough/ipv4:v1' = {
//@[494:538]     "ipv4": {
  scope: rg
  name: 'ipv4'
//@[497:497]       "name": "ipv4",
  params: {
    ipv4: 'test'
//@[506:506]             "value": "test"
  }
}

module ipv4port 'br:127.0.0.1:5000/passthrough/ipv4port:v1' = {
//@[539:583]     "ipv4port": {
  scope: rg
  name: 'ipv4port'
//@[542:542]       "name": "ipv4port",
  params: {
    ipv4port: 'test'
//@[551:551]             "value": "test"
  }
}

module ipv6 'br:[::1]/passthrough/ipv6:v1' = {
//@[584:628]     "ipv6": {
  scope: rg
  name: 'ipv6'
//@[587:587]       "name": "ipv6",
  params: {
    ipv6: 'test'
//@[596:596]             "value": "test"
  }
}

module ipv6port 'br:[::1]:5000/passthrough/ipv6port:v1' = {
//@[629:673]     "ipv6port": {
  scope: rg
  name: 'ipv6port'
//@[632:632]       "name": "ipv6port",
  params: {
    ipv6port: 'test'
//@[641:641]             "value": "test"
  }
}
