namespace Khrusos.Commands;

internal class InformationCommand
{
    private readonly CommandHandler _instance;
    private string _walletAddress;

    public InformationCommand(CommandHandler instance)
    {
        _instance = instance;
        _walletAddress = "";
    }

    public bool Conditions(string? walletAddress)
    {
        if (walletAddress == null)
        {
            Console.WriteLine("Please specify a wallet address.");
            return false;
        }

        _walletAddress = walletAddress;
        return true;
    }

    public void Execute()
    {
        Console.WriteLine("Information");
    }
}