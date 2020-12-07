using System;
namespace NRZ.Data.Interfaces
{
    public interface IEditable
    {
        string UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
    }

    public interface ICreatable
    {
        string CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
    }
}
