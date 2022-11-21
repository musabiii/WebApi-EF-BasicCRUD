# WebApi-EF-BasicCRUD
create`web api` project

add packages

```
Microsoft.EntityFrameworkCore
```

```
Microsoft.EntityFrameworkCore.Tools
```

```
Npgsql.EntityFrameworkCore.PostgreSQL
```

```
Microsoft.EntityFrameworkCore.SqlServer
```



add to `appsetting.json`

```json
"AllowedHosts": "*",
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=BasicCRUD;Username=postgres;password=super"
}
```



add to `program.cs` as second row

```c#
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
```



add `Data/ApplicationContext.cs`

```c#
using Microsoft.EntityFrameworkCore;

namespace BasicCRUD.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {

        }
    }
}
```



add service to `program.sc`

```c#
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //after this string
builder.Services.AddDbContext<ApplicationContext>(options=>options.UseNpgsql(connectionString));
```



add `Models/Student.cs`

```c#
using System.ComponentModel.DataAnnotations;

namespace BasicCRUD.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string? StudentName { get; set; }
        public string? Address { get; set; }
        public string? Standard { get; set; }
    }
}
```



for check connection add migration and remove it

nuget PM

```
add-migration 'initialMigratin'
```

remove migration

```
remove-migration
```



add `dbset` students to `ApplicationContext`

```c#
using BasicCRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicCRUD.Data
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {

        }

        public DbSet<Student>? Students { get; set; }
    }
}
```



create migration and update database

```
add-migration 'initialMigratin'
```

```
update-database
```



add `Controllers/StudentsController.cs`

```c#
using BasicCRUD.Data;
using BasicCRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public StudentsController(ApplicationContext context)
        {
            _context=context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var existingStudent = await _context.Students.FirstOrDefaultAsync(y => y.Id == id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.StudentName = student.StudentName;
            existingStudent.Address = student.Address;
            existingStudent.Standard = student.Standard;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            _context.SaveChangesAsync();
            return Ok();
        }
        
    }
}
```



