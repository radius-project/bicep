param foo string

import radius as radius {
  foo: 'foo'
}

resource env 'Applications.Core/environments@2022-03-15-privatepreview' = {
  name: foo
  location: 'westus2'
  properties: {
    compute:{
      kind: 'kubernetes'
    }
  }
}
