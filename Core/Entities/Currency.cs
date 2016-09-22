using System.Globalization;

namespace Core.Entities
{
	public class Currency
	{
	    public string Symbol { get; }
	    public string Layout { get; }
	    public CultureInfo Culture { get; }
        public string ThousandSeparator { get; }
        public string Format => Layout.Replace("{SYMBOL}", Symbol).Replace("{AMOUNT}", "{0}");
        public static Currency Default => new Currency("$", "{SYMBOL}{AMOUNT}");

        public Currency(string symbol, string layout, CultureInfo culture = null)
	    {
	        Symbol = symbol;
	        Layout = layout;
	        Culture = culture ?? CultureInfo.CreateSpecificCulture("sv-SE");
            ThousandSeparator = " ";
	    }
	}
}