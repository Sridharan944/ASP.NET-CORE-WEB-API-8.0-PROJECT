using Knila_Projects.Entities;

namespace Knila_Projects.InterfaceRepositories
{
    public interface IContactRepository
    {

        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int id);
        Task<Contact> AddContactAsync(Contact contact);
        Task<Contact> UpdateContactAsync(Contact contact);
        Task<bool> DeleteContactAsync(int id);


    }
}
