application app = {
//@[0:629) ApplicationDeclarationSyntax
//@[0:11)  Identifier |application|
//@[12:15)  IdentifierSyntax
//@[12:15)   Identifier |app|
//@[16:17)  Assignment |=|
//@[18:629)  ObjectSyntax
//@[18:19)   LeftBrace |{|
//@[19:20)   NewLine |\n|
  name: 'app'
//@[2:13)   ObjectPropertySyntax
//@[2:6)    IdentifierSyntax
//@[2:6)     Identifier |name|
//@[6:7)    Colon |:|
//@[8:13)    StringSyntax
//@[8:13)     StringComplete |'app'|
//@[13:15)   NewLine |\n\n|

  component backend 'oam.dev/Container@v1alpha1' = {
//@[2:320)   ComponentDeclarationSyntax
//@[2:11)    Identifier |component|
//@[12:19)    IdentifierSyntax
//@[12:19)     Identifier |backend|
//@[20:48)    StringSyntax
//@[20:48)     StringComplete |'oam.dev/Container@v1alpha1'|
//@[49:50)    Assignment |=|
//@[51:320)    ObjectSyntax
//@[51:52)     LeftBrace |{|
//@[52:53)     NewLine |\n|
    name: 'backend'
//@[4:19)     ObjectPropertySyntax
//@[4:8)      IdentifierSyntax
//@[4:8)       Identifier |name|
//@[8:9)      Colon |:|
//@[10:19)      StringSyntax
//@[10:19)       StringComplete |'backend'|
//@[19:20)     NewLine |\n|
    properties: {
//@[4:243)     ObjectPropertySyntax
//@[4:14)      IdentifierSyntax
//@[4:14)       Identifier |properties|
//@[14:15)      Colon |:|
//@[16:243)      ObjectSyntax
//@[16:17)       LeftBrace |{|
//@[17:18)       NewLine |\n|
      run: {
//@[6:219)       ObjectPropertySyntax
//@[6:9)        IdentifierSyntax
//@[6:9)         Identifier |run|
//@[9:10)        Colon |:|
//@[11:219)        ObjectSyntax
//@[11:12)         LeftBrace |{|
//@[12:13)         NewLine |\n|
        container: {
//@[8:198)         ObjectPropertySyntax
//@[8:17)          IdentifierSyntax
//@[8:17)           Identifier |container|
//@[17:18)          Colon |:|
//@[19:198)          ObjectSyntax
//@[19:20)           LeftBrace |{|
//@[20:21)           NewLine |\n|
          image: 'test/test-image:latest'
//@[10:41)           ObjectPropertySyntax
//@[10:15)            IdentifierSyntax
//@[10:15)             Identifier |image|
//@[15:16)            Colon |:|
//@[17:41)            StringSyntax
//@[17:41)             StringComplete |'test/test-image:latest'|
//@[41:42)           NewLine |\n|
          env: [
//@[10:125)           ObjectPropertySyntax
//@[10:13)            IdentifierSyntax
//@[10:13)             Identifier |env|
//@[13:14)            Colon |:|
//@[15:125)            ArraySyntax
//@[15:16)             LeftSquare |[|
//@[16:17)             NewLine |\n|
            {
//@[12:96)             ArrayItemSyntax
//@[12:96)              ObjectSyntax
//@[12:13)               LeftBrace |{|
//@[13:14)               NewLine |\n|
              name: 'LOCATION'
//@[14:30)               ObjectPropertySyntax
//@[14:18)                IdentifierSyntax
//@[14:18)                 Identifier |name|
//@[18:19)                Colon |:|
//@[20:30)                StringSyntax
//@[20:30)                 StringComplete |'LOCATION'|
//@[30:31)               NewLine |\n|
              value: dnsZone.location
//@[14:37)               ObjectPropertySyntax
//@[14:19)                IdentifierSyntax
//@[14:19)                 Identifier |value|
//@[19:20)                Colon |:|
//@[21:37)                PropertyAccessSyntax
//@[21:28)                 VariableAccessSyntax
//@[21:28)                  IdentifierSyntax
//@[21:28)                   Identifier |dnsZone|
//@[28:29)                 Dot |.|
//@[29:37)                 IdentifierSyntax
//@[29:37)                  Identifier |location|
//@[37:38)               NewLine |\n|
            }
//@[12:13)               RightBrace |}|
//@[13:14)             NewLine |\n|
          ]
//@[10:11)             RightSquare |]|
//@[11:12)           NewLine |\n|
        }
//@[8:9)           RightBrace |}|
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
  
//@[2:3)   NewLine |\n|
  deployment dep = {
//@[2:150)   DeploymentDeclarationSyntax
//@[2:12)    Identifier |deployment|
//@[13:16)    IdentifierSyntax
//@[13:16)     Identifier |dep|
//@[17:18)    Assignment |=|
//@[19:150)    ObjectSyntax
//@[19:20)     LeftBrace |{|
//@[20:21)     NewLine |\n|
    name: 'dep'
//@[4:15)     ObjectPropertySyntax
//@[4:8)      IdentifierSyntax
//@[4:8)       Identifier |name|
//@[8:9)      Colon |:|
//@[10:15)      StringSyntax
//@[10:15)       StringComplete |'dep'|
//@[15:16)     NewLine |\n|
    properties: {
//@[4:109)     ObjectPropertySyntax
//@[4:14)      IdentifierSyntax
//@[4:14)       Identifier |properties|
//@[14:15)      Colon |:|
//@[16:109)      ObjectSyntax
//@[16:17)       LeftBrace |{|
//@[17:18)       NewLine |\n|
      components: [
//@[6:85)       ObjectPropertySyntax
//@[6:16)        IdentifierSyntax
//@[6:16)         Identifier |components|
//@[16:17)        Colon |:|
//@[18:85)        ArraySyntax
//@[18:19)         LeftSquare |[|
//@[19:20)         NewLine |\n|
        {
//@[8:57)         ArrayItemSyntax
//@[8:57)          ObjectSyntax
//@[8:9)           LeftBrace |{|
//@[9:10)           NewLine |\n|
          componentName: backend.name
//@[10:37)           ObjectPropertySyntax
//@[10:23)            IdentifierSyntax
//@[10:23)             Identifier |componentName|
//@[23:24)            Colon |:|
//@[25:37)            PropertyAccessSyntax
//@[25:32)             VariableAccessSyntax
//@[25:32)              IdentifierSyntax
//@[25:32)               Identifier |backend|
//@[32:33)             Dot |.|
//@[33:37)             IdentifierSyntax
//@[33:37)              Identifier |name|
//@[37:38)           NewLine |\n|
        }
//@[8:9)           RightBrace |}|
//@[9:10)         NewLine |\n|
      ]
//@[6:7)         RightSquare |]|
//@[7:8)       NewLine |\n|
    }
//@[4:5)       RightBrace |}|
//@[5:6)     NewLine |\n|
  }
//@[2:3)     RightBrace |}|
//@[3:4)   NewLine |\n|
  
//@[2:3)   NewLine |\n|
  instance thingy 'core.oam.dev/ContainerizedWorkload@v1alpha3' = {
//@[2:114)   InstanceDeclarationSyntax
//@[2:10)    Identifier |instance|
//@[11:17)    IdentifierSyntax
//@[11:17)     Identifier |thingy|
//@[18:63)    StringSyntax
//@[18:63)     StringComplete |'core.oam.dev/ContainerizedWorkload@v1alpha3'|
//@[64:65)    Assignment |=|
//@[66:114)    ObjectSyntax
//@[66:67)     LeftBrace |{|
//@[67:68)     NewLine |\n|
    name: 'thingy'
//@[4:18)     ObjectPropertySyntax
//@[4:8)      IdentifierSyntax
//@[4:8)       Identifier |name|
//@[8:9)      Colon |:|
//@[10:18)      StringSyntax
//@[10:18)       StringComplete |'thingy'|
//@[18:19)     NewLine |\n|
    properties: {
//@[4:23)     ObjectPropertySyntax
//@[4:14)      IdentifierSyntax
//@[4:14)       Identifier |properties|
//@[14:15)      Colon |:|
//@[16:23)      ObjectSyntax
//@[16:17)       LeftBrace |{|
//@[17:18)       NewLine |\n|
    }
//@[4:5)       RightBrace |}|
//@[5:6)     NewLine |\n|
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
