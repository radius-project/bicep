application app = {
//@[12:15) Application app. Type: Microsoft.CustomProviders/resourceProviders/Applications@2018-09-01-preview. Declaration start char: 0, length: 629
  name: 'app'

  component backend 'oam.dev/Container@v1alpha1' = {
//@[12:19) Component backend. Type: Microsoft.CustomProviders/resourceProviders/Applications/Components@2018-09-01-preview. Declaration start char: 2, length: 318
    name: 'backend'
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
//@[13:16) Deployment dep. Type: Microsoft.CustomProviders/resourceProviders/Applications/Deployments@2018-09-01-preview. Declaration start char: 2, length: 148
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
//@[11:17) Instance thingy. Type: Microsoft.CustomProviders/resourceProviders/Applications/Components@2018-09-01-preview. Declaration start char: 2, length: 112
    name: 'thingy'
    properties: {
    }
  }
}

resource dnsZone 'Microsoft.Network/dnszones@2018-05-01' = {
//@[9:16) Resource dnsZone. Type: Microsoft.Network/dnszones@2018-05-01. Declaration start char: 0, length: 100
  name: 'myZone'
  location: 'global'
}
