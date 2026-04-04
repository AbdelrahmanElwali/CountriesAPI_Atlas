using System.Text.Json;
using CountriesAPI.Application.Interfaces;
using CountriesAPI.Domain.Entities;
using CountriesAPI.Infrastructure.Data;
using CountriesAPI.Infrastructure.ExternalApi;
using Microsoft.EntityFrameworkCore;

namespace CountriesAPI.Infrastructure.Services;

public class SyncService : ISyncService
{
    private readonly RestCountriesClient _client;
    private readonly AppDbContext _context;
    private readonly ITranslationStore _translationStore;

    public SyncService(
        RestCountriesClient client,
        AppDbContext context,
        ITranslationStore translationStore)
    {
        _client = client;
        _context = context;
        _translationStore = translationStore;
    }

    public async Task SyncAsync()
    {
        var countries = await _client.GetAllCountriesAsync();

        foreach (var item in countries)
        {
            var cca2 = item.GetProperty("cca2").GetString()!;
            var existing = await _context.Countries
                .FirstOrDefaultAsync(c => c.Cca2 == cca2);

            var country = existing ?? new Country();

            var name = item.GetProperty("name");
            country.CommonName = name.GetProperty("common").GetString()!;
            country.OfficialName = name.GetProperty("official").GetString()!;
            country.Cca2 = cca2;
            country.Cca3 = item.GetProperty("cca3").GetString()!;
            country.Region = item.GetProperty("region").GetString() ?? "";
            country.Subregion = item.TryGetProperty("subregion", out var sub)
                ? sub.GetString() ?? "" : "";
            country.Capital = item.TryGetProperty("capital", out var cap)
                && cap.GetArrayLength() > 0
                ? cap[0].GetString() ?? "" : "";
            country.Population = item.GetProperty("population").GetInt64();

            if (item.TryGetProperty("flags", out var flags))
                country.FlagPng = flags.TryGetProperty("png", out var png)
                    ? png.GetString() ?? "" : "";

            country.FlagEmoji = item.TryGetProperty("flag", out var flag)
                ? flag.GetString() ?? "" : "";

            if (item.TryGetProperty("latlng", out var latlng)
                && latlng.GetArrayLength() >= 2)
            {
                country.Lat = latlng[0].GetDouble();
                country.Lng = latlng[1].GetDouble();
            }

            country.RawJson = item.ToString();
            country.LastSyncedAt = DateTime.UtcNow;

            if (existing == null)
                _context.Countries.Add(country);
        }

        await _context.SaveChangesAsync();
    }

    public async Task SyncTranslationsAsync()
    {
        var countries = await _context.Countries.ToListAsync();
        var translationsByLang = new Dictionary<string, Dictionary<string, string>>();

        foreach (var country in countries)
        {
            var json = JsonDocument.Parse(country.RawJson);
            var root = json.RootElement;

            if (!root.TryGetProperty("translations", out var translations))
                continue;

            foreach (var lang in translations.EnumerateObject())
            {
                var langCode = lang.Name;

                if (!translationsByLang.ContainsKey(langCode))
                    translationsByLang[langCode] = new();

                if (lang.Value.TryGetProperty("common", out var commonName))
                {
                    var translatedName = commonName.GetString();
                    if (!string.IsNullOrEmpty(translatedName))
                        translationsByLang[langCode][country.Cca2] = translatedName;
                }
            }
        }

        foreach (var (langCode, translations) in translationsByLang)
        {
            await _translationStore.SaveLanguageAsync(langCode, translations);
        }
    }
}