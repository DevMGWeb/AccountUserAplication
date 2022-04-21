using CursoWebsite.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.Models
{
    public class DbInitialize
    {
        internal static IEnumerable<UserAccount> GetDataUserAccount()
        {
            return new List<UserAccount>()
            {
                new UserAccount(){ 
                    Id = Guid.NewGuid().ToString(), 
                    Name= "User Invited", 
                    Username = "Guest",
                    Password = Encrypt.GetSHA256("Guest"),
                    Email = "mig_24@hotmail.com",
                },
            };
        }
    }
}
