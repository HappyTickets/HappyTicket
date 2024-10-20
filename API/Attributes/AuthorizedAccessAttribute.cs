//using Application.Common.Interfaces.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace API.Attributes
//{
//    public class AuthorizedAccessAttribute(string userIdKey) : ActionFilterAttribute
//    {
//        private readonly string key = userIdKey;

//        public override void OnActionExecuting(ActionExecutingContext context)
//        {

//            var currentUser = (ICurrentUser)context.HttpContext.RequestServices.GetService(typeof(ICurrentUser));
//            var loggedInUserId = currentUser?.Id ?? 0;

//            if (loggedInUserId == 0 || loggedInUserId != _requiredUserId)
//            {
//                context.Result = new UnauthorizedResult();
//            }
//        }
//    }
//}