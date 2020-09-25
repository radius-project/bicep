#! /bin/sh
BASE_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
dotnet publish "$BASE_DIR/../Bicep.LangServer" -o "$BASE_DIR/bicepLanguageServer"
npm run package
