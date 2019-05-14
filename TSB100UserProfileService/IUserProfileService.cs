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
    }
}
