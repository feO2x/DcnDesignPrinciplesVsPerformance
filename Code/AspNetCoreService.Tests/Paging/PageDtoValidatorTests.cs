using AspNetCoreService.Paging;
using FluentValidation.TestHelper;
using Xunit;

namespace AspNetCoreService.Tests.Paging
{
    public sealed class PageDtoValidatorTests
    {
        private PageDtoValidator Validator { get; } = new();

        [Theory]
        [InlineData(0, 100, null)]
        [InlineData(0, 100, "Jeremy")]
        [InlineData(60, 20, "Search Term With *WildCard")]
        public void ValidValues(int skip, int take, string? searchTerm)
        {
            var pageDto = CreatePageDto(skip, take, searchTerm);

            var result = Validator.TestValidate(pageDto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-40)]
        public void InvalidSkip(int invalidSkip)
        {
            var pageDto = CreatePageDto(invalidSkip);

            var result = Validator.TestValidate(pageDto);

            result.ShouldHaveValidationErrorFor(nameof(PageDto.Skip));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-8)]
        [InlineData(101)]
        [InlineData(20953)]
        public void InvalidTake(int invalidTake)
        {
            var pageDto = CreatePageDto(take: invalidTake);

            var result = Validator.TestValidate(pageDto);

            result.ShouldHaveValidationErrorFor(nameof(PageDto.Take));
        }

        private static PageDto CreatePageDto(int skip = 0, int take = 30, string? searchTerm = null) =>
            new() { Skip = skip, Take = take, SearchTerm = searchTerm };
    }
}