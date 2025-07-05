var builder = WebApplication.CreateBuilder(args);

// Leer la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL");

// Aquí puedes usarla, por ejemplo, con EF Core
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