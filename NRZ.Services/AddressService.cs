using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NRZ.Data;
using NRZ.Data.Extensions;
using NRZ.Models.Address;
using NRZ.Services.Interfaces;
using NRZ.Shared.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services
{
    public class AddressService : BaseService, IAddressService
    {
        public AddressService(NRZContext context,
                              IStringLocalizer<SharedResources> localizer)
            : base(context, localizer)
        {
        }

        public async Task CreateAsync(AddressModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("Model can not be null!");
            }

            Address address = model.ToAddress();

            await _context.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(AddressModel model)
        {
            Address address = await _context.Address.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            address.ToUpdate(model);

            await _context.SaveChangesAsync();
        }

        public async Task<AddressModel> GetById(int id)
        {
            AddressModel address = await _context.Address.Where(x => x.Id == id).Select(x => x.ToModel()).FirstOrDefaultAsync();

            return address;
        }
    }
}
