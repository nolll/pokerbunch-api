using System.Collections.Generic;

namespace Infrastructure.Storage
{
    public class SqlParameters : List<SimpleSqlParameter>
    {
        public SqlParameters(params SimpleSqlParameter[] list)
        {
            foreach (var parameter in list)
            {
                Add(parameter);
            }
        }
    }
}