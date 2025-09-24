using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class LocationMapper
{
    internal static Location ToLocation(this LocationDto locationDto)
    {
        return new Location(
            locationDto.Location_Id.ToString(),
            locationDto.Name,
            locationDto.Bunch_Id.ToString(),
            locationDto.Bunch_Slug);
    }
}