using CollegeApp.Data;
using CollegeApp.MyLogging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Debugging;

//"WebApplication.CreateBuilder" Adds the Logging Providers
//There are 4 loggers.out of 4 loggers by default 1.Console and 2. Debug both loggers will start working 
var builder = WebApplication.CreateBuilder(args);

//Built-In Logger which is coming from "WebApplication.CreateBuilder"
#region Built-In Logger
/*
//If we want to limit the loggers out of all the build in loggers
builder.Logging.ClearProviders(); //This line of code will remove the all the loggers
builder.Logging.AddConsole(); //Only console logger will work
builder.Logging.AddDebug(); //Only Debug logger will work
//when we add both. both of them will be work
*/
#endregion

//Log4Net Logger
#region Log4Net Logger  - Third party Logger
//We have to add Log4Net.config file into the project to configure the Log4Net logger
//uncomment both lines in below to execute Log4Net
/*
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();
*/
#endregion

//SeriLog Logger
#region SeriLog Logger  - Third party Logger
/*
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Minute)
    .CreateLogger();

//Use this line to override the built-in loggers (only SeriLog will start to work)
//builder.Host.UseSerilog();
//when we need to Start work seriLog with Built-In logger 
builder.Logging.AddSerilog();
*/
#endregion


//SQL Server Services
//Hard coding the DB connection string is the not best practice
/*
builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseSqlServer("Data Source=JR\\SQLEXPRESS;Initial Catalog=StudentDB;Integrated Security=True;Trust Server Certificate=True"));
*/

//Connection string has added in appsettings.json Configuration file
builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection")));


// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//2. Loosely Coupled Technique
/*Here we need to give Interface name  and Instance name which we need to create.
 * Here we are saying whenever this interface is used inside the constructor parameter
 then what is the type of instance we need to pass(eg: LogToDB, LogToFile, and LogToServerMemory
 
 *There are 3 methods for DI
  1.AddScoped       2.AddSingleton      3.AddTransient
 */
builder.Services.AddScoped<IMyLogger, LogToDB >();
//builder.Services.AddSingleton<IMyLogger, LogToDB>();
//builder.Services.AddTransient<IMyLogger, LogToDB>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
