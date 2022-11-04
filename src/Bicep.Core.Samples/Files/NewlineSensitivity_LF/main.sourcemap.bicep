@allowed(['abc', 'def', 'ghi'])
//@[15:19]       "allowedValues": [
param foo string
//@[13:20]     "foo": {

var singleLineFunction = concat('abc', 'def')
//@[23:23]     "singleLineFunction": "[concat('abc', 'def')]",

var multiLineFunction = concat(
//@[24:24]     "multiLineFunction": "[concat('abc', 'def')]",
  'abc',
  'def'
)

var multiLineFunctionUnusualFormatting = concat(
//@[25:25]     "multiLineFunctionUnusualFormatting": "[concat('abc', createArray('hello'), 'def')]",
              'abc',          any(['hello']),
'def')

var nestedTest = concat(
//@[26:26]     "nestedTest": "[concat(concat(concat(concat(concat('level', 'one'), 'two'), 'three'), 'four'), 'five')]",
concat(
concat(
concat(
concat(
'level',
'one'),
'two'),
'three'),
'four'),
'five')

var singleLineArray = ['abc', 'def']
//@[27:30]     "singleLineArray": [
var singleLineArrayTrailingCommas = ['abc', 'def',]
//@[31:34]     "singleLineArrayTrailingCommas": [

var multiLineArray = [
//@[35:38]     "multiLineArray": [
  'abc'
//@[36:36]       "abc",
  'def'
//@[37:37]       "def"
]

var mixedArray = ['abc', 'def'
//@[39:45]     "mixedArray": [
'ghi', 'jkl'
//@[42:43]       "ghi",
'lmn']
//@[44:44]       "lmn"

var singleLineObject = { abc: 'def', ghi: 'jkl'}
//@[46:49]     "singleLineObject": {
var singleLineObjectTrailingCommas = { abc: 'def', ghi: 'jkl',}
//@[50:53]     "singleLineObjectTrailingCommas": {
var multiLineObject = {
//@[54:57]     "multiLineObject": {
  abc: 'def'
//@[55:55]       "abc": "def",
  ghi: 'jkl'
//@[56:56]       "ghi": "jkl"
}
var mixedObject = { abc: 'abc', def: 'def'
//@[58:64]     "mixedObject": {
ghi: 'ghi', jkl: 'jkl'
//@[61:62]       "ghi": "ghi",
lmn: 'lmn' }
//@[63:63]       "lmn": "lmn"

var nestedMixed = {
//@[65:74]     "nestedMixed": {
  abc: { 'def': 'ghi', abc: 'def', foo: [
//@[66:73]       "abc": {
    'bar', 'blah'
//@[70:71]           "bar",
  ] }
}

var brokenFormatting = [      /*foo */ 'bar'   /*
//@[75:84]     "brokenFormatting": [

hello

*/,        'asdfdsf',             12324,       /*   asdf*/ '',     '''
//@[77:80]       "asdfdsf",


'''
123,      233535
//@[81:82]       123,
true
//@[83:83]       true
              ]

