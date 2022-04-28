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
import radius from radius {
  foo: 'foo'
}

resource foo 'Applications.Core/applications@2022-03-15-privatepreview' existing = {
  name: 'foo'
}
