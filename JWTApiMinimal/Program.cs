var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 2. Agrega contoladores ./Controllers/*
builder.Services.AddControllers();

var app = builder.Build();


// 3. Habilitar Swagger en tiempo de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 4. Usa los controladores agregados en el paso 2
app.MapControllers();


app.Run();


