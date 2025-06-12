using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Extra. SI QUIERES AGREGAR EL BOTON DE AUTHORIZE USA ESTA FUNCION: (La configuracion es aparte)
// builder.Service.AddSwaggerGen( .... Configuracion ...)

// 2. Agrega contoladores ./Controllers/*
builder.Services.AddControllers();

// 3. Agregar configuracion de autenticacion basica
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], //Esto se obtine de la configuracion de tu ./JWTApiMinimal/appsettings.json
            ValidAudience = builder.Configuration["Jwt:Audience"], //Esto se obtine de la configuracion de tu ./JWTApiMinimal/appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)) //Esto se obtine de la configuracion de tu ./JWTApiMinimal/appsettings.json
        };

});


var app = builder.Build();

// 4. Usar el autenticador
app.UseAuthentication();

// 3. Habilitar Swagger en tiempo de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 4. Usa los controladores agregados en el paso 2
app.MapControllers();


app.Run();


