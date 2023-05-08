namespace AutofacApi.Contract;

public interface IRepository<TModel>
{
    Task<TModel> Create(TModel model);
    Task<List<TModel>> GetAll();
    Task<TModel> GetById(int id);
    Task<TModel> Update(TModel model);
    Task Delete(int id);
}
