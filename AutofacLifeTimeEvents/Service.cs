namespace AutofacLifeTimeEvents;

public class Service
{
    public Service()
    {
        Console.WriteLine("Constructor of Service executed");
    }
    
    public string GetText() => "Service GetText() executed";
}