using CommandLine;

namespace Khrusos;

public class CLIOptions
{
    // Subcommand
    [Value(0, Required = true, HelpText = "Subcommand")]
    public string? Subcommand { get; set; }

    // Option to specify the wallet
    [Option('w', "wallet", Required = false, HelpText = "Wallet to use")]
    public string? Wallet { get; set; }

    // Option to specify receiver
    [Option('r', "receiver", Required = false, HelpText = "Receiver of the transaction")]
    public string? Receiver { get; set; }

    // Option to specify amount
    [Option('a', "amount", Required = false, HelpText = "Amount to send")]
    public decimal? Amount { get; set; }

    // Option to specify wallet display name
    [Option('d', "display-name", Required = false, HelpText = "Wallet display name")]
    public string? DisplayName { get; set; }

    // Option for mainnet or testnet
    [Option('n', "network", Required = false, HelpText = "Network to use")]
    public string? Network { get; set; }
}