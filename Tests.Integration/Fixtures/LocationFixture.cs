using Api.Models.LocationModels;

namespace Tests.Integration.Fixtures;

public class LocationFixture(LocationModel resultModel)
{
    public string Id { get; } = resultModel.Id;
    public string Name { get; } = resultModel.Name;
    public string BunchId { get; } = resultModel.Bunch;
}