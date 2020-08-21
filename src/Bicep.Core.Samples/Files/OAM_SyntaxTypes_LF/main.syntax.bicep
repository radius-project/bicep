application app = {
//@[0:35) ApplicationDeclarationSyntax
//@[0:11)  Identifier |application|
//@[12:15)  IdentifierSyntax
//@[12:15)   Identifier |app|
//@[16:17)  Assignment |=|
//@[18:35)  ObjectSyntax
//@[18:19)   LeftBrace |{|
//@[19:20)   NewLine |\n|
  name: 'app'
//@[2:13)   ObjectPropertySyntax
//@[2:6)    IdentifierSyntax
//@[2:6)     Identifier |name|
//@[6:7)    Colon |:|
//@[8:13)    StringSyntax
//@[8:13)     StringComplete |'app'|
//@[13:14)   NewLine |\n|
}
//@[0:1)   RightBrace |}|
//@[1:4) NewLine |\n\n\n|


component backend 'oam.dev/Container@v1alpha1' = {
//@[0:312) ComponentDeclarationSyntax
//@[0:9)  Identifier |component|
//@[10:17)  IdentifierSyntax
//@[10:17)   Identifier |backend|
//@[18:46)  StringSyntax
//@[18:46)   StringComplete |'oam.dev/Container@v1alpha1'|
//@[47:48)  Assignment |=|
//@[49:312)  ObjectSyntax
//@[49:50)   LeftBrace |{|
//@[50:51)   NewLine |\n|
  name: 'backend'
//@[2:17)   ObjectPropertySyntax
//@[2:6)    IdentifierSyntax
//@[2:6)     Identifier |name|
//@[6:7)    Colon |:|
//@[8:17)    StringSyntax
//@[8:17)     StringComplete |'backend'|
//@[17:18)   NewLine |\n|
  application: app.name
//@[2:23)   ObjectPropertySyntax
//@[2:13)    IdentifierSyntax
//@[2:13)     Identifier |application|
//@[13:14)    Colon |:|
//@[15:23)    PropertyAccessSyntax
//@[15:18)     VariableAccessSyntax
//@[15:18)      IdentifierSyntax
//@[15:18)       Identifier |app|
//@[18:19)     Dot |.|
//@[19:23)     IdentifierSyntax
//@[19:23)      Identifier |name|
//@[23:24)   NewLine |\n|
  properties: {
//@[2:217)   ObjectPropertySyntax
//@[2:12)    IdentifierSyntax
//@[2:12)     Identifier |properties|
//@[12:13)    Colon |:|
//@[14:217)    ObjectSyntax
//@[14:15)     LeftBrace |{|
//@[15:16)     NewLine |\n|
    run: {
//@[4:197)     ObjectPropertySyntax
//@[4:7)      IdentifierSyntax
//@[4:7)       Identifier |run|
//@[7:8)      Colon |:|
//@[9:197)      ObjectSyntax
//@[9:10)       LeftBrace |{|
//@[10:11)       NewLine |\n|
      container: {
//@[6:180)       ObjectPropertySyntax
//@[6:15)        IdentifierSyntax
//@[6:15)         Identifier |container|
//@[15:16)        Colon |:|
//@[17:180)        ObjectSyntax
//@[17:18)         LeftBrace |{|
//@[18:19)         NewLine |\n|
        image: 'test/test-image:latest'
//@[8:39)         ObjectPropertySyntax
//@[8:13)          IdentifierSyntax
//@[8:13)           Identifier |image|
//@[13:14)          Colon |:|
//@[15:39)          StringSyntax
//@[15:39)           StringComplete |'test/test-image:latest'|
//@[39:40)         NewLine |\n|
        env: [
//@[8:113)         ObjectPropertySyntax
//@[8:11)          IdentifierSyntax
//@[8:11)           Identifier |env|
//@[11:12)          Colon |:|
//@[13:113)          ArraySyntax
//@[13:14)           LeftSquare |[|
//@[14:15)           NewLine |\n|
          {
//@[10:88)           ArrayItemSyntax
//@[10:88)            ObjectSyntax
//@[10:11)             LeftBrace |{|
//@[11:12)             NewLine |\n|
            name: 'LOCATION'
//@[12:28)             ObjectPropertySyntax
//@[12:16)              IdentifierSyntax
//@[12:16)               Identifier |name|
//@[16:17)              Colon |:|
//@[18:28)              StringSyntax
//@[18:28)               StringComplete |'LOCATION'|
//@[28:29)             NewLine |\n|
            value: dnsZone.location
//@[12:35)             ObjectPropertySyntax
//@[12:17)              IdentifierSyntax
//@[12:17)               Identifier |value|
//@[17:18)              Colon |:|
//@[19:35)              PropertyAccessSyntax
//@[19:26)               VariableAccessSyntax
//@[19:26)                IdentifierSyntax
//@[19:26)                 Identifier |dnsZone|
//@[26:27)               Dot |.|
//@[27:35)               IdentifierSyntax
//@[27:35)                Identifier |location|
//@[35:36)             NewLine |\n|
          }
//@[10:11)             RightBrace |}|
//@[11:12)           NewLine |\n|
        ]
//@[8:9)           RightSquare |]|
//@[9:10)         NewLine |\n|
      }
//@[6:7)         RightBrace |}|
//@[7:8)       NewLine |\n|
    }
//@[4:5)       RightBrace |}|
//@[5:6)     NewLine |\n|
  }
//@[2:3)     RightBrace |}|
//@[3:4)   NewLine |\n|
}
//@[0:1)   RightBrace |}|
//@[1:3) NewLine |\n\n|

