using NRZ.Models.Address;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IAddressService
    {
        Task CreateAsync(AddressModel model);
        Task EditAsync(AddressModel model);
        Task<AddressModel> GetById(int id);
    }
}
