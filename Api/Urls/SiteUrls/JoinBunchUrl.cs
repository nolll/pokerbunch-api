using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class JoinBunchUrl : SiteUrl
    {
        private readonly string _code;
        private readonly string _slug;

        public JoinBunchUrl(string slug)
        {
            _slug = slug;
        }

        public JoinBunchUrl(string slug, string code)
            : this(slug)
        {
            _code = code;
        }

        protected override string Input => _code != null ? InputWithCode : InputWithoutCode;
        private string InputWithCode => RouteParams.Replace(WebRoutes.Bunch.JoinWithCode, RouteReplace.Slug(_slug), RouteReplace.Code(_code));
        private string InputWithoutCode => RouteParams.Replace(WebRoutes.Bunch.Join, RouteReplace.Slug(_slug));
    }
}