using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RaceDay.API.Models;
using System.Collections.Generic;
namespace RaceDay.API.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Enrolment> Enrolments => Set<Enrolment>();
    public DbSet<Result> Results => Set<Result>();
}
