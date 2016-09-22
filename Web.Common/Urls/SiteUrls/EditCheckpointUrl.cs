using Web.Common.Routes;

namespace Web.Common.Urls.SiteUrls
{
    public class EditCheckpointUrl : IdUrl
    {
        public EditCheckpointUrl(int checkpointId)
            : base(WebRoutes.Cashgame.CheckpointEdit, checkpointId)
        {
        }
    }
}