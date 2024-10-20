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
using ProjectManagement.Forms;
using ProjectManagement.Models;
using ProjectManagement.Process;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCMeetingCard : UserControl
    {
        public event EventHandler MeetingCardDeleted;
        

        private Users host = new Users();
        private Users creator = new Users();
        private Meeting meeting = new Meeting();

        public UCMeetingCard()
        {
            InitializeComponent();
        }
        public UCMeetingCard(Meeting meeting, Users host)
        {
            InitializeComponent();

            this.meeting = meeting;
            this.host = host;
            SetInformation();
        }
        public Meeting GetMeeting
        {
            get { return this.meeting; }
        }
        private void SetInformation()
        {
            DateTime start = meeting.StartAt;
            gTextBoxWeekdays.Text = start.ToString("dddd").ToUpper();
            gTextBoxDay.Text = start.ToString("dd");
            gTextBoxMonth.Text = start.ToString("MMMM").ToUpper();
            lblTitle.Text = DataTypeUtil.FormatStringLength(meeting.Title, 30);
            gTextBoxLocation.Text = meeting.Location;
            lblTimeStart.Text = meeting.StartAt.ToString("dd/MM/yyyy HH:mm:ss tt");

            this.creator = UserDAO.SelectOnlyByID(meeting.CreatedBy);
            lblCre.Text = "Created by " + creator.FullName + " at " + meeting.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss tt");

            if (meeting.CreatedBy == host.UserId) gButtonDelete.Show();
            else gButtonDelete.Hide();
        }
        private void ShowMeetingInformation()
        {
            FMeetingDetails fMeetingDetails = new FMeetingDetails(this.meeting, this.host);
            fMeetingDetails.MeetingUpdated += FMeetingDetails_MeetingUpdated;
            fMeetingDetails.ShowDialog();
        }
        private void FMeetingDetails_MeetingUpdated(object? sender, EventArgs e)
        {
            this.meeting = MeetingDAO.SelectOnly(this.meeting.MeetingId);
            SetInformation();
        }
        private void gShadowPanelTeam_Click(object sender, EventArgs e)
        {
            ShowMeetingInformation();
        }
        private void lblSeeMore_Click(object sender, EventArgs e)
        {
            ShowMeetingInformation();
        }
        private void gButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this meeting",
                                                    "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                MeetingCardDeleted?.Invoke(this, e);
            }
        }
    }
}
