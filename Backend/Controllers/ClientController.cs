using Backend.Access;
using Backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(IDbContextFactory<TiendaContext> dbContextFactory) : ControllerBase
    {
        private readonly IDbContextFactory<TiendaContext> dbContextFactory = dbContextFactory;

        // GET: api/<ClientController>
        [HttpGet]
        public List<Client> Get()
        {
            using var context = dbContextFactory.CreateDbContext();
            return context.Set<Client>()
                .AsNoTracking()
                .ToList();
        }

        // GET api/<ClientController>/5
        [HttpGet("{email}")]
        public Client? Get(string email)
        {
            using var context = dbContextFactory.CreateDbContext();
            return context.Set<Client>()
                .AsNoTracking()
                .FirstOrDefault(x => x.Email.Contains(email));
        }

        // POST api/<ClientController>
        [HttpPost]
        public Client Post([FromBody] Client value)
        {
            using var context = dbContextFactory.CreateDbContext();
            context.Add(value);
            context.SaveChanges();

            return value;
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{id}")]
        public void Delete(string email)
        {
            using var context = dbContextFactory.CreateDbContext();
            context.Set<Client>()
                .Where(x => x.Email == email)
                .ExecuteDelete();
        }
    }
}
