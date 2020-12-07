using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Data.Interfaces
{
    public interface IDeletable
    {
        bool Deleted { get; set; }
        string DeletedBy { get; set; }
        DateTime? DeletedOn { get; set; }
    }
}
