using AspNetCoreService.CoreModel;
using AutoMapper;

namespace AspNetCoreService.ContactsWebApi.NewContact
{
    public sealed class NewContactAutoMapperProfile : Profile
    {
        public NewContactAutoMapperProfile()
        {
            CreateMap<NewContactDto, Contact>();
        }
    }
}