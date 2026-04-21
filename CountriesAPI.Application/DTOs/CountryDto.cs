using CountriesAPI.Domain.Entities;

namespace CountriesAPI.Application.DTOs;

public class CountryDto
{
    public string CommonName { get; set; } = string.Empty;
    public string OfficialName { get; set; } = string.Empty;
    public string Cca2 { get; set; } = string.Empty;
    public string Cca3 { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Subregion { get; set; } = string.Empty;
    public string Capital { get; set; } = string.Empty;
    public long Population { get; set; }
    public string FlagEmoji { get; set; } = string.Empty;
    public string FlagPng { get; set; } = string.Empty;
    public double Lat { get; set; }
    public double Lng { get; set; }
    public Dictionary<string, string> Translations { get; set; } = new();

   
    public static CountryDto FromEntity(Country country)
    {
        return new CountryDto
        {
            CommonName = country.CommonName,
            OfficialName = country.OfficialName,
            Cca2 = country.Cca2,
            Cca3 = country.Cca3,
            Region = country.Region,
            Subregion = country.Subregion,
            Capital = country.Capital,
            Population = country.Population,
            FlagEmoji = country.FlagEmoji,
            FlagPng = country.FlagPng,
            Lat = country.Lat,
            Lng = country.Lng
        };
    }
}