using Microsoft.EntityFrameworkCore;
using Backend.Access;
using Backend.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         builder =>
//         {
//             builder.AllowAnyOrigin()
//                     .AllowAnyMethod()
//                     .AllowAnyHeader();
//         });
// });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>{
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
});
builder.Services.AddScoped<EmailService>();

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
// app.UseRouting();
// app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();
