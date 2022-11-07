using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Data;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options) : base(options) { }

    public DbSet<Student> ? Students { get; set; }
    public DbSet<Teacher> ? Teachers { get; set; }
}
