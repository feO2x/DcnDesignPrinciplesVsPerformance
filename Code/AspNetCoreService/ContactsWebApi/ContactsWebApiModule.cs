using AspNetCoreService.ContactsWebApi.DeleteContact;
using AspNetCoreService.ContactsWebApi.GetContact;
using AspNetCoreService.ContactsWebApi.GetContacts;
using AspNetCoreService.ContactsWebApi.NewContact;
using AspNetCoreService.ContactsWebApi.UpdateContact;
using AspNetCoreService.CoreModel;
using AspNetCoreService.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreService.ContactsWebApi
{
    public static class ContactsWebApiModule
    {
        public static IServiceCollection AddContactsWebApiModule(this IServiceCollection services) =>
            services.AddTransient<IGetContactsSession, EfGetContactsSession>()
                    .AddSingleton<ICountryNameValidator, HttpCountryNameValidator>()
                    .AddSingleton<NewContactDtoValidator>()
                    .AddTransient<INewContactSession, EfNewContactSession>()
                    .AddTransient<IGetContactSession, EfGetContactSession>()
                    .AddTransient<IUpdateContactSession, EfUpdateContactSession>()
                    .AddTransient<IDeleteContactSession, EfDeleteContactSession>();
    }
}