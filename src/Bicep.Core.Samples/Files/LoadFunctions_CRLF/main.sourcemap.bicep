var loadedText1 = loadTextContent('Assets/TextFile.CRLF.txt')
//@[51:51]     "loadedText1": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\r\n\tProin varius in nunc et laoreet.\r\n  Nam pulvinar ipsum sed lectus porttitor, at porttitor ipsum faucibus.\r\n  \tAliquam euismod, odio tincidunt convallis pulvinar, felis sem porttitor turpis, a condimentum dui erat nec tellus.\r\n  Duis elementum cursus est, congue efficitur risus.\r\n\tMauris sit amet.\r\nExcepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.\r\n",
var loadedText2 = sys.loadTextContent('Assets/TextFile.LF.txt')
//@[52:52]     "loadedText2": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n Donec laoreet sem tortor, ut dignissim ipsum ornare vel.\n  Duis ac ipsum turpis.\n\tMaecenas at condimentum dui.\n Suspendisse aliquet efficitur iaculis.\nIn hac habitasse platea dictumst.\nEtiam consectetur ut libero ac lobortis.\n\tNullam vitae auctor massa.\nFusce tincidunt urna purus, sit amet.\n",
var loadedTextEncoding1 = loadTextContent('Assets/encoding-ascii.txt', 'us-ascii')
//@[53:53]     "loadedTextEncoding1": "32 = \n33 = !\n34 = \"\n35 = #\n36 = $\n37 = %\n38 = &\n39 = '\n40 = (\n41 = )\n42 = *\n43 = +\n44 = ,\n45 = -\n46 = .\n47 = /\n48 = 0\n49 = 1\n50 = 2\n51 = 3\n52 = 4\n53 = 5\n54 = 6\n55 = 7\n56 = 8\n57 = 9\n58 = :\n59 = ;\n60 = <\n61 = =\n62 = >\n63 = ?\n64 = @\n65 = A\n66 = B\n67 = C\n68 = D\n69 = E\n70 = F\n71 = G\n72 = H\n73 = I\n74 = J\n75 = K\n76 = L\n77 = M\n78 = N\n79 = O\n80 = P\n81 = Q\n82 = R\n83 = S\n84 = T\n85 = U\n86 = V\n87 = W\n88 = X\n89 = Y\n90 = Z\n91 = [\n92 = \\\n93 = ]\n94 = ^\n95 = _\n96 = `\n97 = a\n98 = b\n99 = c\n100 = d\n101 = e\n102 = f\n103 = g\n104 = h\n105 = i\n106 = j\n107 = k\n108 = l\n109 = m\n110 = n\n111 = o\n112 = p\n113 = q\n114 = r\n115 = s\n116 = t\n117 = u\n118 = v\n119 = w\n120 = x\n121 = y\n122 = z\n123 = {\n124 = |\n125 = }\n126 = ~\n",
var loadedTextEncoding2 = loadTextContent('Assets/encoding-utf8.txt', 'utf-8')
//@[54:54]     "loadedTextEncoding2": "💪😊😈🍕☕\r\n🐱‍👤\r\n\r\n朝辞白帝彩云间\r\n千里江陵一日还\r\n两岸猿声啼不住\r\n轻舟已过万重山\r\n\r\nΠ π Φ φ\r\n\r\n😎\r\n\r\nαα\r\nΩω\r\nΘ  \r\n\r\nZażółć gęślą jaźń\r\n\r\náéóúñü - ¡Hola!\r\n\r\n二头肌二头肌\r\n\r\n\r\n二头肌\r\nΘ二头肌α\r\n\r\n𐐷\r\n\\u{10437}\r\n\\u{D801}\\u{DC37}\r\n\r\n❆ Hello\\u{20}World\\u{21} ❁\r\n\r\n\ta\tb\tc\td\te\tf\tg\th\t\r\n8\t♜\t♞\t♝\t♛\t♚\t♝\t♞\t♜\t8\r\n7\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t7\r\n6\t\t\t\t\t\t\t\t\t6\r\n5\t\t\t\t\t\t\t\t\t5\r\n4\t\t\t\t\t\t\t\t\t4\r\n3\t\t\t\t\t\t\t\t\t3\r\n2\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t2\r\n1\t♖\t♘\t♗\t♕\t♔\t♗\t♘\t♖\t1\r\n\ta\tb\tc\td\te\tf\tg\th\r\n",
var loadedTextEncoding3 = loadTextContent('Assets/encoding-utf16.txt', 'utf-16')
//@[55:55]     "loadedTextEncoding3": "💪😊😈🍕☕\r\n🐱‍👤\r\n\r\n朝辞白帝彩云间\r\n千里江陵一日还\r\n两岸猿声啼不住\r\n轻舟已过万重山\r\n\r\nΠ π Φ φ\r\n\r\n😎\r\n\r\nαα\r\nΩω\r\nΘ  \r\n\r\nZażółć gęślą jaźń\r\n\r\náéóúñü - ¡Hola!\r\n\r\n二头肌二头肌\r\n\r\n\r\n二头肌\r\nΘ二头肌α\r\n\r\n𐐷\r\n\\u{10437}\r\n\\u{D801}\\u{DC37}\r\n\r\n❆ Hello\\u{20}World\\u{21} ❁\r\n\r\n\ta\tb\tc\td\te\tf\tg\th\t\r\n8\t♜\t♞\t♝\t♛\t♚\t♝\t♞\t♜\t8\r\n7\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t7\r\n6\t\t\t\t\t\t\t\t\t6\r\n5\t\t\t\t\t\t\t\t\t5\r\n4\t\t\t\t\t\t\t\t\t4\r\n3\t\t\t\t\t\t\t\t\t3\r\n2\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t2\r\n1\t♖\t♘\t♗\t♕\t♔\t♗\t♘\t♖\t1\r\n\ta\tb\tc\td\te\tf\tg\th\r\n",
var loadedTextEncoding4 = loadTextContent('Assets/encoding-utf16be.txt', 'utf-16BE')
//@[56:56]     "loadedTextEncoding4": "💪😊😈🍕☕\r\n🐱‍👤\r\n\r\n朝辞白帝彩云间\r\n千里江陵一日还\r\n两岸猿声啼不住\r\n轻舟已过万重山\r\n\r\nΠ π Φ φ\r\n\r\n😎\r\n\r\nαα\r\nΩω\r\nΘ  \r\n\r\nZażółć gęślą jaźń\r\n\r\náéóúñü - ¡Hola!\r\n\r\n二头肌二头肌\r\n\r\n\r\n二头肌\r\nΘ二头肌α\r\n\r\n𐐷\r\n\\u{10437}\r\n\\u{D801}\\u{DC37}\r\n\r\n❆ Hello\\u{20}World\\u{21} ❁\r\n\r\n\ta\tb\tc\td\te\tf\tg\th\t\r\n8\t♜\t♞\t♝\t♛\t♚\t♝\t♞\t♜\t8\r\n7\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t7\r\n6\t\t\t\t\t\t\t\t\t6\r\n5\t\t\t\t\t\t\t\t\t5\r\n4\t\t\t\t\t\t\t\t\t4\r\n3\t\t\t\t\t\t\t\t\t3\r\n2\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t2\r\n1\t♖\t♘\t♗\t♕\t♔\t♗\t♘\t♖\t1\r\n\ta\tb\tc\td\te\tf\tg\th\r\n",
var loadedTextEncoding5 = loadTextContent('Assets/encoding-iso.txt', 'iso-8859-1')
//@[57:57]     "loadedTextEncoding5": "¡\t\tinverted exclamation mark\n¢\t\tcent\n£\t\tpound\n¤\t\tcurrency\n¥\t\tyen\n¦\t\tbroken vertical bar\n§\t\tsection\n¨\t\tspacing diaeresis\n©\t\tcopyright\nª\t\tfeminine ordinal indicator\n«\t\tangle quotation mark (left)\n¬\t\tnegation\n­\t\tsoft hyphen\n®\t\tregistered trademark\n¯\t\tspacing macron\n°\t\tdegree\n±\t\tplus-or-minus\n²\t\tsuperscript 2\n³\t\tsuperscript 3\n´\t\tspacing acute\n¶\t\tparagraph\n·\t\tmiddle dot\n¸\t\tspacing cedilla\n¹\t\tsuperscript 1\nº\t\tmasculine ordinal indicator\n»\t\tangle quotation mark (right)\n¼\t\tfraction 1/4\n½\t\tfraction 1/2\n¾\t\tfraction 3/4\n¿\t\tinverted question mark\nÀ\t\tcapital a, grave accent\nÁ\t\tcapital a, acute accent\nÂ\t\tcapital a, circumflex accent\nÃ\t\tcapital a, tilde\nÄ\t\tcapital a, umlaut mark\nÅ\t\tcapital a, ring\nÆ\t\tcapital ae\nÇ\t\tcapital c, cedilla\nÈ\t\tcapital e, grave accent\nÉ\t\tcapital e, acute accent\nÊ\t\tcapital e, circumflex accent\nË\t\tcapital e, umlaut mark\nÌ\t\tcapital i, grave accent\nÍ\t\tcapital i, acute accent\nÎ\t\tcapital i, circumflex accent\nÏ\t\tcapital i, umlaut mark\nÐ\t\tcapital eth, Icelandic\nÑ\t\tcapital n, tilde\nÒ\t\tcapital o, grave accent\nÓ\t\tcapital o, acute accent\nÔ\t\tcapital o, circumflex accent\nÕ\t\tcapital o, tilde\nÖ\t\tcapital o, umlaut mark\n×\t\tmultiplication\nØ\t\tcapital o, slash\nÙ\t\tcapital u, grave accent\nÚ\t\tcapital u, acute accent\nÛ\t\tcapital u, circumflex accent\nÜ\t\tcapital u, umlaut mark\nÝ\t\tcapital y, acute accent\nÞ\t\tcapital THORN, Icelandic\nß\t\tsmall sharp s, German\nà\t\tsmall a, grave accent\ná\t\tsmall a, acute accent\nâ\t\tsmall a, circumflex accent\nã\t\tsmall a, tilde\nä\t\tsmall a, umlaut mark\nå\t\tsmall a, ring\næ\t\tsmall ae\nç\t\tsmall c, cedilla\nè\t\tsmall e, grave accent\né\t\tsmall e, acute accent\nê\t\tsmall e, circumflex accent\në\t\tsmall e, umlaut mark\nì\t\tsmall i, grave accent\ní\t\tsmall i, acute accent\nî\t\tsmall i, circumflex accent\nï\t\tsmall i, umlaut mark\nð\t\tsmall eth, Icelandic\nñ\t\tsmall n, tilde\nò\t\tsmall o, grave accent\nó\t\tsmall o, acute accent\nô\t\tsmall o, circumflex accent\nõ\t\tsmall o, tilde\nö\t\tsmall o, umlaut mark\n÷\t\tdivision\nø\t\tsmall o, slash\nù\t\tsmall u, grave accent\nú\t\tsmall u, acute accent\nû\t\tsmall u, circumflex accent\nü\t\tsmall u, umlaut mark\ný\t\tsmall y, acute accent\nþ\t\tsmall thorn, Icelandic\nÿ\t\tsmall y, umlaut mark\n",

