using Core.Entities;
using Core.UseCases;
using NUnit.Framework;

namespace Tests.Core.UseCases.ClearCacheTests
{
    public class WithAdmin : Arrange
    {
        protected override Role Role => Role.Admin;

        [Test]
        public void ClearsCache()
        {
            Sut.Execute(new ClearCache.Request(UserName));
        }
    }
}