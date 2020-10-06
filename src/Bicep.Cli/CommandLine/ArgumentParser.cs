// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System;
using System.IO;
using System.Linq;
using Bicep.Cli.CommandLine.Arguments;

namespace Bicep.Cli.CommandLine
{
    public static class ArgumentParser
    {
        public static ArgumentsBase Parse(string[] args)
        {
            if (args == null || args.Any() == false)
            {
                return new UnrecognizedArguments("");
            }

            // parse verb
            switch (args[0].ToLowerInvariant())
            {
                case CliConstants.CommandBuild:
                    return ParseBuild(args[1..]);
                case CliConstants.ArgumentHelp:
                case CliConstants.ArgumentHelpShort:
                    return new HelpArguments();
                case CliConstants.ArgumentVersion:
                case CliConstants.ArgumentVersionShort:
                    return new VersionArguments();
                default:
                    return new UnrecognizedArguments(string.Join(' ', args));
            }
        }

        public static string GetExeName()
            => ThisAssembly.AssemblyName;

        private static string GetVersionString()
        {
            var versionSplit = ThisAssembly.AssemblyInformationalVersion.Split('+');

            // <major>.<minor>.<patch> (<commmithash>)
            return $"{versionSplit[0]} ({versionSplit[1]})";
        }

        public static void PrintVersion(TextWriter writer)
        {
            var output = $@"Bicep CLI version {GetVersionString()}{Environment.NewLine}";

            writer.Write(output);
            writer.Flush();
        }

        public static void PrintUsage(TextWriter writer)
        {
            var exeName = GetExeName();
            var output = 
$@"Bicep CLI version {GetVersionString()}

Usage:
  {exeName} build [options] <file>|<dir>
    Builds one or more .bicep files

    Arguments:
      <file>        The input file.
      <dir>         The input directory. The file named main.bicep will be 
                    compiled.

    Options:
      --outdir <dir>    Saves the output at the specified directory.
      --outfile <file>  Saves the output as the specified file path.
      --stdout          Prints the output to stdout.

    Examples:
      bicep build file.bicep
      bicep build file.bicep --stdout
      bicep build file.bicep --outdir dir1
      bicep build file.bicep --outfile file.json

  {exeName} [options]
    Options:
      --version  -v   Shows bicep version information
      --help     -h   Shows this usage information
"; // this newline is intentional

            writer.Write(output);
            writer.Flush();
        }

        private static BuildArguments ParseBuild(string[] files)
        {
            return new BuildArguments(files);
        }
    }
}

