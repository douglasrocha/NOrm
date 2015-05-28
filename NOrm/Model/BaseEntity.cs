using NOrm.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NOrm.Model
{
    public abstract class BaseEntity
    {
        public RecordState recordState { get; set; }

        public BaseEntity()
        {
            recordState = RecordState.Browse;
        }
    }
}
