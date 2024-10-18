using System.Collections;

namespace ProjectManagement.Utils
{
    internal class WinformControlUtil
    {
        #region GENERIC UTILS

        public static void RunCheckDataValid(bool flag, ErrorProvider errorProvider, Control control, string error)
        {
            if (flag == false)
            {
                control.Focus();
                errorProvider.SetError(control, error);
            }
            else
            {
                errorProvider.SetError(control, null);
            }
        }
        public static void ShowMessage(string title, string content)
        {
            MessageBox.Show(content, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        #endregion

        #region CREATE WINFORM CONTROL

        public static Label CreateLabel(string content)
        {
            Label label = new Label();

            label.AutoSize = true;
            label.BackColor = Color.Transparent;
            label.Font = new Font("Century Gothic", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label.ForeColor = Color.Gray;
            label.Name = "label";
            label.Size = new Size(315, 62);
            label.TabIndex = 5;
            label.Text = content;

            return label;
        }

        #endregion

        #region FUNCTIONS  IMAGE and AVATAR NAME

        public static string ImageToName(Image image)
        {
            if (ImageEquals(image, Properties.Resources.PicAvatarOne)) return "PicAvatarOne";
            if (ImageEquals(image, Properties.Resources.PicAvatarTwo)) return "PicAvatarTwo";
            if (ImageEquals(image, Properties.Resources.PicAvatarThree)) return "PicAvatarThree";
            if (ImageEquals(image, Properties.Resources.PicAvatarFour)) return "PicAvatarFour";
            if (ImageEquals(image, Properties.Resources.PicAvatarFive)) return "PicAvatarFive";
            if (ImageEquals(image, Properties.Resources.PicAvatarSix)) return "PicAvatarSix";
            if (ImageEquals(image, Properties.Resources.PicAvatarSeven)) return "PicAvatarSeven";
            if (ImageEquals(image, Properties.Resources.PicAvatarEight)) return "PicAvatarEight";
            if (ImageEquals(image, Properties.Resources.PicAvatarNine)) return "PicAvatarNine";
            if (ImageEquals(image, Properties.Resources.PicAvatarTen)) return "PicAvatarTen";
            return "PicAvatarDemoUser";
        }
        public static bool ImageEquals(Image img1, Image img2)
        {
            if (img1 == null || img2 == null)
                return false;

            using (MemoryStream ms1 = new MemoryStream())
            using (MemoryStream ms2 = new MemoryStream())
            {
                img1.Save(ms1, img1.RawFormat);
                img2.Save(ms2, img2.RawFormat);
                byte[] img1Bytes = ms1.ToArray();
                byte[] img2Bytes = ms2.ToArray();

                return StructuralComparisons.StructuralEqualityComparer.Equals(img1Bytes, img2Bytes);
            }
        }
        public static Image NameToImage(string imageName)
        {
            if (imageName.Equals("PicAvatarOne")) return Properties.Resources.PicAvatarOne;
            if (imageName.Equals("PicAvatarTwo")) return Properties.Resources.PicAvatarTwo;
            if (imageName.Equals("PicAvatarThree")) return Properties.Resources.PicAvatarThree;
            if (imageName.Equals("PicAvatarFour")) return Properties.Resources.PicAvatarFour;
            if (imageName.Equals("PicAvatarFive")) return Properties.Resources.PicAvatarFive;
            if (imageName.Equals("PicAvatarSix")) return Properties.Resources.PicAvatarSix;
            if (imageName.Equals("PicAvatarSeven")) return Properties.Resources.PicAvatarSeven;
            if (imageName.Equals("PicAvatarEight")) return Properties.Resources.PicAvatarEight;
            if (imageName.Equals("PicAvatarNine")) return Properties.Resources.PicAvatarNine;
            if (imageName.Equals("PicAvatarTen")) return Properties.Resources.PicAvatarTen;
            return Properties.Resources.PicAvatarDemoUser;
        }

        #endregion

    }
}
