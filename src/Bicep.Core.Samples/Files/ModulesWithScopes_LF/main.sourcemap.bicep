targetScope = 'tenant'

module myManagementGroupMod 'modules/managementgroup.bicep' = {
//@[13:133]     "myManagementGroupMod": {
  name: 'myManagementGroupMod'
//@[16:16]       "name": "myManagementGroupMod",
  scope: managementGroup('myManagementGroup')
}
module myManagementGroupModWithDuplicatedNameButDifferentScope 'modules/managementgroup_empty.bicep' = {
//@[134:160]     "myManagementGroupModWithDuplicatedNameButDifferentScope": {
  name: 'myManagementGroupMod'
//@[137:137]       "name": "myManagementGroupMod",
  scope: managementGroup('myManagementGroup2')
}
module mySubscriptionMod 'modules/subscription.bicep' = {
//@[161:707]     "mySubscriptionMod": {
  name: 'mySubscriptionMod'
//@[164:164]       "name": "mySubscriptionMod",
  scope: subscription('ee44cd78-68c6-43d9-874e-e684ec8d1191')
//@[165:165]       "subscriptionId": "ee44cd78-68c6-43d9-874e-e684ec8d1191",
}

module mySubscriptionModWithCondition 'modules/subscription.bicep' = if (length('foo') == 3) {
//@[708:1255]     "mySubscriptionModWithCondition": {
  name: 'mySubscriptionModWithCondition'
//@[712:712]       "name": "mySubscriptionModWithCondition",
  scope: subscription('ee44cd78-68c6-43d9-874e-e684ec8d1191')
//@[713:713]       "subscriptionId": "ee44cd78-68c6-43d9-874e-e684ec8d1191",
}

module mySubscriptionModWithDuplicatedNameButDifferentScope 'modules/subscription_empty.bicep' = {
//@[1256:1282]     "mySubscriptionModWithDuplicatedNameButDifferentScope": {
  name: 'mySubscriptionMod'
//@[1259:1259]       "name": "mySubscriptionMod",
  scope: subscription('1ad827ac-2669-4c2f-9970-282b93c3c550')
//@[1260:1260]       "subscriptionId": "1ad827ac-2669-4c2f-9970-282b93c3c550",
}


output myManagementGroupOutput string = myManagementGroupMod.outputs.myOutput
//@[1285:1288]     "myManagementGroupOutput": {
output mySubscriptionOutput string = mySubscriptionMod.outputs.myOutput
//@[1289:1292]     "mySubscriptionOutput": {

