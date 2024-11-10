using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Persistence.Helpers
{
    public interface IPropertyAssigner<TSource, TDestination>
    {
        static abstract TDestination MapFrom(TSource source);
    }
}
