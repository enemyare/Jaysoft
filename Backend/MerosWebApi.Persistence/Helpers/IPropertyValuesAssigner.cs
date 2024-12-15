namespace MerosWebApi.Persistence.Helpers
{
    internal interface IPropertyValuesAssigner<TDestination, TFrom>
    {
        static abstract void AssignPropertyValues(TDestination to, TFrom from);
    }
}
