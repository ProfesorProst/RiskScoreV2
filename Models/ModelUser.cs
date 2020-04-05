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
            controllerDepenVuln = new ControllerDepenVulnDB();
            crudUserVulnerabilitesDB = new CRUDUserVulnerabilitesDB();
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

        public List<VulnerabilityDB> GetAllEmptyVulnerabilities()
        {
            var empVul = from n in controllerDepenVuln.GetAllVulnerabilities()
                         where n.techDamage == null || n.threats == null || n.bizDamage == null
                         select n;
            return (empVul != null)? empVul.ToList(): null ;
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
                else if(newUserVulnerabilityDB.techDamage == null || newUserVulnerabilityDB.bizDamage == null 
                        || newUserVulnerabilityDB.threats == null) 
                    break;
            }
            newUserVulnerabilityDB = crudUserVulnerabilitesDB.ReadByUserVuln(newUserVulnerabilityDB.userid, newUserVulnerabilityDB.vulnerability_id);
            return newUserVulnerabilityDB;
        }

        internal UserVulnerabilityDB UserCreateMark(int mark, string v, long vulnId, long userId)
        {
            var userVulnDB = crudUserVulnerabilitesDB.ReadByUserVuln(userId, vulnId);
            bool ifNew = false;
            if(userVulnDB == null)
            {
                ifNew = true;
                userVulnDB = new UserVulnerabilityDB(userId, vulnId);
            }

            switch (v)
            {
                case "threats":
                    if (userVulnDB.threats != null) return null;
                    userVulnDB.threats = mark;
                    break;
                case "techDamage":
                    if (userVulnDB.techDamage != null) return null;
                    userVulnDB.techDamage = mark;
                    break;
                case "bizDamage":
                    if (userVulnDB.bizDamage != null) return null;
                    userVulnDB.bizDamage = mark;
                    break;
                default:
                    return null;
            }
            if (ifNew)
                crudUserVulnerabilitesDB.Create(userVulnDB);
            else
                crudUserVulnerabilitesDB.Update(userVulnDB);
            return userVulnDB;
        }
    }
}
