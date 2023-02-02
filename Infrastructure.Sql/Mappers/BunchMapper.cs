using System.Globalization;
using Core.Entities;
using Infrastructure.Sql.Dtos;

namespace Infrastructure.Sql.Mappers;

internal static class BunchMapper
{
    internal static Bunch ToBunch(this BunchDto bunchDto)
    {
        var culture = CultureInfo.CreateSpecificCulture("sv-SE");
        var currency = new Currency(bunchDto.Currency, bunchDto.Currency_Layout, culture);

        return new Bunch(
            bunchDto.Bunch_Id.ToString(),
            bunchDto.Name,
            bunchDto.Display_Name,
            bunchDto.Description,
            bunchDto.House_Rules,
            TimeZoneInfo.FindSystemTimeZoneById(bunchDto.Timezone),
            bunchDto.Default_Buyin,
            currency);
    }
}