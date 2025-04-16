using AutoMapper;
using ForumBE.DTOs.Bookmarks;
using ForumBE.Models;

namespace ForumBE.Mappings
{
    public class BookmarkMappings : Profile
    {
       public BookmarkMappings()
        {
            CreateMap<Bookmark, BookmarkResponseDto>();
            CreateMap<BookmarkCreateRequestDto, Bookmark>();
        }
    }
}
