using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CursoWebsite.Helpers
{
    public class Session
    {
        public static string GetValue(IPrincipal User, string Property)
        {
            var ClaiValue = ((ClaimsIdentity)User.Identity).FindFirst(Property);
            return ClaiValue.Value;
        }

        public static string GetNameIdentifier(IPrincipal User)
        {
            var ClaiValue = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
            return ClaiValue.Value;
        }

        public static string GetName(IPrincipal User)
        {
            var ClaiValue = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name);
            return ClaiValue.Value;
        }

        public static string GetUserData(IPrincipal User)
        {
            var ClaiValue = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.UserData);
            return ClaiValue.Value;
        }
    }
}
