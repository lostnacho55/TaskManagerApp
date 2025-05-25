using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManagerApp.Forms
{
    public partial class StatsForm : Form
    {
        public StatsForm()
        {
            InitializeComponent();
        }

        private void StatsForm_Load(object sender, EventArgs e)
        {
            var adapter = new TaskDBDataSetTableAdapters.TasksTableAdapter();
            var db = new TaskDBDataSet();
            adapter.Fill(db.Tasks);

            int completed = db.Tasks.Count(t => t.IsCompleted);
            int pending = db.Tasks.Count(t => !t.IsCompleted);

            chartStats.Series.Clear();
            var series = chartStats.Series.Add("Tasks");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series.Points.AddXY("Completed", completed);
            series.Points.AddXY("Pending", pending);
        }
    }
}
