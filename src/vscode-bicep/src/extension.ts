// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
import vscode from "vscode";
import {
  createAzExtOutputChannel,
  registerUIExtensionVariables,
} from "vscode-azureextensionui";

import {
  launchLanguageServiceWithProgressReport,
  BicepCacheContentProvider,
} from "./language";
import { BicepVisualizerViewManager } from "./visualizer";
import {
  BuildCommand,
  CommandManager,
  ShowSourceCommand,
  ShowVisualizerCommand,
  ShowVisualizerToSideCommand,
} from "./commands";
import {
  createLogger,
  resetLogger,
  activateWithTelemetryAndErrorHandling,
  Disposable,
} from "./utils";

class BicepExtension extends Disposable {
  private constructor(public readonly extensionUri: vscode.Uri) {
    super();
  }

  public static create(context: vscode.ExtensionContext) {
    const extension = new BicepExtension(context.extensionUri);
    context.subscriptions.push(extension);

    return extension;
  }
}

export async function activate(
  context: vscode.ExtensionContext
): Promise<void> {
  const stable = vscode.extensions.getExtension('ms-azuretools.vscode-bicep');
  if (stable !== undefined) {
    throw new Error(
      'The Radius Bicep extension cannot be used while the official Bicep extension is also enabled. Please ensure that only one version of the extension is enabled.',
    );
  }

  const extension = BicepExtension.create(context);

  const outputChannel = createAzExtOutputChannel("Bicep", "bicep");

  extension.register(outputChannel);
  extension.register(createLogger(context, outputChannel));
  registerUIExtensionVariables({ context, outputChannel });

  await activateWithTelemetryAndErrorHandling(async () => {
    const languageClient = await launchLanguageServiceWithProgressReport(
      context,
      outputChannel
    );

    // go2def links that point to the bicep cache will have the bicep-cache scheme in their document URIs
    // this content provider will allow VS code to understand that scheme
    // and surface the content as a read-only file
    extension.register(
      vscode.workspace.registerTextDocumentContentProvider(
        "bicep-cache",
        new BicepCacheContentProvider(languageClient)
      )
    );

    const viewManager = extension.register(
      new BicepVisualizerViewManager(extension.extensionUri, languageClient)
    );

    // Register commands.
    await extension
      .register(new CommandManager())
      .registerCommands(
        new BuildCommand(languageClient),
        new ShowVisualizerCommand(viewManager),
        new ShowVisualizerToSideCommand(viewManager),
        new ShowSourceCommand(viewManager)
      );
  });
}

export function deactivate(): void {
  resetLogger();
}
