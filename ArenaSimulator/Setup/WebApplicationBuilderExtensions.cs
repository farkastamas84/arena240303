using ArenaSimulator.Exceptions;
using ArenaSimulator.Logic;
using ArenaSimulator.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ArenaSimulator.Setup;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.Configure<SimulatorConfig>(builder.Configuration.GetSection("Simulator"));

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        builder.Services.AddSingleton<IRandomGenerator, RandomGenerator>();

        builder.Services.AddTransient<IHeroFactory, HeroFactory>();
        builder.Services.AddTransient<IArenaGenerator, ArenaGenerator>();
        builder.Services.AddTransient<IBattleSimulator, BattleSimulator>();
        builder.Services.AddTransient<IRoundStrategy, RoundStrategy>();
        builder.Services.AddTransient<IFightSolver, FightSolver>();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();
        });
    }

    public static void CustomizeProblemDetails(ProblemDetailsContext context)
    {
        var status = context.Exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            BattleException => StatusCodes.Status500InternalServerError,
            NotImplementedException => StatusCodes.Status501NotImplemented,
            _ => context.ProblemDetails.Status
        };

        context.ProblemDetails.Status = status;
    }
}