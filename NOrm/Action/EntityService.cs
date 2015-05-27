using NOrm.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOrm.Action
{
    public static class EntityService
    {
        public static bool IsEntity(object obj)
        {
            return Attribute.IsDefined(obj.GetType(), typeof(ORMEntity));
        }
    }
}