var loadedBinary1 = loadFileAsBase64('Assets/binary')
//@[58:58]     "loadedBinary1": "g8fCjQHuFUfnHlTUHxe3qmjeM3HlYSToV7qTGrcJ6vgFNjvgpmxnexFbzjJV/Ejx8jXKa8L32YUn1f/HUnPY6u5c1SaGaP8OiVyRK4ef52hOtc3Yd29c9ubDsLohwLlmuiQDCvVduNMejR6eZy50ti3eYaLu3e04IKC7kTFO0Ph/vSIhlfkS6lUB9e7EJuHAa+3yJFn0uIVFs/BF67fNV9zwx92XyhFL8tmv3IZNd1+0cAby99+zif6iBPXcJ5XTUUz4UHaPmZLPT75hd4iGZzOk/I+FAsUTxRDra76D+sSXay7qBv5TyVLlhs7kqSVAecki6vQG0Siku64tl3PKKEy9JV1lHItgg/IFDYNd8/DKMUpEi90wunW/CfTpQcctzHzZFjl5euswaXgTDvVt2KRHPpi3likE8b1GuyKFVfKNT47VFGSabuUZlhDzbzx7qECnIpA1M7kH+TUHhGTe3ezmmPq+EO6jybYNMpMs+7gcfYAEtzE4gfpubKHLQI8ZYFKFxPCo0ypOwh4Z1nStJkcrNX2UlSDFfPb+LlCaGRxRKPN9md+2sr4x6qm0TptmI9o8wJF7FvqJUS2obFz2KhnfdKg7seuknpisasENchzEO2hoMtpCf5Lt3ZwsLCnFrllFaNmV2BWfA0k74f5MykjIioppM8ajdzORDtjTv/WcNyxdVlTV6nr2Oe43WFKZT0DFMDGHVgTZRBxVH5JtfT1akurR+IyvTegR6kSMHXSWlE1iEoPK8gdNpFLHdAJ8VwPnMGRC8CoCGzDeAakmwiHuecPOzcg9cOQe2Rlo68kJSr6q2hEcTmm9kPj7vfAeocqlosDd2Ci7xcBAJNb/rC4IVllhZPyqt2C8L1VbN5wZfBNgwpA73oOX0kxIKoCqH+Ni167WvC1ZwTqlVAWeAa1RiSplisinKBwDXahu2qDhVJyOt/RwV8Y+W3ynFGXYOW9BDwXVwKUm2zrKl6j0Y1AUTH5HArfCwwThXW2ZFslbr/fM9nJPAbbxpDY2FQBAlt8ST3PyLrFBfXlsV0JqVhidnw94NAbTiPqck15pTGbncg8VunYUAMw/JAOLf2SIJ8cA75IdJMp0UJXoMcOfVKkVdgMbGi7BHtJ3ZFwIajbNdWoNQV0k/LlwwmqGYGkh71wBX4WEdW6MqvvT8Mt/xyA6xzflqnMzgvQPIe6v29FETR9wxSKG52oCOY/DtPXEo+DgZvQwQn3F4BwX+GZawHbbMjWCIDxoSmph5TfZMzF0EkhEi47Uk93FPgiBQPjKSYMxnJ9Lo9UwZjsxdtk7ONKe1GIxfHdx3no4uuRp8WKqjpz017VBOWubdXPxO8Q8QOv6nT9faVVCMUQApehFlg==",
var loadedBinary2 = sys.loadFileAsBase64('Assets/binary')
//@[59:59]     "loadedBinary2": "g8fCjQHuFUfnHlTUHxe3qmjeM3HlYSToV7qTGrcJ6vgFNjvgpmxnexFbzjJV/Ejx8jXKa8L32YUn1f/HUnPY6u5c1SaGaP8OiVyRK4ef52hOtc3Yd29c9ubDsLohwLlmuiQDCvVduNMejR6eZy50ti3eYaLu3e04IKC7kTFO0Ph/vSIhlfkS6lUB9e7EJuHAa+3yJFn0uIVFs/BF67fNV9zwx92XyhFL8tmv3IZNd1+0cAby99+zif6iBPXcJ5XTUUz4UHaPmZLPT75hd4iGZzOk/I+FAsUTxRDra76D+sSXay7qBv5TyVLlhs7kqSVAecki6vQG0Siku64tl3PKKEy9JV1lHItgg/IFDYNd8/DKMUpEi90wunW/CfTpQcctzHzZFjl5euswaXgTDvVt2KRHPpi3likE8b1GuyKFVfKNT47VFGSabuUZlhDzbzx7qECnIpA1M7kH+TUHhGTe3ezmmPq+EO6jybYNMpMs+7gcfYAEtzE4gfpubKHLQI8ZYFKFxPCo0ypOwh4Z1nStJkcrNX2UlSDFfPb+LlCaGRxRKPN9md+2sr4x6qm0TptmI9o8wJF7FvqJUS2obFz2KhnfdKg7seuknpisasENchzEO2hoMtpCf5Lt3ZwsLCnFrllFaNmV2BWfA0k74f5MykjIioppM8ajdzORDtjTv/WcNyxdVlTV6nr2Oe43WFKZT0DFMDGHVgTZRBxVH5JtfT1akurR+IyvTegR6kSMHXSWlE1iEoPK8gdNpFLHdAJ8VwPnMGRC8CoCGzDeAakmwiHuecPOzcg9cOQe2Rlo68kJSr6q2hEcTmm9kPj7vfAeocqlosDd2Ci7xcBAJNb/rC4IVllhZPyqt2C8L1VbN5wZfBNgwpA73oOX0kxIKoCqH+Ni167WvC1ZwTqlVAWeAa1RiSplisinKBwDXahu2qDhVJyOt/RwV8Y+W3ynFGXYOW9BDwXVwKUm2zrKl6j0Y1AUTH5HArfCwwThXW2ZFslbr/fM9nJPAbbxpDY2FQBAlt8ST3PyLrFBfXlsV0JqVhidnw94NAbTiPqck15pTGbncg8VunYUAMw/JAOLf2SIJ8cA75IdJMp0UJXoMcOfVKkVdgMbGi7BHtJ3ZFwIajbNdWoNQV0k/LlwwmqGYGkh71wBX4WEdW6MqvvT8Mt/xyA6xzflqnMzgvQPIe6v29FETR9wxSKG52oCOY/DtPXEo+DgZvQwQn3F4BwX+GZawHbbMjWCIDxoSmph5TfZMzF0EkhEi47Uk93FPgiBQPjKSYMxnJ9Lo9UwZjsxdtk7ONKe1GIxfHdx3no4uuRp8WKqjpz017VBOWubdXPxO8Q8QOv6nT9faVVCMUQApehFlg==",

