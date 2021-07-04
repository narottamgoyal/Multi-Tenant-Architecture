using AdminPortalService.Model;
using AutoMapper;
using UserManagement.Persistence;

namespace AdminPortalService.AppConfig
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Tenant, TenantDto>().ReverseMap();
            CreateMap<UserDetailDto, UserDetail>()
                .ForMember(x => x.EmailId, y => y.MapFrom(f => f.UserName));
        }
    }
}
