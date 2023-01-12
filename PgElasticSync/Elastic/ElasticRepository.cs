using Nest;
using PgElasticSync.Models;

namespace PgElasticSync.Elastic;

public class ElasticRepository
{
    // наименование индекса в elastic
    private const string IndexName = "employee-index";
    // объект клиента к elasticsearch, через который будем выполнять все запросы
    private readonly ElasticClient _elasticClient;

    public ElasticRepository(ElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task IndexDocument(Employee employee)
    {
        await _elasticClient.IndexAsync(employee, request => request.Index(IndexName));
    }

    public async Task<List<Employee>> Search(SearchDescriptor<Employee> searchDescriptor)
    {
        
        var response = await _elasticClient.SearchAsync<Employee>(searchDescriptor.AllIndices());
        return response.Hits.Select(x => x.Source).ToList();
    }

    public async Task UpdateDocument(Employee employee)
    {
        await _elasticClient.IndexAsync(employee, x => x
            .Index(IndexName) 
            .Id(employee.Id));
    }

    public async Task DeleteDocument(Employee employee)
    {
        await _elasticClient.DeleteAsync(DocumentPath<Employee>.Id(employee), x => x.Index(IndexName));
    }
}