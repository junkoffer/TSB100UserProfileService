using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TSB100UserProfileService
{
    
    public class UserProfileService : IUserProfileService
    {
        public IEnumerable<string> CreateUser(UserType newUser)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> UpdateUser(UserType user)
        {
            throw new NotImplementedException();
        }

        public UserType GetUser(string username)
        {
            throw new NotImplementedException();
        }
    }
}
