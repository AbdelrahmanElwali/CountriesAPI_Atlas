using CountriesAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CountriesAPI.API.Controllers;

[ApiController]
[Route("api/countries")]
public class CountriesController : ControllerBase
{
    private readonly CountryService _countryService;

    public CountriesController(CountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var countries = await _countryService.GetAllAsync();
        return Ok(countries);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var country = await _countryService.GetByCodeAsync(code);
        if (country == null) return NotFound();
        return Ok(country);
    }

    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var country = await _countryService.GetByNameAsync(name);
        if (country == null) return NotFound();

        // برجع array عشان الـ Angular يقدر يتعامل معاه
        return Ok(new[] { country });
    }

    [HttpGet("region/{region}")]
    public async Task<IActionResult> GetByRegion(string region)
    {
        var countries = await _countryService.GetByRegionAsync(region);
        return Ok(countries);
    }
}