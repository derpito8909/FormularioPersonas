using Microsoft.AspNetCore.Mvc.ModelBinding;
using Registro.Web.Application.Errors;

namespace Registro.Web.Helpers;

public static class ModelStateApiErrorExtensions
{
    public static void ApplyApiValidationErrors(this ModelStateDictionary modelState, ApiError error)
    {
        if (error.Errors is null || error.Errors.Count == 0)
            return;
        
        foreach (var kv in error.Errors)
        {
            var field = kv.Key;
            foreach (var msg in kv.Value)
            {
                modelState.AddModelError(field, msg);
            }
        }
    }
}