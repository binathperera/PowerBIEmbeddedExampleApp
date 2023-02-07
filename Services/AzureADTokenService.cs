namespace BlazorAppPowerBI.Services
{
    public class AzureADTokenService
    {
        private readonly HttpClient client;
        public AzureADTokenService(HttpClient _httpClient) { 
            this.client= _httpClient;
        }
        public async Task<String> GetADToken()
        {
            return await client.GetStringAsync("api/GetADToken");
        }
    }
}
