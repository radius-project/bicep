application app = {
  name: 'app'

  component backend 'oam.dev/Container@v1alpha1' = {
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
    name: 'thingy'
    properties: {
    }
  }
}

resource dnsZone 'Microsoft.Network/dnszones@2018-05-01' = {
  name: 'myZone'
  location: 'global'
}
