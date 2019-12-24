using System.Data.SqlClient;
using System.Windows.Forms;

namespace TicketSale
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (textBox1.Text == "")
            {
                errorRegistrationAuthorization("Вы не ввели фамилию");
                return;
            }

            if (textBox2.Text == "")
            {
                errorRegistrationAuthorization("Вы не ввели имя");
                return;
            }

            if (textBox3.Text == "")
            {
                errorRegistrationAuthorization("Вы не ввели отчество");
                return;
            }

            if (maleButton.Checked == false && femaleButton.Checked == false)
            {
                errorRegistrationAuthorization("Вы не указали пол");
                return;
            }

            if (textBox4.Text == "")
            {
                errorRegistrationAuthorization("Вы не ввели электронный адрес");
                return;
            }
            else
            {
                if (textBox4.Text.IndexOf('@') == -1)
                {
                    errorRegistrationAuthorization("Вы неверно указали электронный адрес");
                    return;
                }
            }

            if (textBox5.Text.Length < 8)
            {
                errorRegistrationAuthorization("Вы ввели пароль менее 8 символов\nНеобходимо указать пароль не менее 8 символов");
                return;
            }
            else
            {
                bool result = textBox5.Text.Equals(textBox6.Text);
                if (result == false)
                {
                    errorRegistrationAuthorization("Пароли не совпадают!");
                    return;
                }
            }

            string sex;
            if (maleButton.Checked == true) sex = "Мужской";
            else sex = "Женский";
            string firstName = textBox1.Text;
            string name = textBox2.Text;
            string lastName = textBox3.Text;
            string email = textBox4.Text;
            string password = textBox5.Text;

            try
            {
                SqlConnection sqlConnection = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=TicketSale;Integrated Security=True");
                sqlConnection.Open();
                string cmd = "INSERT INTO Регистрация VALUES ('" + firstName + "', '" + name + "', '" + lastName + "', '" + sex + "', '" + email + "', '" + password + "');";
                SqlCommand sqlCommand = new SqlCommand(cmd, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
                sqlConnection.Dispose();
                MessageBox.Show("Регистрация прошла успешно!\nВ течении 24 часов администратор подтвердит ваши данные",
                "Информация",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка реистрации!\nПовторите позже.", "Внимание!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            this.Close();
            this.Dispose();
        }

        private void errorRegistrationAuthorization(string error)
        {
            MessageBox.Show(error, "Внимание!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
