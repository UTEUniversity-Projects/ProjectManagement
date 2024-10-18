using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectManagement.Database;
using ProjectManagement.Models;
using ProjectManagement.Process;

namespace ProjectManagement
{
    public partial class FProjectEdit : Form
    {
        private UCProjectCreate uCProjectCreate = new UCProjectCreate();

        public FProjectEdit(Users user, Project project)
        {
            InitializeComponent();
            InitUserControl(user, project);
        }
        private void InitUserControl(Users user, Project project)
        {
            gPanelEdit.Controls.Clear();
            uCProjectCreate.SetEditState(user, project);
            gPanelEdit.Controls.Add(uCProjectCreate);

            uCProjectCreate.GButtonCancel.Click += ButtonCancel_Clicked;
        }
        private void ButtonCancel_Clicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
