using Knila_Projects.Entities;
using Knila_Projects.InterfaceRepositories;
using Knila_Projects.JwtToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace Knila_Projects.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    [ServiceFilter(typeof(TokenAuthorizationFilter))]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _repository;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactRepository repository, ILogger<ContactsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetAllContacts()
        {

            try
            {
                var contacts = await _repository.GetAllContactsAsync();
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching contacts");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error fetching contacts: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            try
            {
                var contact = await _repository.GetContactByIdAsync(id);
                if (contact == null)
                {
                    _logger.LogWarning($"Contact with ID {id} not found..");
                    return NotFound();
                }

                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching contact with ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error fetching contact with ID {id}: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
            try
            {
                var createdContact = await _repository.AddContactAsync(contact);
                return CreatedAtAction(nameof(GetContact), new { id = createdContact.ContactID }, createdContact);
            }
            catch (Exception ex)
            {
                
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating contact: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, Contact contact)
        {
            try
            {
                if (id != contact.ContactID)
                {
                    _logger.LogWarning($"Contact ID mismatch for update. Provided ID: {id}, Contact ID: {contact.ContactID}");
                    return BadRequest("Contact ID mismatch.");
                }

                var updatedContact = await _repository.UpdateContactAsync(contact);
                if (updatedContact == null)
                {
                    _logger.LogWarning($"Contact with ID {id} not found for update.");
                    return NotFound();
                }

                return Ok(updatedContact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating contact with ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating contact with ID {id}: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                var success = await _repository.DeleteContactAsync(id);
                if (!success)
                {
                    _logger.LogWarning($"Contact with ID {id} not found for deletion.");
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting contact with ID {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting contact with ID {id}: {ex.Message}");
            }
        }

      

    }

}
