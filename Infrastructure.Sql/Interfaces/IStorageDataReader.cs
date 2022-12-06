namespace Infrastructure.Sql.Interfaces;

public interface IStorageDataReader
{
    string GetStringValue(string key);
    int GetIntValue(string key);
}