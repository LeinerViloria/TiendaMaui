using Backend.Access;
using Backend.DTOS;
using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController(IDbContextFactory<TiendaContext> dbContextFactory, EmailService emailService) : ControllerBase
    {
        private readonly IDbContextFactory<TiendaContext> dbContextFactory = dbContextFactory;
        private readonly EmailService emailService = emailService;

        // POST api/<PurchaseController>
        [HttpPost]
        public IEnumerable<Purchase> Post([FromBody] List<PurchaseDTO> values)
        {

            if (values.Exists(x=>x.Cantidad <= 0))
                throw new DataException("Debe ingresar una cantidad válida");

            var Purchases = values.Select(x => new Purchase()
            {
                RowidCliente = x.RowidCliente,
                RowidProducto = x.RowidProducto,
                Cantidad = x.Cantidad,
            });

            var Client = Purchases.First().RowidCliente;

            if(Purchases.Any(x => x.RowidCliente != Client))
                throw new DataException("Sólo un cliente puede realizar esta compra");

            using var context = dbContextFactory.CreateDbContext();

            try
            {
                context.Database.BeginTransaction();

                var Products = values.Select(x => x.RowidProducto);

                var Items = context.Set<Product>()
                .Where(x => Products.Contains(x.Rowid))
                .ToList();

                var UnavailableProducts = Items.Where(x => x.Stock <= 0 || x.Stock - Purchases.First(y=>y.RowidProducto == x.Rowid).Cantidad < 0);

                if (UnavailableProducts.Any())
                    throw new DataException($"No hay items disponibles para: \n{string.Join('\n', UnavailableProducts.Select(x=>x.Name))}");

                context.AddRange(Purchases);
                context.SaveChanges();

                foreach (var Purchase in Purchases)
                {
                    var CurrentProduct = Items.First(x => x.Rowid == Purchase.RowidProducto);
                    CurrentProduct.Stock -= Purchase.Cantidad;
                }

                UnavailableProducts = Items.Where(x => x.Stock < 0);

                if (UnavailableProducts.Any())
                    throw new DataException($"No hay los items solicitados para: \n{string.Join('\n', UnavailableProducts.Select(x=>x.Name))}");

                context.SaveChanges();

                var Email = context.Set<Client>()
                    .AsNoTracking()
                    .Where(x => Client == x.Rowid)
                    .Select(x => x.Email)
                    .Single();

                var EmailSended = emailService.SendEmail("Compra realizada", "Realizaste tu compra", Email);

                if(!EmailSended)
                    throw new DataException("Error al enviar el email");

                context.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                context.Database.RollbackTransaction();
                throw new DataException(ex.Message);
            }

            return Purchases;
        }
    }
}
