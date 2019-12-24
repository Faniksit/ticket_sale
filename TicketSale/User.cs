using System;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace TicketSale
{
    public partial class User : Form
    {
        PrintDocument document;
        PrintPreviewDialog printPreview;
        Font font = new Font("Arial", 14, FontStyle.Regular);
        int h = 0;
        int lastIndex = 0;
        bool ReportAboutEvent = false;


        private SqlConnection connection = null;
        private SqlCommand command = null;
        private Authorization authorizationWindow = null;
        private bool exitAccont = false;
        private string stringSqlCommand;
        private string user;
        private string fullName;

        private int idVenue;
        private int idTypeEvent;
        private int idEvent;
        private string nameEvent;
        private double priceEvent;
        private int clientPoint;

        public User(string stringConnection, string userValue, Authorization authorization)
        {
            InitializeComponent();
            user = userValue;
            authorizationWindow = authorization;
            connection = new SqlConnection(stringConnection);
            connection.Open();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void User_Load(object sender, EventArgs e)
        {
            stringSqlCommand = "USE TicketSale;SELECT * FROM [Авторизация] WHERE [Логин] = '" + user + "';";
            command = new SqlCommand(stringSqlCommand, connection);
            SqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                while (reader.Read())
                {
                    int indexColumn = reader.GetOrdinal("Фамилия");
                    fullName += reader.GetString(indexColumn) + " ";
                    indexColumn = reader.GetOrdinal("Имя");
                    fullName += reader.GetString(indexColumn) + " ";
                    indexColumn = reader.GetOrdinal("Отчество");
                    fullName += reader.GetString(indexColumn);
                }
            }
            FullNameLabel.Text = fullName;
            reader.Close();
            command.Dispose();
            stringSqlCommand = "USE TicketSale;SELECT * FROM [Тип мероприятия];";
            comboBox1.DataSource = dataSource(stringSqlCommand);
            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Тип";
        }

        private void User_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (exitAccont == false)
            {
                authorizationWindow.Close();
                authorizationWindow.Dispose();
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            exitAccont = true;
            authorizationWindow.Visible = true;
            connection.Close();
            connection.Dispose();
            Close();
        }

        private void parametrsDataGridView(int countRow, int countPoint, string cmd)
        {
            countPoint /= countRow;
            dataGridView1.MultiSelect = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.ColumnCount = countPoint;
            dataGridView1.RowCount = countRow;
            for (int i = 0; i < dataGridView1.RowCount; ++i)
            {
                for(int j = 0; j < dataGridView1.ColumnCount; ++j)
                {
                    dataGridView1.Columns[j].AutoSizeMode = 
                        DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[j].Width = 40;
                    dataGridView1.Rows[i].Height = 40;
                    dataGridView1[j, i].Style.BackColor = Color.Azure;
                }
            }
            command = new SqlCommand(cmd, connection);
            SqlDataReader dataReader = command.ExecuteReader();
            if(dataReader.HasRows)
            {
                while(dataReader.Read())
                {
                    int value = Convert.ToInt32(dataReader.GetValue(0));
                    int positionRow = 0, positionColumn = 0;
                    for(int i = 1; i <= dataGridView1.RowCount; ++i)
                    {
                        if((dataGridView1.ColumnCount * i) >= value)
                        {
                            positionRow = i;
                            break;
                        }
                    }
                    positionColumn = (value - dataGridView1.ColumnCount * (positionRow - 1)) - 1;
                    positionRow = positionRow - 1;
                    dataGridView1[positionColumn, positionRow].Style.BackColor = Color.Red;
                }
            }
            dataReader.Close();
        }

        private DataTable dataSource(string cmd)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd, connection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            return dataTable;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            label2.Visible = true;
            label3.Visible = false;
            label4.Visible = false;
            comboBox2.Visible = true;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            ReportAboutEvent = false;
            stringSqlCommand = "USE TicketSale; SELECT DISTINCT A.Id, A.Наименование " +
                "FROM[Место проведения] A INNER JOIN[Мероприятие] B " +
                "ON A.Id = B.[Id Место проведения] " +
                "WHERE B.[Id Тип мероприятия] = " + 
                (int)((DataRowView)comboBox1.SelectedItem).Row["Id"] + ";";
            comboBox2.DataSource = dataSource(stringSqlCommand);
            comboBox2.ValueMember = "Id";
            comboBox2.DisplayMember = "Наименование";
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            label3.Visible = true;
            label4.Visible = false;
            comboBox3.Visible = true;
            comboBox4.Visible = false;
            ReportAboutEvent = false;
            idVenue = (int)((DataRowView)comboBox2.SelectedItem).Row["Id"];
            idTypeEvent = (int)((DataRowView)comboBox1.SelectedItem).Row["Id"];
            stringSqlCommand = "USE TicketSale; SELECT [Название мероприятия]" +
                " FROM[Мероприятие]  WHERE[Id Место проведения] = " + idVenue +
                " AND [Id Тип мероприятия] = " + idTypeEvent +
                " GROUP BY[Название мероприятия] HAVING COUNT(*) > 1 OR COUNT(*) > 0;";
            comboBox3.DataSource = dataSource(stringSqlCommand);
            comboBox3.ValueMember = "Название мероприятия";
            comboBox3.DisplayMember = "Название мероприятия";
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            label4.Visible = true;
            comboBox4.Visible = true;
            ReportAboutEvent = false;
            nameEvent = ((DataRowView)comboBox3.SelectedItem).Row["Название мероприятия"].ToString();
            stringSqlCommand = "USE TicketSale; SELECT * FROM[Мероприятие] " +
                "WHERE [Название мероприятия] = '" + nameEvent + "';";
            comboBox4.DataSource = dataSource(stringSqlCommand);
            comboBox4.ValueMember = "Id";
            comboBox4.DisplayMember = "Дата";
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            stringSqlCommand = "USE TicketSale; SELECT[Количество рядов], [Количество мест] " +
                "FROM[Место проведения] WHERE Id = " + idVenue + ";";
            int row = (int)dataSource(stringSqlCommand).Rows[0][0];
            int point = (int)dataSource(stringSqlCommand).Rows[0][1];
            idEvent = (int)((DataRowView)comboBox4.SelectedItem).Row["Id"];
            stringSqlCommand = "USE TicketSale; SELECT[Место] FROM[Клиент] " +
                "WHERE[Id мероприятия] = " + idEvent + ";";
            parametrsDataGridView(row, point, stringSqlCommand);
            stringSqlCommand = "USE TicketSale; SELECT Цена " +
                "FROM Мероприятие WHERE Id = " + idEvent + ";";
            priceEvent = Convert.ToDouble(dataSource(stringSqlCommand).Rows[0][0]);
            priceTextBox.Text = priceEvent.ToString();
            ReportAboutEvent = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectRow = dataGridView1.CurrentCell.RowIndex + 1;
            rowTextBox.Text = selectRow.ToString();
            clientPoint = dataGridView1.CurrentCell.ColumnIndex + 1;
            clientPoint = ((selectRow - 1) * dataGridView1.ColumnCount) + clientPoint;
            pointTextBox.Text = clientPoint.ToString();
            priceTextBox.Text = priceEvent.ToString();
        }

        private void reservationButton_Click(object sender, EventArgs e)
        {
            string[] str = FullNameLabel.Text.Split(new char[] { ' ' }, 
                StringSplitOptions.RemoveEmptyEntries);
            stringSqlCommand = "USE TicketSale; INSERT INTO [Клиент] " +
                "VALUES('" + str[0] + "', '" + str[1] + "', '" + str[2] +
                "', " + idEvent + ", " + clientPoint + ");";
            command = new SqlCommand(stringSqlCommand, connection);
            if (dataGridView1.CurrentCell.Style.BackColor == Color.Red)
            {
                MessageBox.Show("Выбранное вами место занято!\nВыберете другое место",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                command.ExecuteNonQuery();
                command.Dispose();
                dataGridView1.CurrentCell.Style.BackColor = Color.Red;
                MessageBox.Show("Вы успешно забронировали!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Ошибка бронирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void theFirstReportPrintPage(object sender, PrintPageEventArgs e)
        {
            int symbols_on_page = 0;
            int lines_on_page = 0;
            DataTable data = dataSource(stringSqlCommand);
            for(int i = lastIndex; i < data.Rows.Count; ++i)
            {
                string information = "";
                if (data.Rows.Count == 0)
                    return;
                information += "Тип мероприятия: " + data.Rows[i][0].ToString() + '\n';
                information += "Мероприятие: " + data.Rows[i][1].ToString() + '\n';
                information += "Дата проведения: " + data.Rows[i][2].ToString() + '\n';
                Rectangle locationText = new Rectangle(e.MarginBounds.X,
                    e.MarginBounds.Y + h,e.MarginBounds.Width,e.MarginBounds.Height - h);
                SizeF sizeText = e.Graphics.MeasureString(information, font, locationText.Size,
                    StringFormat.GenericTypographic, out symbols_on_page, out lines_on_page);
                if (sizeText.Height > e.MarginBounds.Height - h)
                {
                    h = 0;
                    e.HasMorePages = true;
                    lastIndex = i;
                    break;
                }
                else
                {
                    e.Graphics.DrawString(information, font, Brushes.Black, locationText,
                    StringFormat.GenericTypographic);
                    h += ((int)sizeText.Height + 50);
                }
            }
        }

        private void theSecondReportPrintPage(object sender, PrintPageEventArgs e)
        {
            int symbols_on_page = 0;
            int lines_on_page = 0;
            DataTable data = dataSource(stringSqlCommand);
            string information = "";
            information += "Тип мероприятия: " + data.Rows[0][0].ToString() + '\n';
            information += "Мероприятие: " + data.Rows[0][1].ToString() + '\n';
            information += "Дата проведения: " + data.Rows[0][2].ToString() + '\n';
            information += "Длителность (в минутах): " + data.Rows[0][3].ToString() + '\n';
            information += "Место проведения: " + data.Rows[0][4].ToString() + '\n';
            information += "Адрес: " + data.Rows[0][5].ToString() + '\n';
            information += "Цена билета: " + data.Rows[0][6].ToString() + '\n';
            Rectangle locationText = new Rectangle(e.MarginBounds.X,
                e.MarginBounds.Y, e.MarginBounds.Width, e.MarginBounds.Height);
            SizeF sizeText = e.Graphics.MeasureString(information, font, locationText.Size,
                   StringFormat.GenericTypographic, out symbols_on_page, out lines_on_page);
            e.Graphics.DrawString(information, font, Brushes.Black, locationText,
                    StringFormat.GenericTypographic);
            h = h + ((int)sizeText.Height + 30);
            information = "Места в зале:";
            locationText = new Rectangle(e.MarginBounds.X,
                e.MarginBounds.Y + h, e.MarginBounds.Width, e.MarginBounds.Height);
            e.Graphics.DrawString(information, font, Brushes.Black, locationText,
                    StringFormat.GenericTypographic);
            h = h + 40;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                int disnance = 0;
                int point = 0;
                for (int j = 0; j < dataGridView1.Columns.Count; ++j)
                {
                    if (j == 0)
                    {
                        e.Graphics.DrawString((i + 1).ToString() + " Ряд", 
                            new Font("Arial", 10, FontStyle.Regular), 
                            Brushes.Black, e.MarginBounds.X, e.MarginBounds.Y + h);
                        disnance = disnance + 80;
                    }
                    point = (((i + 1) - 1) * dataGridView1.ColumnCount) + (j + 1);
                    e.Graphics.DrawString(point.ToString(), new Font("Arial", 8, FontStyle.Regular), 
                        Brushes.Black, e.MarginBounds.X + disnance, e.MarginBounds.Y + h - 18);
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(e.MarginBounds.X + disnance,
                        e.MarginBounds.Y + h, 15, 15));
                    if (dataGridView1[j, i].Style.BackColor == Color.Red)
                        e.Graphics.FillRectangle(Brushes.Black, new Rectangle(e.MarginBounds.X + disnance,
                            e.MarginBounds.Y + h, 15, 15));
                    disnance = disnance + 25;
                }
                h = h + 40;
            }
        }

        private void theFirstReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report formPeriod = new Report();
            if (formPeriod.ShowDialog() != DialogResult.OK)
                return;
            h = 0;
            lastIndex = 0;
            string start = formPeriod.startDate.Value.ToString("yyyyMMdd");
            string finish = formPeriod.finishDate.Value.ToString("yyyyMMdd");
            stringSqlCommand = "USE TicketSale; SELECT B.Тип, A.[Название мероприятия]" +
                ", A.Дата, A.Продолжительность, C.Наименование, C.Адрес, A.Цена " +
                "FROM Мероприятие A JOIN[Тип мероприятия] B ON A.[Id Тип мероприятия] = B.Id " +
                "JOIN[Место проведения] C ON A.[Id Место проведения] = C.Id " +
                "WHERE Дата BETWEEN '" + start + "' AND '" + finish + "' ORDER BY Дата ASC;";
            document = new PrintDocument();
            printPreview = new PrintPreviewDialog();
            document.PrintPage += new PrintPageEventHandler(theFirstReportPrintPage);
            printPreview.Document = document;
            printPreview.ShowDialog();
        }

        private void theSecondReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ReportAboutEvent == false)
            {
                MessageBox.Show("Заполните все критерии!", "Информация", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            h = 0;
            stringSqlCommand = "USE TicketSale; " +
                "SELECT B.Тип, A.[Название мероприятия], A.Дата, " +
                "A.Продолжительность, C.Наименование, C.Адрес, A.Цена " +
                "FROM Мероприятие A JOIN[Тип мероприятия] B ON A.[Id Тип мероприятия] = B.Id " +
                "JOIN[Место проведения] C ON A.[Id Место проведения] = C.Id " +
                "WHERE A.Id = " + idEvent + ";";
            document = new PrintDocument();
            printPreview = new PrintPreviewDialog();
            document.PrintPage += new PrintPageEventHandler(theSecondReportPrintPage);
            printPreview.Document = document;
            printPreview.ShowDialog();
        }
    }
}