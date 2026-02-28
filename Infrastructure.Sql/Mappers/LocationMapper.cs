using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class LocationMapper
{
    internal static Location ToLocation(this LocationDto locationDto) => new(
        locationDto.LocationId.ToString(),
        locationDto.Name,
        locationDto.BunchSlug);
}