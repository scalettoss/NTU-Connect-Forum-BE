using AutoMapper;
using ForumBE.DTOs.Paginations;

namespace ForumBE.Helpers
{

    public class PageListConverter<TSource, TDestination> : ITypeConverter<PagedList<TSource>, PagedList<TDestination>>
    {
        private readonly IMapper _mapper;

        public PageListConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
        {
            var items = _mapper.Map<List<TDestination>>(source.Items);
            return new PagedList<TDestination>(
                items,
                source.TotalCount,
                source.CurrentPage,
                source.PageSize
            );
        }
    }
}
