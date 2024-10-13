namespace ProjectManagement.Utils
{
    internal class DataTypeUtil
    {
        public static string FormatStringLength(string str, int length)
        {
            if (str.Length <= length)
            {
                return str;
            }

            return str.Substring(0, length - 3) + "...";
        }

        public static int ConvertStringToInt32(string input)
        {
            try
            {
                int number = int.Parse(input);
                return number;
            }
            catch (FormatException)
            {
                MessageBox.Show("The input string is not a valid integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            catch (OverflowException)
            {
                MessageBox.Show("The input string is too long or exceeds the limit of a 32-bit integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
    }
}
