using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSB100UserProfileService.Validation
{
    public class StringValidator
    {
        internal static void MinMax(List<string> errors, string property, int minLength, int maxLength)
        {
            //Adds an error message as written below to the error list regarding the strings length
            if (property.Length < minLength)
            {
                errors.Add($"Validation error: {property}, MinLength: {minLength}, ActualStringLength: {property.Length}");
            }

            if (property.Length > maxLength)
            {
                errors.Add($"Validation error: {property}, MaxLength: {maxLength}, ActualStringLength: {property.Length}");
            }
        }

        internal static void Email(List<string> errors, string email)
        {
            MinMax(errors, email, 0, 50);

            //TODO: check if valid email (split with at least @)
        }
    }
}