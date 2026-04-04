using CountriesAPI.Domain.Entities;

namespace CountriesAPI.Application.Interfaces;

public interface ICountryRepository
{
    Task<IEnumerable<Country>> GetAllAsync();
    Task<Country?> GetByCodeAsync(string code);      // EG أو EGY
    Task<Country?> GetByNameAsync(string name);      // Egypt
    Task<IEnumerable<Country>> GetByRegionAsync(string region);
    Task<IEnumerable<Country>> GetByCurrencyAsync(string currency);
    Task UpsertAsync(IEnumerable<Country> countries); // للـ Sync
}