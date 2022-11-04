
@sys.description('string output description')
//@[26:26]         "description": "string output description"
output myStr string = 'hello'
//@[22:28]     "myStr": {

@sys.description('int output description')
//@[33:33]         "description": "int output description"
output myInt int = 7
//@[29:35]     "myInt": {
output myOtherInt int = 20 / 13 + 80 % -4
//@[36:39]     "myOtherInt": {

@sys.description('bool output description')
//@[44:44]         "description": "bool output description"
output myBool bool = !false
//@[40:46]     "myBool": {
output myOtherBool bool = true
//@[47:50]     "myOtherBool": {

@sys.description('object array description')
//@[55:55]         "description": "object array description"
output suchEmpty array = [
//@[51:57]     "suchEmpty": {
]

output suchEmpty2 object = {
//@[58:61]     "suchEmpty2": {
}

@sys.description('object output description')
//@[83:83]         "description": "object output description"
output obj object = {
//@[62:85]     "obj": {
  a: 'a'
//@[65:65]         "a": "a",
  b: 12
//@[66:66]         "b": 12,
  c: true
//@[67:67]         "c": true,
  d: null
//@[68:68]         "d": null,
  list: [
//@[69:75]         "list": [
    1
//@[70:70]           1,
    2
//@[71:71]           2,
    3
//@[72:72]           3,
    null
//@[73:73]           null,
    {
    }
  ]
  obj: {
//@[76:80]         "obj": {
    nested: [
//@[77:79]           "nested": [
      'hello'
//@[78:78]             "hello"
    ]
  }
}

output myArr array = [
//@[86:93]     "myArr": {
  'pirates'
//@[89:89]         "pirates",
  'say'
//@[90:90]         "say",
   false ? 'arr2' : 'arr'
//@[91:91]         "[if(false(), 'arr2', 'arr')]"
]

output rgLocation string = resourceGroup().location
//@[94:97]     "rgLocation": {

output isWestUs bool = resourceGroup().location != 'westus' ? false : true
//@[98:101]     "isWestUs": {

output expressionBasedIndexer string = {
//@[102:105]     "expressionBasedIndexer": {
  eastus: {
    foo: true
  }
  westus: {
    foo: false
  }
}[resourceGroup().location].foo

var secondaryKeyIntermediateVar = listKeys(resourceId('Mock.RP/type', 'steve'), '2020-01-01').secondaryKey

output primaryKey string = listKeys(resourceId('Mock.RP/type', 'nigel'), '2020-01-01').primaryKey
//@[106:109]     "primaryKey": {
output secondaryKey string = secondaryKeyIntermediateVar
//@[110:113]     "secondaryKey": {

var varWithOverlappingOutput = 'hello'
//@[18:18]     "varWithOverlappingOutput": "hello"
param paramWithOverlappingOutput string
//@[13:15]     "paramWithOverlappingOutput": {

output varWithOverlappingOutput string = varWithOverlappingOutput
//@[114:117]     "varWithOverlappingOutput": {
output paramWithOverlappingOutput string = paramWithOverlappingOutput
//@[118:121]     "paramWithOverlappingOutput": {

// top-level output loops are supported
output generatedArray array = [for i in range(0,10): i]
//@[122:128]     "generatedArray": {

