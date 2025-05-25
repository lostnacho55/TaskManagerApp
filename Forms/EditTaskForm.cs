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
    public partial class EditTaskForm : Form
    {
        public EditTaskForm()
        {
            InitializeComponent();
        }

        public void LoadTask(TaskDBDataSet.TasksRow task)
        {
            lblTaskID.Text = task.ID.ToString();
            txtTitle.Text = task.Title;
            txtDescription.Text = task.Description;
            dtpDueDate.Value = task.DueDate;
            dtpReminderTime.Value = task.ReminderTime;
            chkCompleted.Checked = task.IsCompleted;
        }

        private void EditTaskForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var adapter = new TaskDBDataSetTableAdapters.TasksTableAdapter();
            adapter.UpdateQuery(
                txtTitle.Text,
                txtDescription.Text,
                dtpDueDate.Value,
                dtpReminderTime.Value,
                chkCompleted.Checked,
                int.Parse(lblTaskID.Text)
            );

            MessageBox.Show("Task updated!");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
