using BlogAPIDotnet6.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPIDotnet6.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<PostModel> Posts { get; set; }
    public DbSet<CommentModel> Comments { get; set; }
    public DbSet<AddressModel> Addresses { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }
}