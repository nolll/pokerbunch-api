using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class DeleteCheckpointUrl : IdUrl
    {
        public DeleteCheckpointUrl(int id)
            : base(WebRoutes.Cashgame.CheckpointDelete, id)
        {
        }
    }
}