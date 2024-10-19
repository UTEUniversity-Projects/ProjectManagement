using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ProjectManagement.DAOs;
using ProjectManagement.Database;
using ProjectManagement.MetaData;
using ProjectManagement.Models;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCTaskComment : UserControl
    {
        private Users user = new Users();
        private Users instructor = new Users();
        private Project project = new Project();
        private Tasks task = new Tasks();
        private Team team = new Team();
        private Comment comment = new Comment();

        private TeamDAO TeamDAO = new TeamDAO();
        private CommentDAO commentDAO = new CommentDAO();
        private NotificationDAO NotificationDAO = new NotificationDAO();

        private bool isProcessing = true;

        public UCTaskComment()
        {
            InitializeComponent();
        }

        #region FUNCTIONS

        public void SetUpUserControl(Users user, Users instructor, Project project, Tasks task, bool isProcessing)
        {
            this.user = user;
            this.task = task;
            this.project = project;
            this.instructor = instructor;
            this.isProcessing = isProcessing;
            // this.team = TeamDAO.SelectOnly(task.TeamId);
            InitUserControl();
        }
        private void InitUserControl()
        {
            gCirclePictureBoxCommentator.Image = WinformControlUtil.NameToImage(user.Avatar);
            if (!isProcessing)
            {
                gCirclePictureBoxCommentator.Hide();
                gTextBoxComment.Hide();
                ptbEmoji.Hide();
                gButtonSend.Hide();
                gSeparatorUnder.Location = new Point(14, 505);
                flpComment.Size = new Size(670, 435);
            }
            LoadTaskComment();
        }
        private void LoadTaskComment()
        {
            List<Comment> listComment = CommentDAO.SelectList(this.task.TaskId);

            flpComment.Controls.Clear();
            UCCommentLine line = new UCCommentLine();
            foreach (Comment comment in listComment)
            {
                line = new UCCommentLine(comment);
                flpComment.Controls.Add(line);
            }
            flpComment.ScrollControlIntoView(line);
        }

        #endregion

        #region EVENT COMMENT

        private void SendComment()
        {
            if (gTextBoxComment.Text != string.Empty)
            {
                this.comment = new Comment(gTextBoxComment.Text, DateTime.Now, user.UserId, task.TaskId);
                UCCommentLine line = new UCCommentLine(comment);
                flpComment.Controls.Add(line);
                flpComment.ScrollControlIntoView(line);
                CommentDAO.Insert(comment);

                List<Users> peoples = TeamDAO.GetMembersByTeamId(team.TeamId).Select(m => m.User).ToList();
                peoples.Add(this.instructor);
                string content = Notification.GetContentTypeComment(user.FullName, comment.Content, task.Title);
                NotificationDAO.InsertFollowTeam(this.team.TeamId, content, Enums.ENotificationType.COMMENT);

                gTextBoxComment.Clear();
            }
        }
        private void gButtonSend_Click(object sender, EventArgs e)
        {
            SendComment();
        }
        private void gTextBoxComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendComment();
            }
        }
        private void gTextBoxComment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }

        }

        #endregion

    }
}
