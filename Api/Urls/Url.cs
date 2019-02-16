namespace PokerBunch.Common.Urls
{
    public abstract class Url
    {
        protected abstract string Input { get; }
        public string Relative => Input != null ? string.Concat((string) "/", (string) Input).ToLower() : string.Empty;
        public string Absolute(string host, string protocol = "http") => $"{protocol}://{host}{Relative}";
    }
}
