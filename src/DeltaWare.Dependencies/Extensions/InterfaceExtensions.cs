using System;
using System.Linq;

namespace DeltaWare.Dependencies.Extensions
{
    internal static class InterfaceExtensions
    {
        public static bool HasInterface<TInterface>(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(TInterface));
        }
    }
}
