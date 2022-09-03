using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ContactsController : Controller
    {
        private readonly ContactsApiDbContext _dbContext;
        public ContactsController(ContactsApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var result = await _dbContext.Contacts.ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);

            if (contact != null)
                return Ok(contact);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addedContact)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                FullName = addedContact.FullName,
                Address = addedContact.Address,
                Email = addedContact.Email,
                Phone = addedContact.Phone
            };
            await _dbContext.Contacts.AddAsync(contact);
            await _dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updatedContact)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);

            if (contact == null)
                return NotFound();

            contact.FullName = updatedContact.FullName;
            contact.Address = updatedContact.Address;
            contact.Phone = updatedContact.Phone;
            contact.Email = updatedContact.Email;
            await _dbContext.SaveChangesAsync(); 
            return Ok(contact);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);

            if (contact == null)
                return NotFound();

            _dbContext.Remove(contact);
            await _dbContext.SaveChangesAsync();
            return Ok(contact);
        } 
    }
}
