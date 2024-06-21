using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StockApp
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string API_KEY = "cpqjlshr01qo647no9r0cpqjlshr01qo647no9rg"; 

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter stock ticker symbol: ");
            string ticker = Console.ReadLine();

            var quote = await GetStockQuote(ticker);
            var profile = await GetCompanyProfile(ticker);

            Console.WriteLine($"Stock: {profile.Name} ({ticker})");
            Console.WriteLine($"Current Price: ${quote.CurrentPrice}");
            Console.WriteLine($"High Price: ${quote.HighPrice}");
            Console.WriteLine($"Low Price: ${quote.LowPrice}");
        }

        static async Task<Quote> GetStockQuote(string ticker)
        {
            var response = await client.GetAsync($"https://finnhub.io/api/v1/quote?symbol={ticker}&token={API_KEY}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Quote>(responseBody);
        }

        static async Task<Profile> GetCompanyProfile(string ticker)
        {
            var response = await client.GetAsync($"https://finnhub.io/api/v1/stock/profile2?symbol={ticker}&token={API_KEY}");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Profile>(responseBody);
        }
    }

    public class Quote
    {
        [JsonPropertyName("c")]
        public decimal CurrentPrice { get; set; }
        [JsonPropertyName("h")]
        public decimal HighPrice { get; set; }
        [JsonPropertyName("l")]
        public decimal LowPrice { get; set; }
    }

    public class Profile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
