using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Newtonsoft.Json;
using Serilog;
using TSB100UserProfileService.DataTransferObjects;
using TSB100UserProfileService.Mapping;
using TSB100UserProfileService.ServiceAdapters;

namespace TSB100UserProfileService
{

    public class UserProfileService : IUserProfileService
    {
        private UserProfileEntities db;
        private UserMapper _mapper;
        private UserValidator _validator;

        public UserProfileService()
        {
            db = new UserProfileEntities();
            _mapper = new UserMapper();
            var loginAdapter = new LoginAdapter();
            _validator = new UserValidator(loginAdapter);
        }

        public User CreateUser(NewUser newUserFromWebPage)
        {
            if (!_validator.ValidateNewUser(newUserFromWebPage))
            {
                return null;
            }

            int userId;

            using (var loginService = new LoginServiceRef.LoginServiceClient())
            {
                var newUser = new LoginServiceRef.NewUser
                {
                    Email = newUserFromWebPage.Email,
                    Firstname = newUserFromWebPage.FirstName,
                    Surname = newUserFromWebPage.Surname,
                    Password = newUserFromWebPage.Password,
                    Username = newUserFromWebPage.Username
                };

                var returnUser = loginService.CreateUser(newUser);

                if (returnUser == null)
                {
                    // Logging that something went wrong when trying to save the new user in the other service
                    Log.Warning($"An attempt to create an account with the following values failed: {JsonConvert.SerializeObject(newUser)}");

                    return null;
                }

                // Catches the newly created user's id
                userId = returnUser.ID;
            }

            using (db)
            {
                var dbUser = new UserDb();
                db.UserDb.Add(dbUser);

                dbUser.UserId = userId;
                _mapper.MapNewUserToModel(newUserFromWebPage, dbUser);

                if (!UpdateDatabase())
                {
                    // Logging that something went wrong when trying to save the new user in the other service
                    Log.Warning($"An attempt to create a profile with the following values failed: {JsonConvert.SerializeObject(newUserFromWebPage)}");

                    return null;
                }

                // Return a User object so that one may add more profile data
                var user = _mapper.MapToWebService(dbUser);
                return user;
            }
        }

        public bool UpdateUser(User user)
        {
            // TODO: Login Service does not have an UpdateUser() function - cannot pass parameters (password, email, username, first and lastname) to that service, so that they are updated there
            // TODO: validate like with NewUser (but with the User class)

            if (!_validator.ValidateUser(user))
            {
                return false;
            }

            using (db)
            {
                var dbUser = (from u in db.UserDb
                              where u.UserId == user.Id
                              select u).FirstOrDefault();

                if (dbUser == null)
                {
                    // TODO: Create logging
                    return false;
                }

                _mapper.MapUserToModel(user, dbUser);
                db.Entry(dbUser).State = EntityState.Modified;

                // Saves the changes that have been made, and returns true if succeeded, and false if not
                return UpdateDatabase();
            }
        }

        public User GetUserByUserName(string username)
        {
            using (db)
            {
                var dbUser = (from u in db.UserDb
                              where u.Username == username
                              select u).FirstOrDefault();

                // TODO: Create logging (if null)
                return _mapper.MapToWebService(dbUser);
            }
        }

        public User GetUserByUserId(int userId)
        {
            using (db)
            {
                var dbUser = (from u in db.UserDb
                              where u.UserId == userId
                              select u).FirstOrDefault();

                // TODO: Create logging (if null)

                return _mapper.MapToWebService(dbUser);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            using (db)
            {
                var users = new List<User>();
                var dbUsers = db.UserDb.ToList();

                foreach (var dbUser in dbUsers)
                {
                    var user = _mapper.MapToWebService(dbUser);

                    if (user == null)
                    {
                        // TODO: Create logging
                        return null;
                    }
                    users.Add(user);
                }
                return users;
            }
        }

        //----------------------------------------------------------------------------------------
        // Helper methods
        //----------------------------------------------------------------------------------------
        #region helperMethods

        private bool UpdateDatabase()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception ex) when (
                ex is DbUpdateException
                || ex is DbUpdateConcurrencyException
                || ex is DbEntityValidationException
                || ex is NotSupportedException
                || ex is ObjectDisposedException
                || ex is InvalidOperationException
                )
            {
                //TODO: Create logging
                return false;
            }
        }
        #endregion
    }
}
