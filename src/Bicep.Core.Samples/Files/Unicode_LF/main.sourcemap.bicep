var emojis = '💪😊😈🍕☕'
//@[13:13]     "emojis": "💪😊😈🍕☕",
var ninjaCat = '🐱‍👤'
//@[14:14]     "ninjaCat": "🐱‍👤",

/*
朝辞白帝彩云间
千里江陵一日还
两岸猿声啼不住
轻舟已过万重山
*/

// greek letters in comment: Π π Φ φ plus emoji 😎
var variousAlphabets = {
//@[15:23]     "variousAlphabets": {
  'α': 'α'
//@[16:16]       "α": "α",
  'Ωω': [
//@[17:19]       "Ωω": [
    'Θμ'
//@[18:18]         "Θμ"
  ]
  'ążźćłóę': 'Cześć!'
//@[20:20]       "ążźćłóę": "Cześć!",
  'áéóúñü': '¡Hola!'
//@[21:21]       "áéóúñü": "¡Hola!",

  '二头肌': '二头肌'
//@[22:22]       "二头肌": "二头肌"
}

output concatUnicodeStrings string = concat('Θμ', '二头肌', 'α')
//@[31:34]     "concatUnicodeStrings": {
output interpolateUnicodeStrings string = 'Θμ二${emojis}头肌${ninjaCat}α'
//@[35:38]     "interpolateUnicodeStrings": {

// all of these should produce the same string
var surrogate_char      = '𐐷'
//@[24:24]     "surrogate_char": "𐐷",
var surrogate_codepoint = '\u{10437}'
//@[25:25]     "surrogate_codepoint": "𐐷",
var surrogate_pairs     = '\u{D801}\u{DC37}'
//@[26:26]     "surrogate_pairs": "𐐷",

// ascii escapes
var hello = '❆ Hello\u{20}World\u{21} ❁'
//@[27:27]     "hello": "❆ Hello World! ❁"
