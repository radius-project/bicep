/*
  This is a block comment.
*/

// parameters without default value
@sys.description('''
//@[16:16]         "description": "this is my multi line\ndescription for my myString\n"
this is my multi line
description for my myString
''')
param myString string
//@[13:18]     "myString": {
param myInt int
//@[19:21]     "myInt": {
param myBool bool
//@[22:24]     "myBool": {

// parameters with default value
@sys.description('this is myString2')
//@[29:29]         "description": "this is myString2"
@metadata({
  description: 'overwrite but still valid'
})
param myString2 string = 'string value'
//@[25:31]     "myString2": {
param myInt2 int = 42
//@[32:35]     "myInt2": {
param myTruth bool = true
//@[36:39]     "myTruth": {
param myFalsehood bool = false
//@[40:43]     "myFalsehood": {
param myEscapedString string = 'First line\r\nSecond\ttabbed\tline'
//@[44:47]     "myEscapedString": {

// object default value
@sys.description('this is foo')
//@[72:72]         "description": "this is foo",
@metadata({
  description: 'overwrite but still valid'
  another: 'just for fun'
//@[73:73]         "another": "just for fun"
})
param foo object = {
//@[48:75]     "foo": {
  enabled: true
//@[51:51]         "enabled": true,
  name: 'this is my object'
//@[52:52]         "name": "this is my object",
  priority: 3
//@[53:53]         "priority": 3,
  info: {
//@[54:56]         "info": {
    a: 'b'
//@[55:55]           "a": "b"
  }
  empty: {
//@[57:57]         "empty": {},
  }
  array: [
//@[58:69]         "array": [
    'string item'
//@[59:59]           "string item",
    12
//@[60:60]           12,
    true
//@[61:61]           true,
    [
      'inner'
//@[63:63]             "inner",
      false
//@[64:64]             false
    ]
    {
      a: 'b'
//@[67:67]             "a": "b"
    }
  ]
}

// array default value
param myArrayParam array = [
//@[76:83]     "myArrayParam": {
  'a'
//@[79:79]         "a",
  'b'
//@[80:80]         "b",
  'c'
//@[81:81]         "c"
]

// secure string
@secure()
param password string
//@[84:86]     "password": {

// secure object
@secure()
param secretObject object
//@[87:89]     "secretObject": {

// enum parameter
@allowed([
//@[92:95]       "allowedValues": [
  'Standard_LRS'
//@[93:93]         "Standard_LRS",
  'Standard_GRS'
//@[94:94]         "Standard_GRS"
])
param storageSku string
//@[90:96]     "storageSku": {

@allowed([
//@[99:103]       "allowedValues": [
  1
//@[100:100]         1,
  2
//@[101:101]         2,
  3
//@[102:102]         3
])
param intEnum int
//@[97:104]     "intEnum": {

// length constraint on a string
@minLength(3)
//@[108:108]       "minLength": 3
@maxLength(24)
//@[107:107]       "maxLength": 24,
param storageName string
//@[105:109]     "storageName": {

// length constraint on an array
@minLength(3)
//@[113:113]       "minLength": 3
@maxLength(24)
//@[112:112]       "maxLength": 24,
param someArray array
//@[110:114]     "someArray": {

// empty metadata
@metadata({})
//@[117:117]       "metadata": {}
param emptyMetadata string
//@[115:118]     "emptyMetadata": {

// description
@metadata({
//@[121:123]       "metadata": {
  description: 'my description'
//@[122:122]         "description": "my description"
})
param description string
//@[119:124]     "description": {

@sys.description('my description')
//@[128:128]         "description": "my description"
param description2 string
//@[125:130]     "description2": {

