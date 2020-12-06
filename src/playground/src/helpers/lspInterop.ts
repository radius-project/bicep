import { languages } from 'monaco-editor';

let interop: any;

export function initializeInterop(self: any): Promise<boolean> {
  return new Promise<boolean>((resolve, reject) => {
    self['BicepInitialize'] = (newInterop: any) => {
      interop = newInterop;
      resolve(true);
    }
  
    const test = require('../../../Bicep.Wasm/bin/Release/net5.0/wwwroot/_framework/blazor.webassembly.js');  
  });
}

export function getSemanticTokensLegend(): languages.SemanticTokensLegend {
  return interop.invokeMethod('GetSemanticTokensLegend');
}

export function getSemanticTokens(content: string): languages.SemanticTokens {
  return interop.invokeMethod('GetSemanticTokens', content);
}

export async function sendLspData(message: string) {
  // console.log(message);
  return await interop.invokeMethodAsync('SendLspDataAsync', message);
}

export function onLspData(callback: (message: string | Buffer) => void) {
  (self as any)['ReceiveLspData'] = (message: string) => {
    // console.log(message);
    callback(message);
  }
}

export function compile(content: string): string {
  return interop.invokeMethod('Compile', content);
}

export function decompile(jsonContent: string): string {
  const { bicepFile, error } = interop.invokeMethod('Decompile', jsonContent);

  if (error) {
    throw error;
  }

  return bicepFile;
}