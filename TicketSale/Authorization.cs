using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TicketSale
{
    public partial class Authorization : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommand sqlCommand = null;
        private Administration administration = null;
        private User user = null;
        private string role = null;
        private string administrationRole = "db_owner";
        private string userRole = "userTicketSale";
        private string login;

        public Authorization()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login = textBox1.Text;
            string conectionString = "Integrated Security = false; User Id =" + login +
                "; Password =" + textBox2.Text + "; Initial Catalog=TicketSale;server=MSI\\SQLEXPRESS";
            sqlConnection = new SqlConnection(conectionString);
            try
            {
                sqlConnection.Open();
                string cmd = "use TicketSale; EXECUTE sp_helpuser[" + textBox1.Text + "]";
                sqlCommand = new SqlCommand(cmd, sqlConnection);
                role = null;
                using (var reader = sqlCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int indexColumn = reader.GetOrdinal("RoleName");
                            role = reader.GetString(indexColumn);
                        }
                    }
                }
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch
            {
                MessageBox.Show("Ошибка подключения", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            textBox1.Text = null;
            textBox2.Text = null;
            Visible = false;
            if (role.Equals(administrationRole) == true)
            {
                administration = new Administration(conectionString, this);
                administration.Show();
            }
            else if (role.Equals(userRole) == true)
            {
                user = new User(conectionString, login, this);
                user.Show();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.Show(this);
        }
    }
}