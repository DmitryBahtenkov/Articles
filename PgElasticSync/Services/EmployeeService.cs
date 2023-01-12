using Microsoft.EntityFrameworkCore;
using Nest;
using PgElasticSync.Elastic;
using PgElasticSync.Models;
using PgElasticSync.Pg;

namespace PgElasticSync.Services;

public class EmployeeService
{
    private readonly PgContext _pgContext;
    private readonly ElasticRepository _elasticRepository;
    private readonly RedisService _redis;

    public EmployeeService(PgContext pgContext, ElasticRepository elasticRepository, RedisService redis)
    {
        _pgContext = pgContext;
        _elasticRepository = elasticRepository;
        _redis = redis;
    }

    public async Task<Employee> Create(Employee employee)
    {
        var result = await _pgContext.AddAsync(employee);
        await _pgContext.SaveChangesAsync();
        await _redis.Publish("create", new IndexEmployeeEvent(result.Entity));

        return result.Entity;
    }

    public async Task<Employee?> ById(int id)
    {
        return await _pgContext.Employees.FindAsync(id);
    }

    public async Task<Employee?> Update(Employee employee)
    {
        var result = _pgContext.Employees.Update(employee);
        await _pgContext.SaveChangesAsync();
        await _redis.Publish("update", new IndexEmployeeEvent(result.Entity));

        return result.Entity;
    }

    public async Task<Employee> Delete(int id)
    {
        var employee = await ById(id);
        if (employee is null)
        {
            throw new Exception("Not found");
        }

        var result = _pgContext.Remove(employee);
        await _pgContext.SaveChangesAsync();
        await _elasticRepository.DeleteDocument(result.Entity);

        return result.Entity;
    }

    public async Task<List<Employee>> All()
    {
        return await _pgContext.Employees.ToListAsync();
    }

    public async Task<List<Employee>> Search(
        string text, 
        string? city = null,
        string? university = null,
        DateTime? fromStartDate = null,
        bool useElastic = false)
    {
        if (useElastic)
        {
            var q = new QueryContainerDescriptor<Employee>();
            var queryContainer = q
                .Fuzzy(f => f
                    .Field(ff => ff.FirstName)
                    .Value(text)) | 
                q.Fuzzy(f => f
                    .Field(ff => ff.LastName)
                    .Value(text));

            if (!string.IsNullOrEmpty(city))
            {
                queryContainer &= q.MatchPhrase(x => x
                                        .Field(f => f.City)
                                        .Query(city));
            }
            
            if (!string.IsNullOrEmpty(university))
            {
                queryContainer &= q.MatchPhrase(x => x
                                        .Field(f => f.University)
                                        .Query(university));
            }

            if (fromStartDate.HasValue)
            {
                queryContainer &= q.DateRange(x => x
                    .Field(f => f.StartWorkingDate)
                    .GreaterThanOrEquals(fromStartDate));
            }

            var searchDescriptor = new SearchDescriptor<Employee>()
                .Query(_ => queryContainer);

            return await _elasticRepository.Search(searchDescriptor);
        }

        var query = _pgContext.Employees.Where(x => x.Email == text
                                                    || text.Contains(x.FirstName)
                                                    || text.Contains(x.LastName)
                                                    || text.Contains(x.MiddleName ?? "empty")
                                                    || x.PhoneNumber == text
        );

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(x => x.City == city);
        }

        if (!string.IsNullOrEmpty(university))
        {
            query = query.Where(x => x.University == university);
        }

        if (fromStartDate.HasValue)
        {
            query = query.Where(x => x.StartWorkingDate >= fromStartDate.Value);
        }

        return await query.ToListAsync();
    }
}