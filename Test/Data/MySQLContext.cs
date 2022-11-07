using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Data;

public class MySQLContext : DbContext
{
    public MySQLContext(DbContextOptions<MySQLContext> options) : base(options){}

    public DbSet<Student> ? Students { get; set; }
    public DbSet<Teacher> ? Teachers { get; set; }
}
