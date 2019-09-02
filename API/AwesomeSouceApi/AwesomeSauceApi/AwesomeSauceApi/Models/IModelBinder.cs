using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeSauceApi.Models
{
    interface IModelBinder
    {
        Task BindModelAsync(ModelBindingContext bindingContext);
    }
}
