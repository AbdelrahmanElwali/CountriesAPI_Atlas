using System.Text.Json;

namespace CountriesAPI.Infrastructure.ExternalApi;

public class RestCountriesClient
{
    private readonly HttpClient _httpClient;

    public RestCountriesClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // REST Countries API بتطلب User-Agent
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CountriesAPI/1.0");
    }

    public async Task<JsonElement[]> GetAllCountriesAsync()
    {
        var response = await _httpClient
            .GetStringAsync("https://restcountries.com/v3.1/all?fields=name,cca2,cca3,region,subregion,capital,population,flags,flag,latlng");

        return JsonSerializer.Deserialize<JsonElement[]>(response)!;
    }
}


