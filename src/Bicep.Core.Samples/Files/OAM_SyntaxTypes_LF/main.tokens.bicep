application app = {
//@[0:11) Identifier |application|
//@[12:15) Identifier |app|
//@[16:17) Assignment |=|
//@[18:19) LeftBrace |{|
//@[19:20) NewLine |\n|
  name: 'app'
//@[2:6) Identifier |name|
//@[6:7) Colon |:|
//@[8:13) StringComplete |'app'|
//@[13:14) NewLine |\n|
}
//@[0:1) RightBrace |}|
//@[1:4) NewLine |\n\n\n|


component backend 'oam.dev/Container@v1alpha1' = {
//@[0:9) Identifier |component|
//@[10:17) Identifier |backend|
//@[18:46) StringComplete |'oam.dev/Container@v1alpha1'|
//@[47:48) Assignment |=|
//@[49:50) LeftBrace |{|
//@[50:51) NewLine |\n|
  name: 'backend'
//@[2:6) Identifier |name|
//@[6:7) Colon |:|
//@[8:17) StringComplete |'backend'|
//@[17:18) NewLine |\n|
  application: app.name
//@[2:13) Identifier |application|
//@[13:14) Colon |:|
//@[15:18) Identifier |app|
//@[18:19) Dot |.|
//@[19:23) Identifier |name|
//@[23:24) NewLine |\n|
  properties: {
//@[2:12) Identifier |properties|
//@[12:13) Colon |:|
//@[14:15) LeftBrace |{|
//@[15:16) NewLine |\n|
    run: {
//@[4:7) Identifier |run|
//@[7:8) Colon |:|
//@[9:10) LeftBrace |{|
//@[10:11) NewLine |\n|
      container: {
//@[6:15) Identifier |container|
//@[15:16) Colon |:|
//@[17:18) LeftBrace |{|
//@[18:19) NewLine |\n|
        image: 'test/test-image:latest'
//@[8:13) Identifier |image|
//@[13:14) Colon |:|
//@[15:39) StringComplete |'test/test-image:latest'|
//@[39:40) NewLine |\n|
        env: [
//@[8:11) Identifier |env|
//@[11:12) Colon |:|
//@[13:14) LeftSquare |[|
//@[14:15) NewLine |\n|
          {
//@[10:11) LeftBrace |{|
//@[11:12) NewLine |\n|
            name: 'LOCATION'
//@[12:16) Identifier |name|
//@[16:17) Colon |:|
//@[18:28) StringComplete |'LOCATION'|
//@[28:29) NewLine |\n|
            value: dnsZone.location
//@[12:17) Identifier |value|
//@[17:18) Colon |:|
//@[19:26) Identifier |dnsZone|
//@[26:27) Dot |.|
//@[27:35) Identifier |location|
//@[35:36) NewLine |\n|
          }
//@[10:11) RightBrace |}|
//@[11:12) NewLine |\n|
        ]
//@[8:9) RightSquare |]|
//@[9:10) NewLine |\n|
      }
//@[6:7) RightBrace |}|
//@[7:8) NewLine |\n|
    }
//@[4:5) RightBrace |}|
//@[5:6) NewLine |\n|
  }
//@[2:3) RightBrace |}|
//@[3:4) NewLine |\n|
}
//@[0:1) RightBrace |}|
//@[1:3) NewLine |\n\n|

deployment dep = {
//@[0:10) Identifier |deployment|
//@[11:14) Identifier |dep|
//@[15:16) Assignment |=|
//@[17:18) LeftBrace |{|
//@[18:19) NewLine |\n|
  application: 'app'
//@[2:13) Identifier |application|
//@[13:14) Colon |:|
//@[15:20) StringComplete |'app'|
//@[20:21) NewLine |\n|
  name: 'dep'
//@[2:6) Identifier |name|
//@[6:7) Colon |:|
//@[8:13) StringComplete |'dep'|
//@[13:14) NewLine |\n|
  properties: {
//@[2:12) Identifier |properties|
//@[12:13) Colon |:|
//@[14:15) LeftBrace |{|
//@[15:16) NewLine |\n|
    components: [
//@[4:14) Identifier |components|
//@[14:15) Colon |:|
//@[16:17) LeftSquare |[|
//@[17:18) NewLine |\n|
      {
//@[6:7) LeftBrace |{|
//@[7:8) NewLine |\n|
        componentName: backend.name
//@[8:21) Identifier |componentName|
//@[21:22) Colon |:|
//@[23:30) Identifier |backend|
//@[30:31) Dot |.|
//@[31:35) Identifier |name|
//@[35:36) NewLine |\n|
      }
//@[6:7) RightBrace |}|
//@[7:8) NewLine |\n|
    ]
//@[4:5) RightSquare |]|
//@[5:6) NewLine |\n|
  }
//@[2:3) RightBrace |}|
//@[3:4) NewLine |\n|
}
//@[0:1) RightBrace |}|
//@[1:3) NewLine |\n\n|

instance thingy 'core.oam.dev/ContainerizedWorkload@v1alpha3' = {
//@[0:8) Identifier |instance|
//@[9:15) Identifier |thingy|
//@[16:61) StringComplete |'core.oam.dev/ContainerizedWorkload@v1alpha3'|
//@[62:63) Assignment |=|
//@[64:65) LeftBrace |{|
//@[65:66) NewLine |\n|
  application: 'app'
//@[2:13) Identifier |application|
//@[13:14) Colon |:|
//@[15:20) StringComplete |'app'|
//@[20:21) NewLine |\n|
  name: 'thingy'
//@[2:6) Identifier |name|
//@[6:7) Colon |:|
//@[8:16) StringComplete |'thingy'|
//@[16:17) NewLine |\n|
  properties: {
//@[2:12) Identifier |properties|
//@[12:13) Colon |:|
//@[14:15) LeftBrace |{|
//@[15:16) NewLine |\n|
  }
//@[2:3) RightBrace |}|
//@[3:4) NewLine |\n|
}
//@[0:1) RightBrace |}|
//@[1:3) NewLine |\n\n|

resource dnsZone 'Microsoft.Network/dnszones@2018-05-01' = {
//@[0:8) Identifier |resource|
//@[9:16) Identifier |dnsZone|
//@[17:56) StringComplete |'Microsoft.Network/dnszones@2018-05-01'|
//@[57:58) Assignment |=|
//@[59:60) LeftBrace |{|
//@[60:61) NewLine |\n|
  name: 'myZone'
//@[2:6) Identifier |name|
//@[6:7) Colon |:|
//@[8:16) StringComplete |'myZone'|
//@[16:17) NewLine |\n|
  location: 'global'
//@[2:10) Identifier |location|
//@[10:11) Colon |:|
//@[12:20) StringComplete |'global'|
//@[20:21) NewLine |\n|
}
//@[0:1) RightBrace |}|
//@[1:1) EndOfFile ||
