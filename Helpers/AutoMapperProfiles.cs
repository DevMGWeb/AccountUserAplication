using AutoMapper;
using CursoWebsite.Models;
using CursoWebsite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebsite.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<EditAccount, UserAccount>()
                .ForMember(x => x.Files, options => options.Ignore())
                .ForMember(x => x.Id, options => options.Ignore());

            CreateMap<UserAccount, EditAccount>();
        }

        private ICollection<File> MappaerSoloSiRecibeArchivos(EditAccount editAccount, UserAccount userAccount)
        {
            if(editAccount.Files.Count > 0)
            {
                return editAccount.Files;
            }

            return userAccount.Files;
        }
    }
}
