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
            return crudUser.Read(personId);
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

            UserVulnerabilityDB newUserVulnerabilityDB = null;
            foreach(var proccedVuln in emptyVuln.Except(userVulnerabilityDBs).ToList())
            {
                newUserVulnerabilityDB = crudUserVulnerabilitesDB.ReadByUserVuln(userId, proccedVuln.id);
                if (newUserVulnerabilityDB == null)
                {
                    newUserVulnerabilityDB = new UserVulnerabilityDB(Convert.ToInt32(userId), proccedVuln.id);
                    crudUserVulnerabilitesDB.Create(newUserVulnerabilityDB);
                    break;
                }
                else
                    if(newUserVulnerabilityDB.techDamage == null || newUserVulnerabilityDB.bizDamage == null 
                        || newUserVulnerabilityDB.threats == null)
                    break;
            }
            
            return newUserVulnerabilityDB;
        }

        internal bool UserCreateMark(int mark, string v, long vulnId, long userId)
        {
            var newUserVulnDB = new UserVulnerabilityDB(userId, vulnId);
            switch (v)
            {
                case "threats":
                    if (newUserVulnDB.threats != null) return false;
                    newUserVulnDB.threats = mark;
                    break;
                case "techDamage":
                    if (newUserVulnDB.techDamage != null) return false;
                    newUserVulnDB.techDamage = mark;
                    break;
                case "bizDamage":
                    if (newUserVulnDB.bizDamage != null) return false;
                    newUserVulnDB.bizDamage = mark;
                    break;
                default:
                    return false;
            }
            crudUserVulnerabilitesDB.Update(newUserVulnDB);
            return true;
        }
    }
}
