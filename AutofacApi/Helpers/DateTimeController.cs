using Microsoft.AspNetCore.Mvc;

namespace AutofacApi.Helpers;

[ApiController]
[Route("api/[controller]")]
public class DateTimeController : ControllerBase
{
    private readonly IDateTimeService _dateTimeService;

    public DateTimeController(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    [HttpGet]
    public DateTime Get()
    {
        return _dateTimeService.UtcNow();
    }
}