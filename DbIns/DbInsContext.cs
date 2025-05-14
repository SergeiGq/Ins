using DbIns.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DbIns;

public class DbInsContext :IdentityDbContext<User>
{
    private readonly bool _configure;
    public DbInsContext()
    {

    }
    public DbInsContext(DbContextOptions options): base(options) 
    {
        _configure = true;
    }
    public DbSet<Client>? Clients { get; set; }
    public DbSet<Address>? Addresses { get; set; }
    public DbSet<Agreement>? Agreements { get; set; }
    public DbSet<Animal>? Animals { get; set; }
    public DbSet<Apartment>? Apartments { get; set; }
    public DbSet<Home>? Homes { get; set; }
    public DbSet<City>? Cities { get; set; }
    public DbSet<InsCompany>? InsCompanies { get; set; }
    public DbSet<InsPerson>? InsPersons { get; set; }
    public DbSet<InsType>? InsTypes { get; set; }
    public DbSet<LicenseAuto>? LicenseAutos { get; set; }
    public DbSet<Transport>? Transports { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!_configure)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=Acab1532;Database=Ins");
        }

    }
}