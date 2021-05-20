using AutoMapper;
using System.Collections.Generic;

namespace TMSBusinessLogicLayer
{
    public class AutoMapper<TSource, TDestination>
    {
        public TDestination MapToObject(TSource source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            return config.CreateMapper().Map<TSource, TDestination>(source);
        }

        public IEnumerable<TDestination> MapToList(IEnumerable<TSource> sourceList)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            return config.CreateMapper().Map<IEnumerable<TSource>, IEnumerable<TDestination>>(sourceList);

        }
    }
}
