using Microsoft.AspNetCore.Mvc;
using WebAPI.Builder;
using WebAPI.Entities;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DynamicQueryController : ControllerBase
{
    private readonly static List<User> Users = new()
    {
        new User { Id = 1, Name = "Ömer Furkan", BirthDate = DateTime.Now.AddYears(-30), Phone = "05554675825", Surname = "Doğruyol" },
        new User { Id = 2, Name = "Mehmet", BirthDate = DateTime.Now.AddYears(-12), Phone = "05554675825", Surname = "Hekimoğlu" },
        new User { Id = 2, Name = "Ayhan", BirthDate = DateTime.Now.AddYears(-35), Phone = "05554675825", Surname = "Akif" },
        new User { Id = 2, Name = "Selim", BirthDate = DateTime.Now.AddYears(-32), Phone = "024365897456", Surname = "Şentürk" }
    };
    [HttpPost]
    public IActionResult Post([FromBody] QueryPropertyDto queryBuilder)
    {

        var resultQuery = PredicateBuilder<User>.Predicate(queryBuilder);
        var result = Users.Where(resultQuery.Compile()).ToList();
        return Ok(result);
    }
}
