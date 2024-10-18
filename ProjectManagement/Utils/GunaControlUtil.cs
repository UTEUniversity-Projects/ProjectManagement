using Guna.UI2.WinForms;
using ProjectManagement.Models;

namespace ProjectManagement.Utils
{
    internal class GunaControlUtil
    {

        #region GENERIC UTILS

        public static void SetItemFavorite(Guna2Button button, bool flag)
        {
            if (flag) button.Image = Properties.Resources.PicInLineGradientStar;
            else button.Image = Properties.Resources.PicInLineStar;
        }
        public static void SetComboBoxState(Guna2ComboBox comboBox, bool onlyView)
        {
            if (onlyView)
            {
                comboBox.BorderThickness = 0;
                comboBox.FillColor = SystemColors.ButtonFace;
                comboBox.Enabled = false;
            }
            else
            {
                comboBox.BorderThickness = 1;
                comboBox.FillColor = Color.White;
                comboBox.Enabled = true;
            }
        }
        public static void SetComboBoxDisplayAndValue<T>(Guna2ComboBox comboBox, List<T> list, string displayMember, string valueMember)
        {
            comboBox.DataSource = list;
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
        }
        public static void SetDatePickerState(Guna2DateTimePicker datePicker, bool onlyView)
        {
            if (onlyView)
            {
                datePicker.BorderThickness = 0;
                datePicker.FillColor = SystemColors.ButtonFace;
                datePicker.Enabled = false;
            }
            else
            {
                datePicker.BorderThickness = 1;
                datePicker.FillColor = Color.White;
                datePicker.Enabled = true;
            }
        }

        #endregion

        #region SET TEXT BOX STATE

        public static void SetTextBoxState(List<Guna2TextBox> list, bool onlyView)
        {
            foreach (Guna2TextBox textBox in list)
            {
                SetTextBoxState(textBox, onlyView);
            }
        }
        public static void SetTextBoxState(Guna2TextBox textBox, bool onlyView)
        {
            if (onlyView)
            {
                textBox.BorderThickness = 0;
                textBox.FillColor = SystemColors.ButtonFace;
                textBox.ReadOnly = true;
            }
            else
            {
                textBox.BorderThickness = 1;
                textBox.FillColor = Color.White;
                textBox.ReadOnly = false;
            }
        }

        #endregion

        #region CREATE GUNA2 CONTROL

        public static Guna2PictureBox CreatePictureBox(Image image, Size size)
        {
            Guna2PictureBox pictureBox = new Guna2PictureBox();

            pictureBox.ImageRotate = 0F;
            pictureBox.Image = image;
            pictureBox.Name = "pictureBox";
            pictureBox.Size = size;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;

            return pictureBox;
        }

        #endregion

        #region FUNCTIONS SET GUNA2 BUTTON

        public static void ButtonStandardColor(Guna2GradientButton button)
        {
            button.FillColor = SystemColors.ControlLight;
            button.FillColor2 = SystemColors.ButtonFace;
            button.ForeColor = Color.Black;
            button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        }
        public static void ButtonStandardColor(Guna2GradientButton button, Color one, Color two)
        {
            button.FillColor = one;
            button.FillColor2 = two;
            button.ForeColor = Color.Black;
            button.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        }
        public static void ButtonSettingColor(Guna2GradientButton button)
        {
            button.FillColor = Color.FromArgb(94, 148, 255);
            button.FillColor2 = Color.FromArgb(255, 77, 165);
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        }
        public static void ButtonStandardColor(Guna2Button button)
        {
            button.FillColor = Color.Transparent;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
        }
        public static void ButtonSettingColor(Guna2Button button)
        {
            button.FillColor = Color.White;
            button.ForeColor = Color.Black;
            button.Font = new Font("Trebuchet MS", 10.8F, FontStyle.Bold);
        }
        public static void AllButtonStandardColor(List<Guna2Button> listButton, List<Image> listImage)
        {
            if (listButton.Count != listImage.Count) return;

            for (int i = 0; i < listButton.Count; i++)
            {
                Guna2Button button = listButton[i];
                ButtonStandardColor(button);
                button.CustomImages.Image = listImage[i];
            }
        }

        #endregion

    }
}
