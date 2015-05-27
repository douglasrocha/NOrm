using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOrm.Action
{
    public static class ReflectionService
    {
        public static T CreateObjectWithEmptyConstructor<T>()
        {
            // Invoca tipo construtor do tipo genérico via reflection
            var currentObjectType = typeof(T);
            var currentObjectConstructor = currentObjectType.GetConstructor(new Type[] { });
            return (T)currentObjectConstructor.Invoke(null);
        }
    }
}
