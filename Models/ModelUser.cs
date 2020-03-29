using DependencyCheck.Controller;
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
        CRUDUserVulnerabilitesDB crudUserVulnerabilitesDB;
        ControllerDepenVulnDB controllerDepenVuln;
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
            UserDB person = crudUser.Read(personId);
            return person;
        }

        private List<VulnerabilityDB> GetAllEmptyVulnerabilities()
        {
            return controllerDepenVuln.GetAllVulnerabilities().FindAll(n => n.techDamage == null
                || n.threats == null || n.bizDamage == null).ToList(); ;
        }
        public bool FindEmptyVuln()
        {
            return (GetAllEmptyVulnerabilities() != null) ? true: false;
        }

        internal UserVulnerabilityDB FindTask(long userId)
        {
            var emptyVuln = GetAllEmptyVulnerabilities();
            var userVulnerabilityDBs = crudUserVulnerabilitesDB.GetObjects().Select(x => x.vulnerability).ToList();

            var proccedVuln = emptyVuln.Except(userVulnerabilityDBs).ToList().First();
            UserVulnerabilityDB newUserVulnerabilityDB = new UserVulnerabilityDB(Convert.ToInt32(userId), proccedVuln.id);
            crudUserVulnerabilitesDB.Create(newUserVulnerabilityDB);
            return newUserVulnerabilityDB;
        }
    }
}
