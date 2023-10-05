using BlogAPIDotnet6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogAPIDotnet6.Data.Configurations;

public class CategoryPostConfiguration : IEntityTypeConfiguration<CategoryModel>
{
    public void Configure(EntityTypeBuilder<CategoryModel> builder)
    {
        builder
            .HasMany(c => c.Posts)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull); // atribui o valor null à post quando este for deletado
    }
}