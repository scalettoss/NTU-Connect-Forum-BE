using AutoMapper;
using ForumBE.DTOs.Paginations;
using ForumBE.Helpers;

namespace ForumBE.Mappings
{
    public class PaginationMappings : Profile
    {
        public PaginationMappings() 
        {
            CreateMap(typeof(PagedList<>), typeof(PagedList<>))
                .ConvertUsing(typeof(PageListConverter<,>));
        }
    }
}
