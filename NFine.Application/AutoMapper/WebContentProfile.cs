using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NFine.Domain.Entity.WebManage;

namespace NFine.Application
{
    class WebContentProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<WebContentEntity, WebContentDto>();
        }
    }
}
