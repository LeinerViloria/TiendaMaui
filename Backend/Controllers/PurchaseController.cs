using Backend.Access;
using Backend.Authorization;
using Backend.DTOS;
using Backend.Entities;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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

            var User = (UserDTO) HttpContext.Items["TokenValidationResult"]!;
            IEnumerable<Purchase> Purchases = Enumerable.Empty<Purchase>();

            using var context = dbContextFactory.CreateDbContext();

            try
            {
                context.Database.BeginTransaction();

                var Products = values.Select(x => x.RowidProducto);

                var Items = context.Set<Product>()
                .Where(x => Products.Contains(x.Rowid))
                .ToList();

                var UnavailableProducts = Items.Where(x => x.Stock <= 0 || x.Stock - values.First(y=>y.RowidProducto == x.Rowid).Cantidad < 0);

                if (UnavailableProducts.Any())
                    throw new DataException($"No hay items disponibles para: \n{string.Join('\n', UnavailableProducts.Select(x=>x.Name))}");

                var Client = context.Set<Client>()
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Email == User.Email) ?? new()
                    {
                        Email = User.Email,
                        Status = true
                    };
                
                context.Attach(Client);
                context.Entry(Client);

                context.SaveChanges();

                Purchases = values.Select(x => new Purchase()
                {
                    RowidCliente = Client.Rowid,
                    RowidProducto = x.RowidProducto,
                    Cantidad = x.Cantidad,
                });

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

                var EmailSended = emailService.SendEmail("Compra realizada", "Realizaste tu compra", User.Email);

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
