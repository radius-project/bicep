/* 
  This is a block comment.
*/

// metadata with value
metadata myString2 = 'string value'
metadata myInt2 = 42
metadata myTruth = true
metadata myFalsehood = false
metadata myEscapedString = 'First line\r\nSecond\ttabbed\tline'
metadata myMultiLineString = '''
  This is a multi line string // with comments,
  blocked ${interpolation},
  and a /* newline.
  */
'''

// object value
metadata foo = {
  enabled: true
//@[18:18]       "enabled": true,
  name: 'this is my object'
//@[19:19]       "name": "this is my object",
  priority: 3
//@[20:20]       "priority": 3,
  info: {
//@[21:23]       "info": {
    a: 'b'
//@[22:22]         "a": "b"
  }
  empty: {
//@[24:24]       "empty": {},
  }
  array: [
//@[25:36]       "array": [
    'string item'
//@[26:26]         "string item",
    12
//@[27:27]         12,
    true
//@[28:28]         true,
    [
      'inner'
//@[30:30]           "inner",
      false
//@[31:31]           false
    ]
    {
      a: 'b'
//@[34:34]           "a": "b"
    }
  ]
}

// array value
metadata myArrayMetadata = [
  'a'
//@[39:39]       "a",
  'b'
//@[40:40]       "b",
  'c'
//@[41:41]       "c"
]

// emtpy object and array
metadata myEmptyObj = { }
metadata myEmptyArray = [ ]

// param with same name as metadata is permitted
param foo string
//@[47:49]     "foo": {

