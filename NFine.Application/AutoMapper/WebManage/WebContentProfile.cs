using AutoMapper;
using NFine.Domain.ViewModel;
using NFine.Domain.Entity.WebManage;

namespace NFine.Application.AutoMapper
{
    class WebContentProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<WebContentEntity, WebContentDto>();
        }
    }
}
