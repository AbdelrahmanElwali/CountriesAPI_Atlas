using CountriesAPI.Application.Interfaces;
using CountriesAPI.Domain.Entities;
using CountriesAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CountriesAPI.Infrastructure.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _context;

    public CountryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetAllAsync()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetByCodeAsync(string code)
    {
        code = code.ToUpper();
        return await _context.Countries
            .FirstOrDefaultAsync(c => c.Cca2 == code || c.Cca3 == code);
    }

    public async Task<Country?> GetByNameAsync(string name)
    {
        return await _context.Countries
            .FirstOrDefaultAsync(c =>
                EF.Functions.Like(c.CommonName, $"%{name}%") ||
                EF.Functions.Like(c.OfficialName, $"%{name}%"));
    }

    public async Task<IEnumerable<Country>> GetByRegionAsync(string region)
    {
        return await _context.Countries
            .Where(c => EF.Functions.Like(c.Region, $"%{region}%"))
            .ToListAsync();
    }

    public async Task<IEnumerable<Country>> GetByCurrencyAsync(string currency)
    {
     
        return await _context.Countries
            .Where(c => EF.Functions.Like(c.RawJson, $"%{currency}%"))
            .ToListAsync();
    }

    public async Task UpsertAsync(IEnumerable<Country> countries)
    {
        foreach (var country in countries)
        {
            var existing = await _context.Countries
                .FirstOrDefaultAsync(c => c.Cca2 == country.Cca2);

            if (existing == null)
                _context.Countries.Add(country);
            else
            {
                existing.CommonName = country.CommonName;
                existing.OfficialName = country.OfficialName;
                existing.Region = country.Region;
                existing.Subregion = country.Subregion;
                existing.Capital = country.Capital;
                existing.Population = country.Population;
                existing.FlagEmoji = country.FlagEmoji;
                existing.FlagPng = country.FlagPng;
                existing.Lat = country.Lat;
                existing.Lng = country.Lng;
                existing.RawJson = country.RawJson;
                existing.LastSyncedAt = country.LastSyncedAt;
            }
        }

        await _context.SaveChangesAsync();
    }
}