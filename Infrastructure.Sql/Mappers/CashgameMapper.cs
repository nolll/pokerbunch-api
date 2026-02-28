using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class CashgameMapper
{
    internal static Cashgame ToCashgame(this CashgameDto dto, IList<CheckpointDto> checkpointDtos) => new(
        dto.BunchSlug,
        dto.LocationId.ToString(),
        dto.EventId != null ? dto.EventId.ToString() : null,
        (GameStatus)dto.Status,
        dto.CashgameId.ToString(),
        checkpointDtos.Select(ToCheckpoint).ToList());

    internal static IList<Cashgame> ToCashgameList(this IEnumerable<CashgameDto> dtos, IEnumerable<CheckpointDto> checkpointDtos)
    {
        var checkpointMap = GetGameCheckpointMap(checkpointDtos);

        var cashgames = new List<Cashgame>();
        foreach (var cashgameDto in dtos)
        {
            if (!checkpointMap.TryGetValue(cashgameDto.CashgameId, out var cashgameCheckpointDtos))
                cashgameCheckpointDtos = new List<CheckpointDto>();

            var cashgame = cashgameDto.ToCashgame(cashgameCheckpointDtos);
            cashgames.Add(cashgame);
        }

        return cashgames;
    }

    private static IDictionary<int, IList<CheckpointDto>> GetGameCheckpointMap(IEnumerable<CheckpointDto> checkpoints)
    {
        var checkpointMap = new Dictionary<int, IList<CheckpointDto>>();
        foreach (var checkpoint in checkpoints)
        {
            if (!checkpointMap.TryGetValue(checkpoint.CashgameId, out var checkpointList))
            {
                checkpointList = new List<CheckpointDto>();
                checkpointMap.Add(checkpoint.CashgameId, checkpointList);
            }
            checkpointList.Add(checkpoint);
        }
        return checkpointMap;
    }

    private static Checkpoint ToCheckpoint(this CheckpointDto checkpointDto) => Checkpoint.Create(
        checkpointDto.CheckpointId.ToString(),
        checkpointDto.CashgameId.ToString(),
        checkpointDto.PlayerId.ToString(),
        DateTime.SpecifyKind(checkpointDto.Timestamp, DateTimeKind.Utc),
        (CheckpointType)checkpointDto.Type,
        checkpointDto.Stack,
        checkpointDto.Amount);
}