var loadedTextInterpolation1 = 'Text: ${loadTextContent('Assets/TextFile.CRLF.txt')}'
//@[60:60]     "loadedTextInterpolation1": "[format('Text: {0}', variables('$fxv#0'))]",
var loadedTextInterpolation2 = 'Text: ${loadTextContent('Assets/TextFile.LF.txt')}'
//@[61:61]     "loadedTextInterpolation2": "[format('Text: {0}', variables('$fxv#1'))]",

var loadedTextObject1 = {
//@[62:64]     "loadedTextObject1": {
  'text' : loadTextContent('Assets/TextFile.CRLF.txt')
//@[63:63]       "text": "[variables('$fxv#2')]"
}
var loadedTextObject2 = {
//@[65:67]     "loadedTextObject2": {
  'text' : loadTextContent('Assets/TextFile.LF.txt')  
//@[66:66]       "text": "[variables('$fxv#3')]"
}
var loadedBinaryInObject = {
//@[68:70]     "loadedBinaryInObject": {
  file: loadFileAsBase64('Assets/binary')
//@[69:69]       "file": "[variables('$fxv#4')]"
}

var loadedTextArray = [
//@[71:74]     "loadedTextArray": [
  loadTextContent('Assets/TextFile.LF.txt')
//@[72:72]       "[variables('$fxv#5')]",
  loadFileAsBase64('Assets/binary')
//@[73:73]       "[variables('$fxv#6')]"
]

