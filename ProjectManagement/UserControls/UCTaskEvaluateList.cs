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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace ProjectManagement
{
    public partial class UCTaskEvaluateList : UserControl
    {
        public event EventHandler ClickEvaluate;
        private Project thesis = new Project();
        private Tasks tasks = new Tasks();
        private Team team = new Team();
        private UCUserMiniLine peopleLine = new UCUserMiniLine();
        private EvaluationDAO evaluationDAO = new EvaluationDAO();

        public UCTaskEvaluateList()
        {
            InitializeComponent();
        }
        public UCUserMiniLine GetPeopleLine
        {
            get { return this.peopleLine; }
        }
        public void SetUpUserControl(Project thesis, Tasks tasks, Team team, User people)
        {
            this.thesis = thesis;
            this.tasks = tasks;
            this.team = team;
            flpMembers.Controls.Clear();
            if (people.Role == ERole.Lecture) LoadListRoleLecture();
            else AddPeopleMiniLine(people);
        }
        private void LoadListRoleLecture()
        {
            foreach (User people in team.Members)
            {
                AddPeopleMiniLine(people);
            }
        }
        private void AddPeopleMiniLine(User student)
        {
            UCUserMiniLine line = new UCUserMiniLine(student);
            line.SetSize(new Size(610, 60));
            line.SetDeleteMode(false);

            Evaluation evaluation = evaluationDAO.SelectOnly(tasks.IdTask, student.IdAccount);
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
