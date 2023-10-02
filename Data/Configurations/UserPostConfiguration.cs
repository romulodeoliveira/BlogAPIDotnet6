using BlogAPIDotnet6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPIDotnet6.Data.Configurations;

public class UserPostConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder
            .HasMany(u => u.Posts) // um usuário tem muitos posts
            .WithOne(p => p.User) // um post pertence a um usuário
            .HasForeignKey(p => p.Username) // chave estrangeira em PostModel que liga a UserModel
            .OnDelete(DeleteBehavior.Cascade);
    }
}