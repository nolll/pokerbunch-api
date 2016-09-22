using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class DeleteCheckpointUrl : IdUrl
    {
        public DeleteCheckpointUrl(int id)
            : base(WebRoutes.Cashgame.CheckpointDelete, id)
        {
        }
    }
}