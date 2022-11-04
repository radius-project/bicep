targetScope = 'tenant'

var managementGroups = [
//@[13:22]     "managementGroups": [
  {
    name: 'one'
//@[15:15]         "name": "one",
    displayName: 'The first'
//@[16:16]         "displayName": "The first"
  }
  {
    name: 'two'
//@[19:19]         "name": "two",
    displayName: 'The second'
//@[20:20]         "displayName": "The second"
  }
]

resource singleGroup 'Microsoft.Management/managementGroups@2020-05-01' = {
//@[25:32]     "singleGroup": {
  name: 'myMG'
  properties: {
//@[29:31]       "properties": {
    displayName: 'This one is mine!'
//@[30:30]         "displayName": "This one is mine!"
  }
}

resource manyGroups 'Microsoft.Management/managementGroups@2020-05-01' = [for mg in managementGroups: {
//@[33:47]     "manyGroups": {
  name: mg.name
  properties: {
//@[41:43]       "properties": {
    displayName: '${mg.displayName} (${singleGroup.properties.displayName})'
//@[42:42]         "displayName": "[format('{0} ({1})', variables('managementGroups')[copyIndex()].displayName, reference('singleGroup').displayName)]"
  }
}]

resource anotherSet 'Microsoft.Management/managementGroups@2020-05-01' = [for (mg, index) in managementGroups: {
//@[48:63]     "anotherSet": {
  name: concat(mg.name, '-one-', index)
  properties: {
//@[56:58]       "properties": {
    displayName: '${mg.displayName} (${singleGroup.properties.displayName}) (set 1) (index ${index})'
//@[57:57]         "displayName": "[format('{0} ({1}) (set 1) (index {2})', variables('managementGroups')[copyIndex()].displayName, reference('singleGroup').displayName, copyIndex())]"
  }
  dependsOn: [
    manyGroups
  ]
}]

resource yetAnotherSet 'Microsoft.Management/managementGroups@2020-05-01' = [for mg in managementGroups: {
//@[64:79]     "yetAnotherSet": {
  name: concat(mg.name, '-two')
  properties: {
//@[72:74]       "properties": {
    displayName: '${mg.displayName} (${singleGroup.properties.displayName}) (set 2)'
//@[73:73]         "displayName": "[format('{0} ({1}) (set 2)', variables('managementGroups')[copyIndex()].displayName, reference('singleGroup').displayName)]"
  }
  dependsOn: [
    anotherSet[0]
  ]
}]

output managementGroupIds array = [for i in range(0, length(managementGroups)): {
//@[82:91]     "managementGroupIds": {
  name: yetAnotherSet[i].name
//@[87:87]           "name": "[resourceInfo(format('yetAnotherSet[{0}]', range(0, length(variables('managementGroups')))[copyIndex()])).name]",
  displayName: yetAnotherSet[i].properties.displayName
//@[88:88]           "displayName": "[reference(format('yetAnotherSet[{0}]', range(0, length(variables('managementGroups')))[copyIndex()])).displayName]"
}]

