
// int
@sys.description('an int variable')
var myInt = 42
//@[78:78]     "myInt": 42,

// string
@sys.description('a string variable')
var myStr = 'str'
//@[79:79]     "myStr": "str",
var curliesWithNoInterp = '}{1}{'
//@[80:80]     "curliesWithNoInterp": "}{1}{",
var interp1 = 'abc${123}def'
//@[81:81]     "interp1": "[format('abc{0}def', 123)]",
var interp2 = '${123}def'
//@[82:82]     "interp2": "[format('{0}def', 123)]",
var interp3 = 'abc${123}'
//@[83:83]     "interp3": "[format('abc{0}', 123)]",
var interp4 = 'abc${123}${456}jk$l${789}p$'
//@[84:84]     "interp4": "[format('abc{0}{1}jk$l{2}p$', 123, 456, 789)]",
var doubleInterp = 'abc${'def${123}'}_${'${456}${789}'}'
//@[85:85]     "doubleInterp": "[format('abc{0}_{1}', format('def{0}', 123), format('{0}{1}', 456, 789))]",
var curliesInInterp = '{${123}{0}${true}}'
//@[86:86]     "curliesInInterp": "[format('{{{0}{{0}}{1}}}', 123, true())]",

// #completionTest(0) -> declarations

// verify correct bracket escaping
var bracketInTheMiddle = 'a[b]'
//@[87:87]     "bracketInTheMiddle": "a[b]",
// #completionTest(25) -> empty
var bracketAtBeginning = '[test'
//@[88:88]     "bracketAtBeginning": "[test",
// #completionTest(23) -> symbolsPlusTypes
var enclosingBrackets = '[test]'
//@[89:89]     "enclosingBrackets": "[[test]",
var emptyJsonArray = '[]'
//@[90:90]     "emptyJsonArray": "[[]",
var interpolatedBrackets = '[${myInt}]'
//@[91:91]     "interpolatedBrackets": "[format('[{0}]', variables('myInt'))]",
var nestedBrackets = '[test[]test2]'
//@[92:92]     "nestedBrackets": "[[test[]test2]",
var nestedInterpolatedBrackets = '[${emptyJsonArray}]'
//@[93:93]     "nestedInterpolatedBrackets": "[format('[{0}]', variables('emptyJsonArray'))]",
var bracketStringInExpression = concat('[', '\'test\'',']')
//@[94:94]     "bracketStringInExpression": "[concat('[', '''test''', ']')]",

// booleans
@sys.description('a bool variable')
var myTruth = true
//@[95:95]     "myTruth": true,
var myFalsehood = false
//@[96:96]     "myFalsehood": false,

var myEmptyObj = { }
//@[97:97]     "myEmptyObj": {},
var myEmptyArray = [ ]
//@[98:98]     "myEmptyArray": [],

// object
@sys.description('a object variable')
var myObj = {
//@[99:118]     "myObj": {
  a: 'a'
//@[100:100]       "a": "a",
  b: -12
//@[101:101]       "b": -12,
  c: true
//@[102:102]       "c": true,
  d: !true
//@[103:103]       "d": "[not(true())]",
  list: [
//@[104:112]       "list": [
    1
//@[105:105]         1,
    2
//@[106:106]         2,
    2+1
//@[107:107]         "[add(2, 1)]",
    {
      test: 144 > 33 && true || 99 <= 199
//@[109:109]           "test": "[or(and(greater(144, 33), true()), lessOrEquals(99, 199))]"
    }
    'a' =~ 'b'
//@[111:111]         "[equals(toLower('a'), toLower('b'))]"
  ]
  obj: {
//@[113:117]       "obj": {
    nested: [
//@[114:116]         "nested": [
      'hello'
//@[115:115]           "hello"
    ]
  }
}

@sys.description('a object with interp')
var objWithInterp = {
//@[119:123]     "objWithInterp": {
  '${myStr}': 1
//@[120:120]       "[format('{0}', variables('myStr'))]": 1,
  'abc${myStr}def': 2
//@[121:121]       "[format('abc{0}def', variables('myStr'))]": 2,
  '${interp1}abc${interp2}': '${interp1}abc${interp2}'
//@[122:122]       "[format('{0}abc{1}', variables('interp1'), variables('interp2'))]": "[format('{0}abc{1}', variables('interp1'), variables('interp2'))]"
}

