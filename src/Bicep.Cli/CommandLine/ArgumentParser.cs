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
  {exeName} build [options] [<files>...]
    Builds one or more .bicep files

    Arguments:
      <files>               The list of one or more .bicep files to build

    Options:
      --outdir <directory>  Saves all output at the specified directory.
      --outfiles            Changes the behavior of <files> from list of of input files to a list of input file-output file pairs. 
      --stdout              Prints all output to stdout instead of corresponding files

    Examples:
      bicep build file.bicep
      bicep build file1.bicep file2.bicep file3.bicep
      bicep build --stdout file1.bicep file2.bicep file3.bicep
      bicep build --outdir dir1 file1.bicep file2.bicep
      bicep build --outfiles file1.bicep dir1{Path.DirectorySeparatorChar}file1.json file2.bicep dir2{Path.DirectorySeparatorChar}file2.json

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

