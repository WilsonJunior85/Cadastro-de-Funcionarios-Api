
using Funcionarios.CrossCutting;
using Funcionarios.Infra.Data.Context;
using Funcionarios.Infra.Services.FileService.Settings;
using Funcionarios.Infra.Services.FileService;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Funcionarios.Api.Extensions;
using SCC.Api.Extensions;
using SCC.Api.Settings;

try
{
    Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();

    var builder = WebApplication.CreateBuilder(args);

    var conf = builder.ConfigureAppSettings();

    builder.Host.ConfigureSerilog();

    var identity = new IdentitySettings();
    builder.Configuration.Bind("IdentitySettings", identity);
    builder.Services.AddSingleton(identity);

    var configFile = new FileConf();
    builder.Configuration.GetSection("FileConf").Bind(configFile);
    builder.Services.AddSingleton(configFile);
    builder.Services.AddScoped<UploadService>();
    builder.Services.AddScoped<DownloadService>();

    var services = new List<ApiServicesSettings>();
    builder.Configuration.Bind("Services", services);
    builder.Services.AddSingleton(services);

    builder.Services.AddHttpContextAccessor();

    var connectionString = builder.Configuration.GetConnectionString("ApiConnection");

    builder.Services.AddDbContext<ApiContext>(options =>
            options.UseSqlServer(connectionString, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName)));

    builder.Services.AddControllers(opt =>
    {
        opt.Filters.Add(new AuthorizeFilter("scopes"));
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonRelaxedBooleanStringConverter());
    });


    builder.Services.ConfigureCors();
    builder.Services.ConfigureAuthorization(identity);
    builder.Services.ConfigureAuthentication(identity);
    builder.Services.SuppressModelStateInvalid();


    builder.Services.BootstrapDI(conf);


    // Habilitar CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend",
            policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
    });


    var info = new OpenApiInfo();
    builder.Configuration.Bind("swaggerInfo", info);

    builder.Services.ConfigureSwagger(info);

    var app = builder.Build();


    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    app.UseStaticFiles();

    //app.UseCors("CorsPolicy");
    app.UseCors("AllowFrontend");
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers().AllowAnonymous();
        endpoints.MapSwagger("swagger/{documentName}/swagger.json");
    });
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Intranet.Api v1"));

    IdentityModelEventSource.ShowPII = true;

    app.Run();
}
catch (Exception ex) when (ex.GetType().Name is not "StopTheHostException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}








//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
