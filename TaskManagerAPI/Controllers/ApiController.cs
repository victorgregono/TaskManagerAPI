using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TaskManager.Application.Extensions;
using TaskManager.Application.ViewModels;

namespace TaskManagerAPI.Controllers
{
    /// <summary>
    /// Controlador base abstrato para fornecer respostas personalizadas para APIs.
    /// </summary>
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected ActionResult CustomResponse(OperationResult result)
        {
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            if (!result.IsValid)
            {
                var responseError = ErrorResponse(result);

                return responseError;
            }

            return Ok(result.Content ?? string.Empty);
        }

        private ActionResult ErrorResponse(OperationResult result)
        {
            return result.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(MapErrorsToResponse(result.Result)),
                HttpStatusCode.NotFound => NotFound(MapErrorsToResponse(result.Result)),
                HttpStatusCode.Unauthorized => Unauthorized(MapErrorsToResponse(result.Result)),
                _ => BadRequest(MapErrorsToResponse(result.Result)),
            };
        }

        private static ErrorViewModel MapErrorsToResponse(ValidationResult validationResult)
            => new ErrorViewModel(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
