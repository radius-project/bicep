targetScope = 'subscription'

resource rg 'Microsoft.Resources/resourceGroups@2020-06-01' = {
//@[35:40]     "rg": {
  name: 'adotfrank-rg'
  location: deployment().location
//@[39:39]       "location": "[deployment().location]"
}

module appPlanDeploy 'br:mock-registry-one.invalid/demo/plan:v2' = {
//@[41:101]     "appPlanDeploy": {
  name: 'planDeploy'
//@[44:44]       "name": "planDeploy",
  scope: rg
  params: {
    namePrefix: 'hello'
//@[53:53]             "value": "hello"
  }
}

module appPlanDeploy2 'br/mock-registry-one:demo/plan:v2' = {
//@[102:162]     "appPlanDeploy2": {
  name: 'planDeploy2'
//@[105:105]       "name": "planDeploy2",
  scope: rg
  params: {
    namePrefix: 'hello'
//@[114:114]             "value": "hello"
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
//@[163:263]     "siteDeploy": {
  name: '${site.name}siteDeploy'
//@[170:170]       "name": "[format('{0}siteDeploy', variables('websites')[copyIndex()].name)]",
  scope: rg
  params: {
    appPlanId: appPlanDeploy.outputs.planId
//@[179:179]             "value": "[reference('appPlanDeploy').outputs.planId.value]"
    namePrefix: site.name
//@[182:182]             "value": "[variables('websites')[copyIndex()].name]"
    dockerImage: 'nginxdemos/hello'
//@[185:185]             "value": "nginxdemos/hello"
    dockerImageTag: site.tag
//@[188:188]             "value": "[variables('websites')[copyIndex()].tag]"
  }
}]

module siteDeploy2 'br/demo-two:site:v3' = [for site in websites: {
//@[264:364]     "siteDeploy2": {
  name: '${site.name}siteDeploy2'
//@[271:271]       "name": "[format('{0}siteDeploy2', variables('websites')[copyIndex()].name)]",
  scope: rg
  params: {
    appPlanId: appPlanDeploy.outputs.planId
//@[280:280]             "value": "[reference('appPlanDeploy').outputs.planId.value]"
    namePrefix: site.name
//@[283:283]             "value": "[variables('websites')[copyIndex()].name]"
    dockerImage: 'nginxdemos/hello'
//@[286:286]             "value": "nginxdemos/hello"
    dockerImageTag: site.tag
//@[289:289]             "value": "[variables('websites')[copyIndex()].tag]"
  }
}]

module storageDeploy 'ts:00000000-0000-0000-0000-000000000000/test-rg/storage-spec:1.0' = {
//@[365:387]     "storageDeploy": {
  name: 'storageDeploy'
//@[368:368]       "name": "storageDeploy",
  scope: rg
  params: {
    location: 'eastus'
//@[377:377]             "value": "eastus"
  }
}

module storageDeploy2 'ts/mySpecRG:storage-spec:1.0' = {
//@[388:410]     "storageDeploy2": {
  name: 'storageDeploy2'
//@[391:391]       "name": "storageDeploy2",
  scope: rg
  params: {
    location: 'eastus'
//@[400:400]             "value": "eastus"
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
//@[411:440]     "vnetDeploy": {
  name: '${vnet.name}Deploy'
//@[418:418]       "name": "[format('{0}Deploy', variables('vnets')[copyIndex()].name)]",
  scope: rg
  params: {
    vnetName: vnet.name
//@[427:427]             "value": "[variables('vnets')[copyIndex()].name]"
    subnetName: vnet.subnetName
//@[430:430]             "value": "[variables('vnets')[copyIndex()].subnetName]"
  }
}]

output siteUrls array = [for (site, i) in websites: siteDeploy[i].outputs.siteUrl]
//@[658:664]     "siteUrls": {

module passthroughPort 'br:localhost:5000/passthrough/port:v1' = {
//@[441:483]     "passthroughPort": {
  scope: rg
  name: 'port'
//@[444:444]       "name": "port",
  params: {
    port: 'test'
//@[453:453]             "value": "test"
  }
}

module ipv4 'br:127.0.0.1/passthrough/ipv4:v1' = {
//@[484:526]     "ipv4": {
  scope: rg
  name: 'ipv4'
//@[487:487]       "name": "ipv4",
  params: {
    ipv4: 'test'
//@[496:496]             "value": "test"
  }
}

module ipv4port 'br:127.0.0.1:5000/passthrough/ipv4port:v1' = {
//@[527:569]     "ipv4port": {
  scope: rg
  name: 'ipv4port'
//@[530:530]       "name": "ipv4port",
  params: {
    ipv4port: 'test'
//@[539:539]             "value": "test"
  }
}

module ipv6 'br:[::1]/passthrough/ipv6:v1' = {
//@[570:612]     "ipv6": {
  scope: rg
  name: 'ipv6'
//@[573:573]       "name": "ipv6",
  params: {
    ipv6: 'test'
//@[582:582]             "value": "test"
  }
}

module ipv6port 'br:[::1]:5000/passthrough/ipv6port:v1' = {
//@[613:655]     "ipv6port": {
  scope: rg
  name: 'ipv6port'
//@[616:616]       "name": "ipv6port",
  params: {
    ipv6port: 'test'
//@[625:625]             "value": "test"
  }
}
