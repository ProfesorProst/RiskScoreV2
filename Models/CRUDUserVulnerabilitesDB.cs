using DependencyCheck.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DependencyCheck.Models
{
    class CRUDUserVulnerabilitesDB
    {
        Context context;

        public CRUDUserVulnerabilitesDB()
        {
            context = new Context();
        }

        public void Create(UserVulnerabilityDB userVulnerabilityDB)
        {
            context = new Context();
            context.userVulnerabilityDBs.Add(userVulnerabilityDB);
            context.SaveChanges();
        }

        public void Update(UserVulnerabilityDB obj) 
        {
            context = new Context();
            var entity = context.userVulnerabilityDBs.Find(obj.id);
            if (entity == null)
                return;
                        
            context.Entry(entity).CurrentValues.SetValues(obj);
            context.SaveChanges();
            return;
        }

        public void Delet(UserVulnerabilityDB obj) 
        {
            context = new Context();
            obj = context.userVulnerabilityDBs.Find(obj.id);
            if (obj == null)
                return;
            context.userVulnerabilityDBs.Remove(obj);
            context.SaveChanges();
        }

        public UserVulnerabilityDB Read(long id)
        {
            return context.userVulnerabilityDBs.Find(id);
        }

        public UserVulnerabilityDB ReadByUserVuln(long personId, long vulnId)
        {
            return context.userVulnerabilityDBs.Where(x => x.userid == personId && x.vulnerabilityid == vulnId).ToList().FirstOrDefault();
        }

        public List<UserVulnerabilityDB> GetObjects()
        {
            return context.userVulnerabilityDBs.ToList();
        }
    }
}
