namespace Zaaby.AspNetCore.Formatters.Demo.Controllers;

[Route("api/[controller]/[action]")]
public class ValuesController : ControllerBase
{
    [HttpPost]
    public IEnumerable<TestDto> Post([FromBody] IEnumerable<TestDto> dtos) => dtos;
}