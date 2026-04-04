using System.Globalization;
using System.Text;
using System.Text.Json;
using CountriesAPI.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CountriesAPI.Infrastructure.FileStorage;

public class TranslationStore : ITranslationStore
{
    private readonly string _translationsPath;
    private Dictionary<string, Dictionary<string, string>> _cache = new();

    public TranslationStore(IConfiguration configuration)
    {
        
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var configPath = configuration["TranslationsPath"] ?? "translations";
        _translationsPath = Path.Combine(basePath, configPath);

        Directory.CreateDirectory(_translationsPath);
        LoadAll();
    }

    private void LoadAll()
    {
        _cache.Clear();
        if (!Directory.Exists(_translationsPath)) return;

        foreach (var file in Directory.GetFiles(_translationsPath, "*.json"))
        {
            var lang = Path.GetFileNameWithoutExtension(file);
            var content = File.ReadAllText(file, Encoding.UTF8);
            _cache[lang] = JsonSerializer
                .Deserialize<Dictionary<string, string>>(content) ?? new();
        }
    }

    public string? FindCountryCode(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return null;

        searchTerm = searchTerm.Trim();
        var needle = searchTerm.Normalize(NormalizationForm.FormC);

        foreach (var lang in _cache.Values)
        {
            foreach (var (code, value) in lang)
            {
                if (string.IsNullOrEmpty(value)) continue;
                var hay = value.Normalize(NormalizationForm.FormC);
                if (hay.Equals(needle, StringComparison.OrdinalIgnoreCase))
                    return code;
            }
        }

        if (needle.Length < 2) return null;

        foreach (var lang in _cache.Values)
        {
            foreach (var (code, value) in lang)
            {
                if (string.IsNullOrEmpty(value)) continue;
                var hay = value.Normalize(NormalizationForm.FormC);
                if (hay.Contains(needle, StringComparison.OrdinalIgnoreCase))
                    return code;
            }
        }

        return null;
    }

    public Dictionary<string, string> GetTranslationsForCountry(string cca2)
    {
        var result = new Dictionary<string, string>();
        foreach (var (lang, translations) in _cache)
        {
            if (translations.TryGetValue(cca2, out var name))
                result[lang] = name;
        }
        return result;
    }

    public async Task SaveLanguageAsync(string langCode,
     Dictionary<string, string> translations)
    {
        var filePath = Path.Combine(_translationsPath, $"{langCode}.json");
        var json = JsonSerializer.Serialize(translations,
            new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
        await File.WriteAllTextAsync(filePath, json);
        _cache[langCode] = translations;
    }

    public IEnumerable<string> GetAvailableLanguages()
    {
        return _cache.Keys;
    }
}