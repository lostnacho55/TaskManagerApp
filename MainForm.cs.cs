using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskManagerApp.Forms;

namespace TaskManagerApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadTasks()
        {
            var adapter = new TaskDBDataSetTableAdapters.TasksTableAdapter();
            var db = new TaskDBDataSet();
            adapter.Fill(db.Tasks);
            dgvTasks.DataSource = db.Tasks;
        }

        private void ExportTasksToFile()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text File|*.txt";
            saveDialog.Title = "Export Tasks";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    foreach (TaskDBDataSet.TasksRow task in taskDBDataSet.Tasks)
                    {
                        writer.WriteLine($"Title: {task.Title}");
                        writer.WriteLine($"Due: {task.DueDate}");
                        writer.WriteLine($"Reminder: {task.ReminderTime}");
                        writer.WriteLine($"Completed: {task.IsCompleted}");
                        writer.WriteLine(new string('-', 40));
                    }
                }

                MessageBox.Show("Tasks exported successfully!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'taskDBDataSet.Tasks' table. You can move, or remove it, as needed.
            this.tasksTableAdapter.Fill(this.taskDBDataSet.Tasks);

        }

        private void seeAllTasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tasksTableAdapter.Fill(this.taskDBDataSet.Tasks);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddTaskForm addForm = new AddTaskForm();
            addForm.FormClosed += (s, args) =>
            {
                this.tasksTableAdapter.Fill(this.taskDBDataSet.Tasks);
            };
            addForm.ShowDialog();
        }


        private void reminderTimer_Tick(object sender, EventArgs e)
        {
            // Load the latest data
            this.tasksTableAdapter.Fill(this.taskDBDataSet.Tasks);

            DateTime now = DateTime.Now;

            foreach (TaskDBDataSet.TasksRow task in taskDBDataSet.Tasks)
            {
                // If the task is not completed and the reminder time is in the past 1 minute
                if (!task.IsCompleted && task.ReminderTime >= now.AddMinutes(-1) && task.ReminderTime <= now)
                {
                    MessageBox.Show(
                        $"Reminder: Task '{task.Title}' is due soon!\nDue: {task.DueDate.ToShortTimeString()}",
                        "Task Reminder",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            AddTaskForm addForm = new AddTaskForm();
            addForm.FormClosed += (s, args) =>
            {
                this.tasksTableAdapter.Fill(this.taskDBDataSet.Tasks);
            };
            addForm.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTasks.CurrentRow == null)
            {
                MessageBox.Show("Please select a task.");
                return;
            }

            var row = ((DataRowView)dgvTasks.CurrentRow.DataBoundItem).Row as TaskDBDataSet.TasksRow;

            EditTaskForm editForm = new EditTaskForm();
            editForm.LoadTask(row);
            editForm.FormClosed += (s, args) => this.tasksTableAdapter.Fill(this.taskDBDataSet.Tasks);
            editForm.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTasks.CurrentRow == null)
            {
                MessageBox.Show("Please select a task to delete.");
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                var row = ((DataRowView)dgvTasks.CurrentRow.DataBoundItem).Row as TaskDBDataSet.TasksRow;
                var adapter = new TaskDBDataSetTableAdapters.TasksTableAdapter();
                adapter.DeleteQuery(row.ID);
                this.tasksTableAdapter.Fill(this.taskDBDataSet.Tasks);
            }
        }

        private void graphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StatsForm statsForm = new StatsForm();
            statsForm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportTasksToFile();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTaskForm addForm = new AddTaskForm();
            addForm.FormClosed += (s, args) =>
            {
                this.tasksTableAdapter.Fill(this.taskDBDataSet.Tasks);
            };
            addForm.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Cum sa folosesti aplicatia\n\n" +
                "- Add Task: Adaugi un nou task cu due date si reminder.\n" +
                "- Edit Task: Modifici un task deja adaugat.\n" +
                "- Delete Task: Stergi un task din lista.\n" +
                "- View All: Reincarca toate task-urile din baza de date.\n" +
                "- Reminder: Te anunta cand un task trebuie facut.\n" +
                "- Export: Salveaza toate task-urile intr-un fisier text.\n" +
                "- Statistici: Vezi cate task-uri sunt terminate si cate nu.\n" +
                "- Exit: Inchide aplicatia.",
                "Ajutor",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