var loadedTextArrayInObject = {
//@[75:80]     "loadedTextArrayInObject": {
  'files' : [
//@[76:79]       "files": [
    loadTextContent('Assets/TextFile.CRLF.txt')
//@[77:77]         "[variables('$fxv#7')]",
    loadFileAsBase64('Assets/binary')
//@[78:78]         "[variables('$fxv#8')]"
  ]
}

var loadedTextArrayInObjectFunctions = {
//@[81:88]     "loadedTextArrayInObjectFunctions": {
  'files' : [
//@[82:87]       "files": [
    length(loadTextContent('Assets/TextFile.CRLF.txt'))
//@[83:83]         "[length(variables('$fxv#9'))]",
    sys.length(loadTextContent('Assets/TextFile.LF.txt'))
//@[84:84]         "[length(variables('$fxv#10'))]",
    length(loadFileAsBase64('Assets/binary'))
//@[85:85]         "[length(variables('$fxv#11'))]",
    sys.length(loadFileAsBase64('Assets/binary'))
//@[86:86]         "[length(variables('$fxv#12'))]"
  ]
}


module module1 'modulea.bicep' = {
//@[118:152]     "module1": {
  name: 'module1'
//@[121:121]       "name": "module1",
  params: {
    text: loadTextContent('Assets/TextFile.LF.txt')
//@[129:129]             "value": "[variables('$fxv#13')]"
  }
}

