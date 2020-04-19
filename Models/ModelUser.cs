using DependencyCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiskScore.Models
{
    class ModelUser
    {
        CRUDUser crudUser;
        public ModelUser()
        {
            crudUser = new CRUDUser();
        }

        public UserDB CreateNewUser(long id, string userName)
        {
            UserDB userDB = new UserDB(id, userName);
            crudUser.Create(userDB);
            return userDB;
        }

        internal List<UserDB> GetObjects()
        {
            return crudUser.GetObjects();
        }

        public bool TryIfExist(long id)
        {
            try
            {
                if (GetObject(id) == null)
                    return false;
            }
            catch (Exception) { return false; }
            return true;
        }

        private UserDB GetObject(long personId)
        {
            return crudUser.Read(personId);
        }
    }
}