// array
var myArr = [
//@[124:128]     "myArr": [
  'pirates'
//@[125:125]       "pirates",
  'say'
//@[126:126]       "say",
  'arr'
//@[127:127]       "arr"
]

// array with objects
var myArrWithObjects = [
//@[129:138]     "myArrWithObjects": [
  {
    name: 'one'
//@[131:131]         "name": "one",
    enable: true
//@[132:132]         "enable": true
  }
  {
    name: 'two'
//@[135:135]         "name": "two",
    enable: false && false || 'two' !~ 'three'
//@[136:136]         "enable": "[or(and(false(), false()), not(equals(toLower('two'), toLower('three'))))]"
  }
]

var expressionIndexOnAny = any({
//@[139:139]     "expressionIndexOnAny": "[createObject()[resourceGroup().location]]",
})[az.resourceGroup().location]

var anyIndexOnAny = any(true)[any(false)]
//@[140:140]     "anyIndexOnAny": "[true()[false()]]",

var deploymentName = deployment().name
//@[141:141]     "deploymentName": "[deployment().name]",
var templateContentVersion = deployment().properties.template.contentVersion
//@[142:142]     "templateContentVersion": "[deployment().properties.template.contentVersion]",
var templateLinkUri = deployment().properties.templateLink.uri
//@[143:143]     "templateLinkUri": "[deployment().properties.templateLink.uri]",
var templateLinkId = deployment().properties.templateLink.id
//@[144:144]     "templateLinkId": "[deployment().properties.templateLink.id]",

var portalEndpoint = environment().portal
//@[145:145]     "portalEndpoint": "[environment().portal]",
var loginEndpoint = environment().authentication.loginEndpoint
//@[146:146]     "loginEndpoint": "[environment().authentication.loginEndpoint]",

var namedPropertyIndexer = {
//@[147:147]     "namedPropertyIndexer": "[createObject('foo', 's').foo]",
  foo: 's'
}['foo']

var intIndexer = [
//@[148:148]     "intIndexer": "[createArray('s')[0]]",
  's'
][0]

var functionOnIndexer1 = concat([
//@[149:149]     "functionOnIndexer1": "[concat(createArray('s')[0], 's')]",
  's'
][0], 's')

var functionOnIndexer2 = concat([
//@[150:150]     "functionOnIndexer2": "[concat(createArray()[0], 's')]",
][0], 's')

var functionOnIndexer3 = concat([
//@[151:151]     "functionOnIndexer3": "[concat(createArray()[0], 's')]",
][0], any('s'))

var singleQuote = '\''
//@[152:152]     "singleQuote": "'",
var myPropertyName = '${singleQuote}foo${singleQuote}'
//@[153:153]     "myPropertyName": "[format('{0}foo{1}', variables('singleQuote'), variables('singleQuote'))]",

var unusedIntermediate = listKeys(resourceId('Mock.RP/type', 'steve'), '2020-01-01')
var unusedIntermediateRef = unusedIntermediate.secondaryKey

// previously this was not possible to emit correctly
var previousEmitLimit = [
//@[154:166]     "previousEmitLimit": [
  concat('s')
//@[155:155]       "[concat('s')]",
  '${4}'
//@[156:156]       "[format('{0}', 4)]",
  {
    a: {
//@[158:164]         "a": {
      b: base64('s')
//@[159:159]           "b": "[base64('s')]",
      c: concat([
//@[160:160]           "c": "[concat(createArray(add(12, 3)), createArray(not(true()), 'hello'))]",
        12 + 3
      ], [
        !true
        'hello'
      ])
      d: az.resourceGroup().location
//@[161:161]           "d": "[resourceGroup().location]",
      e: concat([
//@[162:162]           "e": "[concat(createArray(true()))]",
        true
      ])
      f: concat([
//@[163:163]           "f": "[concat(createArray(equals('s', 12)))]"
        's' == 12
      ])
    }
  }
]

