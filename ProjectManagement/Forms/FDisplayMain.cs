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
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement
{
    public partial class FDisplayMain : Form
    {
        private UCDisplayWelcome uCDisplayWelcome = new UCDisplayWelcome();
        private UCDisplayLogin uCDisplayLogin = new UCDisplayLogin();
        private UCDisplayRegister uCDisplayRegister = new UCDisplayRegister();

        public FDisplayMain()
        {
            InitializeComponent();

            #region Record CLICK events

            gPanelDisplay.Controls.Add(uCDisplayWelcome);

            uCDisplayWelcome.GGradientButtonLecture.Click += DWelcomeButtonLecture_Click;
            uCDisplayWelcome.GGradientButtonStudent.Click += DWelcomeButtonStudent_Click;
            uCDisplayWelcome.GGradientButtonRegister.Click += DWelcomeButtonRegister_Click;
            uCDisplayWelcome.GButtonLogin.Click += DWelcomeButtonToLogin_Click;

            uCDisplayLogin.GButtonLogin.Click += DLoginButtonLogin_Click;
            uCDisplayLogin.GButtonBack.Click += DLoginButtonBack_Click;

            uCDisplayRegister.GButtonBack.Click += DRegisterBack_Click;
            uCDisplayRegister.GButtonLoadLogin.Click += DRegisterLoadLogin_Click;

            #endregion

            #region Record KEYDOWN events

            uCDisplayLogin.GTextBoxEmail.KeyDown += DLoginTextBoxEmail_KeyDown;
            uCDisplayLogin.GTextBoxPassword.KeyDown += DLoginTextBoxPassword_KeyDown;

            #endregion
        }

        #region FUNCTIONS

        private void SetDisplay(UserControl userControl)
        {
            gPanelDisplay.Controls.Clear();
            gPanelDisplay.Controls.Add(userControl);
        }
        private void SetNewDisplayUser(Users user)
        {
            UCDisplayUser uCDisplayUser = new UCDisplayUser();
            uCDisplayUser.SetInformation(user);
            uCDisplayUser.GButtonLogOut.Click += DButtonLogOut_Click;
            SetDisplay(uCDisplayUser);
        }

        #endregion

        #region EVENT CLICK

        private void DWelcomeButtonLecture_Click(object sender, EventArgs e)
        {
            Users user = UserDAO.SelectOnlyByID("242200001");
            SetNewDisplayUser(user);
        }
        private void DWelcomeButtonStudent_Click(object sender, EventArgs e)
        {
            Users user = UserDAO.SelectOnlyByID("243300002");
            SetNewDisplayUser(user);
        }
        private void DWelcomeButtonRegister_Click(object sender, EventArgs e)
        {
            uCDisplayRegister.InitDataControls();
            SetDisplay(uCDisplayRegister);
        }
        private void DWelcomeButtonToLogin_Click(object sender, EventArgs e)
        {
            uCDisplayLogin.InitDataControls();
            SetDisplay(uCDisplayLogin);
        }
        private void DButtonLogOut_Click(object sender, EventArgs e)
        {
            SetDisplay(uCDisplayWelcome);
        }
        private void DLoginButtonLogin_Click(object sender, EventArgs e)
        {
            
            string email = uCDisplayLogin.GTextBoxEmail.Text;
            string password = uCDisplayLogin.GTextBoxPassword.Text;
            string reminder = "email or password is incorrect !";

            Users user = UserDAO.SelectOnlyByEmailAndPassword(email, password);
            if (user == null)
            {
                uCDisplayLogin.GTextBoxReminder.Text = reminder;
            }
            else
            {
                uCDisplayLogin.GTextBoxReminder.Text = string.Empty;
                SetNewDisplayUser(user);
            }
        }
        private void DLoginButtonBack_Click(object sender, EventArgs e)
        {
            SetDisplay(uCDisplayWelcome);
        }
        private void DRegisterLoadLogin_Click(Object sender, EventArgs e)
        {
            uCDisplayLogin.InitDataControls();
            SetDisplay(uCDisplayLogin);
        }
        private void DRegisterBack_Click(object sender, EventArgs e)
        {
            uCDisplayRegister.FlagCheck = true;
            uCDisplayRegister.RunCheckUserInfor();
            SetDisplay(uCDisplayWelcome);
        }

        #endregion

        #region EVENT KEYDOWN

        private void DLoginTextBoxEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DLoginButtonLogin_Click(sender, e);
            }
        }
        private void DLoginTextBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DLoginButtonLogin_Click(sender, e);
            }
        }

        #endregion

    }
}
