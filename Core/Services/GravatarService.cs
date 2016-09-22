namespace Core.Services
{
	public static class GravatarService
    {
		public static string GetAvatarUrl(string email)
        {
            const string urlFormat = "http://www.gravatar.com/avatar/{0}?s=100";
            var hash = EncryptionService.GetMd5Hash(email);

		    return string.Format(urlFormat, hash);
		}
	}
}