// previously this was not possible to emit correctly
var previousEmitLimit2 = [
//@[167:179]     "previousEmitLimit2": [
  concat('s')
//@[168:168]       "[concat('s')]",
  '${4}'
//@[169:169]       "[format('{0}', 4)]",
  {
    a: {
//@[171:177]         "a": {
      b: base64('s')
//@[172:172]           "b": "[base64('s')]",
      c: union({
//@[173:173]           "c": "[union(createObject('a', add(12, 3)), createObject('b', not(true()), 'c', 'hello'))]",
        a: 12 + 3
      }, {
        b: !true
        c: 'hello'
      })
      d: az.resourceGroup().location
//@[174:174]           "d": "[resourceGroup().location]",
      e: union({
//@[175:175]           "e": "[union(createObject('x', true()), createObject())]",
        x: true
      }, {})
      f: intersection({
//@[176:176]           "f": "[intersection(createObject('q', equals('s', 12)), createObject())]"
        q: 's' == 12
      }, {})
    }
  }
]

// previously this was not possible to emit correctly
var previousEmitLimit3 = {
//@[180:185]     "previousEmitLimit3": {
  a: {
//@[181:184]       "a": {
    b: {
//@[182:182]         "b": "[equals(createObject('a', resourceGroup().location), 2)]",
      a: az.resourceGroup().location
    } == 2
    c: concat([
//@[183:183]         "c": "[concat(createArray(), createArray(true()))]"

    ], [
      true
    ])
  }
}

// #completionTest(0) -> declarations

var myVar = 'hello'
//@[186:186]     "myVar": "hello",
var myVar2 = any({
//@[187:189]     "myVar2": {
  something: myVar
//@[188:188]       "something": "[variables('myVar')]"
})
var myVar3 = any(any({
//@[190:192]     "myVar3": {
  something: myVar
//@[191:191]       "something": "[variables('myVar')]"
}))
var myVar4 = length(any(concat('s','a')))
//@[193:193]     "myVar4": "[length(concat('s', 'a'))]",

// verify that unqualified banned function identifiers can be used as declaration identifiers
var variables = true
//@[194:194]     "variables": true,
param parameters bool = true
//@[13:16]     "parameters": {
var if = true
//@[195:195]     "if": true,
var createArray = true
//@[196:196]     "createArray": true,
var createObject = true
//@[197:197]     "createObject": true,
var add = true
//@[198:198]     "add": true,
var sub = true
//@[199:199]     "sub": true,
var mul = true
//@[200:200]     "mul": true,
var div = true
//@[201:201]     "div": true,
param mod bool = true
//@[17:20]     "mod": {
var less = true
//@[202:202]     "less": true,
var lessOrEquals = true
//@[203:203]     "lessOrEquals": true,
var greater = true
//@[204:204]     "greater": true,
var greaterOrEquals = true
//@[205:205]     "greaterOrEquals": true,
param equals bool = true
//@[21:24]     "equals": {
var not = true
//@[206:206]     "not": true,
var and = true
//@[207:207]     "and": true,
var or = true
//@[208:208]     "or": true,
var I_WANT_IT_ALL = variables && parameters && if && createArray && createObject && add && sub && mul && div && mod && less && lessOrEquals && greater && greaterOrEquals && equals && not && and && or
//@[209:209]     "I_WANT_IT_ALL": "[and(and(and(and(and(and(and(and(and(and(and(and(and(and(and(and(and(variables('variables'), parameters('parameters')), variables('if')), variables('createArray')), variables('createObject')), variables('add')), variables('sub')), variables('mul')), variables('div')), parameters('mod')), variables('less')), variables('lessOrEquals')), variables('greater')), variables('greaterOrEquals')), parameters('equals')), variables('not')), variables('and')), variables('or'))]",

// identifiers can have underscores
var _ = 3
//@[210:210]     "_": 3,
var __ = 10 * _
//@[211:211]     "__": "[mul(10, variables('_'))]",
var _0a_1b = true
//@[212:212]     "_0a_1b": true,
var _1_ = _0a_1b || (__ + _ % 2 == 0)
//@[213:213]     "_1_": "[or(variables('_0a_1b'), equals(add(variables('__'), mod(variables('_'), 2)), 0))]",

