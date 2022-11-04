var doggos = [
//@[20:25]     "doggos": [
  'Evie'
//@[21:21]       "Evie",
  'Casper'
//@[22:22]       "Casper",
  'Indy'
//@[23:23]       "Indy",
  'Kira'
//@[24:24]       "Kira"
]

var numbers = range(0, 4)
//@[26:26]     "numbers": "[range(0, 4)]",

var sayHello = map(doggos, i => 'Hello ${i}!')
//@[27:27]     "sayHello": "[map(variables('doggos'), lambda('i', format('Hello {0}!', lambdaVariables('i'))))]",

var isEven = filter(numbers, i => 0 == i % 2)
//@[28:28]     "isEven": "[filter(variables('numbers'), lambda('i', equals(0, mod(lambdaVariables('i'), 2))))]",

var evenDoggosNestedLambdas = map(filter(numbers, i => contains(filter(numbers, j => 0 == j % 2), i)), x => doggos[x])
//@[29:29]     "evenDoggosNestedLambdas": "[map(filter(variables('numbers'), lambda('i', contains(filter(variables('numbers'), lambda('j', equals(0, mod(lambdaVariables('j'), 2)))), lambdaVariables('i')))), lambda('x', variables('doggos')[lambdaVariables('x')]))]",

var flattenedArrayOfArrays = flatten([0, 1], [2, 3], [4, 5])
//@[30:30]     "flattenedArrayOfArrays": "[flatten(createArray(0, 1), createArray(2, 3), createArray(4, 5))]",
var flattenedEmptyArray = flatten()
//@[31:31]     "flattenedEmptyArray": "[flatten()]",

var mapSayHi = map(['abc', 'def', 'ghi'], foo => 'Hi ${foo}!')
//@[32:32]     "mapSayHi": "[map(createArray('abc', 'def', 'ghi'), lambda('foo', format('Hi {0}!', lambdaVariables('foo'))))]",
var mapEmpty = map([], foo => 'Hi ${foo}!')
//@[33:33]     "mapEmpty": "[map(createArray(), lambda('foo', format('Hi {0}!', lambdaVariables('foo'))))]",
var mapObject = map(range(0, length(doggos)), i => {
//@[34:34]     "mapObject": "[map(range(0, length(variables('doggos'))), lambda('i', createObject('i', lambdaVariables('i'), 'doggo', variables('doggos')[lambdaVariables('i')], 'greeting', format('Ahoy, {0}!', variables('doggos')[lambdaVariables('i')]))))]",
  i: i
  doggo: doggos[i]
  greeting: 'Ahoy, ${doggos[i]}!'
})
var mapArray = flatten(map(range(1, 3), i => [i * 2, (i * 2) + 1]))
//@[35:35]     "mapArray": "[flatten(map(range(1, 3), lambda('i', createArray(mul(lambdaVariables('i'), 2), add(mul(lambdaVariables('i'), 2), 1)))))]",
var mapMultiLineArray = flatten(map(range(1, 3), i => [
//@[36:36]     "mapMultiLineArray": "[flatten(map(range(1, 3), lambda('i', createArray(mul(lambdaVariables('i'), 3), add(mul(lambdaVariables('i'), 3), 1), add(mul(lambdaVariables('i'), 3), 2)))))]",
  i * 3
  (i * 3) + 1
  (i * 3) + 2
]))

var filterEqualityCheck = filter(['abc', 'def', 'ghi'], foo => 'def' == foo)
//@[37:37]     "filterEqualityCheck": "[filter(createArray('abc', 'def', 'ghi'), lambda('foo', equals('def', lambdaVariables('foo'))))]",
var filterEmpty = filter([], foo => 'def' == foo)
//@[38:38]     "filterEmpty": "[filter(createArray(), lambda('foo', equals('def', lambdaVariables('foo'))))]",

