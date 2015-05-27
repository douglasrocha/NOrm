using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOrm.Exceptions
{
    public class InvalidEntityException : Exception
    {
        public InvalidEntityException() : base("Not a valid entity object")
        {

        }
    }
}
