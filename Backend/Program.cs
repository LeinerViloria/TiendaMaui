using Microsoft.EntityFrameworkCore;
using Backend.Access;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var ConnectionString = builder.Configuration.GetSection("ConnectionString").Get<string>();

Action<IServiceProvider, DbContextOptionsBuilder> DbContextOptions = (sp, options) => {
    options.UseNpgsql(ConnectionString);
};

builder.Services.AddDbContextFactory<TiendaContext>(DbContextOptions, ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
