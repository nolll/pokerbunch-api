namespace Api.Urls
{
    public abstract class Url
    {
        protected abstract string Host { get; }
        protected abstract string Input { get; }
        public string Relative => Input != null ? string.Concat("/", Input).ToLower() : string.Empty;
        public override string ToString() => Relative;
        public string Absolute => $"https://{Host}{Relative}";
    }
}