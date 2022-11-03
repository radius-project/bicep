var boolVal = true
//@[27:27]     "boolVal": true,

var vmProperties = {
//@[28:37]     "vmProperties": {
  diagnosticsProfile: {
//@[29:35]       "diagnosticsProfile": {
    bootDiagnostics: {
//@[30:34]         "bootDiagnostics": {
      enabled: 123
//@[31:31]           "enabled": 123,
      storageUri: true
//@[32:32]           "storageUri": true,
      unknownProp: 'asdf'
//@[33:33]           "unknownProp": "asdf"
    }
  }
  evictionPolicy: boolVal
//@[36:36]       "evictionPolicy": "[variables('boolVal')]"
}

resource vm 'Microsoft.Compute/virtualMachines@2020-12-01' = {
//@[40:46]     "vm": {
  name: 'vm'
  location: 'West US'
//@[44:44]       "location": "West US",
  properties: vmProperties
//@[45:45]       "properties": "[variables('vmProperties')]"
}

var ipConfigurations = [for i in range(0, 2): {
//@[14:25]         "name": "ipConfigurations",
  id: true
//@[18:18]           "id": true,
  name: 'asdf${i}'
//@[19:19]           "name": "[format('asdf{0}', range(0, 2)[copyIndex('ipConfigurations')])]",
  properties: {
//@[20:23]           "properties": {
    madeUpProperty: boolVal
//@[21:21]             "madeUpProperty": "[variables('boolVal')]",
    subnet: 'hello'
//@[22:22]             "subnet": "hello"
  }
}]

resource nic 'Microsoft.Network/networkInterfaces@2020-11-01' = {
//@[47:54]     "nic": {
  name: 'abc'
  properties: {
//@[51:53]       "properties": {
    ipConfigurations: ipConfigurations
//@[52:52]         "ipConfigurations": "[variables('ipConfigurations')]"
  }
}

resource nicLoop 'Microsoft.Network/networkInterfaces@2020-11-01' = [for i in range(0, 2): {
//@[55:68]     "nicLoop": {
  name: 'abc${i}'
  properties: {
//@[63:67]       "properties": {
    ipConfigurations: [
//@[64:66]         "ipConfigurations": [
      // TODO: fix this
      ipConfigurations[i]
//@[65:65]           "[variables('ipConfigurations')[range(0, 2)[copyIndex()]]]"
    ]
  }
}]

resource nicLoop2 'Microsoft.Network/networkInterfaces@2020-11-01' = [for ipConfig in ipConfigurations: {
//@[69:82]     "nicLoop2": {
  name: 'abc${ipConfig.name}'
  properties: {
//@[77:81]       "properties": {
    ipConfigurations: [
//@[78:80]         "ipConfigurations": [
      // TODO: fix this
      ipConfig
//@[79:79]           "[variables('ipConfigurations')[copyIndex()]]"
    ]
  }
}]

