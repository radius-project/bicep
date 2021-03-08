// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
import * as vscode from "vscode";

import { createLogger } from "./utils/logger";
import { launchLanguageServiceWithProgressReport } from "./language/client";
import { activateWithTelemetryAndErrorHandling } from "./utils/telemetry";
import { createAzExtOutputChannel } from "vscode-azureextensionui";

export async function activate(
  context: vscode.ExtensionContext
): Promise<void> {
  const stable = vscode.extensions.getExtension('ms-azuretools.vscode-bicep');
  if (stable !== undefined) {
    throw new Error(
      'The Radius Bicep extension cannot be used while the official Bicep extension is also enabled. Please ensure that only one version of the extension is enabled.',
    );
  }

  const outputChannel = createAzExtOutputChannel("Bicep", "bicep");

  await activateWithTelemetryAndErrorHandling(
    context,
    outputChannel,
    async () => {
      createLogger(context, outputChannel);

      await launchLanguageServiceWithProgressReport(context, outputChannel);
    }
  );
}

// eslint-disable-next-line @typescript-eslint/no-empty-function
export function deactivate(): void { }