module module2 'modulea.bicep' = {
//@[153:187]     "module2": {
  name: 'module2'
//@[156:156]       "name": "module2",
  params: {
    text: loadFileAsBase64('Assets/binary')
//@[164:164]             "value": "[variables('$fxv#14')]"
  }
}

var textFileInSubdirectories = loadTextContent('Assets/../Assets/path/../path/../../Assets/path/to/deep/file/../../../to/deep/file/TextFile.txt')
//@[89:89]     "textFileInSubdirectories": "Lorem ipsum dolor sit amet, consectetur adipiscing elit.\n Donec laoreet sem tortor, ut dignissim ipsum ornare vel.\n  Duis ac ipsum turpis.\n\tMaecenas at condimentum dui.\n Suspendisse aliquet efficitur iaculis.\nIn hac habitasse platea dictumst.\nEtiam consectetur ut libero ac lobortis.\n\tNullam vitae auctor massa.\nFusce tincidunt urna purus, sit amet.\n",
var binaryFileInSubdirectories = loadFileAsBase64('Assets/../Assets/path/../path/../../Assets/path/to/deep/file/../../../to/deep/file/binary')
//@[90:90]     "binaryFileInSubdirectories": "g8fCjQHuFUfnHlTUHxe3qmjeM3HlYSToV7qTGrcJ6vgFNjvgpmxnexFbzjJV/Ejx8jXKa8L32YUn1f/HUnPY6u5c1SaGaP8OiVyRK4ef52hOtc3Yd29c9ubDsLohwLlmuiQDCvVduNMejR6eZy50ti3eYaLu3e04IKC7kTFO0Ph/vSIhlfkS6lUB9e7EJuHAa+3yJFn0uIVFs/BF67fNV9zwx92XyhFL8tmv3IZNd1+0cAby99+zif6iBPXcJ5XTUUz4UHaPmZLPT75hd4iGZzOk/I+FAsUTxRDra76D+sSXay7qBv5TyVLlhs7kqSVAecki6vQG0Siku64tl3PKKEy9JV1lHItgg/IFDYNd8/DKMUpEi90wunW/CfTpQcctzHzZFjl5euswaXgTDvVt2KRHPpi3likE8b1GuyKFVfKNT47VFGSabuUZlhDzbzx7qECnIpA1M7kH+TUHhGTe3ezmmPq+EO6jybYNMpMs+7gcfYAEtzE4gfpubKHLQI8ZYFKFxPCo0ypOwh4Z1nStJkcrNX2UlSDFfPb+LlCaGRxRKPN9md+2sr4x6qm0TptmI9o8wJF7FvqJUS2obFz2KhnfdKg7seuknpisasENchzEO2hoMtpCf5Lt3ZwsLCnFrllFaNmV2BWfA0k74f5MykjIioppM8ajdzORDtjTv/WcNyxdVlTV6nr2Oe43WFKZT0DFMDGHVgTZRBxVH5JtfT1akurR+IyvTegR6kSMHXSWlE1iEoPK8gdNpFLHdAJ8VwPnMGRC8CoCGzDeAakmwiHuecPOzcg9cOQe2Rlo68kJSr6q2hEcTmm9kPj7vfAeocqlosDd2Ci7xcBAJNb/rC4IVllhZPyqt2C8L1VbN5wZfBNgwpA73oOX0kxIKoCqH+Ni167WvC1ZwTqlVAWeAa1RiSplisinKBwDXahu2qDhVJyOt/RwV8Y+W3ynFGXYOW9BDwXVwKUm2zrKl6j0Y1AUTH5HArfCwwThXW2ZFslbr/fM9nJPAbbxpDY2FQBAlt8ST3PyLrFBfXlsV0JqVhidnw94NAbTiPqck15pTGbncg8VunYUAMw/JAOLf2SIJ8cA75IdJMp0UJXoMcOfVKkVdgMbGi7BHtJ3ZFwIajbNdWoNQV0k/LlwwmqGYGkh71wBX4WEdW6MqvvT8Mt/xyA6xzflqnMzgvQPIe6v29FETR9wxSKG52oCOY/DtPXEo+DgZvQwQn3F4BwX+GZawHbbMjWCIDxoSmph5TfZMzF0EkhEi47Uk93FPgiBQPjKSYMxnJ9Lo9UwZjsxdtk7ONKe1GIxfHdx3no4uuRp8WKqjpz017VBOWubdXPxO8Q8QOv6nT9faVVCMUQApehFlg==",

