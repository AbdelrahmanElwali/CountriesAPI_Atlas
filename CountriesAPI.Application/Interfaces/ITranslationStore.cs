namespace CountriesAPI.Application.Interfaces;

public interface ITranslationStore
{
    // بيدور على اسم الدولة في كل اللغات ويرجع الكود
    string? FindCountryCode(string searchTerm);

    // بيرجع كل الترجمات للدولة دي
    Dictionary<string, string> GetTranslationsForCountry(string cca2);

    // بيضيف أو يعدل لغة
    Task SaveLanguageAsync(string langCode, Dictionary<string, string> translations);

    // بيرجع اسماء اللغات المتاحة
    IEnumerable<string> GetAvailableLanguages();
}