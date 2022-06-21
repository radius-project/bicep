// import kubernetes from kubernetes

// resource secret 'kubernetes.core/Secret@v1' = {
//   metadata: {
//     name: 'redis-conn'
//     namespace: 'default'
//     labels: {
//       format: 'custom'
//     }
//   }

//   stringData: {
//     connectionString: 'test'
//   }
// }

// resource app 'radius.dev/Application@v1alpha3' = {
//   name: 'test'
// }
// import az as az

import radius as radius {
  foo: 'foo'
}

// import az as az2

resource env 'Applications.Core/environments@2022-03-15-privatepreview' = {
  name: 'prayingthiswor2'
  location: 'westus2'
  properties: {
    compute:{
      kind: 'kubernetes'
      resourceId: radius.foo
    }
  }
}

// resource foo 'Applications.Core/applications@2022-03-15-privatepreview' = {
//   name: 'foo'
//   location: 'westus2'
//   properties: {
//     environment: 'ucp:/planes/radius/local/resourceGroups/r1/providers/Applications.Core/environments/cool-env'
//   }
// }

// resource bar 'Microsoft.AAD/domainServices@2021-05-01' existing = {
//   name: 'foo'
// }

// resource account 'az:Microsoft.DocumentDB/databaseAccounts@2020-04-01' = {
//   name: 'account-${guid(resourceGroup().name)}'
//   location: resourceGroup().location
//   kind: 'MongoDB'
//   tags: {
//     radiustest: 'azure-resources-mongodb'
//   }
//   properties: {
//     consistencyPolicy: {
//       defaultConsistencyLevel: 'Session'
//     }
//     locations: [
//       {
//         locationName: resourceGroup().location
//         failoverPriority: 0
//         isZoneRedundant: false
//       }
//     ]
//     databaseAccountOfferType: 'Standard'
//   }

//   resource dbinner 'mongodbDatabases' = {
//     name: 'mydb'
//     properties: {
//       resource: {
//         id: 'mydb'
//       }
//       options: { 
//         throughput: 400
//       }
//     }
//   }
// }
