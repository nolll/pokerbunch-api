using Xunit;

namespace Tests.Integration.Tests;

[Collection(nameof(TestSetup))]
[TestCaseOrderer(typeof(TestSorter))]
public partial class IntegrationTests(TestSetup fixture);
