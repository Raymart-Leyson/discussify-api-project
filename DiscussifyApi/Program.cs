using System.Reflection;
using DiscussifyApi.Context;
using DiscussifyApi.Repositories;
using DiscussifyApi.Services;
using DiscussifyApi.Hubs;
using DiscussifyApi.Middlewares;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

app.UseCors("CorsPolicy");

app.UseMiddleware<AuthMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MessageHub>("/ws/rooms/{id}/messages");

app.Run();

// Add services to the container.
void ConfigureServices(IServiceCollection services)
{
    services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });

    services.AddSignalR();

    // Controller support
    services.AddControllers().ConfigureApiBehaviorOptions(x => { x.SuppressMapClientErrors = true; });

    services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["Jwt:ISSUER"],
                ValidAudience = builder.Configuration["Jwt:AUDIENCE"],
                IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:ACCESS_SECRET_KEY"] ?? string.Empty)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

    services.AddAuthorization();
    
    services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

    // Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        // Add header documentation in swagger
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Discussify App (Discussify API)",
            Description = "An online platform for teachers and students to engage in open and honest discussions without the fear of being judged. Teachers can post questions and students can anonymously comment on them. This promotes a safe and inclusive environment where students can freely express their thoughts and perspectives.",
        });

        options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();

        // Feed generated xml api docs to swagger
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    // Configure Automapper
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Our services, interface, or DB Contexts that we want to inject
    services.AddTransient<DapperContext>();
    services.AddScoped<ITokenService, TokenService>();
    services.AddScoped<IAnonymousService, AnonymousService>();
    services.AddScoped<IAnonymousRepository, AnonymousRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IRoomRepository, RoomRepository>();
    services.AddScoped<IRoomService, RoomService>();
    services.AddScoped<IMessageRepository, MessageRepository>();
    services.AddScoped<IMessageService, MessageService>();
}