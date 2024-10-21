using Guna.UI2.WinForms;
using ProjectManagement.Models;
using ProjectManagement.DAOs;
using ProjectManagement.Enums;
using ProjectManagement.Utils;
using ProjectManagement.MetaData;

namespace ProjectManagement
{
    public partial class UCMeetingCreate : UserControl
    {
        public event EventHandler MeetingCreated;


        private Users host = new Users();
        private Users instructor = new Users();
        private Project project = new Project();
        private Team team = new Team();
        private Meeting meeting = new Meeting();

        private bool flagCheck = true;
        private EDatabaseOperation eOperation = new EDatabaseOperation();

        public UCMeetingCreate()
        {
            InitializeComponent();
            gDateTimePickerStart.Format = DateTimePickerFormat.Custom;
            gDateTimePickerStart.CustomFormat = "dd/MM/yyyy HH:mm:ss tt";
        }

        #region PROPERTIES

        public Guna2Button GButtonCancel
        {
            get { return this.gButtonCancel; }
        }
        public Meeting GetMeeting
        {
            get { return this.meeting; }
        }

        #endregion

        #region FUNCTIONS

        public void SetUpUserControl(Users host, Project project, Team team)
        {
            this.host = host;
            this.project = project;
            this.team = team;
            this.instructor = UserDAO.SelectOnlyByID(project.InstructorId);
            this.eOperation = EDatabaseOperation.CREATE;
            SetUpMode();
        }
        public void SetInformation(Meeting meeting, Users host, EDatabaseOperation eOperation)
        {
            this.meeting = meeting;
            this.host = host;
            this.eOperation = eOperation;
            this.project = ProjectDAO.SelectOnly(meeting.ProjectId);
            this.instructor = UserDAO.SelectOnlyByID(project.InstructorId);
            SetUpMode();
        }
        public void SetBackColor(Color color)
        {
            this.BackColor = color;
        }
        private void SetUpMode()
        {
            switch (this.eOperation)
            {
                case EDatabaseOperation.CREATE:
                    SetCreateMode();
                    break;
                case EDatabaseOperation.VIEW:
                    InitInformation();
                    lblCre.Show();
                    SetViewMode();
                    break;
                case EDatabaseOperation.EDIT:
                    InitInformation();
                    SetEditMode();
                    break;
            }
        }
        private void InitInformation()
        {
            gTextBoxTitle.Text = meeting.Title;
            gTextBoxDescription.Text = meeting.Description;
            gDateTimePickerStart.Value = meeting.StartAt;
            gTextBoxLocation.Text = meeting.Location;
            gTextBoxLink.Text = meeting.Link;
        }
        private void SetViewMode()
        {
            GunaControlUtil.SetTextBoxState(gTextBoxTitle, true);
            GunaControlUtil.SetTextBoxState(gTextBoxDescription, true);
            GunaControlUtil.SetTextBoxState(gTextBoxLocation, true);
            GunaControlUtil.SetTextBoxState(gTextBoxLink, true);
            gDateTimePickerStart.Enabled = false;

            Users creator = UserDAO.SelectOnlyByID(meeting.CreatedBy);
            lblCre.Text = "Created by " + creator.FullName + " at " + meeting.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss tt");
            gButtonCancel.Location = new Point(531, 447);
            gButtonCancel.Text = "Close";
            gButtonCancel.Show();
            gButtonCreate.Hide();
            lblCre.Show();
            gButtonCancel.Focus();
        }
        private void SetEditMode()
        {
            this.flagCheck = true;
            GunaControlUtil.SetTextBoxState(gTextBoxTitle, false);
            GunaControlUtil.SetTextBoxState(gTextBoxDescription, false);
            GunaControlUtil.SetTextBoxState(gTextBoxLocation, false);
            GunaControlUtil.SetTextBoxState(gTextBoxLink, false);
            gDateTimePickerStart.Enabled = true;
            gButtonCreate.Text = "Save";
            gButtonCancel.Show();
            gButtonCancel.Text = "Cancel";
            gButtonCreate.Show();
            lblCre.Hide();
        }
        public void SetCreateMode()
        {
            this.flagCheck = true;
            gTextBoxTitle.Clear();
            gTextBoxDescription.Clear();
            gTextBoxLocation.Clear();
            gTextBoxLink.Clear();
            gDateTimePickerStart.Value = DateTime.Now;
            lblCre.Hide();
        }
        private bool CheckInformationValid()
        {
            WinformControlUtil.RunCheckDataValid(meeting.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
            WinformControlUtil.RunCheckDataValid(meeting.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");
            WinformControlUtil.RunCheckDataValid(meeting.CheckStart() || flagCheck, erpStart, gDateTimePickerStart, "Invalid start time");
            WinformControlUtil.RunCheckDataValid(meeting.CheckLocation() || flagCheck, erpLocation, gTextBoxLocation, "Location cannot be empty");

            return meeting.CheckTitle() && meeting.CheckDescription() && meeting.CheckStart() && meeting.CheckLocation();
        }

        #endregion

        #region EVENT CREATE or EDIT

        private void SolveForCreate()
        {
            this.meeting = new Meeting(gTextBoxTitle.Text, gTextBoxDescription.Text, gDateTimePickerStart.Value, gTextBoxLocation.Text,
                gTextBoxLink.Text, DateTime.Now, host.UserId, project.ProjectId);
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                MeetingDAO.Insert(this.meeting);

                string content = Notification.GetContentTypeMeeting(meeting.Title, host.FullName);
                var peoples = new List<Users> { instructor };
                peoples.AddRange(TeamDAO.GetMembersByTeamId(team.TeamId).Select(m => m.User));
                NotificationDAO.InsertFollowPeoples(peoples, content, ENotificationType.MEETING);

                MeetingCreated?.Invoke(this.meeting, EventArgs.Empty);
                gButtonCancel.PerformClick();
            }
        }
        private void SolveForEdit()
        {
            this.meeting = new Meeting(this.meeting.MeetingId, gTextBoxTitle.Text, gTextBoxDescription.Text, gDateTimePickerStart.Value, gTextBoxLocation.Text,
                gTextBoxLink.Text, DateTime.Now, host.UserId, project.ProjectId);
            this.flagCheck = false;
            if (CheckInformationValid())
            {
                MeetingDAO.Update(this.meeting);

                string content = Notification.GetContentTypeMeetingUpdated(meeting.Title);
                var peoples = new List<Users> { instructor };
                peoples.AddRange(TeamDAO.GetMembersByTeamId(team.TeamId).Select(m => m.User));
                NotificationDAO.InsertFollowPeoples(peoples, content, ENotificationType.COMMENT);

                MeetingCreated?.Invoke(this.meeting, EventArgs.Empty);
                gButtonCancel.PerformClick();
            }
        }
        private void gButtonCreate_Click(object sender, EventArgs e)
        {
            if (this.eOperation == EDatabaseOperation.CREATE) SolveForCreate();
            else SolveForEdit();
        }

        #endregion

        #region EVENT TEXTCHANGED and VALUECHANGED

        private void gTextBoxTitle_TextChanged(object sender, EventArgs e)
        {
            this.meeting.Title = gTextBoxTitle.Text;
            WinformControlUtil.RunCheckDataValid(meeting.CheckTitle() || flagCheck, erpTitle, gTextBoxTitle, "Title cannot be empty");
        }
        private void gTextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            this.meeting.Description = gTextBoxDescription.Text;
            WinformControlUtil.RunCheckDataValid(meeting.CheckDescription() || flagCheck, erpDescription, gTextBoxDescription, "Description cannot be empty");

        }
        private void gTextBoxLocation_TextChanged(object sender, EventArgs e)
        {
            this.meeting.Location = gTextBoxLocation.Text;
            WinformControlUtil.RunCheckDataValid(meeting.CheckLocation() || flagCheck, erpLocation, gTextBoxLocation, "Location cannot be empty");
        }
        private void gDateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            this.meeting.StartAt = gDateTimePickerStart.Value;
            WinformControlUtil.RunCheckDataValid(meeting.CheckStart() || flagCheck, erpStart, gDateTimePickerStart, "Invalid start time");
        }

        #endregion

    }
}
