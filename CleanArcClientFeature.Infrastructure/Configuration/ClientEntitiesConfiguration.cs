//using CleanArcClientFeature.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CleanArcClientFeature.Infrastructure.Configuration;

//public class ClientEntitiesConfiguration : IEntityTypeConfiguration<Client>
//{
//    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Client> builder)
//    {
//        builder.HasKey(c => c.Id);
       
//        builder.Property(c => c.NomeFantasia).HasMaxLength(100).IsRequired();
           
//        builder.Property(c => c.Cnpj).HasMaxLength(14).IsRequired();

//        builder.Property(c => c.Ativo).IsRequired();
//    }

//}
