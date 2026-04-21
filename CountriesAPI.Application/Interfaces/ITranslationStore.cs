namespace CountriesAPI.Application.Interfaces;

public interface ITranslationStore
{
    string? FindCountryCode(string searchTerm);

    Dictionary<string, string> GetTranslationsForCountry(string cca2);

    Task SaveLanguageAsync(string langCode, Dictionary<string, string> translations);

    IEnumerable<string> GetAvailableLanguages();
}