var sortNumeric = sort([8, 3, 10, -13, 5], (x, y) => x < y)
//@[39:39]     "sortNumeric": "[sort(createArray(8, 3, 10, -13, 5), lambda('x', 'y', less(lambdaVariables('x'), lambdaVariables('y'))))]",
var sortAlpha = sort(['ghi', 'abc', 'def'], (x, y) => x < y)
//@[40:40]     "sortAlpha": "[sort(createArray('ghi', 'abc', 'def'), lambda('x', 'y', less(lambdaVariables('x'), lambdaVariables('y'))))]",
var sortAlphaReverse = sort(['ghi', 'abc', 'def'], (x, y) => x > y)
//@[41:41]     "sortAlphaReverse": "[sort(createArray('ghi', 'abc', 'def'), lambda('x', 'y', greater(lambdaVariables('x'), lambdaVariables('y'))))]",
var sortByObjectKey = sort([
//@[42:42]     "sortByObjectKey": "[sort(createArray(createObject('key', 124, 'name', 'Second'), createObject('key', 298, 'name', 'Third'), createObject('key', 24, 'name', 'First'), createObject('key', 1232, 'name', 'Fourth')), lambda('x', 'y', less(int(lambdaVariables('x').key), int(lambdaVariables('y').key))))]",
  { key: 124, name: 'Second' }
  { key: 298, name: 'Third' }
  { key: 24, name: 'First' }
  { key: 1232, name: 'Fourth' }
], (x, y) => int(x.key) < int(y.key))
var sortEmpty = sort([], (x, y) => int(x) < int(y))
//@[43:43]     "sortEmpty": "[sort(createArray(), lambda('x', 'y', less(int(lambdaVariables('x')), int(lambdaVariables('y')))))]",

var reduceStringConcat = reduce(['abc', 'def', 'ghi'], '', (cur, next) => concat(cur, next))
//@[44:44]     "reduceStringConcat": "[reduce(createArray('abc', 'def', 'ghi'), '', lambda('cur', 'next', concat(lambdaVariables('cur'), lambdaVariables('next'))))]",
var reduceFactorial = reduce(range(1, 5), 1, (cur, next) => cur * next)
//@[45:45]     "reduceFactorial": "[reduce(range(1, 5), 1, lambda('cur', 'next', mul(lambdaVariables('cur'), lambdaVariables('next'))))]",
var reduceObjectUnion = reduce([
//@[46:46]     "reduceObjectUnion": "[reduce(createArray(createObject('foo', 123), createObject('bar', 456), createObject('baz', 789)), createObject(), lambda('cur', 'next', union(lambdaVariables('cur'), lambdaVariables('next'))))]",
  { foo: 123 }
  { bar: 456 }
  { baz: 789 }
], {}, (cur, next) => union(cur, next))
var reduceEmpty = reduce([], 0, (cur, next) => cur)
//@[47:47]     "reduceEmpty": "[reduce(createArray(), 0, lambda('cur', 'next', lambdaVariables('cur')))]",

var itemForLoop = [for item in range(0, 10): item]
//@[14:18]         "name": "itemForLoop",
var filteredLoop = filter(itemForLoop, i => i > 5)
//@[48:48]     "filteredLoop": "[filter(variables('itemForLoop'), lambda('i', greater(lambdaVariables('i'), 5)))]",

output doggoGreetings array = [for item in mapObject: item.greeting]
//@[101:107]     "doggoGreetings": {

resource storageAcc 'Microsoft.Storage/storageAccounts@2021-09-01' existing = {
//@[52:57]     "storageAcc": {
  name: 'asdfsadf'
}
var mappedResProps = map(items(storageAcc.properties.secondaryEndpoints), item => item.value)

module myMod './test.bicep' = {
//@[58:98]     "myMod": {
  name: 'asdfsadf'
//@[61:61]       "name": "asdfsadf",
  params: {
    outputThis: map(mapObject, obj => obj.doggo)
//@[69:69]             "value": "[map(variables('mapObject'), lambda('obj', lambdaVariables('obj').doggo))]"
  }
}
var mappedModOutputProps = map(myMod.outputs.outputThis, doggo => '${doggo} says bork')

var parentheses = map([123], (i => '${i}'))
//@[49:49]     "parentheses": "[map(createArray(123), lambda('i', format('{0}', lambdaVariables('i'))))]"

