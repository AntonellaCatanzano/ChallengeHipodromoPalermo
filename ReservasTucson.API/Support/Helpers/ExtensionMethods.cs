    using Microsoft.AspNetCore.Mvc.ModelBinding;


    namespace ReservasTucson.API.Support.Helpers
    {
        public static class ExtensionMethods
        {
            #region ModelState
            public static List<string> GetModelValidations(this ModelStateDictionary model)
            {
                return model.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.Exception == null ? x.ErrorMessage : x.Exception.Message)
                            .ToList();
            }

            // Opcional: incluir nombre del campo
            public static IEnumerable<string> GetModelValidationsWithField(this ModelStateDictionary model)
            {
                return model.SelectMany(kvp => kvp.Value.Errors
                                    .Select(e => $"{kvp.Key}: {(e.Exception?.Message ?? e.ErrorMessage)}"));
            }
            #endregion
        }
    }

