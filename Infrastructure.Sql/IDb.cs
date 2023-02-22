using SqlKata.Execution;

namespace Infrastructure.Sql;

public interface IDb : IDisposable
{
    DbEngine Engine { get; }
    QueryFactory QueryFactory { get; }
    Task<int> Execute(string sql, object? @params = null);
}