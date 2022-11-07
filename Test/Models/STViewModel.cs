

using System.ComponentModel.DataAnnotations;

namespace Test.Models;

public class STViewModel
{
    [Required]
    public string ? StudentName { get; set; }
    [Required]
    public string ? StudentRoll { get; set; }
    [Required]
    public string? TeacherName { get; set; }
    [Required]
    public string? TeacherDepartment { get; set; }
}
