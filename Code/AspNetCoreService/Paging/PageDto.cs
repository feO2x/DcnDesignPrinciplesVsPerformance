namespace AspNetCoreService.Paging
{
    public sealed class PageDto
    {
        public int Skip { get; init; } = 0;

        public int Take { get; init; } = 30;

        public string? SearchTerm { get; init; }
    }
}