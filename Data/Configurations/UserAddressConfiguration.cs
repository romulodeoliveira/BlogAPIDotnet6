using BlogAPIDotnet6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPIDotnet6.Data.Configurations;

public class UserAddressConfiguration : IEntityTypeConfiguration<AddressModel>
{
    public void Configure(EntityTypeBuilder<AddressModel> builder)
    {
        // Configurações do relacionamento e mapeamento aqui
        builder
            .HasOne(e => e.User)
            .WithOne(e => e.Address)
            .HasForeignKey<AddressModel>(e => e.Username)
            .IsRequired(false);
    }
}