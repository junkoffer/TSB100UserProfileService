using System;
using System.Collections.Generic;
using TSB100UserProfileService.DataTransferObjects;
using TSB100UserProfileService.ServiceAdapters;
using TSB100UserProfileService.Validation;

namespace TSB100UserProfileService
{
    internal class UserValidator
    {
        private LoginAdapter _loginAdapter;

        public UserValidator(LoginAdapter loginAdapter)
        {
            _loginAdapter = loginAdapter;
        }

        internal bool ValidateNewUser(NewUser newUser)
        {
            var errors = new List<string>();

            if (_loginAdapter.UsernameExists(newUser.Username))
            {
                errors.Add($"Validation error: {newUser.Username} already exists.");
            }
            
            StringValidator.MinMax(errors, newUser.Email, 0, 50);
            StringValidator.MinMax(errors, newUser.FirstName, 0, 100);
            StringValidator.MinMax(errors, newUser.Surname, 0, 100);
            StringValidator.MinMax(errors, newUser.Username, 0, 50);
            StringValidator.Email(errors, newUser.Email);
            //Password: don't know min or max length for it

            return (errors.Count > 0);
        }
    }
}