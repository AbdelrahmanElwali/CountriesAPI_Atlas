using CountriesAPI.Application.DTOs;
using CountriesAPI.Application.Interfaces;

namespace CountriesAPI.Application.Services;

public class CountryService
{
    private readonly ICountryRepository _repository;
    private readonly ITranslationStore _translationStore;

    public CountryService(
        ICountryRepository repository,
        ITranslationStore translationStore)
    {
        _repository = repository;
        _translationStore = translationStore;
    }

    public async Task<IEnumerable<CountryDto>> GetAllAsync()
    {
        var countries = await _repository.GetAllAsync();
        return countries.Select(c =>
        {
            var dto = CountryDto.FromEntity(c);
            dto.Translations = _translationStore
                .GetTranslationsForCountry(c.Cca2);
            return dto;
        });
    }

    public async Task<CountryDto?> GetByCodeAsync(string code)
    {
        var country = await _repository.GetByCodeAsync(code);
        if (country == null) return null;

        var dto = CountryDto.FromEntity(country);
        dto.Translations = _translationStore
            .GetTranslationsForCountry(country.Cca2);
        return dto;
    }

    public async Task<CountryDto?> GetByNameAsync(string name)
    {
        
        var country = await _repository.GetByNameAsync(name);

       
        if (country == null)
        {
            var code = _translationStore.FindCountryCode(name);
            if (code != null)
                country = await _repository.GetByCodeAsync(code);
        }

        if (country == null) return null;

        var dto = CountryDto.FromEntity(country);
        dto.Translations = _translationStore
            .GetTranslationsForCountry(country.Cca2);
        return dto;
    }

    public async Task<IEnumerable<CountryDto>> GetByRegionAsync(string region)
    {
        var countries = await _repository.GetByRegionAsync(region);
        return countries.Select(CountryDto.FromEntity);
    }
}