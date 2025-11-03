using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Services.Abstractions.Cache
{
    public interface ICacheService
    {
        Task SetAsync(string key, object value, TimeSpan duration);
        Task<string?> GetAsync(string key);
    }
}