// random extra metadata
@metadata({
//@[133:141]       "metadata": {
  description: 'my description'
//@[134:134]         "description": "my description",
  a: 1
//@[135:135]         "a": 1,
  b: true
//@[136:136]         "b": true,
  c: [
//@[137:137]         "c": [],
  ]
  d: {
//@[138:140]         "d": {
    test: 'abc'
//@[139:139]           "test": "abc"
  }
})
param additionalMetadata string
//@[131:142]     "additionalMetadata": {

// all modifiers together
@secure()
@minLength(3)
//@[154:154]       "minLength": 3
@maxLength(24)
//@[153:153]       "maxLength": 24,
@allowed([
//@[148:152]       "allowedValues": [
  'one'
//@[149:149]         "one",
  'two'
//@[150:150]         "two",
  'three'
//@[151:151]         "three"
])
@metadata({
//@[145:147]       "metadata": {
  description: 'Name of the storage account'
//@[146:146]         "description": "Name of the storage account"
})
param someParameter string
//@[143:155]     "someParameter": {

param defaultExpression bool = 18 != (true || false)
//@[156:159]     "defaultExpression": {

@allowed([
//@[162:165]       "allowedValues": [
  'abc'
//@[163:163]         "abc",
  'def'
//@[164:164]         "def"
])
param stringLiteral string
//@[160:166]     "stringLiteral": {

@allowed([
//@[170:174]       "allowedValues": [
  'abc'
//@[171:171]         "abc",
  'def'
//@[172:172]         "def",
  'ghi'
//@[173:173]         "ghi"
])
param stringLiteralWithAllowedValuesSuperset string = stringLiteral
//@[167:175]     "stringLiteralWithAllowedValuesSuperset": {

@secure()
@minLength(2)
//@[183:183]       "minLength": 2
  @maxLength(10)
//@[182:182]       "maxLength": 10,
@allowed([
//@[178:181]       "allowedValues": [
  'Apple'
//@[179:179]         "Apple",
  'Banana'
//@[180:180]         "Banana"
])
param decoratedString string
//@[176:184]     "decoratedString": {

@minValue(200)
//@[188:188]       "minValue": 200
param decoratedInt int = 123
//@[185:189]     "decoratedInt": {

// negative integer literals are allowed as decorator values
@minValue(-10)
//@[193:193]       "minValue": -10
@maxValue(-3)
//@[192:192]       "maxValue": -3,
param negativeValues int
//@[190:194]     "negativeValues": {

@sys.description('A boolean.')
//@[199:199]         "description": "A boolean.",
@metadata({
    description: 'I will be overrode.'
    foo: 'something'
//@[200:200]         "foo": "something",
    bar: [
//@[201:205]         "bar": [
        {          }
        true
//@[203:203]           true,
        123
//@[204:204]           123
    ]
})
param decoratedBool bool = (true && false) != true
//@[195:207]     "decoratedBool": {

@secure()
param decoratedObject object = {
//@[208:231]     "decoratedObject": {
  enabled: true
//@[211:211]         "enabled": true,
  name: 'this is my object'
//@[212:212]         "name": "this is my object",
  priority: 3
//@[213:213]         "priority": 3,
  info: {
//@[214:216]         "info": {
    a: 'b'
//@[215:215]           "a": "b"
  }
  empty: {
//@[217:217]         "empty": {},
  }
  array: [
//@[218:229]         "array": [
    'string item'
//@[219:219]           "string item",
    12
//@[220:220]           12,
    true
//@[221:221]           true,
    [
      'inner'
//@[223:223]             "inner",
      false
//@[224:224]             false
    ]
    {
      a: 'b'
//@[227:227]             "a": "b"
    }
  ]
}

@sys.metadata({
    description: 'An array.'
//@[239:239]         "description": "An array."
})
@sys.maxLength(20)
//@[241:241]       "maxLength": 20
@sys.description('I will be overrode.')
param decoratedArray array = [
//@[232:242]     "decoratedArray": {
    utcNow()
//@[235:235]         "[utcNow()]",
    newGuid()
//@[236:236]         "[newGuid()]"
]

