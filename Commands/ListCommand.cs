using NBitcoin;

namespace Khrusos.Commands;

internal class ListCommand
{
    private readonly CommandHandler _instance;

    public ListCommand(CommandHandler instance)
    {
        _instance = instance;
    }

    public void Execute()
    {
        Console.WriteLine("Saved keys  (" + (_instance.Network == Network.Main ? "Mainnet" : "Testnet") + ")");
        var path = _instance.Network == Network.Main ? "keys.txt" : "keys-testnet.txt";
        foreach (var line in File.ReadLines("keys.txt"))
        {
            var data = line.Split(":");

            var network = Network.GetNetwork(data[1]);
            if (network == null) return;
            var walletAddress = Key.Parse(data[0], network).GetAddress(ScriptPubKeyType.Legacy, network);

            Console.WriteLine("  (" + data[2] + ") " + walletAddress);

            var balance = _instance.BlockchainAPI.GetBalance(walletAddress.ToString()).Result;
            Console.WriteLine("     - BTC:" + balance);
        }
    }
}