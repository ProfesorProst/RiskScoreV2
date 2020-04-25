using System;
using System.Windows;
using System.Threading;
using DependencyCheck.Models;
using RiskScore.Entity;
using RiskScore.Controller;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using DependencyCheck.Entity;
using System.Text.RegularExpressions;
using RiskScore.Models;
using Microsoft.Win32;
using System.IO;

namespace DependencyCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBoxOutputter outputter;
        ProcessDepend processDepend;
        List<DependencyVulnerabilityDB> dependencyVulnerabilityDBs;
        CRUDDepenVulnDB cdv;
        BackgroundWorker bgWorker = new BackgroundWorker();
        TelegramBotControler telegramBotControler = new TelegramBotControler();


        string projectName = "index";
        string pathToProject = "C:\\Users\\profe\\Desktop\\index";
        string outFromat = "JSON";
        public MainWindow()
        {
            InitializeComponent(); 
            outputter = new TextBoxOutputter(TestBox);
            Console.SetOut(outputter);

            RiskRules riskRules = new RiskRules();
            processDepend = new ProcessDepend(riskRules);

            bgWorker.DoWork += bw_DoWork;
            bgWorker.WorkerReportsProgress = true;        
            bgWorker.WorkerSupportsCancellation = true;   
            bgWorker.ProgressChanged += bw_ProgressChanged;
            bgWorker.RunWorkerCompleted += bw_RunWorkerCompleted;

            SliderOfUsers.Minimum = 0;
            SliderOfUsers.Maximum = 1;
            progressBarStatus.Maximum = (telegramBotControler.UserCount() > 1) ? telegramBotControler.UserCount() * 3 :3;
            progressBarStatus.Minimum = 0;
            progressBarStatus.Value = 0;
        }

        void MySlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {

        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Ref<string> rezult = new Ref<string>();
            Ref<string> rezultOld = new Ref<string>();
            telegramBotControler.bw_DoWork(rezult);
            while (true)
            {
            if (bgWorker.CancellationPending)   
            {                                   
                bgWorker.CancelAsync();
                return;     
            }
            Thread.Sleep(1);

                if(rezultOld.Value != rezult.Value)
                {
                    rezultOld.Value = rezult.Value;
                    bgWorker.ReportProgress(0,rezult.Value);
                }
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (new Regex(@"^[+]\d,").IsMatch(e.UserState.ToString()))
            {
                progressBarStatus.Value++;

                if (progressBarStatus.Value == progressBarStatus.Maximum)
                    bgWorker.CancelAsync();
            }
        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Worker completed");
            ModelVulnerabilityDB modelVulnerabilityDB = new ModelVulnerabilityDB();
            var emptyVuln = modelVulnerabilityDB.GetAllEmptyVulnerabilities();
            var userVulnerabilityDBs = new CRUDUserVulnerabilitesDB().GetObjects().ToList();
            AntColonyPheromone antColony = new AntColonyPheromone();
            foreach (var item in emptyVuln)
            {
                var userVulns = userVulnerabilityDBs.Where(x => x.id == item.id).ToList();
                int[][] jArray = new int[userVulns.Count][];
                int index = 0;   
                
                foreach (var userVuln in userVulns)
                {
                    jArray[index] = new int[] { (int)userVuln.threats.GetValueOrDefault(),
                        (int)userVuln.techDamage.GetValueOrDefault(), (int)userVuln.bizDamage.GetValueOrDefault() };
                    index++;
                }
                var rezult = antColony.Calculation(jArray);

                item.threats = rezult[0];
                item.techDamage = rezult[1];
                item.bizDamage = rezult[2];

                modelVulnerabilityDB.UpdateVulnerabilityDB(item);
            }
            
            DateTime dateTime = new DateTime();
            List<RiskScoreEntities> riskScoreEntities = new List<RiskScoreEntities>();

            dependencyVulnerabilityDBs = cdv.GetList();

            dependencyVulnerabilityDBs = dependencyVulnerabilityDBs.Where(x => x.fileScaning == pathToProject + projectName)
                .OrderBy(x => x.dateTime).ToList();

            foreach (DependencyVulnerabilityDB dependencyVulnerabilityDB in dependencyVulnerabilityDBs.ToList())
            {
                List<VulnerabilityDB> vulnerabilitiesNew = new List<VulnerabilityDB>();
                double sum = 0;

                foreach (VulnerabilityDB vulnerability in dependencyVulnerabilityDB.vulnerabilityDBs)
                {
                    vulnerabilitiesNew.Add(processDepend.SetParamsConsole(vulnerability));
                    sum += vulnerability.rezult.GetValueOrDefault();
                }

                dependencyVulnerabilityDB.vulnerabilityDBs = vulnerabilitiesNew;
                cdv.Save(dependencyVulnerabilityDB);

                if (dependencyVulnerabilityDB.dateTime != dateTime)
                {
                    dateTime = dependencyVulnerabilityDB.dateTime;
                    RiskScoreEntities risk = new RiskScoreEntities(dateTime);
                    riskScoreEntities.Add(risk);
                }

                riskScoreEntities.Find(x => x.dateTime == dateTime).AddDependencyVulnerabilityDBs(dependencyVulnerabilityDB);
                riskScoreEntities.Find(x => x.dateTime == dateTime).score += sum;
            }

            foreach (RiskScoreEntities riskScore in riskScoreEntities)
                Console.WriteLine(riskScore.score);
            Console.ReadLine();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IdentifyVulnerabilities identifyVulnerabilities = new IdentifyVulnerabilities();
            dependencyVulnerabilityDBs = identifyVulnerabilities.OWASPDependencyCheck(projectName, pathToProject, outFromat);

            cdv = new CRUDDepenVulnDB();
            cdv.SaveList(dependencyVulnerabilityDBs);
            //Console.Read();

            dependencyVulnerabilityDBs = cdv.GetList();

            dependencyVulnerabilityDBs = dependencyVulnerabilityDBs.Where(x => x.fileScaning == pathToProject + projectName)
                .OrderBy(x => x.dateTime).ToList();

            foreach (DependencyVulnerabilityDB dependencyVulnerabilityDB in dependencyVulnerabilityDBs.ToList())
                if (processDepend.CheckIfNeedParams(dependencyVulnerabilityDB.vulnerabilityDBs))
                {
                    Start.IsEnabled = false;
                    SliderOfUsers.IsEnabled = false;
                    SliderOfUsers.Maximum = 1; //!!!!!!!!
                    break;
                }


            int usrCount = telegramBotControler.UserCount();
            int emptyVulner = telegramBotControler.GetAllEmptyVulnerabilitiesCount();
            int countOfWork = usrCount * emptyVulner * 3;
            SliderOfUsers.Maximum = countOfWork;
            progressBarStatus.Maximum = (countOfWork > 1) ? countOfWork : 1;

            Start.IsEnabled = true;
            Cansel.IsEnabled = true;

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {


            telegramBotControler.SendStartMessagesToAll();
            bgWorker.RunWorkerAsync();
        }

        private void Cansel_Click(object sender, RoutedEventArgs e)
        {
            bgWorker.CancelAsync();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = " files (*.html)|*.html|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                programPath.Text = openFileDialog.FileName;
                projectName = openFileDialog.SafeFileName;
                pathToProject = programPath.Text.Replace(projectName, "");
            }                
        }
    }
}
