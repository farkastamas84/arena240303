using ArenaSimulator.Mapping;
using ArenaSimulator.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArenaSimulator.UnitTests;

public abstract class ControllerTestBase
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly IMapper _mapper;

    protected ControllerTestBase()
    {
        var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Arena")
            .Options;

        _dbContext = new ApplicationDbContext(dbContextOptions);

        var myProfile = new MappingProfile();
        var mapperConfiguration = new MapperConfiguration(cfg =>
            cfg.AddProfile(myProfile));

        _mapper = new Mapper(mapperConfiguration);
    }
}