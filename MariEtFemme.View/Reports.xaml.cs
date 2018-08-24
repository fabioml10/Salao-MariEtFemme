using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Reporting.WinForms;
using MariEtFemme.BLL;
using MariEtFemme.DTO;

namespace MariEtFemme.View
{
    public partial class Reports : Window
    {
        public Reports()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool _isReportViewerLoaded;

        private void reportTeste_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                ReportDataSource reportDataSource1 = new ReportDataSource();

                ProdutoBLL produtoBLL = new ProdutoBLL();
                DataTable dt = new DataTable();
                dt = produtoBLL.ReadNameTeste(string.Empty);

                reportDataSource1.Name = "DataSet1";

                reportDataSource1.Value = dt;
                reportTeste.LocalReport.DataSources.Add(reportDataSource1);

                reportTeste.LocalReport.ReportPath = "../../Report2.rdlc";

                reportTeste.RefreshReport();
                _isReportViewerLoaded = true;
            }
        }
    }
}