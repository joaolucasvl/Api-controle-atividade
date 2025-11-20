using ControleFuncionarios.Services;
using brevo_csharp.Client;


var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var brevoApiKey = builder.Configuration["Brevo:ApiKey"];

if (!string.IsNullOrEmpty(brevoApiKey))
{
    Configuration.Default.ApiKey.Add("api-key", brevoApiKey);
}


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmailService, BrevoEmailService>();
builder.Services.AddControllers();



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
       
            policy.WithOrigins("http://localhost:5173",
                    "https://front-end-controle-atividades.netlify.app/")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();
app.Run();