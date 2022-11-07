using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models;

public class Student
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "NVARCHAR(250)")]
    public string ? Name { get; set; }

    [Column(TypeName = "NVARCHAR(250)")]
    public string ? Roll { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
