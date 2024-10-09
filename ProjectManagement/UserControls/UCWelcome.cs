﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement
{
    public partial class UCWelcome : UserControl
    {
        private MyProcess myProcess = new MyProcess();

        public UCWelcome()
        {
            InitializeComponent();
        }
        public UCWelcome(User people)
        {
            InitializeComponent();

            gCirclePictureBoxAvatar.Image = myProcess.NameToImage(people.AvatarName);
            lblViewHandle.Text = people.Handle;
            gTextBoxFullname.Text = people.FullName;
            gTextBoxCitizencode.Text = people.CitizenCode;
            gTextBoxBirthday.Text = people.Birthday.ToString("dd/MM/yyyy");
            gTextBoxEmail.Text = people.Email;
            gTextBoxPhonenumber.Text = people.PhoneNumber;
        }
    }
}
