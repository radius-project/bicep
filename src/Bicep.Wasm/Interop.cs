// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.JSInterop;
using Bicep.Core.Diagnostics;
using Bicep.Core.Text;
using Bicep.Core.Emit;
using Bicep.Core.Semantics;
using Bicep.Core.Syntax;
using Bicep.Wasm.LanguageHelpers;
using System.Linq;
using Bicep.Core.TypeSystem;
using Bicep.Core.TypeSystem.Az;
using Bicep.Core.FileSystem;
using Bicep.Core.Workspaces;
using Bicep.Core.Extensions;
using Bicep.Decompiler;
using System.IO.Pipelines;
using Bicep.LanguageServer;
using SemanticTokenVisitor = Bicep.Wasm.LanguageHelpers.SemanticTokenVisitor;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Buffers;
using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Concurrency;

namespace Bicep.Wasm
{
    public class Interop
    {
        private static readonly IResourceTypeProvider resourceTypeProvider = new AzResourceTypeProvider();

        private readonly IJSRuntime jsRuntime;
        private readonly Server server;
        private readonly PipeWriter inputWriter;
        private readonly PipeReader outputReader;

        public Interop(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
            var inputPipe = new Pipe();
            var outputPipe = new Pipe();

            server = new Server(inputPipe.Reader, outputPipe.Writer, new Server.CreationOptions {
                FileResolver = new FileResolver(),
                ResourceTypeProvider = resourceTypeProvider,
            }, options => options.Services.AddSingleton<IScheduler>(ImmediateScheduler.Instance));

            inputWriter = inputPipe.Writer;
            outputReader = outputPipe.Reader;

#pragma warning disable VSTHRD110
            Task.Run(() => server.RunAsync(CancellationToken.None));
            Task.Run(() => ProcessInputStreamAsync());
#pragma warning restore VSTHRD110
        }

        [JSInvokable]
        public async Task SendLspDataAsync(string jsonContent)
        {
            var cancelToken = CancellationToken.None;

            await inputWriter.WriteAsync(Encoding.UTF8.GetBytes(jsonContent)).ConfigureAwait(false);
        }

        private async Task ProcessInputStreamAsync()
        {
            try
            {
                do
                {
                    var result = await outputReader.ReadAsync(CancellationToken.None).ConfigureAwait(false);
                    var buffer = result.Buffer;

                    await jsRuntime.InvokeVoidAsync("ReceiveLspData", Encoding.UTF8.GetString(buffer.Slice(buffer.Start, buffer.End)));
                    outputReader.AdvanceTo(buffer.End, buffer.End);

                    // Stop reading if there's no more data coming.
                    if (result.IsCompleted && buffer.IsEmpty)
                    {
                        break;
                    }
                    // TODO: Add cancellation token
                } while (!CancellationToken.None.IsCancellationRequested);
            }
            catch (Exception e)
            {
                // TODO: Needed?
                await Console.Error.WriteLineAsync(e.Message);
                await Console.Error.WriteLineAsync(e.StackTrace);
            }
        }

        [JSInvokable]
        public string Compile(string bicepContent)
        {
            try
            {
                var compilation = GetCompilation(bicepContent);
                var emitter = new TemplateEmitter(compilation.GetEntrypointSemanticModel());

                var templateWriter = new StringWriter();
                var emitResult = emitter.Emit(templateWriter);

                if (emitResult.Status != EmitStatus.Failed)
                {
                    // compilation was successful or had warnings - return the compiled template
                    return templateWriter.ToString();
                }

                // compilation failed
                return "Compilation failed!";
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }

        public record DecompileResult(string? bicepFile, string? error);

        [JSInvokable]
        public DecompileResult Decompile(string jsonContent)
        {
            var jsonUri = new Uri("inmemory:///main.json");

            var fileResolver = new InMemoryFileResolver(new Dictionary<Uri, string> {
                [jsonUri] = jsonContent,
            });

            try
            {
                var (entrypointUri, filesToSave) = TemplateDecompiler.DecompileFileWithModules(resourceTypeProvider, fileResolver, jsonUri);

                return new DecompileResult(filesToSave[entrypointUri], null);
            }
            catch (Exception exception)
            {
                return new DecompileResult(null, exception.Message);
            }
        }

        [JSInvokable]
        public object GetSemanticTokensLegend()
        {
            var tokenTypes = Enum.GetValues(typeof(SemanticTokenType)).Cast<SemanticTokenType>();
            var tokenStrings = tokenTypes.OrderBy(t => (int)t).Select(t => t.ToString().ToLowerInvariant());

            return new {
                tokenModifiers = new string[] { },
                tokenTypes = tokenStrings.ToArray(),
            };
        }

        [JSInvokable]
        public object GetSemanticTokens(string content)
        {
            var compilation = GetCompilation(content);
            var tokens = SemanticTokenVisitor.BuildSemanticTokens(compilation.SyntaxTreeGrouping.EntryPoint);

            var data = new List<int>();
            SemanticToken? prevToken = null;
            foreach (var token in tokens) {
                if (prevToken == null) {
                    data.Add(token.Line);
                    data.Add(token.Character);
                    data.Add(token.Length);
                } else if (prevToken.Line != token.Line) {
                    data.Add(token.Line - prevToken.Line);
                    data.Add(token.Character);
                    data.Add(token.Length);
                } else {
                    data.Add(0);
                    data.Add(token.Character - prevToken.Character);
                    data.Add(token.Length);
                }

                data.Add((int)token.TokenType);
                data.Add(0);

                prevToken = token;
            }

            return new {
                data = data.ToArray(),
            };
        }

        private static Compilation GetCompilation(string fileContents)
        {
            var fileUri = new Uri("inmemory:///main.bicep");
            var workspace = new Workspace();
            var syntaxTree = SyntaxTree.Create(fileUri, fileContents);
            workspace.UpsertSyntaxTrees(syntaxTree.AsEnumerable());

            var syntaxTreeGrouping = SyntaxTreeGroupingBuilder.Build(new FileResolver(), workspace, fileUri);

            return new Compilation(resourceTypeProvider, syntaxTreeGrouping);
        }
    }
}
