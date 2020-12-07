using Microsoft.EntityFrameworkCore;
using NRZ.Models.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace NRZ.Data.Extensions
{
    public static class PersonExtensions
    {
        public static PersonModel ToModel(this Person person)
        {
            if (person == null)
            {
                return null;
            }

            var model = new PersonModel
            {
                Id = person.Id,
                UserId = person.UserId,
                Email = person.Email,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                Phone = person.Phone,
                IdentificationNumber = person.IdentificationNumber,
                IdentificationType = person.IdentificationNumberType,
                Address = person.Address.ToModel()
            };

            return model;
        }

        public static Person ToPerson(this PersonModel model)
        {
            if (model == null)
            {
                return null;
            }

            Person person = new Person()
            {
                LastName = model.LastName,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                UserId = model.UserId,
                Email = model.Email,
                Phone = model.Phone,
                IdentificationNumberType = model.IdentificationType,
                IdentificationNumber = model.IdentificationNumber,
                Address = model.Address.ToAddress()
            };

            return person;
        }

        public static Person ToUpdate(this Person person, PersonModel model)
        {
            if (person == null || model == null)
            {
                return null;
            }

            person.LastName = model.LastName;
            person.FirstName = model.FirstName;
            person.MiddleName = model.MiddleName;
            person.Phone = model.Phone;
            person.Email = model.Email;
            person.IdentificationNumberType = model.IdentificationType;
            person.IdentificationNumber = model.IdentificationNumber;
            return person;
        }


        public static RegixPersonModel ToModel(this RegixPerson entity)
        {
            if (entity == null)
            {
                return null;
            }

            RegixPersonModel model = new RegixPersonModel
            {
                Id = entity.Id,
                Identifier = entity.Identifier,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                DateOfBirth = entity.DateOfBirth.HasValue == true ? DateTime.SpecifyKind(entity.DateOfBirth.Value, DateTimeKind.Utc) : default(DateTime?),
                DateOfDeath = entity.DateOfDeath.HasValue == true ? DateTime.SpecifyKind(entity.DateOfDeath.Value, DateTimeKind.Utc) : default(DateTime?),
                RequestId = entity.RequestId,
                UpdatedAt = entity.UpdatedAt.HasValue == true ? DateTime.SpecifyKind(entity.UpdatedAt.Value, DateTimeKind.Utc) : default(DateTime?),
            };

            return model;
        }

        public static RegixPerson ToEntity(this RegixPersonModel model)
        {
            if (model == null)
            {
                return null;
            }

            RegixPerson entity = new RegixPerson
            {
                Identifier = model.Identifier,
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth.HasValue == true ? DateTime.SpecifyKind(model.DateOfBirth.Value, DateTimeKind.Utc) : default(DateTime?),
                DateOfDeath = model.DateOfDeath.HasValue == true ? DateTime.SpecifyKind(model.DateOfDeath.Value, DateTimeKind.Utc) : default(DateTime?),
                RequestId = model.RequestId,
                UpdatedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
            };

            return entity;
        }


    }
}
