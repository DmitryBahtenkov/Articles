namespace AutofacApi.Helpers;

public interface IDateTimeService
{
    public DateTime UtcNow();
}

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow()
    {
        return DateTime.UtcNow;
    }
}

public class StaticDateTimeService : IDateTimeService
{
    private readonly DateTime _staticTime;

    public StaticDateTimeService(DateTime staticTime)
    {
        _staticTime = staticTime;
    }

    public DateTime UtcNow()
    {
        return _staticTime;
    }
}