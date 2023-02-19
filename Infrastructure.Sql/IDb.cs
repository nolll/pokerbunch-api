using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Infrastructure.Sql;

public interface IDb : IDisposable
{
    DbEngine Engine { get; }
    QueryFactory QueryFactory { get; }
    Compiler Compiler { get; }
    Task<T?> Single<T>(string sql, object @params);
    Task<IEnumerable<T>> List<T>(string sql, object? @params = null);
    Task<IEnumerable<T>> List<T>(string sql, ListParam param);
    Task<int> Execute(string sql, object? @params = null);
    Task<int> Insert(string sql, object? @params = null);
}