// fully qualified access
var resourceGroup = 'something'
//@[214:214]     "resourceGroup": "something",
var resourceGroupName = az.resourceGroup().name
//@[215:215]     "resourceGroupName": "[resourceGroup().name]",
var resourceGroupObject = az.resourceGroup()
//@[216:216]     "resourceGroupObject": "[resourceGroup()]",
var propertyAccessFromObject = resourceGroupObject.name
//@[217:217]     "propertyAccessFromObject": "[variables('resourceGroupObject').name]",
var isTrue = sys.max(1, 2) == 3
//@[218:218]     "isTrue": "[equals(max(1, 2), 3)]",
var isFalse = !isTrue
//@[219:219]     "isFalse": "[not(variables('isTrue'))]",
var someText = isTrue ? sys.concat('a', sys.concat('b', 'c')) : 'someText'
//@[220:220]     "someText": "[if(variables('isTrue'), concat('a', concat('b', 'c')), 'someText')]",

// Bicep functions that cannot be converted into ARM functions
var scopesWithoutArmRepresentation = {
//@[221:224]     "scopesWithoutArmRepresentation": {
  subscription: subscription('10b57a01-6350-4ce2-972a-6a13642f00bf')
//@[222:222]       "subscription": "[createObject()]",
  resourceGroup: az.resourceGroup('10b57a01-6350-4ce2-972a-6a13642f00bf', 'myRgName')
//@[223:223]       "resourceGroup": "[createObject()]"
}

var scopesWithArmRepresentation = {
//@[225:229]     "scopesWithArmRepresentation": {
  tenant: tenant()
//@[226:226]       "tenant": "[tenant()]",
  subscription: subscription()
//@[227:227]       "subscription": "[subscription()]",
  resourceGroup: az.resourceGroup()
//@[228:228]       "resourceGroup": "[resourceGroup()]"
}

// Issue #1332
var issue1332_propname = 'ptest'
//@[230:230]     "issue1332_propname": "ptest",
var issue1332 = true ? {
//@[231:231]     "issue1332": "[if(true(), createObject('prop1', createObject(format('{0}', variables('issue1332_propname')), createObject())), createObject())]",
    prop1: {
        '${issue1332_propname}': {}
    }
} : {}

// Issue #486
var myBigInt = 2199023255552
//@[232:232]     "myBigInt": 2199023255552,
var myIntExpression = 5 * 5
//@[233:233]     "myIntExpression": "[mul(5, 5)]",
var myBigIntExpression = 2199023255552 * 2
//@[234:234]     "myBigIntExpression": "[mul(json('2199023255552'), 2)]",
var myBigIntExpression2 = 2199023255552 * 2199023255552
//@[235:235]     "myBigIntExpression2": "[mul(json('2199023255552'), json('2199023255552'))]",

// variable loops
var incrementingNumbers = [for i in range(0,10) : i]
//@[28:32]         "name": "incrementingNumbers",
var loopInput = [
//@[236:239]     "loopInput": [
  'one'
//@[237:237]       "one",
  'two'
//@[238:238]       "two"
]
var arrayOfStringsViaLoop = [for (name, i) in loopInput: 'prefix-${i}-${name}']
//@[33:37]         "name": "arrayOfStringsViaLoop",
var arrayOfObjectsViaLoop = [for (name, i) in loopInput: {
//@[38:46]         "name": "arrayOfObjectsViaLoop",
  index: i
//@[42:42]           "index": "[copyIndex('arrayOfObjectsViaLoop')]",
  name: name
//@[43:43]           "name": "[variables('loopInput')[copyIndex('arrayOfObjectsViaLoop')]]",
  value: 'prefix-${i}-${name}-suffix'
//@[44:44]           "value": "[format('prefix-{0}-{1}-suffix', copyIndex('arrayOfObjectsViaLoop'), variables('loopInput')[copyIndex('arrayOfObjectsViaLoop')])]"
}]
var arrayOfArraysViaLoop = [for (name, i) in loopInput: [
//@[47:51]         "name": "arrayOfArraysViaLoop",
  i
  name
  'prefix-${i}-${name}-suffix'
]]
var arrayOfBooleans = [for (name, i) in loopInput: i % 2 == 0]
//@[52:56]         "name": "arrayOfBooleans",
var arrayOfHardCodedNumbers = [for i in range(0,10): 3]
//@[57:61]         "name": "arrayOfHardCodedNumbers",
var arrayOfHardCodedBools = [for i in range(0,10): false]
//@[62:66]         "name": "arrayOfHardCodedBools",
var arrayOfHardCodedStrings = [for i in range(0,3): 'hi']
//@[67:71]         "name": "arrayOfHardCodedStrings",
var arrayOfNonRuntimeFunctionCalls = [for i in range(0,3): concat('hi', i)]
//@[72:76]         "name": "arrayOfNonRuntimeFunctionCalls",

