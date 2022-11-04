resource basicParent 'My.Rp/parentType@2020-12-01' = {
//@[116:123]     "basicParent": {
  name: 'basicParent'
  properties: {
//@[120:122]       "properties": {
    size: 'large'
//@[121:121]         "size": "large"
  }

  resource basicChild 'childType' = {
//@[42:53]     "basicParent::basicChild": {
    name: 'basicChild'
    properties: {
//@[46:49]       "properties": {
      size: basicParent.properties.large
//@[47:47]         "size": "[reference('basicParent').large]",
      style: 'cool'
//@[48:48]         "style": "cool"
    }

    resource basicGrandchild 'grandchildType' = {
//@[30:41]     "basicParent::basicChild::basicGrandchild": {
      name: 'basicGrandchild'
      properties: {
//@[34:37]       "properties": {
        size: basicParent.properties.size
//@[35:35]         "size": "[reference('basicParent').size]",
        style: basicChild.properties.style
//@[36:36]         "style": "[reference('basicParent::basicChild').style]"
      }
    }
  }

  resource basicSibling 'childType' = {
//@[54:66]     "basicParent::basicSibling": {
    name: 'basicSibling'
    properties: {
//@[58:61]       "properties": {
      size: basicParent.properties.size
//@[59:59]         "size": "[reference('basicParent').size]",
      style: basicChild::basicGrandchild.properties.style
//@[60:60]         "style": "[reference('basicParent::basicChild::basicGrandchild').style]"
    }
  }
}
// #completionTest(50) -> childResources
output referenceBasicChild string = basicParent::basicChild.properties.size
//@[143:146]     "referenceBasicChild": {
// #completionTest(67) -> grandChildResources
output referenceBasicGrandchild string = basicParent::basicChild::basicGrandchild.properties.style
//@[147:150]     "referenceBasicGrandchild": {

resource existingParent 'My.Rp/parentType@2020-12-01' existing = {
//@[124:129]     "existingParent": {
  name: 'existingParent'

  resource existingChild 'childType' existing = {
//@[76:81]     "existingParent::existingChild": {
    name: 'existingChild'

    resource existingGrandchild 'grandchildType' = {
//@[67:75]     "existingParent::existingChild::existingGrandchild": {
      name: 'existingGrandchild'
      properties: {
//@[71:74]       "properties": {
        size: existingParent.properties.size
//@[72:72]         "size": "[reference('existingParent').size]",
        style: existingChild.properties.style
//@[73:73]         "style": "[reference('existingParent::existingChild').style]"
      }
    }
  }
}

param createParent bool
//@[13:15]     "createParent": {
param createChild bool
//@[16:18]     "createChild": {
param createGrandchild bool
//@[19:21]     "createGrandchild": {
resource conditionParent 'My.Rp/parentType@2020-12-01' = if (createParent) {
//@[83:135]       "condition": "[and(and(parameters('createParent'), parameters('createChild')), parameters('createGrandchild'))]",
  name: 'conditionParent'

  resource conditionChild 'childType' = if (createChild) {
//@[95:103]     "conditionParent::conditionChild": {
    name: 'conditionChild'

    resource conditionGrandchild 'grandchildType' = if (createGrandchild) {
//@[82:94]     "conditionParent::conditionChild::conditionGrandchild": {
      name: 'conditionGrandchild'
      properties: {
//@[87:90]       "properties": {
        size: conditionParent.properties.size
//@[88:88]         "size": "[reference('conditionParent').size]",
        style: conditionChild.properties.style
//@[89:89]         "style": "[reference('conditionParent::conditionChild').style]"
      }
    }
  }
}

var items = [
//@[24:27]     "items": [
  'a'
//@[25:25]       "a",
  'b'
//@[26:26]       "b"
]
resource loopParent 'My.Rp/parentType@2020-12-01' = {
//@[136:140]     "loopParent": {
  name: 'loopParent'

  resource loopChild 'childType' = [for item in items: {
//@[104:115]     "loopParent::loopChild": {
    name: 'loopChild'
  }]
}

output loopChildOutput string = loopParent::loopChild[0].name
//@[151:154]     "loopChildOutput": {
