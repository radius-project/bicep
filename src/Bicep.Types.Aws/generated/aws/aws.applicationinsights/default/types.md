# AWS.ApplicationInsights @ default

## Resource AWS.ApplicationInsights/Application@default
* **Valid Scope(s)**: Unknown
### Properties
* **alias**: string (Required): the resource alias
* **name**: string: the resource name
* **properties**: [AWS.ApplicationInsights/ApplicationProperties](#awsapplicationinsightsapplicationproperties) (Required): properties of the resource

## AWS.ApplicationInsights/ApplicationProperties
### Properties
* **ApplicationARN**: string (ReadOnly, Identifier): The ARN of the ApplicationInsights application.
* **AutoConfigurationEnabled**: bool: If set to true, application will be configured with recommended monitoring configuration.
* **ComponentMonitoringSettings**: [ComponentMonitoringSetting](#componentmonitoringsetting)[]: The monitoring settings of the components.
* **CustomComponents**: [CustomComponent](#customcomponent)[]: The custom grouped components.
* **CWEMonitorEnabled**: bool: Indicates whether Application Insights can listen to CloudWatch events for the application resources.
* **GroupingType**: string: The grouping type of the application
* **LogPatternSets**: [LogPatternSet](#logpatternset)[]: The log pattern sets.
* **OpsCenterEnabled**: bool: When set to true, creates opsItems for any problems detected on an application.
* **OpsItemSNSTopicArn**: string: The SNS topic provided to Application Insights that is associated to the created opsItem.
* **ResourceGroupName**: string (Required): The name of the resource group.
* **Tags**: [Tag](#tag)[]: The tags of Application Insights application.

## ComponentMonitoringSetting
### Properties
* **ComponentARN**: string: The ARN of the compnonent.
* **ComponentConfigurationMode**: string (Required): The component monitoring configuration mode.
* **ComponentName**: string: The name of the component.
* **CustomComponentConfiguration**: [ComponentConfiguration](#componentconfiguration): The monitoring configuration of the component.
* **DefaultOverwriteComponentConfiguration**: [ComponentConfiguration](#componentconfiguration): The overwritten settings on default component monitoring configuration.
* **Tier**: string (Required): The tier of the application component.

## ComponentConfiguration
### Properties
* **ConfigurationDetails**: [ConfigurationDetails](#configurationdetails): The configuration settings
* **SubComponentTypeConfigurations**: [SubComponentTypeConfiguration](#subcomponenttypeconfiguration)[]: Sub component configurations of the component.

## ConfigurationDetails
### Properties
* **AlarmMetrics**: [AlarmMetric](#alarmmetric)[]: A list of metrics to monitor for the component.
* **Alarms**: [Alarm](#alarm)[]: A list of alarms to monitor for the component.
* **HAClusterPrometheusExporter**: [HAClusterPrometheusExporter](#haclusterprometheusexporter): The HA cluster Prometheus Exporter settings.
* **HANAPrometheusExporter**: [HANAPrometheusExporter](#hanaprometheusexporter): The HANA DB Prometheus Exporter settings.
* **JMXPrometheusExporter**: [JMXPrometheusExporter](#jmxprometheusexporter): The JMX Prometheus Exporter settings.
* **Logs**: [Log](#log)[]: A list of logs to monitor for the component.
* **WindowsEvents**: [WindowsEvent](#windowsevent)[]: A list of Windows Events to log.

## AlarmMetric
### Properties
* **AlarmMetricName**: string (Required): The name of the metric to be monitored for the component.

## Alarm
### Properties
* **AlarmName**: string (Required): The name of the CloudWatch alarm to be monitored for the component.
* **Severity**: string: Indicates the degree of outage when the alarm goes off.

## HAClusterPrometheusExporter
### Properties
* **PrometheusPort**: string: Prometheus exporter port.

## HANAPrometheusExporter
### Properties
* **AgreeToInstallHANADBClient**: bool (Required): A flag which indicates agreeing to install SAP HANA DB client.
* **HANAPort**: string (Required): The HANA DB port.
* **HANASecretName**: string (Required): The secret name which manages the HANA DB credentials e.g. {
  "username": "<>",
  "password": "<>"
}.
* **HANASID**: string (Required): HANA DB SID.
* **PrometheusPort**: string: Prometheus exporter port.

## JMXPrometheusExporter
### Properties
* **HostPort**: string: Java agent host port
* **JMXURL**: string: JMX service URL.
* **PrometheusPort**: string: Prometheus exporter port.

## Log
### Properties
* **Encoding**: string: The type of encoding of the logs to be monitored.
* **LogGroupName**: string: The CloudWatch log group name to be associated to the monitored log.
* **LogPath**: string: The path of the logs to be monitored.
* **LogType**: string (Required): The log type decides the log patterns against which Application Insights analyzes the log.
* **PatternSet**: string: The name of the log pattern set.

## WindowsEvent
### Properties
* **EventLevels**: string[] (Required): The levels of event to log. 
* **EventName**: string (Required): The type of Windows Events to log.
* **LogGroupName**: string (Required): The CloudWatch log group name to be associated to the monitored log.
* **PatternSet**: string: The name of the log pattern set.

## SubComponentTypeConfiguration
### Properties
* **SubComponentConfigurationDetails**: [SubComponentConfigurationDetails](#subcomponentconfigurationdetails) (Required): The configuration settings of sub components.
* **SubComponentType**: string (Required): The sub component type.

## SubComponentConfigurationDetails
### Properties
* **AlarmMetrics**: [AlarmMetric](#alarmmetric)[]: A list of metrics to monitor for the component.
* **Logs**: [Log](#log)[]: A list of logs to monitor for the component.
* **WindowsEvents**: [WindowsEvent](#windowsevent)[]: A list of Windows Events to log.

## CustomComponent
### Properties
* **ComponentName**: string (Required): The name of the component.
* **ResourceList**: string[] (Required): The list of resource ARNs that belong to the component.

## LogPatternSet
### Properties
* **LogPatterns**: [LogPattern](#logpattern)[] (Required): The log patterns of a set.
* **PatternSetName**: string (Required): The name of the log pattern set.

## LogPattern
### Properties
* **Pattern**: string (Required): The log pattern.
* **PatternName**: string (Required): The name of the log pattern.
* **Rank**: int (Required): Rank of the log pattern.

## Tag
### Properties
* **Key**: string (Required): The key name of the tag. You can specify a value that is 1 to 127 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 
* **Value**: string (Required): The value for the tag. You can specify a value that is 1 to 255 Unicode characters in length and cannot be prefixed with aws:. You can use any of the following characters: the set of Unicode letters, digits, whitespace, _, ., /, =, +, and -. 

