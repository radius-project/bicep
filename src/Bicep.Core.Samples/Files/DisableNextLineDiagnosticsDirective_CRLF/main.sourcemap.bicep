var vmProperties = {
//@[31:40]     "vmProperties": {
  diagnosticsProfile: {
//@[32:38]       "diagnosticsProfile": {
    bootDiagnostics: {
//@[33:37]         "bootDiagnostics": {
      enabled: 123
//@[34:34]           "enabled": 123,
      storageUri: true
//@[35:35]           "storageUri": true,
      unknownProp: 'asdf'
//@[36:36]           "unknownProp": "asdf"
    }
  }
  evictionPolicy: 'Deallocate'
//@[39:39]       "evictionPolicy": "Deallocate"
}
resource vm 'Microsoft.Compute/virtualMachines@2020-12-01' = {
//@[43:49]     "vm": {
  name: 'vm'
  location: 'West US'
//@[47:47]       "location": "West US",
#disable-next-line BCP036 BCP037
  properties: vmProperties
//@[48:48]       "properties": "[variables('vmProperties')]"
}
#disable-next-line no-unused-params
param storageAccount1 string = 'testStorageAccount'
//@[13:16]     "storageAccount1": {
#disable-next-line          no-unused-params
param storageAccount2 string = 'testStorageAccount'
//@[17:20]     "storageAccount2": {
#disable-next-line   no-unused-params                /* Test comment 1 */
param storageAccount3 string = 'testStorageAccount'
//@[21:24]     "storageAccount3": {
         #disable-next-line   no-unused-params                // Test comment 2
param storageAccount5 string = 'testStorageAccount'
//@[25:28]     "storageAccount5": {
