using AutoMapper;
using TweetsAPI.Model;

namespace CoreWebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserCredential>();
            
        }
    }
}
