using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Web;

namespace TSB100UserProfileService
{
   // enum UserStatus { Active, Paused, Blocked}

    [DataContract]
    public class User
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public int? PersonalCodeNumber { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public int? ZipCode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Phonenumber { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Picture { get; set; }

        [DataMember]
        public IEnumerable<Review> Review { get; set; }

        [DataMember]
        public IEnumerable<AccountStatus> AccountStatus { get; set; }
    }

}