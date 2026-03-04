using Api.Models.EventModels;

namespace Tests.Integration.Fixtures;

public class EventFixture(EventModel resultModel)
{
    public string Id { get; } = resultModel.Id;
    public string Name { get; } = resultModel.Name;
    public string BunchId { get; } = resultModel.BunchId;
}