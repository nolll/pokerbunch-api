using Api.Routes;

namespace Api.Urls.SiteUrls
{
    public class EditCheckpointUrl : IdUrl
    {
        public EditCheckpointUrl(int checkpointId)
            : base(WebRoutes.Cashgame.CheckpointEdit, checkpointId)
        {
        }
    }
}