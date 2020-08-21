application app = {
//@[12:15) Application app. Type: Microsoft.CustomProviders/resourceProviders/Applications@2018-09-01-preview. Declaration start char: 0, length: 35
  name: 'app'
}


component backend 'oam.dev/Container@v1alpha1' = {
//@[10:17) Component backend. Type: Microsoft.CustomProviders/resourceProviders/Applications/Components@2018-09-01-preview. Declaration start char: 0, length: 312
  name: 'backend'
  application: app.name
  properties: {
    run: {
      container: {
        image: 'test/test-image:latest'
        env: [
          {
            name: 'LOCATION'
            value: dnsZone.location
          }
        ]
      }
    }
  }
}

deployment dep = {
//@[11:14) Deployment dep. Type: Microsoft.CustomProviders/resourceProviders/Applications/Deployments@2018-09-01-preview. Declaration start char: 0, length: 151
  application: 'app'
  name: 'dep'
  properties: {
    components: [
      {
        componentName: backend.name
      }
    ]
  }
}

instance thingy 'core.oam.dev/ContainerizedWorkload@v1alpha3' = {
//@[9:15) Instance thingy. Type: Microsoft.CustomProviders/resourceProviders/Applications/Components@2018-09-01-preview. Declaration start char: 0, length: 125
  application: 'app'
  name: 'thingy'
  properties: {
  }
}

resource dnsZone 'Microsoft.Network/dnszones@2018-05-01' = {
//@[9:16) Resource dnsZone. Type: Microsoft.Network/dnszones@2018-05-01. Declaration start char: 0, length: 100
  name: 'myZone'
  location: 'global'
}
