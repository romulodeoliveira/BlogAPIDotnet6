using BlogAPIDotnet6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPIDotnet6.Data.Configurations;

public class UserAddressConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder
            .HasOne(e => e.Address)
            .WithOne(e => e.User)
            .HasForeignKey<UserModel>(e => e.AddressId)
            .IsRequired(false);
    }
}