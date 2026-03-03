using Xunit.Sdk;
using Xunit.v3;

namespace Tests.Integration;

public class TestSorter : ITestCaseOrderer
{
    public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases) where TTestCase : notnull, ITestCase
    {
        return testCases.OrderBy(o =>
        {
            var orderAttribute = ((IXunitTestCase)o).TestMethod.Method.GetCustomAttributes(typeof(OrderAttribute), false)
                .FirstOrDefault();
            return (orderAttribute as OrderAttribute)?.SortOrder ?? int.MaxValue;
            
        }).ToList();
    }
}
