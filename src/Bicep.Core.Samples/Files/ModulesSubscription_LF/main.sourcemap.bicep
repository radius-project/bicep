targetScope = 'subscription'

param prefix string = 'majastrz'
//@[13:16]     "prefix": {
var groups = [
//@[19:24]     "groups": [
  'bicep1'
//@[20:20]       "bicep1",
  'bicep2'
//@[21:21]       "bicep2",
  'bicep3'
//@[22:22]       "bicep3",
  'bicep4'
//@[23:23]       "bicep4"
]

var scripts = take(groups, 2)
//@[25:25]     "scripts": "[take(variables('groups'), 2)]"

resource resourceGroups 'Microsoft.Resources/resourceGroups@2020-06-01' = [for name in groups: {
//@[28:37]     "resourceGroups": {
  name: '${prefix}-${name}'
  location: 'westus'
//@[36:36]       "location": "westus"
}]

module scopedToSymbolicName 'hello.bicep' = [for (name, i) in scripts: {
//@[38:99]     "scopedToSymbolicName": {
  name: '${prefix}-dep-${i}'
//@[45:45]       "name": "[format('{0}-dep-{1}', parameters('prefix'), copyIndex())]",
  params: {
    scriptName: 'test-${name}-${i}'
//@[54:54]             "value": "[format('test-{0}-{1}', variables('scripts')[copyIndex()], copyIndex())]"
  }
  scope: resourceGroups[i]
}]

module scopedToResourceGroupFunction 'hello.bicep' = [for (name, i) in scripts: {
//@[100:158]     "scopedToResourceGroupFunction": {
  name: '${prefix}-dep-${i}'
//@[107:107]       "name": "[format('{0}-dep-{1}', parameters('prefix'), copyIndex())]",
  params: {
    scriptName: 'test-${name}-${i}'
//@[116:116]             "value": "[format('test-{0}-{1}', variables('scripts')[copyIndex()], copyIndex())]"
  }
  scope: resourceGroup(concat(name, '-extra'))
}]


