using NRZ.Models.Address;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class AddressExtensions
    {
        public static Address ToAddress(this AddressModel model)
        {
            if (model == null)
            {
                return null;
            }

            Address address = new Address
            {
                CityId = model.CityId,
                MunicipalityId = model.MunicipalityId,
                RegionId = model.RegionId,
                StreetAddress = model.StreetAddress,
            };

            return address;
        }

        public static AddressModel ToModel(this Address address)
        {
            if (address == null)
            {
                return null;
            }

            AddressModel model = new AddressModel
            {
                Id = address.Id,
                CityId = address.CityId,
                MunicipalityId = address.MunicipalityId,
                RegionId = address.RegionId,
                StreetAddress = address.StreetAddress,
            };

            return model;
        }

        public static Address ToUpdate(this Address address, AddressModel model)
        {
            if (model == null || address == null)
            {
                return null;
            }

            address.CityId = model.CityId;
            address.MunicipalityId = model.MunicipalityId;
            address.RegionId = model.RegionId;
            address.StreetAddress = model.StreetAddress;

            return address;
        }
    }
}
