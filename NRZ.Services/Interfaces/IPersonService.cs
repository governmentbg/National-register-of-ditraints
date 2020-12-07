using NRZ.Models.Person;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NRZ.Services.Interfaces
{
    public interface IPersonService
    {
        Task CreateAsync(PersonModel model, string userId = null);
        Task EditAsync(PersonModel model, string userId);
        Task DeleteAsync(int personId, string userId);
        Task<PersonModel> GetPersonModelAsyncByUserId(string userId);
        Task<RegixPersonModel> AddRegixPersonAsync(RegixPersonModel model);
        Task<RegixPersonModel> GetPersonFromRegixAsync(string identifier);
        Task<Data.Person> GetByPersonIdentification(string IdType, string IdNumber);
    }
}
