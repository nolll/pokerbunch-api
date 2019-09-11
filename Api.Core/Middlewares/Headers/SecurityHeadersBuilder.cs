namespace Api.Middlewares.Headers
{
    public class SecurityHeadersBuilder
    {
        private readonly SecurityHeadersPolicy _policy = new SecurityHeadersPolicy();

        public SecurityHeadersBuilder AddHeader(string header, string value)
        {
            _policy.SetHeaders[header] = value;
            return this;
        }

        public SecurityHeadersBuilder RemoveHeader(string header)
        {
            _policy.RemoveHeaders.Add(header);
            return this;
        }

        public SecurityHeadersPolicy Build()
        {
            return _policy;
        }
    }
}