
using System;
using System.Windows;
using System.Threading;
using DependencyCheck.Models;
using RiskScore.Entity;
using RiskScore.Controller;
using System.Collections.Generic;
using DependencyCheck.Controller;
using System.Linq;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

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
        ControllerDepenVulnDB cdv;
        BackgroundWorker bgWorker = new BackgroundWorker();
        TelegramBotControler telegramBotControler = new TelegramBotControler();
        public MainWindow()
        {
            InitializeComponent(); 
            outputter = new TextBoxOutputter(TestBox);
            Console.SetOut(outputter);

            RiskRules riskRules = new RiskRules();
            processDepend = new ProcessDepend(riskRules);

            bgWorker.DoWork += bw_DoWork;
            bgWorker.WorkerReportsProgress = true;        // needed to be able to report progress
            bgWorker.WorkerSupportsCancellation = true;   // needed to be able to stop the thread using CancelAsync();
            bgWorker.ProgressChanged += bw_ProgressChanged;
            bgWorker.RunWorkerCompleted += bw_RunWorkerCompleted;

            // ProgressBar is added to the form manually, and here I am just setting some initial values
            SliderOfUsers.Minimum = 0;
            SliderOfUsers.Maximum = telegramBotControler.UserCount();
            progressBarStatus.Maximum = (telegramBotControler.UserCount() > 1) ? telegramBotControler.UserCount() : 1;
            progressBarStatus.Minimum = 0;
            progressBarStatus.Value = 0;
        }

        void MySlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {

        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            telegramBotControler.bw_DoWork(sender, e);
            while (true)
            {
            if (bgWorker.CancellationPending)   
            {                                   
                bgWorker.CancelAsync();
                return;     
            }
            Thread.Sleep(1);


            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ( e.UserState == "+1")
            {
                progressBarStatus.Value++;

            }
            else
            {
                
            }
        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Worker completed");

            DateTime dateTime = new DateTime();
            List<RiskScoreEntities> riskScoreEntities = new List<RiskScoreEntities>();

            foreach (DependencyVulnerabilityDB dependencyVulnerabilityDB in dependencyVulnerabilityDBs.ToList())
            {
                List<VulnerabilityDB> vulnerabilitiesNew = new List<VulnerabilityDB>();
                double sum = 0;

                /*
                if (processDepend.CheckIfNeedParams(dependencyVulnerabilityDB.vulnerabilityDBs))
                {
                    
                }
                */

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
            string projectName = "index";
            string pathToProject = "C:\\Users\\profe\\Desktop\\index";
            string outFromat = "JSON";
            IdentifyVulnerabilities identifyVulnerabilities = new IdentifyVulnerabilities();
            dependencyVulnerabilityDBs = identifyVulnerabilities.OWASPDependencyCheck(projectName, pathToProject, outFromat);

            cdv = new ControllerDepenVulnDB();
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
    }
}
