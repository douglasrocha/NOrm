using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ORMEntity : Attribute
    {
        public string Name { get; private set; }

        public ORMEntity(string name)
        {
            this.Name = name;
        }
    }
}