var loadWithEncoding01 = loadTextContent('Assets/encoding-iso.txt', 'iso-8859-1')
//@[91:91]     "loadWithEncoding01": "¡\t\tinverted exclamation mark\n¢\t\tcent\n£\t\tpound\n¤\t\tcurrency\n¥\t\tyen\n¦\t\tbroken vertical bar\n§\t\tsection\n¨\t\tspacing diaeresis\n©\t\tcopyright\nª\t\tfeminine ordinal indicator\n«\t\tangle quotation mark (left)\n¬\t\tnegation\n­\t\tsoft hyphen\n®\t\tregistered trademark\n¯\t\tspacing macron\n°\t\tdegree\n±\t\tplus-or-minus\n²\t\tsuperscript 2\n³\t\tsuperscript 3\n´\t\tspacing acute\n¶\t\tparagraph\n·\t\tmiddle dot\n¸\t\tspacing cedilla\n¹\t\tsuperscript 1\nº\t\tmasculine ordinal indicator\n»\t\tangle quotation mark (right)\n¼\t\tfraction 1/4\n½\t\tfraction 1/2\n¾\t\tfraction 3/4\n¿\t\tinverted question mark\nÀ\t\tcapital a, grave accent\nÁ\t\tcapital a, acute accent\nÂ\t\tcapital a, circumflex accent\nÃ\t\tcapital a, tilde\nÄ\t\tcapital a, umlaut mark\nÅ\t\tcapital a, ring\nÆ\t\tcapital ae\nÇ\t\tcapital c, cedilla\nÈ\t\tcapital e, grave accent\nÉ\t\tcapital e, acute accent\nÊ\t\tcapital e, circumflex accent\nË\t\tcapital e, umlaut mark\nÌ\t\tcapital i, grave accent\nÍ\t\tcapital i, acute accent\nÎ\t\tcapital i, circumflex accent\nÏ\t\tcapital i, umlaut mark\nÐ\t\tcapital eth, Icelandic\nÑ\t\tcapital n, tilde\nÒ\t\tcapital o, grave accent\nÓ\t\tcapital o, acute accent\nÔ\t\tcapital o, circumflex accent\nÕ\t\tcapital o, tilde\nÖ\t\tcapital o, umlaut mark\n×\t\tmultiplication\nØ\t\tcapital o, slash\nÙ\t\tcapital u, grave accent\nÚ\t\tcapital u, acute accent\nÛ\t\tcapital u, circumflex accent\nÜ\t\tcapital u, umlaut mark\nÝ\t\tcapital y, acute accent\nÞ\t\tcapital THORN, Icelandic\nß\t\tsmall sharp s, German\nà\t\tsmall a, grave accent\ná\t\tsmall a, acute accent\nâ\t\tsmall a, circumflex accent\nã\t\tsmall a, tilde\nä\t\tsmall a, umlaut mark\nå\t\tsmall a, ring\næ\t\tsmall ae\nç\t\tsmall c, cedilla\nè\t\tsmall e, grave accent\né\t\tsmall e, acute accent\nê\t\tsmall e, circumflex accent\në\t\tsmall e, umlaut mark\nì\t\tsmall i, grave accent\ní\t\tsmall i, acute accent\nî\t\tsmall i, circumflex accent\nï\t\tsmall i, umlaut mark\nð\t\tsmall eth, Icelandic\nñ\t\tsmall n, tilde\nò\t\tsmall o, grave accent\nó\t\tsmall o, acute accent\nô\t\tsmall o, circumflex accent\nõ\t\tsmall o, tilde\nö\t\tsmall o, umlaut mark\n÷\t\tdivision\nø\t\tsmall o, slash\nù\t\tsmall u, grave accent\nú\t\tsmall u, acute accent\nû\t\tsmall u, circumflex accent\nü\t\tsmall u, umlaut mark\ný\t\tsmall y, acute accent\nþ\t\tsmall thorn, Icelandic\nÿ\t\tsmall y, umlaut mark\n",
var loadWithEncoding06 = loadTextContent('Assets/encoding-ascii.txt', 'us-ascii')
//@[92:92]     "loadWithEncoding06": "32 = \n33 = !\n34 = \"\n35 = #\n36 = $\n37 = %\n38 = &\n39 = '\n40 = (\n41 = )\n42 = *\n43 = +\n44 = ,\n45 = -\n46 = .\n47 = /\n48 = 0\n49 = 1\n50 = 2\n51 = 3\n52 = 4\n53 = 5\n54 = 6\n55 = 7\n56 = 8\n57 = 9\n58 = :\n59 = ;\n60 = <\n61 = =\n62 = >\n63 = ?\n64 = @\n65 = A\n66 = B\n67 = C\n68 = D\n69 = E\n70 = F\n71 = G\n72 = H\n73 = I\n74 = J\n75 = K\n76 = L\n77 = M\n78 = N\n79 = O\n80 = P\n81 = Q\n82 = R\n83 = S\n84 = T\n85 = U\n86 = V\n87 = W\n88 = X\n89 = Y\n90 = Z\n91 = [\n92 = \\\n93 = ]\n94 = ^\n95 = _\n96 = `\n97 = a\n98 = b\n99 = c\n100 = d\n101 = e\n102 = f\n103 = g\n104 = h\n105 = i\n106 = j\n107 = k\n108 = l\n109 = m\n110 = n\n111 = o\n112 = p\n113 = q\n114 = r\n115 = s\n116 = t\n117 = u\n118 = v\n119 = w\n120 = x\n121 = y\n122 = z\n123 = {\n124 = |\n125 = }\n126 = ~\n",
var loadWithEncoding07 = loadTextContent('Assets/encoding-ascii.txt', 'iso-8859-1')
//@[93:93]     "loadWithEncoding07": "32 = \n33 = !\n34 = \"\n35 = #\n36 = $\n37 = %\n38 = &\n39 = '\n40 = (\n41 = )\n42 = *\n43 = +\n44 = ,\n45 = -\n46 = .\n47 = /\n48 = 0\n49 = 1\n50 = 2\n51 = 3\n52 = 4\n53 = 5\n54 = 6\n55 = 7\n56 = 8\n57 = 9\n58 = :\n59 = ;\n60 = <\n61 = =\n62 = >\n63 = ?\n64 = @\n65 = A\n66 = B\n67 = C\n68 = D\n69 = E\n70 = F\n71 = G\n72 = H\n73 = I\n74 = J\n75 = K\n76 = L\n77 = M\n78 = N\n79 = O\n80 = P\n81 = Q\n82 = R\n83 = S\n84 = T\n85 = U\n86 = V\n87 = W\n88 = X\n89 = Y\n90 = Z\n91 = [\n92 = \\\n93 = ]\n94 = ^\n95 = _\n96 = `\n97 = a\n98 = b\n99 = c\n100 = d\n101 = e\n102 = f\n103 = g\n104 = h\n105 = i\n106 = j\n107 = k\n108 = l\n109 = m\n110 = n\n111 = o\n112 = p\n113 = q\n114 = r\n115 = s\n116 = t\n117 = u\n118 = v\n119 = w\n120 = x\n121 = y\n122 = z\n123 = {\n124 = |\n125 = }\n126 = ~\n",
var loadWithEncoding08 = loadTextContent('Assets/encoding-ascii.txt', 'utf-8')
//@[94:94]     "loadWithEncoding08": "32 = \n33 = !\n34 = \"\n35 = #\n36 = $\n37 = %\n38 = &\n39 = '\n40 = (\n41 = )\n42 = *\n43 = +\n44 = ,\n45 = -\n46 = .\n47 = /\n48 = 0\n49 = 1\n50 = 2\n51 = 3\n52 = 4\n53 = 5\n54 = 6\n55 = 7\n56 = 8\n57 = 9\n58 = :\n59 = ;\n60 = <\n61 = =\n62 = >\n63 = ?\n64 = @\n65 = A\n66 = B\n67 = C\n68 = D\n69 = E\n70 = F\n71 = G\n72 = H\n73 = I\n74 = J\n75 = K\n76 = L\n77 = M\n78 = N\n79 = O\n80 = P\n81 = Q\n82 = R\n83 = S\n84 = T\n85 = U\n86 = V\n87 = W\n88 = X\n89 = Y\n90 = Z\n91 = [\n92 = \\\n93 = ]\n94 = ^\n95 = _\n96 = `\n97 = a\n98 = b\n99 = c\n100 = d\n101 = e\n102 = f\n103 = g\n104 = h\n105 = i\n106 = j\n107 = k\n108 = l\n109 = m\n110 = n\n111 = o\n112 = p\n113 = q\n114 = r\n115 = s\n116 = t\n117 = u\n118 = v\n119 = w\n120 = x\n121 = y\n122 = z\n123 = {\n124 = |\n125 = }\n126 = ~\n",
var loadWithEncoding11 = loadTextContent('Assets/encoding-utf8.txt', 'utf-8')
//@[95:95]     "loadWithEncoding11": "💪😊😈🍕☕\r\n🐱‍👤\r\n\r\n朝辞白帝彩云间\r\n千里江陵一日还\r\n两岸猿声啼不住\r\n轻舟已过万重山\r\n\r\nΠ π Φ φ\r\n\r\n😎\r\n\r\nαα\r\nΩω\r\nΘ  \r\n\r\nZażółć gęślą jaźń\r\n\r\náéóúñü - ¡Hola!\r\n\r\n二头肌二头肌\r\n\r\n\r\n二头肌\r\nΘ二头肌α\r\n\r\n𐐷\r\n\\u{10437}\r\n\\u{D801}\\u{DC37}\r\n\r\n❆ Hello\\u{20}World\\u{21} ❁\r\n\r\n\ta\tb\tc\td\te\tf\tg\th\t\r\n8\t♜\t♞\t♝\t♛\t♚\t♝\t♞\t♜\t8\r\n7\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t7\r\n6\t\t\t\t\t\t\t\t\t6\r\n5\t\t\t\t\t\t\t\t\t5\r\n4\t\t\t\t\t\t\t\t\t4\r\n3\t\t\t\t\t\t\t\t\t3\r\n2\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t2\r\n1\t♖\t♘\t♗\t♕\t♔\t♗\t♘\t♖\t1\r\n\ta\tb\tc\td\te\tf\tg\th\r\n",
var loadWithEncoding12 = loadTextContent('Assets/encoding-utf8-bom.txt', 'utf-8')
//@[96:96]     "loadWithEncoding12": "💪😊😈🍕☕\r\n🐱‍👤\r\n\r\n朝辞白帝彩云间\r\n千里江陵一日还\r\n两岸猿声啼不住\r\n轻舟已过万重山\r\n\r\nΠ π Φ φ\r\n\r\n😎\r\n\r\nαα\r\nΩω\r\nΘ  \r\n\r\nZażółć gęślą jaźń\r\n\r\náéóúñü - ¡Hola!\r\n\r\n二头肌二头肌\r\n\r\n\r\n二头肌\r\nΘ二头肌α\r\n\r\n𐐷\r\n\\u{10437}\r\n\\u{D801}\\u{DC37}\r\n\r\n❆ Hello\\u{20}World\\u{21} ❁\r\n\r\n\ta\tb\tc\td\te\tf\tg\th\t\r\n8\t♜\t♞\t♝\t♛\t♚\t♝\t♞\t♜\t8\r\n7\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t♟\t7\r\n6\t\t\t\t\t\t\t\t\t6\r\n5\t\t\t\t\t\t\t\t\t5\r\n4\t\t\t\t\t\t\t\t\t4\r\n3\t\t\t\t\t\t\t\t\t3\r\n2\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t♙\t2\r\n1\t♖\t♘\t♗\t♕\t♔\t♗\t♘\t♖\t1\r\n\ta\tb\tc\td\te\tf\tg\th\r\n",

