namespace MerosWebApi.Persistence.Helpers
{
    public interface IPropertyAssigner<TSource, TDestination>
    {
        static abstract TDestination MapFrom(TSource source);
    }
}
