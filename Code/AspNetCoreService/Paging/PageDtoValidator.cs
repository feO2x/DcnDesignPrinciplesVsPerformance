using FluentValidation;

namespace AspNetCoreService.Paging
{
    public sealed class PageDtoValidator : AbstractValidator<PageDto>
    {
        public PageDtoValidator()
        {
            RuleFor(page => page.Skip).GreaterThanOrEqualTo(0);
            RuleFor(page => page.Take).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}