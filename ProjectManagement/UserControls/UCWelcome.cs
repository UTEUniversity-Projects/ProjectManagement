using ProjectManagement.Models;
using ProjectManagement.Utils;

namespace ProjectManagement
{
    public partial class UCWelcome : UserControl
    { 
        public UCWelcome()
        {
            InitializeComponent();
        }
        public UCWelcome(Users user)
        {
            InitializeComponent();

            gCirclePictureBoxAvatar.Image = WinformControlUtil.NameToImage(user.Avatar);
            lblViewHandle.Text = user.UserName;
            gTextBoxFullname.Text = user.FullName;
            gTextBoxWorkCode.Text = user.WorkCode;
            gTextBoxBirthday.Text = user.DateOfBirth.ToString("dd/MM/yyyy");
            gTextBoxEmail.Text = user.Email;
            gTextBoxPhonenumber.Text = user.PhoneNumber;
        }
    }
}
