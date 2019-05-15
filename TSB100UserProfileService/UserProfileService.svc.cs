using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
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

        public User CreateUser(NewUser newUser)
        {
            // TODO: Check login service if username is free or already taken
            // TODO: password, username and email should be passed to login service for validation and registration

            if (!_validator.ValidateNewUser(newUser))
            {
                return null;
            }

            using (db)
            {
                var dbUser = new UserDb();
                db.UserDb.Add(dbUser);
                _mapper.MapNewUserToModel(newUser, dbUser);

                if (!UpdateDatabase())
                {
                    return null;
                }

                // The database has now created an Id, let's copy that to the UserId property
                dbUser.UserId = dbUser.Id;

                // Saves the UserId to right column in the database
                if (!UpdateDatabase())
                {
                    return null;
                }

                // Return a User object so that one may add more profile data
                var user = _mapper.MapToWebService(dbUser);
                return user;
            }
        }

        public bool UpdateUser(User user)
        {
            // TODO: password, username and email should be passed to another service method (login service)
            // TODO: validate newUser

            using (db)
            {
                var dbUser = (from u in db.UserDb
                              where u.Username == user.Username
                              select u).FirstOrDefault();

                if (dbUser == null)
                {
                    return false;
                }

                _mapper.MapUserToModel(user, dbUser);
                db.Entry(dbUser).State = EntityState.Modified;

                // Saves the changes that have been made, and returns a true if succeeded, and false if not
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

                Log.Information("Testing logging: Information");

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
                return false;
            }
        }
        #endregion
    }
}
