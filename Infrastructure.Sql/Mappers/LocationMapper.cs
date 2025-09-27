using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class LocationMapper
{
    internal static Location ToLocation(this LocationDto locationDto) => new(
        locationDto.Location_Id.ToString(),
        locationDto.Name,
        locationDto.Bunch_Slug);
}