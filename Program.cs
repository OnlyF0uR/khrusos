using CommandLine;
using Khrusos.Commands;

namespace Khrusos;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        var commandHandler = new CommandHandler();

        return await Parser.Default.ParseArguments<CLIOptions>(args)
            .MapResult(async (CLIOptions opts) =>
                {
                    try
                    {
                        return await commandHandler.HandleOptions(opts);
                    }
                    catch
                    {
                        return -3;
                    }
                },
                errs => Task.FromResult(-1)); // Invalid arguments
    }
}