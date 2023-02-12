using System.Globalization;

namespace Core.Entities;

public class Currency
{
    private const string SymbolPlaceholder = "{SYMBOL}";
    private const string AmountPlaceholder = "{AMOUNT}";
    private const string DefaultSymbol = "$";
    private const string DefaultLayout = $"{SymbolPlaceholder}{AmountPlaceholder}";
    private const string DefaultCulture = "sv-SE";

    public string Symbol { get; }
    public string Layout { get; }
    public CultureInfo Culture { get; }
    public string ThousandSeparator { get; }
    public string Format => Layout.Replace(SymbolPlaceholder, Symbol).Replace(AmountPlaceholder, "{0}");
    public static Currency Default => new(DefaultSymbol, DefaultLayout);

    public Currency(string? symbol, string? layout, CultureInfo? culture = null)
    {
        Symbol = symbol ?? DefaultSymbol;
        Layout = layout ?? DefaultLayout;
        Culture = culture ?? CultureInfo.CreateSpecificCulture(DefaultCulture);
        ThousandSeparator = " ";
    }
}