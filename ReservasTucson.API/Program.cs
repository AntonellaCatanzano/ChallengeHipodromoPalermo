using ReservasTucson.API.Support;
using ReservasTucson.DataAccess.Support;
using ReservasTucson.Domain.Support;
using ReservasTucson.Repositories.Support;
using ReservasTucson.Services.Support;
using ReservasTucson.Authentication.Support;
using NLog;
using NLog.Web;
using ReservasTucson.Repositories.Interfaces;
using ReservasTucson.Repositories.Seeds;

var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

logger.Debug("Iniciado");

var builder = WebApplication.CreateBuilder(args);

// ---------- LOGGING ----------
builder.Services.AddReservasTucsonLogger(builder.Configuration);

// ---------- CORS ----------
builder.Services.AddReservasTucsonCors();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ---------- AUTH JWT ----------
builder.Services.AddCustomizedAuthentication(builder.Configuration);

// ---------- DATABASE (SQL o InMemory) ----------
var persistence = builder.Configuration["Persistence"];

if (persistence == "InMemory")
    builder.Services.AddInMemoryDatabaseForTesting();
else
    builder.Services.AddCustomizedDatabase(builder.Configuration);
/* En appsettings.json colocar
 *{
 * "Persistence": "Sql" o "InMemory"
 *}
*/
// ---------- DOMAIN ----------
builder.Services.AddEntitiesMappings();

// ---------- REPOSITORIES ----------
builder.Services.AddRepositories();

// ---------- SERVICES ----------
builder.Services.AddServices();

// ---------- SWAGGER ----------
builder.Services.AddCustomizedSwagger();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ---------- Logging providers ----------
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var usuarioRepository = scope.ServiceProvider
        .GetRequiredService<IUsuarioRepository>();

    await UsuarioSeed.SeedAsync(usuarioRepository);
}

// ---------- SWAGGER ----------
app.UseReservasTucsonSwagger();

// ---------- HTTPS ----------
app.UseHttpsRedirection();

// ---------- CORS ----------
app.UseRouting();
app.UseCors();

// ---------- AUTH ----------
app.UseAuthentication();
app.UseAuthorization();

// ---------- ROUTING ----------
app.MapControllers();

// ---------- RUN ----------
app.Run();


