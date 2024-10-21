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
        private Tasks task = new Tasks();
        private List<Member> assignedStudent = new List<Member>();
        private UCUserMiniLine peopleLine = new UCUserMiniLine();

        public UCTaskEvaluateList()
        {
            InitializeComponent();
        }
        public UCUserMiniLine GetUserLine
        {
            get { return this.peopleLine; }
        }
        public void SetUpUserControl(Tasks task, Users user)
        {
            this.task = task;
            this.assignedStudent = TeamDAO.GetMembersByTaskId(this.task.TaskId);

            flpMembers.Controls.Clear();
            if (user.Role == EUserRole.LECTURE)
            {
                LoadListRoleLecture();
            }
            else
            {
                if (this.assignedStudent.FirstOrDefault(s => s.User.UserId == user.UserId) != null)
                {
                    AddUserMiniLine(user);
                }
            }
        }
        private void LoadListRoleLecture()
        {
            foreach (Member member in this.assignedStudent)
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
