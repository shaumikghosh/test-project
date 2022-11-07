using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Test.Models;

public class Teacher
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "NVARCHAR(250)")]
    public string? Name { get; set; }

    [Column(TypeName = "NVARCHAR(250)")]
    public string ? Department { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
