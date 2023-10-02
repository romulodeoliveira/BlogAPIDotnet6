using BlogAPIDotnet6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPIDotnet6.Data.Configurations;

public class UserAddressConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder
            .HasOne(u => u.Address)
            .WithOne(a => a.User)
            .HasForeignKey<UserModel>(a => a.AddressId) // Chave estrangeira para AddressModel
            .IsRequired(false);
    }
}