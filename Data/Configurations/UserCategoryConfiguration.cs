using BlogAPIDotnet6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogAPIDotnet6.Data.Configurations;

public class UserCategoryConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder
            .HasMany(u => u.Categories) // um usuário tem muitas categorias
            .WithOne(p => p.User) // uma categoria pertence a um usuário
            .HasForeignKey(p => p.Username) // chave estrangeira em CategoryModel que liga a UserModel
            .OnDelete(DeleteBehavior.Cascade);
    }
}