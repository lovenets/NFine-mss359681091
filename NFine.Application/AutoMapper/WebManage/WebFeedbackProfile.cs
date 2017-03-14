using AutoMapper;
using NFine.Domain.ViewModel;
using NFine.Domain.Entity.WebManage;

namespace NFine.Application.AutoMapper
{
    class WebFeedbackProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<WebFeedbackEntity, WebFeedbackDto>();
        }
    }
}
