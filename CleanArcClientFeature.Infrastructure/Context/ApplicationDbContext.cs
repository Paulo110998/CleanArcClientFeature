//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;
//using CleanArcClientFeature.Domain.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace CleanArcClientFeature.Infrastructure.Context;

//public class ApplicationDbContext : DbContext
//{
//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//        : base(options)
//    {
//    }

//    public DbSet<Client> Clients { get; set; }

//    protected override void OnModelCreating(ModelBuilder builder)
//    {
//        base.OnModelCreating(builder);

//        builder.ApplyConfigurationsFromAssembly(
//            typeof(ApplicationDbContext).Assembly
//        );
//    }
//}
