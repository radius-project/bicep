application app = {
  name: 'app'
}


component backend 'oam.dev/Container@v1alpha1' = {
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
  application: 'app'
  name: 'thingy'
  properties: {
  }
}

resource dnsZone 'Microsoft.Network/dnszones@2018-05-01' = {
  name: 'myZone'
  location: 'global'
}
