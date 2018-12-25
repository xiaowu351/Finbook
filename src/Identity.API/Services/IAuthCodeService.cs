using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Services
{
    public interface IAuthCodeService
    {
        Task<bool> ValidateAsync(string phone, string authCode);
    }
}
