using DependencyCheck.Data;
using DependencyCheck.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace RiskScore.Models
{
    class CRUDDepenVulnDB
    {
        Context db;

        public CRUDDepenVulnDB()
        {
            db = new Context();
        }

        public List<DependencyVulnerabilityDB> GetList()
        {
            return db.dependencyVulnerabilityDBs.ToList();
        }

        public void SaveList(List<DependencyVulnerabilityDB> dependencyVulnerabilityDBs)
        {
            //var depvulnDB = db.dependencyVulnerabilityDBs.ToList();
            var dependencies = (from dv in dependencyVulnerabilityDBs
                                select dv.dependency).ToList();

            foreach (DependencyVulnerabilityDB depvul in dependencyVulnerabilityDBs)
                Save(depvul);

            var users = db.dependencyVulnerabilityDBs;
            Console.WriteLine("Список объектов:");
            foreach (DependencyVulnerabilityDB u in users)
            {
                Console.WriteLine("{0}.{1} - {2}", u.id, u.dependency.name, u.vulnerabilityDBs);
            }
        }

        public void Save(DependencyVulnerabilityDB depvul)
        {
            try
            {
                depvul.vulnerabilityDBs = CheckIfInDB(depvul.vulnerabilityDBs).ToList();

                DependencyDB dp = db.dependencyDBs.ToList().Find(x => x.name == depvul.dependency.name 
                                                                && x.fileName == depvul.dependency.fileName);
                depvul.dependency = dp == null ? depvul.dependency : dp;

                db.dependencyVulnerabilityDBs.AddOrUpdate(depvul);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public List<VulnerabilityDB> GetAllVulnerabilities()
        {
            return db.vulnerabilityDBs.ToList();
        }

        private ICollection<VulnerabilityDB> CheckIfInDB(List<VulnerabilityDB> vulnerabilityDBs)
        {
            ICollection<VulnerabilityDB> vulners = new List<VulnerabilityDB>();
            foreach (VulnerabilityDB vul in vulnerabilityDBs)
            {
                VulnerabilityDB vulnerabilityDB = db.vulnerabilityDBs.ToList().Find(x => x.name == vul.name);
                if (vulnerabilityDB!=null)
                    vulners.Add(vulnerabilityDB);
            }                

            HashSet<string> diffids = new HashSet<string>(db.vulnerabilityDBs.ToList().Select(s => s.name));
            var results = vulnerabilityDBs.Where(m => !diffids.Contains(m.name)).ToList();

            vulnerabilityDBs.Clear();
            foreach (VulnerabilityDB vul in results)
                vulners.Add(vul);

            foreach (VulnerabilityDB vul in vulners)
                vulnerabilityDBs.Add(vul);

            return vulnerabilityDBs;
        }
    }
}
