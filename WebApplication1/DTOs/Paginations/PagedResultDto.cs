using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;

namespace ForumBE.DTOs.Paginations
{
    public class PaginationData<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PagedResultDto Pagination { get; set; }
    }

    public class PagedResultDto
    {
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
