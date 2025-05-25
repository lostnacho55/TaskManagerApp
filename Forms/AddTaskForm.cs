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
    public partial class AddTaskForm : Form
    {
        public AddTaskForm()
        {
            InitializeComponent();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Access the DataSet and TableAdapter created by the wizard
                var db = new TaskDBDataSet(); // Auto-generated name
                var adapter = new TaskDBDataSetTableAdapters.TasksTableAdapter();

                // Insert a new row into the database
                adapter.Insert(
                    txtTitle.Text,
                    txtDescription.Text,
                    dtpDueDate.Value,
                    dtpReminderTime.Value,
                    chkCompleted.Checked
                );

                MessageBox.Show("Task added successfully!");

                // Close the AddTaskForm after saving
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
