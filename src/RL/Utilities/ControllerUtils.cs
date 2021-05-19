using BLL.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace RL.Utilities
{
    public static class ControllerUtilities
    {
        public static ActionResult CheckResult(Result result)
        {
            if (result.Failure)
                return new BadRequestObjectResult(result.Error);

            return new OkResult();
        }
        
        public static ActionResult<T> CheckResult<T>(Result<T> result)
        {
            if (result.Failure)
                return new BadRequestObjectResult(result.Error);

            return new OkObjectResult(result.Value);
        }
    }
}