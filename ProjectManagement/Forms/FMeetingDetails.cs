using ProjectManagement.Models;
using ProjectManagement.Enums;

namespace ProjectManagement.Forms
{
    public partial class FMeetingDetails : Form
    {
        public event EventHandler MeetingUpdated;
        private Meeting meeting = new Meeting();

        public FMeetingDetails()
        {
            InitializeComponent();
        }
        public FMeetingDetails(Meeting meeting, Users host)
        {
            InitializeComponent();

            this.meeting = meeting;
            UCMeetingCreate uCMeetingCreate = new UCMeetingCreate();
            if (meeting.CreatedBy == host.UserId)
            {
                uCMeetingCreate.SetInformation(meeting, host, EDatabaseOperation.EDIT);
                this.BackColor = SystemColors.ButtonFace;
                uCMeetingCreate.SetBackColor(SystemColors.ButtonFace);
            }
            else
            {
                uCMeetingCreate.SetInformation(meeting, host, EDatabaseOperation.VIEW);
                this.BackColor = Color.White;
                uCMeetingCreate.SetBackColor(Color.White);
            }

            uCMeetingCreate.GButtonCancel.Click += GButtonCancel_Click;
            uCMeetingCreate.MeetingCreated += UCMeetingCreate_MeetingCreated;
            flpBackground.Controls.Add(uCMeetingCreate);
        }
        private void UCMeetingCreate_MeetingCreated(object? sender, EventArgs e)
        {
            MeetingUpdated?.Invoke(this.meeting, e);
        }
        private void GButtonCancel_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
