using DependencyCheck.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DependencyCheck
{
    class IdentifyVulnerabilities
    {
        internal List<DependencyVulnerabilityDB> OWASPDependencyCheck(string projectName, string pathToProject, string outFromat)
        {
            string baseDirectory = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));

            // dependency-check.bat --project "index" --scan "C:\Users\profe\Desktop\index" --format "JSON"
            string strCmdText = baseDirectory + "resoures\\dependency-check\\bin\\dependency-check.bat " +
                "--project \""+ projectName + "\" --scan \"" + pathToProject + "\" --out \"" + baseDirectory.Remove(baseDirectory.Length - 1) + "\" --format \"" + outFromat + "\"";
            //Console.WriteLine(strCmdText);
            executeCommand(strCmdText,true,true,true);

            string rezult = File.ReadAllText(baseDirectory + "dependency-check-report.json");

            File.Delete(baseDirectory + "dependency-check-report.json");
            return ParseJSONFromOwaspDC(rezult, pathToProject+projectName);
        }

        private List<DependencyVulnerabilityDB> ParseJSONFromOwaspDC(string rezult, string filescaning)
        {
            JObject obj = JObject.Parse(rezult);
            var token = (JArray)obj.SelectToken("dependencies");

            GeneralInfo test = JsonConvert.DeserializeObject<GeneralInfo>(rezult);

            List<DependencyVulnerabilityDB> dependencyVulnerabilityDBs = (test.Dependencies.Where(x => x.Vulnerabilities != null)
                .Select((x) => new DependencyVulnerabilityDB
                {
                    dependency = (new DependencyDB { fileName = x.FileName, filePath = x.FilePath, name = x.Packages.First().Id }),
                    vulnerabilityDBs = x.Vulnerabilities.Select(x => new VulnerabilityDB
                    { name = x.Name, vulnerability = x.Cvssv3.BaseScore, description = x.Description }).ToList(),
                    dateTime = test.ProjectInfo.ReportDate.UtcDateTime,
                    fileScaning = filescaning
                })).ToList();

            return dependencyVulnerabilityDBs;
        }

        private static void executeCommand(string command, bool waitForExit = true, bool hideWindow = true, bool runAsAdministrator = false)
        {
            System.Diagnostics.ProcessStartInfo psi =
            new System.Diagnostics.ProcessStartInfo("cmd", "/C " + command);

            if (hideWindow)
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            
            if (runAsAdministrator)
                psi.Verb = "runas";

            if (waitForExit)
                System.Diagnostics.Process.Start(psi).WaitForExit();
            else
                System.Diagnostics.Process.Start(psi);
        }
    }
}