var testJson = json(loadTextContent('./Assets/test.json.txt'))
//@[97:97]     "testJson": "[json(variables('$fxv#15'))]",
var testJsonString = testJson.string
//@[98:98]     "testJsonString": "[variables('testJson').string]",
var testJsonInt = testJson.int
//@[99:99]     "testJsonInt": "[variables('testJson').int]",
var testJsonArrayVal = testJson.array[0]
//@[100:100]     "testJsonArrayVal": "[variables('testJson').array[0]]",
var testJsonObject = testJson.object
//@[101:101]     "testJsonObject": "[variables('testJson').object]",
var testJsonNestedString = testJson.object.nestedString
//@[102:102]     "testJsonNestedString": "[variables('testJson').object.nestedString]",

var testJson2 = loadJsonContent('./Assets/test.json.txt')
//@[103:103]     "testJson2": "[variables('$fxv#16')]",
var testJsonString2 = testJson.string
//@[104:104]     "testJsonString2": "[variables('testJson').string]",
var testJsonString2_1 = loadJsonContent('./Assets/test.json.txt', '.string')
//@[105:105]     "testJsonString2_1": "[variables('$fxv#17')]",
var testJsonInt2 = testJson.int
//@[106:106]     "testJsonInt2": "[variables('testJson').int]",
var testJsonInt2_1 = loadJsonContent('./Assets/test.json.txt', '.int')
//@[107:107]     "testJsonInt2_1": "[variables('$fxv#18')]",
var testJsonArrayVal2 = testJson.array[0]
//@[108:108]     "testJsonArrayVal2": "[variables('testJson').array[0]]",
var testJsonArrayVal2_1 = loadJsonContent('./Assets/test.json.txt', '.array[0]')
//@[109:109]     "testJsonArrayVal2_1": "[variables('$fxv#19')]",
var testJsonObject2 = testJson.object
//@[110:110]     "testJsonObject2": "[variables('testJson').object]",
var testJsonObject2_1 = loadJsonContent('./Assets/test.json.txt', '.object')
//@[111:111]     "testJsonObject2_1": "[variables('$fxv#20')]",
var testJsonNestedString2 = testJson.object.nestedString
//@[112:112]     "testJsonNestedString2": "[variables('testJson').object.nestedString]",
var testJsonNestedString2_1 = testJsonObject2_1.nestedString
//@[113:113]     "testJsonNestedString2_1": "[variables('testJsonObject2_1').nestedString]",
var testJsonNestedString2_2 = loadJsonContent('./Assets/test.json.txt', '.object.nestedString')
//@[114:114]     "testJsonNestedString2_2": "[variables('$fxv#21')]",

var testJsonTokensAsArray = loadJsonContent('./Assets/test2.json.txt', '.products[?(@.price > 3)].name')
//@[115:115]     "testJsonTokensAsArray": "[variables('$fxv#22')]"