deployment dep = {
//@[0:151) DeploymentDeclarationSyntax
//@[0:10)  Identifier |deployment|
//@[11:14)  IdentifierSyntax
//@[11:14)   Identifier |dep|
//@[15:16)  Assignment |=|
//@[17:151)  ObjectSyntax
//@[17:18)   LeftBrace |{|
//@[18:19)   NewLine |\n|
  application: 'app'
//@[2:20)   ObjectPropertySyntax
//@[2:13)    IdentifierSyntax
//@[2:13)     Identifier |application|
//@[13:14)    Colon |:|
//@[15:20)    StringSyntax
//@[15:20)     StringComplete |'app'|
//@[20:21)   NewLine |\n|
  name: 'dep'
//@[2:13)   ObjectPropertySyntax
//@[2:6)    IdentifierSyntax
//@[2:6)     Identifier |name|
//@[6:7)    Colon |:|
//@[8:13)    StringSyntax
//@[8:13)     StringComplete |'dep'|
//@[13:14)   NewLine |\n|
  properties: {
//@[2:95)   ObjectPropertySyntax
//@[2:12)    IdentifierSyntax
//@[2:12)     Identifier |properties|
//@[12:13)    Colon |:|
//@[14:95)    ObjectSyntax
//@[14:15)     LeftBrace |{|
//@[15:16)     NewLine |\n|
    components: [
//@[4:75)     ObjectPropertySyntax
//@[4:14)      IdentifierSyntax
//@[4:14)       Identifier |components|
//@[14:15)      Colon |:|
//@[16:75)      ArraySyntax
//@[16:17)       LeftSquare |[|
//@[17:18)       NewLine |\n|
      {
//@[6:51)       ArrayItemSyntax
//@[6:51)        ObjectSyntax
//@[6:7)         LeftBrace |{|
//@[7:8)         NewLine |\n|
        componentName: backend.name
//@[8:35)         ObjectPropertySyntax
//@[8:21)          IdentifierSyntax
//@[8:21)           Identifier |componentName|
//@[21:22)          Colon |:|
//@[23:35)          PropertyAccessSyntax
//@[23:30)           VariableAccessSyntax
//@[23:30)            IdentifierSyntax
//@[23:30)             Identifier |backend|
//@[30:31)           Dot |.|
//@[31:35)           IdentifierSyntax
//@[31:35)            Identifier |name|
//@[35:36)         NewLine |\n|
      }
//@[6:7)         RightBrace |}|
//@[7:8)       NewLine |\n|
    ]
//@[4:5)       RightSquare |]|
//@[5:6)     NewLine |\n|
  }
//@[2:3)     RightBrace |}|
//@[3:4)   NewLine |\n|
}
//@[0:1)   RightBrace |}|
//@[1:3) NewLine |\n\n|

instance thingy 'core.oam.dev/ContainerizedWorkload@v1alpha3' = {
//@[0:125) InstanceDeclarationSyntax
//@[0:8)  Identifier |instance|
//@[9:15)  IdentifierSyntax
//@[9:15)   Identifier |thingy|
//@[16:61)  StringSyntax
//@[16:61)   StringComplete |'core.oam.dev/ContainerizedWorkload@v1alpha3'|
//@[62:63)  Assignment |=|
//@[64:125)  ObjectSyntax
//@[64:65)   LeftBrace |{|
//@[65:66)   NewLine |\n|
  application: 'app'
//@[2:20)   ObjectPropertySyntax
//@[2:13)    IdentifierSyntax
//@[2:13)     Identifier |application|
//@[13:14)    Colon |:|
//@[15:20)    StringSyntax
//@[15:20)     StringComplete |'app'|
//@[20:21)   NewLine |\n|
  name: 'thingy'
//@[2:16)   ObjectPropertySyntax
//@[2:6)    IdentifierSyntax
//@[2:6)     Identifier |name|
//@[6:7)    Colon |:|
//@[8:16)    StringSyntax
//@[8:16)     StringComplete |'thingy'|
//@[16:17)   NewLine |\n|
  properties: {
//@[2:19)   ObjectPropertySyntax
//@[2:12)    IdentifierSyntax
//@[2:12)     Identifier |properties|
//@[12:13)    Colon |:|
//@[14:19)    ObjectSyntax
//@[14:15)     LeftBrace |{|
//@[15:16)     NewLine |\n|
  }
//@[2:3)     RightBrace |}|
//@[3:4)   NewLine |\n|
}
//@[0:1)   RightBrace |}|
//@[1:3) NewLine |\n\n|

resource dnsZone 'Microsoft.Network/dnszones@2018-05-01' = {
//@[0:100) ResourceDeclarationSyntax
//@[0:8)  Identifier |resource|
//@[9:16)  IdentifierSyntax
//@[9:16)   Identifier |dnsZone|
//@[17:56)  StringSyntax
//@[17:56)   StringComplete |'Microsoft.Network/dnszones@2018-05-01'|
//@[57:58)  Assignment |=|
//@[59:100)  ObjectSyntax
//@[59:60)   LeftBrace |{|
//@[60:61)   NewLine |\n|
  name: 'myZone'
//@[2:16)   ObjectPropertySyntax
//@[2:6)    IdentifierSyntax
//@[2:6)     Identifier |name|
//@[6:7)    Colon |:|
//@[8:16)    StringSyntax
//@[8:16)     StringComplete |'myZone'|
//@[16:17)   NewLine |\n|
  location: 'global'
//@[2:20)   ObjectPropertySyntax
//@[2:10)    IdentifierSyntax
//@[2:10)     Identifier |location|
//@[10:11)    Colon |:|
//@[12:20)    StringSyntax
//@[12:20)     StringComplete |'global'|
//@[20:21)   NewLine |\n|
}
//@[0:1)   RightBrace |}|
//@[1:1) EndOfFile ||
