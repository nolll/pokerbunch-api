//using Core.Exceptions;

//namespace Api.Extensions
//{
//    public class HttpNotFoundAwareControllerActionSelector : ApiControllerActionSelector
//    {
//        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
//        {
//            try
//            {
//                return base.SelectAction(controllerContext);
//            }
//            catch (HttpResponseException e)
//            {
//                throw new NotFoundException("Not found");
//            }
//        }
//    }
//}