﻿// Automation Runbook
resource automationAccount 'Microsoft.Automation/automationAccounts@2019-06-01' = {
  name: /*${1:'name'}*/'name'
}

resource /*${2:automationRunbook}*/automationRunbook 'Microsoft.Automation/automationAccounts/runbooks@2019-06-01' = {
  parent: automationAccount
  name: /*${3:'name'}*/'name'
  location: /*${4:location}*/'location'
  properties: {
    logVerbose: /*${5|true,false|}*/true
    logProgress: /*${6|true,false|}*/true
    runbookType: /*'${7|Script,Graph,PowerShellWorkflow,PowerShell,GraphPowerShellWorkflow,GraphPowerShell|}'*/'Script'
    publishContentLink: {
      uri: /*${8:'uri'}*/'uri'
      version: /*${9:'1.0.0.0'}*/'1.0.0.0'
    }
    description: /*${10:'description'}*/'description'
  }
}
