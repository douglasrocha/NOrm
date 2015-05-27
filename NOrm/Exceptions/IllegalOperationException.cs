using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOrm.Exceptions
{
    public class IllegalOperationException : Exception
    {
        public IllegalOperationException() : base("Operação ilegal!")
        {

        }
    }
}
