using System.Globalization;

namespace Core.Entities;

public class Currency(string? symbol, string? layout, CultureInfo? culture = null)
{
    private const string SymbolPlaceholder = "{SYMBOL}";
    private const string AmountPlaceholder = "{AMOUNT}";
    private const string DefaultSymbol = "$";
    private const string DefaultLayout = $"{SymbolPlaceholder}{AmountPlaceholder}";
    private const string DefaultCulture = "sv-SE";

    public string Symbol { get; } = symbol ?? DefaultSymbol;
    public string Layout { get; } = layout ?? DefaultLayout;
    public CultureInfo Culture { get; } = culture ?? CultureInfo.CreateSpecificCulture(DefaultCulture);
    public string ThousandSeparator { get; } = " ";
    public string Format => Layout.Replace(SymbolPlaceholder, Symbol).Replace(AmountPlaceholder, "{0}");
    public static Currency Default => new(DefaultSymbol, DefaultLayout);
}