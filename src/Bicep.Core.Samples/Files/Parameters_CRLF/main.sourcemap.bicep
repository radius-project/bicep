/* 
  This is a block comment.
*/

// parameters without default value
param myString string
//@[13:15]     "myString": {
param myInt int
//@[16:18]     "myInt": {
param myBool bool
//@[19:21]     "myBool": {

// parameters with default value
param myString2 string = 'string value'
//@[22:25]     "myString2": {
param myInt2 int = 42
//@[26:29]     "myInt2": {
param myTruth bool = true
//@[30:33]     "myTruth": {
param myFalsehood bool = false
//@[34:37]     "myFalsehood": {
param myEscapedString string = 'First line\r\nSecond\ttabbed\tline'
//@[38:41]     "myEscapedString": {

// object default value
param foo object = {
//@[42:65]     "foo": {
  enabled: true
//@[45:45]         "enabled": true,
  name: 'this is my object'
//@[46:46]         "name": "this is my object",
  priority: 3
//@[47:47]         "priority": 3,
  info: {
//@[48:50]         "info": {
    a: 'b'
//@[49:49]           "a": "b"
  }
  empty: {
//@[51:51]         "empty": {},
  }
  array: [
//@[52:63]         "array": [
    'string item'
//@[53:53]           "string item",
    12
//@[54:54]           12,
    true
//@[55:55]           true,
    [
      'inner'
//@[57:57]             "inner",
      false
//@[58:58]             false
    ]
    {
      a: 'b'
//@[61:61]             "a": "b"
    }
  ]
}

// array default value
param myArrayParam array = [
//@[66:73]     "myArrayParam": {
  'a'
//@[69:69]         "a",
  'b'
//@[70:70]         "b",
  'c'
//@[71:71]         "c"
]

// secure string
@secure()
param password string
//@[74:76]     "password": {

// secure object
@secure()
param secretObject object
//@[77:79]     "secretObject": {

// enum parameter
@allowed([
//@[82:85]       "allowedValues": [
  'Standard_LRS'
//@[83:83]         "Standard_LRS",
  'Standard_GRS'
//@[84:84]         "Standard_GRS"
])
param storageSku string
//@[80:86]     "storageSku": {

// length constraint on a string
@minLength(3)
//@[90:90]       "minLength": 3
@maxLength(24)
//@[89:89]       "maxLength": 24,
param storageName string
//@[87:91]     "storageName": {

// length constraint on an array
@minLength(3)
//@[95:95]       "minLength": 3
@maxLength(24)
//@[94:94]       "maxLength": 24,
param someArray array
//@[92:96]     "someArray": {

// empty metadata
@metadata({})
//@[99:99]       "metadata": {}
param emptyMetadata string
//@[97:100]     "emptyMetadata": {

// description
@metadata({
//@[103:105]       "metadata": {
  description: 'my description'
//@[104:104]         "description": "my description"
})
param description string
//@[101:106]     "description": {

@sys.description('my description')
//@[110:110]         "description": "my description"
param description2 string
//@[107:112]     "description2": {

// random extra metadata
@metadata({
//@[115:123]       "metadata": {
  description: 'my description'
//@[116:116]         "description": "my description",
  a: 1
//@[117:117]         "a": 1,
  b: true
//@[118:118]         "b": true,
  c: [
//@[119:119]         "c": [],
  ]
  d: {
//@[120:122]         "d": {
    test: 'abc'
//@[121:121]           "test": "abc"
  }
})
param additionalMetadata string
//@[113:124]     "additionalMetadata": {

// all modifiers together
@secure()
@minLength(3)
//@[136:136]       "minLength": 3
@maxLength(24)
//@[135:135]       "maxLength": 24,
@allowed([
//@[130:134]       "allowedValues": [
  'one'
//@[131:131]         "one",
  'two'
//@[132:132]         "two",
  'three'
//@[133:133]         "three"
])
@metadata({
//@[127:129]       "metadata": {
  description: 'Name of the storage account'
//@[128:128]         "description": "Name of the storage account"
})
param someParameter string
//@[125:137]     "someParameter": {

param defaultExpression bool = 18 != (true || false)
//@[138:141]     "defaultExpression": {

@allowed([
//@[144:147]       "allowedValues": [
  'abc'
//@[145:145]         "abc",
  'def'
//@[146:146]         "def"
])
param stringLiteral string
//@[142:148]     "stringLiteral": {

@allowed([
//@[152:156]       "allowedValues": [
  'abc'
//@[153:153]         "abc",
  'def'
//@[154:154]         "def",
  'ghi'
//@[155:155]         "ghi"
])
param stringLiteralWithAllowedValuesSuperset string = stringLiteral
//@[149:157]     "stringLiteralWithAllowedValuesSuperset": {

@secure()
@minLength(2)
//@[165:165]       "minLength": 2
  @maxLength(10)
//@[164:164]       "maxLength": 10,
@allowed([
//@[160:163]       "allowedValues": [
  'Apple'
//@[161:161]         "Apple",
  'Banana'
//@[162:162]         "Banana"
])
param decoratedString string
//@[158:166]     "decoratedString": {

@minValue(200)
//@[170:170]       "minValue": 200
param decoratedInt int = 123
//@[167:171]     "decoratedInt": {

// negative integer literals are allowed as decorator values
@minValue(-10)
//@[175:175]       "minValue": -10
@maxValue(-3)
//@[174:174]       "maxValue": -3,
param negativeValues int
//@[172:176]     "negativeValues": {

@sys.description('A boolean.')
//@[181:181]         "description": "A boolean.",
@metadata({
    description: 'I will be overrode.'
    foo: 'something'
//@[182:182]         "foo": "something",
    bar: [
//@[183:187]         "bar": [
        {          }
        true
//@[185:185]           true,
        123
//@[186:186]           123
    ]
})
param decoratedBool bool = (true && false) != true
//@[177:189]     "decoratedBool": {

@secure()
param decoratedObject object = {
//@[190:213]     "decoratedObject": {
  enabled: true
//@[193:193]         "enabled": true,
  name: 'this is my object'
//@[194:194]         "name": "this is my object",
  priority: 3
//@[195:195]         "priority": 3,
  info: {
//@[196:198]         "info": {
    a: 'b'
//@[197:197]           "a": "b"
  }
  empty: {
//@[199:199]         "empty": {},
  }
  array: [
//@[200:211]         "array": [
    'string item'
//@[201:201]           "string item",
    12
//@[202:202]           12,
    true
//@[203:203]           true,
    [
      'inner'
//@[205:205]             "inner",
      false
//@[206:206]             false
    ]
    {
      a: 'b'
//@[209:209]             "a": "b"
    }
  ]
}

@sys.metadata({
    description: 'An array.'
//@[221:221]         "description": "An array."
})
@sys.maxLength(20)
//@[223:223]       "maxLength": 20
@sys.description('I will be overrode.')
param decoratedArray array = [
//@[214:224]     "decoratedArray": {
    utcNow()
//@[217:217]         "[utcNow()]",
    newGuid()
//@[218:218]         "[newGuid()]"
]

