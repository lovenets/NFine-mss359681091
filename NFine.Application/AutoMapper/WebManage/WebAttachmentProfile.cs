using AutoMapper;
using NFine.Domain.ViewModel;
using NFine.Domain.Entity.WebManage;

namespace NFine.Application.AutoMapper
{
    class WebAttachmentProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<WebAttachmentEntity, WebAttachmentDto>();
        }
    }
}
