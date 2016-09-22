namespace Web.Common.Urls.SiteUrls
{
    public abstract class BunchWithOptionalYearUrl : SiteUrl
    {
        protected BunchWithOptionalYearUrl(string format, string formatWithYear, string slug, int? year)
            : base(FormatBunchWithOptionalYear(format, formatWithYear, slug, year))
        {
        }

        private static string FormatBunchWithOptionalYear(string format, string formatWithYear, string slug, int? year)
        {
            if (!year.HasValue)
                return RouteParams.ReplaceSlug(format, slug);
            var url = RouteParams.ReplaceSlug(formatWithYear, slug);
            return RouteParams.ReplaceOptionalYear(url, year.Value);
        }
    }
}