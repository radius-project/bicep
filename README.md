# What is Radius Bicep

This is a fork of the official [Azure Bicep](https://github.com/Azure/bicep) to support [Radius](https://github.com/project-radius/radius). This fork is temporary while we upstream our extensibility updates to the official Bicep repository. For all you need to know about the Bicep language, check out the [Bicep documentation](https://docs.microsoft.com/azure/azure-resource-manager/bicep/).

Refer to the [Azure Bicep](https://github.com/Azure/bicep) to understand more about the Bicep language and its internal workings.

## What is Radius?

Radius is a platform for developers and IT operators building cloud-native applications. With Radius, teams can model, deploy, manage, and troubleshoot entire applications across on-premises and multi-cloud environments, with a consistent set of tools and a common experience across it all. More information about Radius can be found [here](https://docs.radapp.dev/).

## How does Radius Bicep work?

First, author your Radius application using the Bicep language service as part of the [Radius Bicep VS Code extension](https://docs.radapp.dev/getting-started/install/#setup-vs-code).

Install [Radius CLI](https://docs.radapp.dev/getting-started/install/#install-rad-cli) and set up a [Radius environment](https://docs.radapp.dev/author-apps/dev-environment/initialize-environment/)

Then, use the Radius CLI to compile your Bicep code and deploy your application to your Radius environment.

```bash
rad deploy app.bicep
```

## FAQ

**What unique benefits do you get with Radius Bicep?**

1. Tooling to author and deploy Radius resources. Radius resources are platform-agnostic, allowing applications to be written once and deployed to any platform, such as Kubernetes, Microsoft Azure, Amazon Web Services (AWS)
1. Tooling to author and deploy AWS resource types using the [AWS extensibility provider](https://github.com/project-radius/bicep-types-aws)
1. 

**Can you install both the official Bicep extension and the Radius Bicep extension?**
No, you can only have one Bicep extension installed at a time. If you have both installed, you will need to uninstall one of them. To build on Radius, you will need to uninstall the official Bicep and use the Radius Bicep extension.

**Can you use the Radius Bicep extension to author Azure resources?**
Yes you can use the Radius Bicep extension to author Azure resources. 

**What are the future plans for Radius Bicep?**
We are currently working with the Bicep team to upstream our extensibility updates to the official Bicep repository. Once that is complete, we will deprecate this repository and use the official Bicep for Radius.

**Is this ready for production use?**
Yes, Radius Bicep is ready for production use. 

## Get Help, Report an issue



## Reference



## Telemetry

When using the Bicep VS Code extension, VS Code collects usage data and sends it to Microsoft to help improve our products and services. Read our [privacy statement](https://go.microsoft.com/fwlink/?LinkID=528096&clcid=0x409) to learn more. If you don’t wish to send usage data to Microsoft, you can set the `telemetry.enableTelemetry` setting to `false`. Learn more in our [FAQ](https://code.visualstudio.com/docs/supporting/faq#_how-to-disable-telemetry-reporting).

## License

All files except for the [Azure Architecture SVG Icons](./src/vscode-bicep/src/visualizer/app/assets/icons/azure) in the repository are subject to the [MIT license](./LICENSE).

The [Azure Architecture SVG Icons](./src/vscode-bicep/src/visualizer/app/assets/icons/azure) used in the Bicep VS Code extension are subject to the [Terms of Use](https://docs.microsoft.com/azure/architecture/icons/#terms).

## Contributing

See [Contributing to Bicep](./CONTRIBUTING.md) for information on building/running the code, contributing code, contributing examples and contributing feature requests or bug reports.
