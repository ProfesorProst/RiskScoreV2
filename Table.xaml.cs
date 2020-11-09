using Microsoft.Win32;
using RiskScore.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RiskScoreV2
{
    /// <summary>
    /// Interaction logic for Table.xaml
    /// </summary>
    public partial class Table : Window
    {
        public Table(string fullPathToProject)
        {
            InitializeComponent();

            var dependencyVulnerabilityDBs = new CRUDDepenVulnDB().GetList();
            var vulnerabilityDBs = new CRUDVulnerabilityDB().GetObjects();

            dependencyVulnerabilityDBs = dependencyVulnerabilityDBs.Where(x => x.fileScaning == fullPathToProject)
                .OrderBy(x => x.dateTime).ToList();

            DataContext = from p in dependencyVulnerabilityDBs
                          from t in p.vulnerabilityDBs
                          select new { ProjectName = p.dependency.name, TaskName = t.name, Test = Math.Round(t.rezult.GetValueOrDefault(), 2), Complete = t.description };
        }
        private void SaveExal(object sender, RoutedEventArgs e)
        {
            vulnerGrid.SelectAllCells();
            vulnerGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, vulnerGrid);
            String resultat = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            String result = (string)Clipboard.GetData(DataFormats.Text);
            vulnerGrid.UnselectAllCells();

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "unknown.xls";
            savefile.Filter = "Text files (*.xls)|*.xls";

            if (savefile.ShowDialog() == true)
            {
                System.IO.StreamWriter file1 = new System.IO.StreamWriter(savefile.FileName);
                file1.WriteLine(result.Replace(',', ' '));
                file1.Close();
            }

            MessageBox.Show(" Exporting DataGrid data to Excel file created.xls");
        }
    }
}
