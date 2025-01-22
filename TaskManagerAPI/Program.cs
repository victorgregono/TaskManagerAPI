//using TaskManagerAPI.Configurations;
//using TaskManagerAPI.Extensions;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//builder.Services.AddDatabaseConfig(builder.Configuration);
//builder.Services.AddAutoMapperConfig();
//builder.Services.AddDependencyInjectionConfig();


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.UseExceptionHandlerMiddleware();

//app.MapControllers();

//app.Run();




//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;
//using TaskManagerAPI.Configurations;
//using TaskManagerAPI.Extensions;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//builder.Services.AddDatabaseConfig(builder.Configuration);
//builder.Services.AddAutoMapperConfig();
//builder.Services.AddDependencyInjectionConfig();

//// Configurar políticas de autorização
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRoles.ADMIN));
//    options.AddPolicy("UserPolicy", policy => policy.RequireRole(UserRoles.USER));
//});



//builder.Services.AddHttpContextAccessor();


//var app = builder.Build();

//// Middleware para verificar o cabeçalho personalizado
//app.Use(async (context, next) =>
//{
//    if (context.Request.Headers.TryGetValue("X-User-Role", out var role))
//    {
//        var claims = new List<Claim> { new Claim(ClaimTypes.Role, role) };
//        var identity = new ClaimsIdentity(claims, "Custom");
//        context.User = new ClaimsPrincipal(identity);
//    }

//    await next.Invoke();
//});

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.UseExceptionHandlerMiddleware();

//app.MapControllers();

//app.Run();

//// Definição das roles
//public static class UserRoles
//{
//    public const string ADMIN = "Admin";
//    public const string USER = "User";
//}



using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using TaskManager.Data;
using TaskManagerAPI;
using TaskManagerAPI.Configurations;
using TaskManagerAPI.Extensions;
using TaskManagerAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDatabaseConfig(builder.Configuration);
builder.Services.AddAutoMapperConfig();
builder.Services.AddDependencyInjectionConfig();

// Registrar IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configurar autenticação baseada em cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

// Configurar políticas de autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

var app = builder.Build();

// Middleware para verificar o cabeçalho personalizado
app.UseUserRoleMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandlerMiddleware();

app.MapControllers();

// Inicializar o banco de dados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FactoryContext>();
    context.InitializeDatabaseAsync().Wait();
}

app.Run();
