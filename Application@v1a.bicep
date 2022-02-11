resource app 'radius.dev/Application@v1alpha3' = {
  name: 'azure-resources-dapr-pubsub-generic'

  resource publisher 'ContainerComponent' = {
    name: 'publisher'
    properties: {
      connections: {
        daprpubsub: {
          kind: 'dapr.io/PubSubTopic'
          source: pubsub.id
        }
      }
      container: {
        image: 'radius.azurecr.io/magpie:latest'
      }
    }
  }

  resource Identifier 'dapr.io.SecretStoreComponent' = {
    
  }
 
  resource pubsub 'dapr.io.StateStoreComponent' = {
    name: 'pubsub'
    properties: {
      kind: 'any'
      
      
    }
  }
}

