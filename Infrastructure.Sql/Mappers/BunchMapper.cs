using System.Globalization;
using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class BunchMapper
{
    internal static Bunch ToBunch(this BunchDto bunchDto) => new(
        bunchDto.Bunch_Id.ToString(),
        bunchDto.Name,
        bunchDto.Display_Name,
        bunchDto.Description,
        bunchDto.House_Rules,
        bunchDto.Timezone is not null
            ? TimeZoneInfo.FindSystemTimeZoneById(bunchDto.Timezone)
            : TimeZoneInfo.Utc,
        bunchDto.Default_Buyin,
        new Currency(bunchDto.Currency, bunchDto.Currency_Layout, CultureInfo.CreateSpecificCulture("sv-SE")));
}