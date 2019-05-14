using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSB100UserProfileService.ServiceAdapters
{
    public class LoginAdapter
    {
        public bool UsernameExists(string username)
        {
            using (var service = new LoginServiceRef.LoginServiceClient())
            {
                //TODO: waiting for the other team to finish their function
                return true;
            }
        }
    }
}