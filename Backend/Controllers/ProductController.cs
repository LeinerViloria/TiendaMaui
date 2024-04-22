using Backend.Access;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using Backend.Entities;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController (IDbContextFactory<TiendaContext> dbContextFactory) : ControllerBase
    {
        private readonly IDbContextFactory<TiendaContext> dbContextFactory = dbContextFactory;

        // GET: api/<ProductController>
        [HttpGet]
        public List<Product> Get()
        {
            using var context = dbContextFactory.CreateDbContext();
            return context.Set<Product>()
                .AsNoTracking()
                .ToList();
        }

        // GET api/<ProductController>/5
        [HttpGet("{Name}")]
        public Product? Get(string Name)
        {
            using var context = dbContextFactory.CreateDbContext();
            return context.Set<Product>()
                .AsNoTracking()
                .FirstOrDefault(x => x.Name.Contains(Name));
        }

        // POST api/<ProductController>
        [HttpPost]
        public Product Post([FromBody] Product value)
        {
            using var context = dbContextFactory.CreateDbContext();
            context.Add(value);
            context.SaveChanges();

            return value;
        }

        // POST api/<ProductController>
        [HttpPost("addRange")]
        public Product[] AddRange([FromBody] Product[] value)
        {
            using var context = dbContextFactory.CreateDbContext();
            context.AddRange(value);
            context.SaveChanges();

            return value;
        }

        // PUT api/<ProductController>/5
        [HttpPut("{rowid}")]
        public Product Put(int rowid, [FromBody] Product value)
        {
            using var context = dbContextFactory.CreateDbContext();
            context.Set<Product>()
                .Where(x => x.Rowid == rowid)
                .ExecuteUpdate(x => x.SetProperty(y => y.Name, value.Name)
                    .SetProperty(y=>y.RutaImagen, value.RutaImagen)
                    .SetProperty(y=>y.Precio, value.Precio));

            return context.Set<Product>()
                 .AsNoTracking()
                .First(x => x.Rowid == rowid);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{rowid}")]
        public bool Delete(int rowid)
        {
            using var context = dbContextFactory.CreateDbContext();
            var result = context.Set<Product>()
                .Where(x => x.Rowid == rowid)
                .ExecuteDelete();
            return result > 0;
        }
    }
}
