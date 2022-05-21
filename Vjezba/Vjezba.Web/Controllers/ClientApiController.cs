using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;

namespace Vjezba.Web.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientApiController : Controller
    {
        private ClientManagerDbContext _dbContext;

        public ClientApiController(ClientManagerDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<ClientDTO> clients = _dbContext.Clients
                .Include(c => c.City)
                .Select(c => ClientToClientDTO(c))
                .ToList();

            return Ok(clients);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            ClientDTO? client = _dbContext.Clients
                .Include(c => c.City)
                .Where(c => c.ID == id)
                .Select(c => ClientToClientDTO(c))
                .FirstOrDefault();

            return client == null ? NotFound() : Ok(client);
        }

        [HttpGet]
        [Route("pretraga/{q}")]
        public IActionResult Get(string q)
        {
            ClientDTO? client = _dbContext.Clients
                .Include(c => c.City)
                .Where(c => c.FirstName.Contains(q))
                .Select(c => ClientToClientDTO(c))
                .FirstOrDefault();

            return client == null ? NotFound() : Ok(client);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            this._dbContext.Add(client);
            this._dbContext.SaveChanges();

            return Get(client.ID);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            Client clientForUpdate = this._dbContext.Clients.First(c => c.ID == id);
            clientForUpdate.FirstName = client.FirstName;
            clientForUpdate.LastName = client.LastName;
            clientForUpdate.Email = client.Email;
            clientForUpdate.DateOfBirth = client.DateOfBirth;
            clientForUpdate.WorkingExperience = client.WorkingExperience;
            clientForUpdate.Gender = client.Gender;
            clientForUpdate.Address = client.Address;
            clientForUpdate.PhoneNumber = client.PhoneNumber;
            clientForUpdate.CityID = client.CityID;

            this._dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            Client clientToDelete = this._dbContext.Clients.FirstOrDefault(c => c.ID == id);

            if(clientToDelete == null)
            {
                return NotFound();
            }

            this._dbContext.Clients.Remove(clientToDelete);
            this._dbContext.SaveChanges();

            return Ok();
        }

        public static ClientDTO ClientToClientDTO(Client c)
        {
            return new ClientDTO()
            {
                ID = c.ID,
                FirstName = c.FirstName,
                Address = c.Address,
                City = new CityDTO() { ID = c.City.ID, Name = c.City.Name },
                Email = c.Email
            };
        }
    }

    public class CityDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class ClientDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string? Address { get; set; }
        public CityDTO? City { get; set; }
        public string? Email { get; set; }
    }
}
