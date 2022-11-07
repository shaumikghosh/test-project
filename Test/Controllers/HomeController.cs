using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Test.Data;
using Test.Models;

namespace Test.Controllers;
public class HomeController : Controller
{
    private readonly SqlContext _sqlContext;
    private readonly MySQLContext _mySQLContext;
    private readonly IConfiguration _configuration;

    public HomeController(MySQLContext mySQLContext, SqlContext sqlContext, IConfiguration configuration)
    {
        _mySQLContext = mySQLContext;
        _sqlContext = sqlContext;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        var conn = _configuration.GetValue<string>("DataConnection");
        ViewData["DataConnection"] = conn;
        switch (conn)
        {
            case "MySQL":
                ViewData["StudentsOfMySQL"] = await _mySQLContext.Students!.ToListAsync();
                ViewData["TeacherOfMySQL"] = await _mySQLContext.Teachers!.ToListAsync();
                break;
            default:
                ViewData["StudentsOfSQL"] = await _sqlContext.Students!.ToListAsync();
                ViewData["TeacherOfSQL"] = await _sqlContext.Teachers!.ToListAsync();
                break;
        }
        
        return View();
    }

    public IActionResult Privacy()
    {
        var model = new DbViewModel { DbName = _configuration.GetValue<string>("DataConnection")};
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Privacy([FromForm] DbViewModel model)
    {
        string json = System.IO.File.ReadAllText(@"appsettings.json");
        dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json)!;
        jsonObj["DataConnection"] = model.DbName;
        string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
        System.IO.File.WriteAllText(@"appsettings.json", output);
        TempData["Message"] = $"You have successfully switched to {model.DbName} Database now";
        return RedirectToAction("Privacy", "Home");
    }

    public IActionResult AddRecord()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddRecord([FromForm] STViewModel model)
    {
        if (ModelState.IsValid)
        {
            var student = new Student
            {
                Name = model.StudentName,
                Roll = model.StudentRoll
            };
            var teacher = new Teacher
            {
                Name = model.TeacherName,
                Department = model.TeacherDepartment
            };
            await _mySQLContext.Students!.AddAsync(student);
            await _mySQLContext.Teachers!.AddAsync(teacher);
            await _mySQLContext.SaveChangesAsync();

            var student2 = new Student
            {
                Name = model.StudentName,
                Roll = model.StudentRoll
            };
            var teacher2 = new Teacher
            {
                Name = model.TeacherName,
                Department = model.TeacherDepartment
            };
            await _sqlContext.Students!.AddAsync(student2);
            await _sqlContext.Teachers!.AddAsync(teacher2);
            await _sqlContext.SaveChangesAsync();

            TempData["Message"] = "New record successfully created in both SQL and MySQL Database";

            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }


    public async Task<IActionResult> EditRecord(int id)
    {
        var studentFromSQL = await _sqlContext.Students!.FindAsync(id);
        var teacherFromSQL = await _sqlContext.Teachers!.FindAsync(id);
        var combinedData = new STViewModel
        {
            StudentName = studentFromSQL!.Name,
            StudentRoll = studentFromSQL.Roll,
            TeacherName = teacherFromSQL!.Name,
            TeacherDepartment = teacherFromSQL.Department
        };
        return View(combinedData);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRecord([FromForm] STViewModel model, int id)
    {
        if (ModelState.IsValid)
        {
            var student = await _mySQLContext.Students!.FindAsync(id);
            student!.Name = model.StudentName;
            student.Roll = model.StudentRoll;
            _mySQLContext.Students!.Update(student);

            var teacher = await _mySQLContext.Teachers!.FindAsync(id);
            teacher!.Name = model.TeacherName;
            teacher.Department = model.TeacherDepartment;
             
            _mySQLContext.Teachers!.Update(teacher);
            await _mySQLContext.SaveChangesAsync();

            var student2 = await _sqlContext.Students!.FindAsync(id);
            student2!.Name = model.StudentName;
            student2.Roll = model.StudentRoll;
            _sqlContext.Students!.Update(student2);

            var teacher2 = await _sqlContext.Teachers!.FindAsync(id);
            teacher2!.Name = model.TeacherName;
            teacher2.Department = model.TeacherDepartment;

            _sqlContext.Teachers!.Update(teacher2);
            await _sqlContext.SaveChangesAsync();

            TempData["Message"] = "Record successfully updated in both SQL and MySQL Database";

            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}