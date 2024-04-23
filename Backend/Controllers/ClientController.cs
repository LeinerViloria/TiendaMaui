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
                .Where(x => x.Status)
                .ToList();
        }

        // GET api/<ClientController>/5
        [HttpGet("{email}")]
        public Client? Get(string email)
        {
            using var context = dbContextFactory.CreateDbContext();
            return context.Set<Client>()
                .AsNoTracking()
                .FirstOrDefault(x => x.Email.Contains(email) && x.Status);
        }

        // POST api/<ClientController>
        [HttpPost]
        public Client Post([FromBody] Client value)
        {
            using var context = dbContextFactory.CreateDbContext();

            value.Status = true;

            context.Add(value);
            context.SaveChanges();

            return value;
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{email}")]
        public bool Delete(string email)
        {
            using var context = dbContextFactory.CreateDbContext();

            var ClientHasPurchases = context.Set<Purchase>()
                .AsNoTracking()
                .Any(x => x.Client!.Email == email);
            
            int Result;
            var Client = context.Set<Client>()
                    .Where(x => x.Email == email);

            if(ClientHasPurchases)
                Result = Client.ExecuteUpdate(x => x.SetProperty(y=>y.Status, false));
            else
                Result = Client.ExecuteDelete();

            return Result > 0;
        }
    }
}
