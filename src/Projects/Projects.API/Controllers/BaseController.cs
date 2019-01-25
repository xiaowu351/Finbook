
using Microsoft.AspNetCore.Mvc;
using Projects.API.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 

namespace Projects.API.Controllers
{
    public class BaseController : Controller
    {
        private UserIdentity _userIdentity;
        protected UserIdentity UserIdentity
        {
            get
            {
                if (_userIdentity == null)
                {
                    var claims = HttpContext.User.Claims;
                    _userIdentity = new UserIdentity ();
                    _userIdentity.UserId =int.Parse(claims.FirstOrDefault(c => c.Type.Equals("sub")).Value);
                    _userIdentity.Name = claims.FirstOrDefault(c => c.Type.Equals(nameof(_userIdentity.Name).ToLower())).Value;
                    _userIdentity.Company = claims.FirstOrDefault(c => c.Type.Equals(nameof(_userIdentity.Company).ToLower())).Value;
                    _userIdentity.Title = claims.FirstOrDefault(c => c.Type.Equals(nameof(_userIdentity.Title).ToLower())).Value;
                    _userIdentity.Avatar = claims.FirstOrDefault(c => c.Type.Equals(nameof(_userIdentity.Avatar).ToLower())).Value;

                }
                return _userIdentity;
            }
        }

        
    }
}
