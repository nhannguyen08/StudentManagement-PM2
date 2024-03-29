﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1.StudentManagement
{
    public partial class IndexForm : Form
    {
        private LogicLayer Business;
        //private Student studentview;
        public IndexForm()
        {
            InitializeComponent();
            this.Business = new LogicLayer();
            this.Load += IndexForm_Load;
            this.btnAdd.Click += btnCreate_Click;
            this.btnDelete.Click += btnDelete_Click;
            this.grdStudent.DoubleClick += grdStudents_DoubleClick;
            this.btnStatistic.Click += BtnStatistic_Click;
            txtSearch.Text = "Please enter code";
            txtSearch.ForeColor = Color.Gray;
            this.txtSearch.Leave += TxtSearch_Leave;
            this.txtSearch.Enter += TxtSearch_Enter;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnAddClass.Click += BtnAddClass_Click;
            this.btnAddFaculty.Click += BtnAddFaculty_Click;
            this.btnSortForm.Click += BtnSortForm_Click;
        }

        private void BtnSortForm_Click(object sender, EventArgs e)
        {
            new SortForm().ShowDialog();
            this.ShowAllStudent();
        }

        private void BtnAddFaculty_Click(object sender, EventArgs e)
        {
            new CreateFacultyForm().ShowDialog();
            this.ShowAllStudent();
        }

        private void BtnAddClass_Click(object sender, EventArgs e)
        {
            new CreateClassForm().ShowDialog();
            this.ShowAllStudent(); 
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            grdStudent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in grdStudent.Rows)
                {
                    if (row.Cells[1].Value.ToString().Equals(txtSearch.Text.ToLower()))
                    {
                        row.Selected = true;
                        grdStudent.CurrentCell = row.Cells[1];
                        return;
                    }
                }
                throw new Exception();
            }
            catch (Exception)
            {
                MessageBox.Show("This code does not exist");
                grdStudent.ClearSelection();
            }
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Please enter code")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                txtSearch.Text = "Please enter code";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void BtnStatistic_Click(object sender, EventArgs e)
        {
            new StatisticForm().ShowDialog();
            this.ShowAllStudent();
        }

        void grdStudents_DoubleClick(object sender, EventArgs e)
        {
            if (this.grdStudent.SelectedRows.Count == 1)
            {
                var row = this.grdStudent.SelectedRows[0];
                var studentView = (StudentView)row.DataBoundItem;

                new UpdateForm(studentView.Id).ShowDialog();
            }
            this.ShowAllStudent();
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.grdStudent.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Do you want to delete this!!", "Comfirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    var @student = (StudentView)this.grdStudent.SelectedRows[0].DataBoundItem;
                    this.Business.DeleteStudent(@student.Id);
                    MessageBox.Show("Delete class successfully!!");
                    this.ShowAllStudent();
                }
            }
        }

        void btnCreate_Click(object sender, EventArgs e)
        {
            new CreateForm().ShowDialog();
            this.ShowAllStudent();
        }

        private void ShowAllStudent()
        {
            var students = this.Business.GetStudents();
            var studentViews = new StudentView[students.Length];
            for (int i = 0; i < students.Length; i++)
                studentViews[i] = new StudentView(students[i]);
            this.grdStudent.DataSource = studentViews;

            this.grdStudent.Columns[0].HeaderText = "ID";
            this.grdStudent.Columns[1].HeaderText = "Student Code";
            this.grdStudent.Columns[2].HeaderText = "Full name";
            this.grdStudent.Columns[3].HeaderText = "Date of Birth";
            this.grdStudent.Columns[4].HeaderText = "Class";
            this.grdStudent.Columns[5].HeaderText = "Email";
            this.grdStudent.Columns[6].HeaderText = "Home Town";
            this.grdStudent.Columns[7].HeaderText = "Faculty ID";
            this.grdStudent.Columns[8].HeaderText = "Average Score";
        }
        void IndexForm_Load(object sender, EventArgs e)
        {
            this.ShowAllStudent();
        }
    }
}
