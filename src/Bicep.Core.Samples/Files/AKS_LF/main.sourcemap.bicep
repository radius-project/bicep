// mandatory params
param dnsPrefix string
//@[13:15]     "dnsPrefix": {
param linuxAdminUsername string
//@[16:18]     "linuxAdminUsername": {
param sshRSAPublicKey string
//@[19:21]     "sshRSAPublicKey": {

@secure()
param servcePrincipalClientId string
//@[22:24]     "servcePrincipalClientId": {

@secure()
param servicePrincipalClientSecret string
//@[25:27]     "servicePrincipalClientSecret": {

// optional params
param clusterName string = 'aks101cluster'
//@[28:31]     "clusterName": {
param location string = resourceGroup().location
//@[32:35]     "location": {

@minValue(0)
//@[40:40]       "minValue": 0
@maxValue(1023)
//@[39:39]       "maxValue": 1023,
param osDiskSizeGB int = 0
//@[36:41]     "osDiskSizeGB": {

@minValue(1)
//@[46:46]       "minValue": 1
@maxValue(50)
//@[45:45]       "maxValue": 50,
param agentCount int = 3
//@[42:47]     "agentCount": {

param agentVMSize string = 'Standard_DS2_v2'
//@[48:51]     "agentVMSize": {
// osType was a defaultValue with only one allowedValue, which seems strange?, could be a good TTK test

resource aks 'Microsoft.ContainerService/managedClusters@2020-03-01' = {
//@[54:85]     "aks": {
    name: clusterName
    location: location
//@[58:58]       "location": "[parameters('location')]",
    properties: {
//@[59:84]       "properties": {
        dnsPrefix: dnsPrefix
//@[60:60]         "dnsPrefix": "[parameters('dnsPrefix')]",
        agentPoolProfiles: [
//@[61:69]         "agentPoolProfiles": [
            {
                name: 'agentpool'
//@[63:63]             "name": "agentpool",
                osDiskSizeGB: osDiskSizeGB
//@[64:64]             "osDiskSizeGB": "[parameters('osDiskSizeGB')]",
                vmSize: agentVMSize
//@[65:65]             "vmSize": "[parameters('agentVMSize')]",
                osType: 'Linux'
//@[66:66]             "osType": "Linux",
                storageProfile: 'ManagedDisks'
//@[67:67]             "storageProfile": "ManagedDisks"
            }
        ]
        linuxProfile: {
//@[70:79]         "linuxProfile": {
            adminUsername: linuxAdminUsername
//@[71:71]           "adminUsername": "[parameters('linuxAdminUsername')]",
            ssh: {
//@[72:78]           "ssh": {
                publicKeys: [
//@[73:77]             "publicKeys": [
                    {
                        keyData: sshRSAPublicKey
//@[75:75]                 "keyData": "[parameters('sshRSAPublicKey')]"
                    }
                ]
            }
        }
        servicePrincipalProfile: {
//@[80:83]         "servicePrincipalProfile": {
            clientId: servcePrincipalClientId
//@[81:81]           "clientId": "[parameters('servcePrincipalClientId')]",
            secret: servicePrincipalClientSecret
//@[82:82]           "secret": "[parameters('servicePrincipalClientSecret')]"
        }
    }
}

// fyi - dot property access (aks.fqdn) has not been spec'd
//output controlPlaneFQDN string = aks.properties.fqdn 
