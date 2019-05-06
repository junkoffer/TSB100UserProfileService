using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TSB100UserProfileService
{

    public class UserProfileService : IUserProfileService
    {
        private UserProfileEntities db = new UserProfileEntities();

        public IEnumerable<string> CreateUser(User newUser)
        {
            var errors = new List<string>();

            //TODO: password, username and email should be passed to another service method (login service)
            using (db)
            {
                db.UserDb.Add(new UserDb()
                {
                    CreatedDate = DateTime.Now,
                    Username = newUser.Username,
                    Address = newUser.Address,
                    City = newUser.City,
                    Email = newUser.Email,
                    FirstName = newUser.Name,
                    PersonalIdentityNumber = newUser.PersonalCodeNumber,
                    PhoneNumber = newUser.Phonenumber,
                    PictureUrl = newUser.Picture,
                    Surname = newUser.Surname,
                    ZipCode = newUser.ZipCode
                });

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            errors.Add($"Property: {validationError.PropertyName} Error: { validationError.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex) when (
                    ex is DbUpdateException
                    )
                {
                    errors.Add(ex.Message);
                }
            }
            return errors;
        }

        public IEnumerable<string> UpdateUser(User user)
        {
            var errors = new List<string>();

            using (db)
            {
                var dbUser = (from u in db.UserDb
                             where u.Username == user.Username
                             select u).FirstOrDefault();

                if (dbUser == null)
                {
                    errors.Add("Användaren finns inte registrerad.");
                    return errors;
                }

                dbUser.Address = user.Address ?? string.Empty;
                dbUser.City = user.City ?? string.Empty;
                dbUser.Email = user.Email ?? string.Empty;
                dbUser.FirstName = user.Name ?? string.Empty;
                dbUser.PersonalIdentityNumber = user.PersonalCodeNumber;
                dbUser.PhoneNumber = user.Phonenumber;
                dbUser.Surname = user.Surname ?? string.Empty;
                dbUser.Username = user.Username ?? string.Empty;
                dbUser.ZipCode = user.ZipCode;
                dbUser.PictureUrl = user.Picture ?? string.Empty;

                db.Entry(dbUser).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            errors.Add($"Property: {validationError.PropertyName} Error: { validationError.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex) when (
                    ex is DbUpdateException
                )
                {
                    errors.Add(ex.Message);
                }
            }
            return errors;
        }

        public User GetUser(string username)
        {
            using (db)
            {
                try
                {
                    var dbUser = (from u in db.UserDb
                                  where u.Username == username
                                  select u).FirstOrDefault();

                    var user = new User
                    {
                        Address = dbUser?.Address ?? string.Empty,
                        City = dbUser?.City ?? string.Empty,
                        Email = dbUser?.Email ?? string.Empty,
                        Name = dbUser?.FirstName,
                        PersonalCodeNumber = dbUser?.PersonalIdentityNumber,
                        Phonenumber = dbUser?.PhoneNumber ?? string.Empty,
                        Picture = dbUser?.PictureUrl ?? string.Empty,
                        Surname = dbUser?.Surname ?? string.Empty,
                        Username = dbUser?.Username ?? string.Empty,
                        ZipCode = dbUser?.ZipCode
                    };
                    return user;
                }
                catch
                {
                    //TODO: hör med resten av gruppen hur vi ska göra med errors
                    return null;
                }
            }
        }
    }
}
