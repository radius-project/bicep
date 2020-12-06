import { BaseLanguageClient, CloseAction, createConnection, ErrorAction, MonacoLanguageClient, MonacoServices } from 'monaco-languageclient';
import { createMessageConnection, StreamMessageReader, StreamMessageWriter } from 'vscode-jsonrpc';
import { onLspData, sendLspData } from './lspInterop';
import { Duplex } from 'stream';
// @ts-expect-error
import { CommandsRegistry } from 'monaco-editor/esm/vs/platform/commands/common/commands';

function marshalToString(data : any, encoding: BufferEncoding | 'buffer') {
  return Buffer.isBuffer(data) ? data.toString(encoding === 'buffer' ? undefined : encoding) : typeof data === 'string' ? data : data.toString();
}

function createStream() {
  const output = new Duplex({
    write: (data, encoding, cb) => {
      sendLspData(marshalToString(data, encoding));
      cb();
    }
  });

  onLspData(data => output.push(marshalToString(data, 'utf8')));

  return [new StreamMessageReader(output, 'utf8'), new StreamMessageWriter(output, 'utf8')] as const;
}

export async function createLanguageClient(): Promise<BaseLanguageClient> {
  MonacoServices.install(CommandsRegistry);

  const [reader, writer] = createStream();
  const messageConnection = createMessageConnection(reader, writer);

  const client = new MonacoLanguageClient({
    name: "Bicep Monaco Client",
    clientOptions: {
      documentSelector: [{ language: 'bicep' }],
      errorHandler: {
        error: () => ErrorAction.Continue,
        closed: () => CloseAction.DoNotRestart
      }
    },
    connectionProvider: {
      get: (errorHandler, closeHandler) => {
        return Promise.resolve(createConnection(messageConnection, errorHandler, closeHandler))
      }
    }
  });

  client.start();
  await client.onReady();

  return client;
}