var multilineString = '''
//@[240:240]     "multilineString": "HELLO!\n",
HELLO!
'''

var multilineEmpty = ''''''
//@[241:241]     "multilineEmpty": "",
var multilineEmptyNewline = '''
//@[242:242]     "multilineEmptyNewline": "",
'''

// evaluates to '\'abc\''
var multilineExtraQuotes = ''''abc''''
//@[243:243]     "multilineExtraQuotes": "'abc'",

// evaluates to '\'\nabc\n\''
var multilineExtraQuotesNewlines = ''''
//@[244:244]     "multilineExtraQuotesNewlines": "'\nabc\n'",
abc
''''

var multilineSingleLine = '''hello!'''
//@[245:245]     "multilineSingleLine": "hello!",

var multilineFormatted = format('''
//@[246:246]     "multilineFormatted": "[format('Hello,\nmy\nname is\n{0}\n', 'Anthony')]",
Hello,
my
name is
{0}
''', 'Anthony')

var multilineJavaScript = '''
//@[247:247]     "multilineJavaScript": "// NOT RECOMMENDED PATTERN\nconst fs = require('fs');\n\nmodule.exports = function (context) {\n    fs.readFile('./hello.txt', (err, data) => {\n        if (err) {\n            context.log.error('ERROR', err);\n            // BUG #1: This will result in an uncaught exception that crashes the entire process\n            throw err;\n        }\n        context.log(`Data from file: ${data}`);\n        // context.done() should be called here\n    });\n    // BUG #2: Data is not guaranteed to be read before the Azure Function's invocation ends\n    context.done();\n}\n",
// NOT RECOMMENDED PATTERN
const fs = require('fs');

module.exports = function (context) {
    fs.readFile('./hello.txt', (err, data) => {
        if (err) {
            context.log.error('ERROR', err);
            // BUG #1: This will result in an uncaught exception that crashes the entire process
            throw err;
        }
        context.log(`Data from file: ${data}`);
        // context.done() should be called here
    });
    // BUG #2: Data is not guaranteed to be read before the Azure Function's invocation ends
    context.done();
}
'''

var providersTest = providers('Microsoft.Resources').namespace
//@[248:248]     "providersTest": "[providers('Microsoft.Resources').namespace]",
var providersTest2 = providers('Microsoft.Resources', 'deployments').locations
//@[249:249]     "providersTest2": "[providers('Microsoft.Resources', 'deployments').locations]",

var copyBlockInObject = {
//@[250:258]     "copyBlockInObject": {
  copy: [
//@[251:257]       "[string('copy')]": [
    {
      name: 'blah'
//@[253:253]           "name": "blah",
      count: '[notAFunction()]'
//@[254:254]           "count": "[[notAFunction()]",
      input: {}
//@[255:255]           "input": {}
    }
  ]
}

var joinedString = join(['I', 'love', 'Bicep!'], ' ')
//@[259:259]     "joinedString": "[join(createArray('I', 'love', 'Bicep!'), ' ')]",

var prefix = take('food', 3)
//@[260:260]     "prefix": "[take('food', 3)]",
var isPrefixed = startsWith('food', 'foo')
//@[261:261]     "isPrefixed": "[startsWith('food', 'foo')]"

