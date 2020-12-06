import * as monacoEditor from 'monaco-editor';
import { BaseLanguageClient } from 'monaco-languageclient';
import React, { useState } from 'react';
import MonacoEditor from 'react-monaco-editor';
import { compile, getSemanticTokens, getSemanticTokensLegend } from '../helpers/lspInterop';

interface Props {
  client: BaseLanguageClient,
  initialContent: string,
  onBicepChange: (bicepContent: string) => void,
  onJsonChange: (jsonContent: string) => void,
}

const modelUri = 'inmemory:///main.bicep';
const editorOptions: monacoEditor.editor.IStandaloneEditorConstructionOptions = {
  scrollBeyondLastLine: false,
  automaticLayout: true,
  model: monacoEditor.editor.createModel('', 'bicep', monacoEditor.Uri.parse(modelUri)),
  minimap: {
    enabled: false,
  },
  insertSpaces: true,
  tabSize: 2,
  suggestSelection: 'first',
  suggest: {
    snippetsPreventQuickSuggestions: false,
    showWords: false,
  },
  'semanticHighlighting.enabled': true,
};

function configureEditorForBicep(client: BaseLanguageClient, editor: monacoEditor.editor.IStandaloneCodeEditor, monaco: typeof monacoEditor) {
  monaco.languages.register({
    id: 'bicep',
    extensions: ['.bicep'],
    aliases: ['bicep'],
  });

  // TODO use the below once monaco-languageclient has proper support for semantic tokens
  // client.registerProposedFeatures();
  monaco.languages.registerDocumentSemanticTokensProvider('bicep', {
    getLegend: () => getSemanticTokensLegend(),
    provideDocumentSemanticTokens: (model, lastResultId, token) => getSemanticTokens(model.getValue()),
    releaseDocumentSemanticTokens: () => { }
  });
  
  // @ts-expect-error
  editor._themeService._theme.getTokenStyleMetadata = (type, modifiers) => {
    // see 'monaco-editor/esm/vs/editor/standalone/common/themes.js' to understand these indices
    switch (type) {
      case 'keyword':
        return { foreground: 12 };
      case 'comment':
        return { foreground: 7 };
      case 'parameter':
        return { foreground: 2 };
      case 'property':
        return { foreground: 3 };
      case 'type':
        return { foreground: 8 };
      case 'member':
        return { foreground: 6 };
      case 'string':
        return { foreground: 5 };
      case 'variable':
        return { foreground: 4 };
      case 'operator':
        return { foreground: 9 };
      case 'function':
        return { foreground: 13 };
      case 'number':
        return { foreground: 15 };
      case 'class':
      case 'enummember':
      case 'event':
      case 'modifier':
      case 'label':
      case 'typeParameter':
      case 'macro':
      case 'interface':
      case 'enum':
      case 'regexp':
      case 'struct':
      case 'namespace':
        return { foreground: 0 };
    }
  };
}

export const BicepEditor: React.FC<Props> = (props) => {
  const { client } = props;
  const [editor, setEditor] = useState<monacoEditor.editor.IStandaloneCodeEditor>();
  const [initialContent, setinitialContent] = useState('');

  const handleContentChange = (text: string) => {
    props.onBicepChange(text);

    const template = compile(text);
    props.onJsonChange(template);
  }

  const handleEditorDidMount = (editor: monacoEditor.editor.IStandaloneCodeEditor, monaco: typeof monacoEditor) => {
    setEditor(editor);
    configureEditorForBicep(client, editor, monaco);
  }

  if (editor && initialContent != props.initialContent) {
    setinitialContent(props.initialContent);
    editor.getModel().setValue(props.initialContent);

    // clear the selection after this completes
    setTimeout(() => {
      editor.setSelection({ startColumn: 1, startLineNumber: 1, endColumn: 1, endLineNumber: 1 });
      editor.setScrollPosition({ scrollLeft: 0, scrollTop: 0 });
    }, 0);
  }

  return <MonacoEditor
    language="bicep"
    theme="vs-dark"
    options={editorOptions}
    onChange={handleContentChange}
    editorDidMount={handleEditorDidMount}
  />
};