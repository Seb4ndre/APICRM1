var builder = WebApplication.CreateBuilder(args);

// Leer la cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL");

// Aqu� puedes usarla, por ejemplo, con EF Core
// builder.Services.AddDbContext<TuContexto>(options =>
//     options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();