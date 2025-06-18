using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;


public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }
    DbSet<RebootRequest> rebootRequests
    {
        get;
        set;
    }
    DbSet<ConnectionTestModel> connectionTestModels
    {
        get;
        set;
    }
    DbSet<ServerHealth> serverHealths
    {
        get;
        set;
    }
    DbSet<MemoryProcessInfo> MemoryProcessInfos
    {
        get;
        set;
    }
    DbSet<CpuProcessInfo> cpuProcessInfos
    {
        get;
        set;
    }
    DbSet<SystemInfo> systemInfos
    {
        get;
        set;
    }
    DbSet<DiskInfoModel> diskInfos
    {
        get;
        set;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<SystemInfo>()
        .HasKey(x => new { x.dateTime, x.ServerName });
        modelBuilder.Entity<CpuProcessInfo>()
            .HasOne(pi => pi.systemInfo)  // Specify the navigation property
            .WithMany()                   // Assuming SystemInfo does not have a collection of ProcessInfo (or it's not defined in your model)
            .HasForeignKey(pi => new { pi.date, pi.ServerName });
        modelBuilder.Entity<CpuProcessInfo>()
        .HasKey(x => new { x.date, x.ServerName, x.PID,  });
        modelBuilder.Entity<MemoryProcessInfo>()
            .HasOne(pi => pi.systemInfo)  // Specify the navigation property
            .WithMany()                   // Assuming SystemInfo does not have a collection of ProcessInfo (or it's not defined in your model)
            .HasForeignKey(pi => new { pi.date, pi.ServerName });
        modelBuilder.Entity<MemoryProcessInfo>()
        .HasKey(x => new { x.date, x.ServerName,x.PID });
    }


}
