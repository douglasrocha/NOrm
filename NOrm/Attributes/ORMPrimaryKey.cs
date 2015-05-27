using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ORMPrimaryKey : Attribute
    {
        public bool AutoIncrement { get; private set; }

        public ORMPrimaryKey()
        {
            this.AutoIncrement = false;
        }

        public ORMPrimaryKey(bool autoIncrement)
        {
            this.AutoIncrement = autoIncrement;
        }
    }
}
