using System.Globalization;
using Core.Entities;
using Infrastructure.Sql.Dtos;
using Infrastructure.Sql.Models;

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
    
    internal static Bunch ToBunch(this PbBunch bunchDto) => new(
        bunchDto.BunchId.ToString(),
        bunchDto.Name,
        bunchDto.DisplayName,
        bunchDto.Description,
        bunchDto.HouseRules,
        bunchDto.Timezone is not null
            ? TimeZoneInfo.FindSystemTimeZoneById(bunchDto.Timezone)
            : TimeZoneInfo.Utc,
        bunchDto.DefaultBuyin,
        new Currency(bunchDto.Currency, bunchDto.CurrencyLayout, CultureInfo.CreateSpecificCulture("sv-SE")));
}