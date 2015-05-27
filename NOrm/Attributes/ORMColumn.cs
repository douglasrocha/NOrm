using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ORMColumn : Attribute
    {
        public string Name { get; private set; }
        public string FriendlyName { get; private set; }

        public ORMColumn(string name, string friendlyName)
        {
            this.Name = name;
            this.FriendlyName = friendlyName;
        }
    }
}
