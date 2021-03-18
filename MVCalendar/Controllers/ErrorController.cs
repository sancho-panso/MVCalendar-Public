using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCalendar.Models;
using System.Diagnostics;

namespace MVCalendar.Controllers
{ // all NotFound and BadRequest responses are redirected to this Error controller  and Error View
    public class ErrorController : Controller
    {
        private static readonly NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ILogger<ErrorController> _logger;
        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("Error/{statusCode}")]
        public IActionResult StatusCodeHandler(int statusCode)
        {
            var exDetails = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            _logger.LogWarning($"{statusCode} Error Occured. Path ={exDetails.OriginalPath} and QueryString={exDetails.OriginalQueryString}");
            NLogger.Error($"{statusCode} Error Occured. Path ={exDetails.OriginalPath} and QueryString={exDetails.OriginalQueryString}");

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = " The requested resource can not be found";
                    break;
                case 400:
                    ViewBag.ErrorMessage = " The server response with bad request message";
                    break;
                case 500:
                    ViewBag.ErrorMessage = " Sorry, the server experience some internal problems";
                    break;

            }
            
            return View("NotFound");
        }

        [AllowAnonymous]
        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"The path {exDetails.Path} threws exception {exDetails.Error}");
            NLogger.Error($"The path {exDetails.Path} threws exception {exDetails.Error}");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
