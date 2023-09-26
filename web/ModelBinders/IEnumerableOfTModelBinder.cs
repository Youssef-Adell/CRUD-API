using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;

namespace Web.ModelBinders;

public class IEnumerableOfTModelBinder : IModelBinder
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
        var genericTypeInIEnumarable = bindingContext.ModelType.GenericTypeArguments[0];

        TypeConverter converter = TypeDescriptor.GetConverter(genericTypeInIEnumarable);

        var convertedValueObjects = providedValue.Split(",", StringSplitOptions.TrimEntries)
                                            .Select(idStr => converter.ConvertFromString(idStr))
                                            .ToArray();

        var convertedValueActualType = Array.CreateInstance(genericTypeInIEnumarable, convertedValueObjects.Length);
        convertedValueObjects.CopyTo(convertedValueActualType, 0);


        bindingContext.Result = ModelBindingResult.Success(convertedValueActualType);
        return Task.CompletedTask;
    }

}
