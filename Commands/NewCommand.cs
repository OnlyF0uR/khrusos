using NBitcoin;

namespace Khrusos.Commands;

internal class NewCommand
{
    private readonly CommandHandler _instance;
    private string _displayName;

    public NewCommand(CommandHandler instance)
    {
        _instance = instance;
        _displayName = "";
    }

    public bool Conditions(string? displayName)
    {
        if (displayName == null)
        {
            Console.WriteLine("Please specify a display name for the key.");
            return false;
        }

        if (displayName.Contains(":"))
        {
            Console.WriteLine("Display name cannot contain a colon.");
            return false;
        }

        _displayName = displayName;
        return true;
    }

    public void Execute()
    {
        var privateKey = new Key();
        var secretKey = privateKey.GetWif(_instance.Network);

        Console.WriteLine("\nPrivate key: " + privateKey.ToHex());
        Console.WriteLine("Private key WIF: " + secretKey);

        Console.WriteLine("\nPublic key: " + privateKey.PubKey.ToHex());
        Console.WriteLine(
            "Public key hash: " + privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, _instance.Network));

        // TODO: Encrypt the keys :/

        var path = _instance.Network == Network.Main ? "keys.txt" : "keys-testnet.txt";
        if (!File.Exists(path))
        {
            using var sw = File.CreateText(path);
            sw.WriteLine(secretKey + ":" + _instance.Network.Name + ":" + _displayName);
        }
        else
        {
            using var sw = new StreamWriter(path, true);
            sw.WriteLine(secretKey + ":" + _instance.Network.Name + ":" + _displayName);
        }

        Console.WriteLine("\nKey saved.");
    }
}