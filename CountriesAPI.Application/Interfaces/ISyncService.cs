using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesAPI.Application.Interfaces;

public interface ISyncService
{
    Task SyncAsync();
    Task SyncTranslationsAsync();
}
