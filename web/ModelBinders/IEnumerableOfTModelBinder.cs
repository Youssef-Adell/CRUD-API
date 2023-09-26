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

        #region Get the Value from the request and ensure that it is not null
        string? providedValue = bindingContext.ValueProvider
                                .GetValue(bindingContext.ModelName)
                                .FirstValue;

        if (providedValue.IsNullOrEmpty())
        {
            bindingContext.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }
        #endregion

        #region Convert the ProvidedValue type to the needed model type
        // Getting the T in IEnumarable<T> which is the type of action parameter need to be bound 
        var genericTypeInIEnumarable = bindingContext.ModelType.GenericTypeArguments[0];

        TypeConverter converter = TypeDescriptor.GetConverter(genericTypeInIEnumarable);

        // Converting the string provided in the request to array of strings then convert this array to array of T type
        // but unfortunately the converter method returns object type not T type
        var convertedValueObjects = providedValue.Split(",", StringSplitOptions.TrimEntries)
                                            .Select(idStr => converter.ConvertFromString(idStr))
                                            .ToArray();

        // So we created another array of T type and copy the previous array of Objects to it
        var convertedValueActualType = Array.CreateInstance(genericTypeInIEnumarable, convertedValueObjects.Length);
        convertedValueObjects.CopyTo(convertedValueActualType, 0);

        #endregion

        bindingContext.Result = ModelBindingResult.Success(convertedValueActualType);
        return Task.CompletedTask;
    }

}
