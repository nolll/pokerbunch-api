using System.Linq;
using Core.Entities;
using Core.Entities.Checkpoints;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class CashgameMapper
{
    internal static Cashgame ToCashgame(this CashgameDto dto, IList<CheckpointDto> checkpointDtos)
    {
        var checkpoints = checkpointDtos.Select(ToCheckpoint).ToList();

        return new Cashgame(
            dto.Bunch_Id.ToString(),
            dto.Location_Id.ToString(),
            dto.Event_Id != 0 ? dto.Event_Id.ToString() : null,
            (GameStatus)dto.Status,
            dto.Cashgame_Id.ToString(),
            checkpoints);
    }

    internal static IList<Cashgame> ToCashgameList(this IEnumerable<CashgameDto> dtos, IEnumerable<CheckpointDto> checkpointDtos)
    {
        var checkpointMap = GetGameCheckpointMap(checkpointDtos);

        var cashgames = new List<Cashgame>();
        foreach (var cashgameDto in dtos)
        {
            if (!checkpointMap.TryGetValue(cashgameDto.Cashgame_Id, out var cashgameCheckpointDtos))
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
            if (!checkpointMap.TryGetValue(checkpoint.Cashgame_Id, out var checkpointList))
            {
                checkpointList = new List<CheckpointDto>();
                checkpointMap.Add(checkpoint.Cashgame_Id, checkpointList);
            }
            checkpointList.Add(checkpoint);
        }
        return checkpointMap;
    }

    private static Checkpoint ToCheckpoint(this CheckpointDto checkpointDto)
    {
        return Checkpoint.Create(
            checkpointDto.Checkpoint_Id.ToString(),
            checkpointDto.Cashgame_Id.ToString(),
            checkpointDto.Player_Id.ToString(),
            TimeZoneInfo.ConvertTimeToUtc(checkpointDto.Timestamp),
            (CheckpointType)checkpointDto.Type,
            checkpointDto.Stack,
            checkpointDto.Amount);
    }
}