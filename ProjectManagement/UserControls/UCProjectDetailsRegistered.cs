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

namespace ProjectManagement
{
    public partial class UCProjectDetailsRegistered : UserControl
    {
        public UCProjectDetailsRegistered()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            flpTeamRegistered.Controls.Clear();
        }
        public void AddTeam(UCTeamLine line)
        {
            flpTeamRegistered.Controls.Add(line);
        }
    }
}
