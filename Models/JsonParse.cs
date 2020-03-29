using System;
using Newtonsoft.Json;

namespace DependencyCheck.Models
{

    public partial class GeneralInfo
    {
        [JsonProperty("reportSchema")]
        public string ReportSchema { get; set; }

        [JsonProperty("scanInfo")]
        public ScanInfo ScanInfo { get; set; }

        [JsonProperty("projectInfo")]
        public ProjectInfo ProjectInfo { get; set; }

        [JsonProperty("dependencies")]
        public Dependency[] Dependencies { get; set; }
    }

    public partial class Dependency
    {
        [JsonProperty("isVirtual")]
        public bool IsVirtual { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("sha256")]
        public string Sha256 { get; set; }

        [JsonProperty("evidenceCollected")]
        public EvidenceCollected EvidenceCollected { get; set; }

        [JsonProperty("packages", NullValueHandling = NullValueHandling.Ignore)]
        public Package[] Packages { get; set; }

        [JsonProperty("vulnerabilities", NullValueHandling = NullValueHandling.Ignore)]
        public Vulnerability[] Vulnerabilities { get; set; }
    }

    public partial class EvidenceCollected
    {
        [JsonProperty("vendorEvidence")]
        public Evidence[] VendorEvidence { get; set; }

        [JsonProperty("productEvidence")]
        public Evidence[] ProductEvidence { get; set; }

        [JsonProperty("versionEvidence")]
        public Evidence[] VersionEvidence { get; set; }
    }

    public partial class Evidence
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Package
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Vulnerability
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("cvssv2")]
        public Cvssv2 Cvssv2 { get; set; }

        [JsonProperty("cvssv3")]
        public Cvssv3 Cvssv3 { get; set; }

        [JsonProperty("cwes")]
        public string[] Cwes { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("references")]
        public Reference[] References { get; set; }

        [JsonProperty("vulnerableSoftware")]
        public VulnerableSoftware[] VulnerableSoftware { get; set; }
    }

    public partial class Cvssv2
    {
        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("accessVector")]
        public string AccessVector { get; set; }

        [JsonProperty("accessComplexity")]
        public string AccessComplexity { get; set; }

        [JsonProperty("authenticationr")]
        public string Authenticationr { get; set; }

        [JsonProperty("confidentialImpact")]
        public string ConfidentialImpact { get; set; }

        [JsonProperty("integrityImpact")]
        public string IntegrityImpact { get; set; }

        [JsonProperty("availabilityImpact")]
        public string AvailabilityImpact { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }
    }

    public partial class Cvssv3
    {
        [JsonProperty("baseScore")]
        public double BaseScore { get; set; }

        [JsonProperty("attackVector")]
        public string AttackVector { get; set; }

        [JsonProperty("attackComplexity")]
        public string AttackComplexity { get; set; }

        [JsonProperty("privilegesRequired")]
        public string PrivilegesRequired { get; set; }

        [JsonProperty("userInteraction")]
        public string UserInteraction { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("confidentialityImpact")]
        public string ConfidentialityImpact { get; set; }

        [JsonProperty("integrityImpact")]
        public string IntegrityImpact { get; set; }

        [JsonProperty("availabilityImpact")]
        public string AvailabilityImpact { get; set; }

        [JsonProperty("baseSeverity")]
        public string BaseSeverity { get; set; }
    }

    public partial class Reference
    {
        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class VulnerableSoftware
    {
        [JsonProperty("software")]
        public Software Software { get; set; }
    }

    public partial class Software
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("versionEndExcluding", NullValueHandling = NullValueHandling.Ignore)]
        public string VersionEndExcluding { get; set; }

        [JsonProperty("versionStartIncluding", NullValueHandling = NullValueHandling.Ignore)]
        public string VersionStartIncluding { get; set; }

        [JsonProperty("versionEndIncluding", NullValueHandling = NullValueHandling.Ignore)]
        public string VersionEndIncluding { get; set; }
    }

    public partial class ProjectInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("reportDate")]
        public DateTimeOffset ReportDate { get; set; }

        [JsonProperty("credits")]
        public Credits Credits { get; set; }
    }

    public partial class Credits
    {
        [JsonProperty("NVD")]
        public string Nvd { get; set; }

        [JsonProperty("NPM")]
        public string Npm { get; set; }

        [JsonProperty("RETIREJS")]
        public string Retirejs { get; set; }

        [JsonProperty("OSSINDEX")]
        public string Ossindex { get; set; }
    }

    public partial class ScanInfo
    {
        [JsonProperty("engineVersion")]
        public string EngineVersion { get; set; }

        [JsonProperty("dataSource")]
        public DataSource[] DataSource { get; set; }
    }

    public partial class DataSource
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }
}

