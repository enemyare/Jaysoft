using Amazon.Auth.AccessControlPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Persistence.Helpers
{
    internal interface IPropertyValuesAssigner<TDestination, TFrom>
    {
        static abstract void AssignPropertyValues(TDestination to, TFrom from);
    }
}
