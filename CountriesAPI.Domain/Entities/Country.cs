using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesAPI.Domain.Entities;

public class Country
{
    public int Id { get; set; }

    
    public string CommonName { get; set; } = string.Empty;
    public string OfficialName { get; set; } = string.Empty;

    // الكود
    public string Cca2 { get; set; } = string.Empty;   // EG
    public string Cca3 { get; set; } = string.Empty;   // EGY

   
    public string Region { get; set; } = string.Empty;
    public string Subregion { get; set; } = string.Empty;

    
    public string Capital { get; set; } = string.Empty;
    public long Population { get; set; }

   
    public string FlagEmoji { get; set; } = string.Empty;
    public string FlagPng { get; set; } = string.Empty;

    // الموقع الجغرافي
    public double Lat { get; set; }
    public double Lng { get; set; }

    // الداتا الكاملة من REST Countries
    public string RawJson { get; set; } = string.Empty;

    // متى اتعمل آخر sync
    public DateTime LastSyncedAt { get; set; }
}
