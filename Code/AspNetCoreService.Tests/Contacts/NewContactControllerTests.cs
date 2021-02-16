using System;
using System.Threading.Tasks;
using AspNetCoreService.ContactsWebApi.NewContact;
using AspNetCoreService.CoreModel;
using AspNetCoreService.Tests.CoreModel;
using AspNetCoreService.Tests.TestHelpers;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCoreService.Tests.Contacts
{
    public sealed class NewContactControllerTests : WebApiControllerTests<NewContactController>
    {
        public NewContactControllerTests(ITestOutputHelper output) : base("api/contacts/new")
        {
            CountryNameValidator = new CountryNameValidatorStub();
            var loggerFactory = output.CreateLoggerFactory();
            var validator = new NewContactDtoValidator(CountryNameValidator, loggerFactory.CreateLogger<NewContactDtoValidator>());
            Session = new NewContactSessionMock();
            SessionFactory = new FactorySpy<NewContactSessionMock>(Session);
            Mapper = new Mapper(new MapperConfiguration(expression => expression.AddProfile(new NewContactAutoMapperProfile())));
            Controller = new NewContactController(validator, SessionFactory.GetInstance, Mapper, loggerFactory.CreateLogger<NewContactController>());
        }

        private NewContactController Controller { get; }

        private CountryNameValidatorStub CountryNameValidator { get; }

        private NewContactSessionMock Session { get; }

        private FactorySpy<NewContactSessionMock> SessionFactory { get; }

        private IMapper Mapper { get; }

        [Fact]
        public static void CreateNewContactMustBeHttpPost() =>
            ControllerType.GetMethod(nameof(NewContactController.CreateNewContact)).Should().BeDecoratedWith<HttpPostAttribute>();

        [Fact]
        public static void NewContactDtoValidatorMustDeriveFromBaseContactValidator() =>
            typeof(NewContactDtoValidator).Should().BeDerivedFrom<BaseContactValidator<NewContactDto>>();

        [Fact]
        public async Task CreateNewContact()
        {
            var dto = CreateDto();

            var result = await Controller.CreateNewContact(dto);

            var expectedContact = Mapper.Map<NewContactDto, Contact>(dto);
            Session.CapturedContact.Should().BeEquivalentTo(expectedContact);
            Session.SaveChangesMustHaveBeenCalled()
                   .MustHaveBeenDisposed();
            var expectedResult = Controller.Created("/api/contacts/0", Session.CapturedContact);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task InvalidCountry()
        {
            var dto = CreateDto();
            dto.CountryOfOrigin = "Invalid Country Name";
            CountryNameValidator.IsValidCountry = false;

            var result = await Controller.CreateNewContact(dto);

            CheckForValidationError(result);
        }

        [Fact]
        public async Task ExceptionWhileValidatingCountry()
        {
            var dto = CreateDto();
            CountryNameValidator.ThrowException = true;

            var result = await Controller.CreateNewContact(dto);

            CheckForValidationError(result);
        }

        private void CheckForValidationError(IActionResult result)
        {
            var expectedResult = Controller.ValidationProblem();
            result.MustBeEquivalentToValidationProblem(expectedResult);
            SessionFactory.InstanceMustNotHaveBeenCreated();
            Session.SaveChangesMustNotHaveBeenCalled();
        }

        private static NewContactDto CreateDto() =>
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "135 Fantasy Road, Michigan",
                CountryOfOrigin = "USA",
                DateOfBirth = new DateTime(1979, 8, 18),
                EmailAddress = "john.doe@gmail.com"
            };


        private sealed class NewContactSessionMock : BaseSessionMock<NewContactSessionMock>, INewContactSession
        {
            public Contact? CapturedContact { get; private set; }

            public void AddContact(Contact contact)
            {
                CapturedContact = contact;
            }
        }
    }
}