using Backend.Access;
using Backend.DTOS;
using Backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController(IDbContextFactory<TiendaContext> dbContextFactory) : ControllerBase
    {
        private readonly IDbContextFactory<TiendaContext> dbContextFactory = dbContextFactory;

        // POST api/<PurchaseController>
        [HttpPost]
        public Purchase Post([FromBody] PurchaseDTO value)
        {
            if (value.Cantidad <= 0)
                throw new DataException("Debe ingresar una cantidad válida");

            using var context = dbContextFactory.CreateDbContext();

            var Purchase = new Purchase()
            {
                RowidCliente = value.RowidCliente,
                RowidProducto = value.RowidProducto,
                Cantidad = value.Cantidad,
            };

            try
            {
                context.Database.BeginTransaction();

                var AvailableItems = context.Set<Product>()
                .AsNoTracking()
                .Where(x => x.Rowid == value.RowidProducto)
                .Select(x => x.Stock)
                .Single();

                if (AvailableItems <= 0)
                    throw new DataException("No hay items disponibles");

                if (AvailableItems - value.Cantidad < 0)
                    throw new DataException("No hay items disponibles");

                context.Add(Purchase);
                context.SaveChanges();

                var Product = context.Set<Product>()
                    .Where(x => x.Rowid == value.RowidProducto)
                    .First();

                Product.Stock --;

                if(Product.Stock < 0)
                    throw new DataException("No hay items disponibles");

                context.SaveChanges();

                context.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                context.Database.RollbackTransaction();
                throw new DataException(ex.Message);
            }

            return Purchase;
        }
    }
}
