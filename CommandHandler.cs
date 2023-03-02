using Khrusos.API;
using NBitcoin;

namespace Khrusos.Commands;

internal class CommandHandler
{
    public Network Network;
    public BlockchainAPI BlockchainAPI;

    public CommandHandler()
    {
        Network = Network.Main;
        BlockchainAPI = new BlockchainAPI(Network);
    }

    public Task<int> HandleOptions(CLIOptions opts)
    {
        if (opts.Subcommand == null) return Task.FromResult(-1); // Bit useless but resolves as warning

        // Check for testnet
        if (opts.Network != null && opts.Network.ToLower() == "test")
            Network = Network.TestNet;
        else
            Network = Network.Main;

        switch (opts.Subcommand.ToLower())
        {
            case "delete":
            {
                var dc = new DeleteCommand(this);
                if (!dc.Conditions(opts.Wallet)) return Task.FromResult(-1);
                dc.Execute();
                break;
            }
            case "info":
            {
                var ic = new InformationCommand(this);
                if (!ic.Conditions(opts.Wallet)) return Task.FromResult(-1);
                ic.Execute();
                break;
            }
            case "list":
            {
                var lc = new ListCommand(this);
                lc.Execute();
                break;
            }
            case "new":
            {
                var nc = new NewCommand(this);
                if (!nc.Conditions(opts.DisplayName)) return Task.FromResult(-1);
                nc.Execute();
                break;
            }
            default:
                Console.WriteLine("Invalid subcommand. (list, new, rename, info, send, txs, del)");
                return Task.FromResult(-1);
        }

        return Task.FromResult(0);
    }
}