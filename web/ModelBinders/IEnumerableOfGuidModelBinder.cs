using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;

namespace Web.ModelBinders;

public class IEnumerableOfGuidModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (!bindingContext.ModelMetadata.IsEnumerableType)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        // Get the Value from the request and ensure that it is not null
        string? providedValue = bindingContext.ValueProvider
                                .GetValue(bindingContext.ModelName)
                                .FirstValue;

        if (providedValue.IsNullOrEmpty())
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        // Convert the ProvidedValue type to the needed model type
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Guid));

        IEnumerable<Guid> convertedValue = providedValue.Split(",", StringSplitOptions.TrimEntries)
                                            .Select(idStr => (Guid)converter.ConvertFromString(idStr));

        bindingContext.Result = ModelBindingResult.Success(convertedValue);
        return Task.CompletedTask;
    }

}
