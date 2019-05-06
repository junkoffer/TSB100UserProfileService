using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TSB100UserProfileService
{
    [ServiceContract]
    public interface IUserProfileService
    {
        //TODO: Behöver ha en dialog med överiga tjänster om funktioners input och output

        //Måste kolla av mot inloggningstjänst om användarnamnet finns eller inte
        [OperationContract]
        IEnumerable<string> CreateUser(User newUser);

        [OperationContract]
        IEnumerable<string> UpdateUser(User user);

        [OperationContract]
        User GetUser(string username);
    }
}
