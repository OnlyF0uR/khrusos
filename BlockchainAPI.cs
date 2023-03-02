using NBitcoin;
using Newtonsoft.Json.Linq;

namespace Khrusos.API;

public class BlockchainAPI
{
    private readonly string _baseUrl;
    private readonly HttpClient _client;

    public BlockchainAPI(Network network)
    {
        // https://github.com/Blockstream/esplora/blob/master/API.md
        _baseUrl = network == Network.Main ? "https://blockstream.info/api/" : "https://blockstream.info/testnet/api/";
        _client = new HttpClient();
    }

    public async Task<decimal> GetBalance(string address)
    {
        var url = $"{_baseUrl}address/{address}";
        try
        {
            using var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            if (responseBody.Contains("Invalid Bitcoin address"))
                throw new Exception("Invalid Bitcoin address");

            var jsonBody = JObject.Parse(responseBody);

            if (jsonBody["chain_stats"]?["funded_txo_sum"] == null || jsonBody["chain_stats"]?["spent_txo_sum"] == null)
                throw new Exception("Remote API error occurred");

            var funded = jsonBody["chain_stats"]!["funded_txo_sum"]!.Value<decimal>();
            var spent = jsonBody["chain_stats"]!["spent_txo_sum"]!.Value<decimal>();

            return (funded - spent) / 100000000;
        }
        catch (Exception ex)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", ex.Message);
            return 0;
        }
    }
}