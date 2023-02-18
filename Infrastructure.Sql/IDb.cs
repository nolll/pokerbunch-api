using JetBrains.Annotations;
using SqlKata;

namespace Infrastructure.Sql;

public interface IDb : IDisposable
{
    DbEngine Engine { get; }
    Task<T?> Single<T>(string sql, object @params);
    Task<T?> Single<T>(Query query);
    Task<IEnumerable<T>> List<T>(string sql, object? @params = null);
    Task<IEnumerable<T>> List<T>(string sql, ListParam param);
    Task<int> Execute(string sql, object? @params = null);
    Task<int> Insert(string sql, object? @params = null);
}