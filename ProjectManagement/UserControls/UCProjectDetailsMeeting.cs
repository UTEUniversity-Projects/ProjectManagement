using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.DAOs;
using ProjectManagement.Enums;
using ProjectManagement.Models;

namespace ProjectManagement
{
    public partial class UCProjectDetailsMeeting : UserControl
    {
        private Users host = new Users();
        private Project project = new Project();
        private Team team = new Team();
        private MeetingDAO MeetingDAO = new MeetingDAO();
        private UCMeetingCreate uCMeetingCreate = new UCMeetingCreate();

        private List<Meeting> meetings = new List<Meeting>();

        public UCProjectDetailsMeeting()
        {
            InitializeComponent();
            InitUserControl();
        }
        private void InitUserControl()
        {
            flpMeetingList.Location = new Point(12, 12);
            uCMeetingCreate.Location = new Point(12, 12);
            uCMeetingCreate.GButtonCancel.Click += GButtonCancel_Click;
            gShadowPanelBack.Controls.Add(uCMeetingCreate);
            flpMeetingList.Show();
            uCMeetingCreate.Hide();
        }
        public void SetUpUserControl(Users host, Project project, Team team)
        {
            this.host = host;
            this.project = project;
            this.team = team;
            this.meetings.Clear();
            this.meetings.AddRange(MeetingDAO.SelectByProject(project.ProjectId));
            uCMeetingCreate.SetUpUserControl(host, project, team);
            uCMeetingCreate.MeetingCreated += UCMeetingCreate_MeetingCreated;

            foreach (var meeting in meetings)
            {
                UCMeetingCard uCMeetingCard = new UCMeetingCard(meeting, host);
                uCMeetingCard.MeetingCardDeleted += UCMeetingCard_MeetingCardDeleted;
                flpMeetingList.Controls.Add(uCMeetingCard);
            }

            if (this.project.Status == EProjectStatus.COMPLETED)
            {
                gGradientButtonAddMeeting.Hide();
            }
        }
        private void UCMeetingCreate_MeetingCreated(object? sender, EventArgs e)
        {
            Meeting meeting = MeetingDAO.SelectOnly(uCMeetingCreate.GetMeeting.MeetingId);

            if (meeting != null)
            {
                this.meetings.Add(meeting);
                UCMeetingCard card = new UCMeetingCard(meeting, host);
                card.MeetingCardDeleted += UCMeetingCard_MeetingCardDeleted;
                flpMeetingList.Controls.Add(card);
                flpMeetingList.Controls.SetChildIndex(card, 0);
            }
        }
        private void UCMeetingCard_MeetingCardDeleted(object? sender, EventArgs e)
        {
            UCMeetingCard card = sender as UCMeetingCard;

            if (card != null)
            {
                MeetingDAO.Delete(card.GetMeeting);

                foreach (Control control in flpMeetingList.Controls)
                {
                    if (control.GetType() == typeof(UCMeetingCard))
                    {
                        UCMeetingCard meetingCard = (UCMeetingCard)control;
                        if (meetingCard == card)
                        {
                            flpMeetingList.Controls.Remove(control);
                            this.meetings.Remove(card.GetMeeting);
                            control.Dispose();
                            break;
                        }
                    }
                }
            }
        }
        private void GButtonCancel_Click(object? sender, EventArgs e)
        {
            lblMeetingList.Text = "MEETING LIST";
            flpMeetingList.Show();
            uCMeetingCreate.Hide();
        }
        private void gGradientButtonAddMeeting_Click(object sender, EventArgs e)
        {
            lblMeetingList.Text = "CREATE MEETING";
            flpMeetingList.Hide();
            uCMeetingCreate.SetCreateMode();
            uCMeetingCreate.Show();
        }
    }
}
