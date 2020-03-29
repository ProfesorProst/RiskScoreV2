using DependencyCheck.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskScore.Models
{
    class ModelUser
    {
        CRUDUser cRUDUser;
        public ModelUser()
        {
            cRUDUser = new CRUDUser();
        }

        public UserDB CreateNewUser(long id, string userName)
        {
            UserDB userDB = new UserDB(id, userName);
            cRUDUser.Create(userDB);
            return userDB;
        }

        internal List<UserDB> GetObjects()
        {
            return cRUDUser.GetObjects();
        }
    }
}
