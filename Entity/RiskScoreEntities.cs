using System;
using System.Collections.Generic;
using System.Text;
using DependencyCheck.Models;

namespace RiskScore.Entity
{
    class RiskScoreEntities
    {
        public DateTime dateTime {  get; }

        List<DependencyVulnerabilityDB> dependencyVulnerabilityDBs { get; }

        public double score { get; set; }

        public RiskScoreEntities(DateTime dateTime)
        {
            this.dateTime = dateTime;
            dependencyVulnerabilityDBs = new List<DependencyVulnerabilityDB>();
        }

        public void AddDependencyVulnerabilityDBs(DependencyVulnerabilityDB dependencyVulnerabilityDB)
        {
            dependencyVulnerabilityDBs.Add(dependencyVulnerabilityDB as DependencyVulnerabilityDB);
        }

    }
}
