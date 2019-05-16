using System.Collections.Generic;
using System.ServiceModel;
using TSB100UserProfileService.DataTransferObjects;

namespace TSB100UserProfileService
{
    [ServiceContract]
    public interface IUserProfileService
    {
        [OperationContract]
        User CreateUser(NewUser newUser);

        [OperationContract]
        bool UpdateUser(User user);

        [OperationContract]
        IEnumerable<User> GetAllUsers();

        [OperationContract]
        User GetUserByUserName(string username);

        [OperationContract]
        User GetUserByUserId(int userId);

        //TODO: ska denna funktion finnas med?
        //Vem ansvarar för vad? Ska vi koppla ihop med inloggningstjänsten och kalla på deras DeleteUser()?
        //[OperationContract]
        //bool DeleteUser();

        //TODO: create function for getting logs from today
    }
}
