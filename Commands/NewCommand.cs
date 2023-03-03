using NBitcoin;
using Utils = khrusos.Utils;

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
        
        var pass = Utils.PromptPassword();
        var encryptedKey = secretKey.Encrypt(pass);

        Console.WriteLine("Private key: " + privateKey.ToHex());
        Console.WriteLine("Private key WIF: " + secretKey);
        Console.WriteLine("Private key WIF (encrypted): " + encryptedKey);

        Console.WriteLine("\nPublic key: " + privateKey.PubKey.ToHex());
        Console.WriteLine("Public key hash: " + privateKey.PubKey.GetAddress(ScriptPubKeyType.Legacy, _instance.Network));

        var path = _instance.Network == Network.Main ? "keys.txt" : "keys-testnet.txt";
        if (!File.Exists(path))
        {
            using var sw = File.CreateText(path);
            sw.WriteLine(encryptedKey + ":" + _instance.Network.Name + ":" + _displayName);
        }
        else
        {
            using var sw = new StreamWriter(path, true);
            sw.WriteLine(encryptedKey + ":" + _instance.Network.Name + ":" + _displayName);
        }

        Console.WriteLine("\nKey saved.");
    }
}