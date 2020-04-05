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
            var uv = context.userVulnerabilityDBs.Where(x => x.userid == personId && x.vulnerability_id == vulnId).ToList().FirstOrDefault();
            if (uv != null) uv.vulnerability = context.vulnerabilityDBs.Where(x => x.id == uv.vulnerability_id).ToList().FirstOrDefault();
            return uv;
        }

        public List<UserVulnerabilityDB> GetObjects()
        {
            var temp = context.userVulnerabilityDBs;
            return temp.ToList();
        }
    }
}
