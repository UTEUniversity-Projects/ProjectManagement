using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.Models;
using ProjectManagement.DAOs;
using ProjectManagement.Enums;
using ProjectManagement.MetaData;

namespace ProjectManagement
{
    public partial class UCTaskEvaluateList : UserControl
    {
        public event EventHandler ClickEvaluate;
        private Project project = new Project();
        private Tasks task = new Tasks();
        private Team team = new Team();
        private UCUserMiniLine peopleLine = new UCUserMiniLine();

        public UCTaskEvaluateList()
        {
            InitializeComponent();
        }
        public UCUserMiniLine GetUserLine
        {
            get { return this.peopleLine; }
        }
        public void SetUpUserControl(Project project, Tasks task, Team team, Users user)
        {
            this.project = project;
            this.task = task;
            this.team = team;
            flpMembers.Controls.Clear();
            if (user.Role == EUserRole.LECTURE) LoadListRoleLecture();
            else AddUserMiniLine(user);
        }
        private void LoadListRoleLecture()
        {
            foreach (Member member in TaskStudentDAO.GetMembersByTaskId(this.task.TaskId))
            {
                AddUserMiniLine(member.User);
            }
        }
        private void AddUserMiniLine(Users student)
        {
            UCUserMiniLine line = new UCUserMiniLine(student);
            line.SetSize(new Size(610, 60));
            line.SetDeleteMode(false);

            Evaluation evaluation = EvaluationDAO.SelectOnly(task.TaskId, student.UserId);
            line.SetEvaluateMode(evaluation, true);
            line.ClickEvaluate += Line_ClickEvaluate;
            flpMembers.Controls.Add(line);
        }
        private void Line_ClickEvaluate(object sender, EventArgs e)
        {
            UCUserMiniLine line = sender as UCUserMiniLine;
            if (line != null)
            {
                this.peopleLine = line;
            }
            OnClickEvaluate(EventArgs.Empty);
        }
        private void OnClickEvaluate(EventArgs e)
        {
            ClickEvaluate?.Invoke(this, e);
        }
    }
}
