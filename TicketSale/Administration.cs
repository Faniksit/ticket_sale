using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TicketSale
{
    public partial class Administration : Form
    {
        Venue venue = new Venue();
        Event _event = new Event();
        Client client = new Client();
        TypeEvent typeEvent = new TypeEvent();

        private Authorization authorizationWindow = null;
        private СonfirmationRegistration сonfirmation = null;
        private SqlConnection connection = null;
        private SqlCommand command = null;
        private bool registrationButton;
        private DataGridViewSelectedRowCollection select = null;
        private string selectTable = null;
        private bool exitAccont = false;
        private string commandForSqlCommand = null;

        public Administration(string stringConnection, Authorization authorization)
        {
            InitializeComponent();
            connection = new SqlConnection(stringConnection);
            authorizationWindow = authorization;
            connection.Open();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMMM yyyy, H:mm";
        }

        private DataTable dataSource(string cmd)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd, connection);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            return dataTable;
        }

        private void registrationRequests_Click(object sender, EventArgs e)
        {
            commandForSqlCommand = "USE TicketSale; SELECT * FROM[Регистрация];";
            dataSource(commandForSqlCommand);
            dataGridView1.DataSource = dataSource(commandForSqlCommand);
            comboBox1.Visible = false;
            registrationButton = true;
            buttonRun.Visible = true;
            buttonRun.Text = "Зарегистрировать";
            VisibleAndEnabled();
        }

        private void typeEventButton_Click(object sender, EventArgs e)
        {
            commandForSqlCommand = "USE TicketSale; SELECT * FROM[Тип мероприятия];";
            dataSource(commandForSqlCommand);
            dataGridView1.DataSource = dataSource(commandForSqlCommand);
            selectTable = "Тип мероприятия";
            registrationButton = false;
            comboBox1.Visible = true;
            comboBox1.Text = "Выберете действие";
            buttonRun.Text = "Выполнить";
            buttonRun.Visible = true;
            if(comboBox1.SelectedItem != null)
                if(!comboBox1.SelectedItem.Equals("Изменить") || comboBox4.Visible)
                    VisibleAndEnabled();
        }

        private void eventButton_Click(object sender, EventArgs e)
        {
            commandForSqlCommand = "USE TicketSale;" +
                "SELECT A.Id, B.Тип, A.[Название мероприятия], A.Дата, " +
                "A.Продолжительность, C.Наименование AS 'Место проведения', A.Цена " +
                "FROM[Мероприятие] A " +
                "JOIN[Тип мероприятия] B ON A.[Id Тип мероприятия] = B.Id " +
                "JOIN[Место проведения] C ON A.[Id Место проведения] = C.Id; ";
            dataSource(commandForSqlCommand);
            dataGridView1.DataSource = dataSource(commandForSqlCommand);
            selectTable = "Мероприятие";
            registrationButton = false;
            comboBox1.Visible = true;
            comboBox1.Text = "Выберете действие";
            buttonRun.Text = "Выполнить";
            buttonRun.Visible = true;
            if (comboBox1.SelectedItem != null)
                if(!comboBox1.SelectedItem.Equals("Изменить") || comboBox4.Visible)
                    VisibleAndEnabled();
        }

        private void venueButton_Click(object sender, EventArgs e)
        {
            commandForSqlCommand = "USE TicketSale; SELECT* FROM[Место проведения]; ";
            dataSource(commandForSqlCommand);
            dataGridView1.DataSource = dataSource(commandForSqlCommand);
            selectTable = "Место проведения";
            registrationButton = false;
            comboBox1.Visible = true;
            comboBox1.Text = "Выберете действие";
            buttonRun.Text = "Выполнить";
            buttonRun.Visible = true;
            if (comboBox1.SelectedItem != null)
                if (!comboBox1.SelectedItem.Equals("Изменить") || comboBox4.Visible)
                    VisibleAndEnabled();
        }

        private void clientButton_Click(object sender, EventArgs e)
        {
            commandForSqlCommand = "USE TicketSale;" +
                "SELECT A.Id, A.Фамилия, A.Имя, A.Отчество, B.[Название мероприятия], A.Место " +
                "FROM Клиент A " +
                "JOIN Мероприятие B ON A.[Id мероприятия] = B.Id;";
            dataSource(commandForSqlCommand);
            dataGridView1.DataSource = dataSource(commandForSqlCommand);
            selectTable = "Клиент";
            registrationButton = false;
            comboBox1.Visible = true;
            comboBox1.Text = "Выберете действие";
            buttonRun.Text = "Выполнить";
            buttonRun.Visible = true;
            if (comboBox1.SelectedItem != null)
                if (!comboBox1.SelectedItem.Equals("Изменить") || comboBox4.Visible)
                    VisibleAndEnabled();
        }

        private void exitAccountButton_Click(object sender, EventArgs e)
        {
            exitAccont = true;
            authorizationWindow.Visible = true;
            connection.Close();
            connection.Dispose();
            Close();
        }

        private void Administration_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (exitAccont == false)
            {
                authorizationWindow.Close();
                authorizationWindow.Dispose();
            }
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if(registrationButton == true)
            {
                select = dataGridView1.SelectedRows;
                if (select.Count > 0)
                {
                    try
                    {
                        string fullName = select[0].Cells["Фамилия"].Value.ToString() + " " +
                            select[0].Cells["Имя"].Value.ToString() + " " +
                            select[0].Cells["Отчество"].Value.ToString();
                        string login = select[0].Cells["Электронный адрес"].Value.ToString();
                        string password = select[0].Cells["Пароль"].Value.ToString();
                        сonfirmation = new СonfirmationRegistration();
                        сonfirmation.label1.Text = fullName;
                        сonfirmation.ShowDialog();
                        string cmd = "";
                        if (сonfirmation.DialogResult == DialogResult.OK)
                            cmd = "use TicketSale; EXECUTE sp_addlogin[" + login + "], '" + password + "';" +
                            "EXECUTE sp_adduser[" + login + "], [" + login + "], userTicketSale;" +
                            "DELETE FROM Регистрация WHERE [Электронный адрес] = '" + login + "';" +
                            "INSERT INTO Авторизация VALUES ('" + select[0].Cells["Фамилия"].Value.ToString() + "', '" + 
                            select[0].Cells["Имя"].Value.ToString() + "', '" + 
                            select[0].Cells["Отчество"].Value.ToString() + "', '" + login + "');";

                        else if (сonfirmation.DialogResult == DialogResult.Yes)
                            cmd = "use TicketSale; EXECUTE sp_addlogin[" + login + "], '" + password + "';" +
                            "EXECUTE sp_adduser[" + login + "], [" + login + "], db_owner;" +
                            "EXECUTE sp_addsrvrolemember [" + login + "], 'sysadmin';" +
                            "DELETE FROM Регистрация WHERE [Электронный адрес] = '" + login + "';";

                        else if (сonfirmation.DialogResult == DialogResult.Cancel)
                            return;

                        command = new SqlCommand(cmd, connection);
                        command.ExecuteNonQuery();
                        command.Dispose();
                        MessageBox.Show("Пользователь успешно зарегистрирован!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка регистрации!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Вы не выбрали пользователя!", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                registrationRequests.PerformClick();
            }
            else
            {
                if (comboBox1.SelectedItem.Equals("Добавить"))
                {
                    if(selectTable.Equals("Тип мероприятия"))
                    {
                        typeEvent.Type = textBox1.Text;
                        typeEvent.insertInDatabase(connection);
                        typeEventButton.PerformClick();
                    }
                    else if(selectTable.Equals("Мероприятие"))
                    {
                        _event.idTypeEvent = (int)((DataRowView)comboBox2.SelectedItem).Row["Id"];
                        _event.NameEvent = textBox7.Text;
                        _event.Date = dateTimePicker1.Value;
                        _event.Length = Convert.ToInt32(numericUpDown2.Value);
                        _event.idVenue = (int)((DataRowView)comboBox3.SelectedItem).Row["Id"];
                        _event.Price = Convert.ToInt32(numericUpDown1.Value);
                        _event.insertInDatabase(connection);
                        eventButton.PerformClick();
                    }
                    else if (selectTable.Equals("Место проведения"))
                    {
                        venue.Name = textBox1.Text;
                        venue.CountSeats = Convert.ToInt32(numericUpDown1.Value);
                        venue.CountRow = Convert.ToInt32(numericUpDown2.Value);
                        venue.Address = textBox2.Text;
                        venue.insertInDatabase(connection);
                        venueButton.PerformClick();
                    }
                    else if (selectTable.Equals("Клиент"))
                    {
                        client.FirstName = textBox1.Text;
                        client.Name = textBox2.Text;
                        client.LastName = textBox3.Text;
                        client.idEvent = (int)((DataRowView)comboBox2.SelectedItem).Row["Id"];
                        client.Location = Convert.ToInt32(numericUpDown1.Value);
                        client.insertInDatabase(connection);
                        clientButton.PerformClick();
                    }
                }
                else if (comboBox1.SelectedItem.Equals("Изменить"))
                {
                    if (selectTable.Equals("Тип мероприятия"))
                    {
                        typeEvent.Id = Convert.ToInt32(select[0].Cells[0].Value);
                        typeEvent.Type = textBox2.Text;
                        commandForSqlCommand = "USE TicketSale; UPDATE[Тип мероприятия] " +
                            "SET Тип = '" + typeEvent.Type + "' WHERE Id = " + typeEvent.Id + ";";
                        command = new SqlCommand(commandForSqlCommand, connection);
                        command.ExecuteNonQuery();
                        command.Dispose();
                        typeEventButton.PerformClick();
                    }
                    else if (selectTable.Equals("Мероприятие"))
                    {
                        _event.Id = Convert.ToInt32(select[0].Cells[0].Value);
                        if (comboBox4.SelectedItem.Equals("Тип"))
                        {
                            _event.idTypeEvent = (int)((DataRowView)comboBox2.SelectedItem).Row["Id"];
                            commandForSqlCommand = "USE TicketSale; UPDATE[Мероприятие] " +
                                "SET [Id Тип мероприятия] = " + _event.idTypeEvent + " " +
                                "WHERE Id = " + _event.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Название мероприятия"))
                        {
                            _event.NameEvent = textBox2.Text;
                            commandForSqlCommand = "USE TicketSale; UPDATE[Мероприятие] " +
                                "SET [Название мероприятия] = '" + _event.NameEvent + "' " +
                                "WHERE Id = " + _event.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Дата"))
                        {
                            _event.Date = dateTimePicker1.Value;
                            commandForSqlCommand = "USE TicketSale; UPDATE[Мероприятие] " +
                                "SET [Дата] = '" + _event.Date + "' " +
                                "WHERE Id = " + _event.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Продолжительность"))
                        {
                            _event.Length = Convert.ToInt32(numericUpDown2.Value);
                            commandForSqlCommand = "USE TicketSale; UPDATE[Мероприятие] " +
                                "SET [Продолжительность] = '" + _event.Length + "' " +
                                "WHERE Id = " + _event.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Место проведения"))
                        {
                            _event.idVenue = (int)((DataRowView)comboBox2.SelectedItem).Row["Id"];
                            commandForSqlCommand = "USE TicketSale; UPDATE[Мероприятие] " +
                                "SET [Id Место проведения] = " + _event.idVenue + " " +
                                "WHERE Id = " + _event.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Цена"))
                        {
                            _event.Price = Convert.ToInt32(numericUpDown2.Value);
                            commandForSqlCommand = "USE TicketSale; UPDATE[Мероприятие] " +
                                "SET [Цена] = '" + _event.Price + "' " +
                                "WHERE Id = " + _event.Id + ";";
                        }
                        command = new SqlCommand(commandForSqlCommand, connection);
                        command.ExecuteNonQuery();
                        command.Dispose();
                        eventButton.PerformClick();
                    }
                    else if (selectTable.Equals("Место проведения"))
                    {
                        venue.Id = Convert.ToInt32(select[0].Cells[0].Value);
                        if (comboBox4.SelectedItem.Equals("Наименование"))
                        {
                            venue.Name = textBox2.Text;
                            commandForSqlCommand = "USE TicketSale; UPDATE[Место проведения] " +
                                "SET [Наименование] = '" + venue.Name + "' " +
                                "WHERE Id = " + venue.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Количество мест"))
                        {
                            venue.CountSeats = Convert.ToInt32(numericUpDown2.Value);
                            commandForSqlCommand = "USE TicketSale; UPDATE[Место проведения] " +
                                "SET [Количество мест] = '" + venue.CountSeats + "' " +
                                "WHERE Id = " + venue.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Количество рядов"))
                        {
                            venue.CountRow = Convert.ToInt32(numericUpDown2.Value);
                            commandForSqlCommand = "USE TicketSale; UPDATE[Место проведения] " +
                                "SET [Количество рядов] = '" + venue.CountRow + "' " +
                                "WHERE Id = " + venue.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Адрес"))
                        {
                            venue.Address = textBox2.Text;
                            commandForSqlCommand = "USE TicketSale; UPDATE[Место проведения] " +
                                "SET [Адрес] = '" + venue.Address + "' " +
                                "WHERE Id = " + venue.Id + ";";
                        }
                        command = new SqlCommand(commandForSqlCommand, connection);
                        command.ExecuteNonQuery();
                        command.Dispose();
                        venueButton.PerformClick();
                    }
                    else if (selectTable.Equals("Клиент"))
                    {
                        client.Id = Convert.ToInt32(select[0].Cells[0].Value);
                        if (comboBox4.SelectedItem.Equals("Фамилия"))
                        {
                            client.FirstName = textBox2.Text;
                            commandForSqlCommand = "USE TicketSale; UPDATE[Клиент] " +
                                "SET [Фамилия] = '" + client.FirstName + "' " +
                                "WHERE Id = " + client.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Имя"))
                        {
                            client.Name = textBox2.Text;
                            commandForSqlCommand = "USE TicketSale; UPDATE[Клиент] " +
                                "SET [Имя] = '" + client.Name + "' " +
                                "WHERE Id = " + client.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Отчество"))
                        {
                            client.LastName = textBox2.Text;
                            commandForSqlCommand = "USE TicketSale; UPDATE[Клиент] " +
                                "SET [Отчество] = '" + client.LastName + "' " +
                                "WHERE Id = " + client.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Название мероприятия"))
                        {
                            client.idEvent = (int)((DataRowView)comboBox2.SelectedItem).Row["Id"];
                            commandForSqlCommand = "USE TicketSale; UPDATE[Клиент] " +
                                "SET [Id мероприятия] = " + client.idEvent + " " +
                                "WHERE Id = " + client.Id + ";";
                        }
                        else if (comboBox4.SelectedItem.Equals("Место"))
                        {
                            client.Location = Convert.ToInt32(numericUpDown2.Value);
                            commandForSqlCommand = "USE TicketSale; UPDATE[Клиент] " +
                                "SET [Место] = " + client.Location + " " +
                                "WHERE Id = " + client.Id + ";";
                        }
                        command = new SqlCommand(commandForSqlCommand, connection);
                        command.ExecuteNonQuery();
                        command.Dispose();
                        clientButton.PerformClick();
                    }
                    
                }
                else if (comboBox1.SelectedItem.Equals("Удалить"))
                {
                    select = dataGridView1.SelectedRows;
                    if (select.Count == 0)
                    {
                        MessageBox.Show("Вы не выбрали пользователя!", "Внимание",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (selectTable.Equals("Тип мероприятия"))
                    {
                        typeEvent.deleteDataInDB(connection, select[0].Cells[0].Value);
                        typeEventButton.PerformClick();
                    }
                    else if (selectTable.Equals("Мероприятие"))
                    {
                        _event.deleteDataInDB(connection, select[0].Cells[0].Value);
                        eventButton.PerformClick();
                    }
                    else if (selectTable.Equals("Место проведения"))
                    {
                        venue.deleteDataInDB(connection, select[0].Cells[0].Value);
                        venueButton.PerformClick();
                    }
                    else if (selectTable.Equals("Клиент"))
                    {
                        client.deleteDataInDB(connection, select[0].Cells[0].Value);
                        clientButton.PerformClick();
                    }
                }
                else
                    MessageBox.Show("Выберете действие!",
                        "Внимание",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            VisibleAndEnabled();
            if (selectTable.Equals("Тип мероприятия"))
            {
                if(comboBox1.SelectedItem.Equals("Добавить"))
                {
                    label1.Location = new Point(5, 35); label1.Text = "Тип мероприятия";
                    textBox1.Location = new Point(5, 50);
                    label1.Visible = textBox1.Visible = true;
                }
                else if(comboBox1.SelectedItem.Equals("Изменить"))
                {
                    label1.Location = new Point(5, 40);
                    label1.Text = "Выберете столбец:";
                    label1.Visible = true;
                    comboBox4.Location = new Point(120, 35);
                    comboBox4.Items.Clear();
                    for(int i = 1; i < dataGridView1.Columns.Count; ++i)
                        comboBox4.Items.Add(dataGridView1.Columns[i].HeaderText.ToString());
                    comboBox4.Visible = true;

                }
                else if(comboBox1.SelectedItem.Equals("Удалить"))
                {
                    MessageBox.Show("Выберете запись в таблице и нажмите\nКнопку 'Выполнить'\nБУДЬТЕ ВНИМАТЕЛЬНЫ!",
                           "Информация",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
                }
            }
            else if(selectTable.Equals("Место проведения"))
            {
                if (comboBox1.SelectedItem.Equals("Добавить"))
                {
                    label1.Location = new Point(5, 35); label1.Text = "Наименование";
                    label2.Location = new Point(150, 35); label2.Text = "Кол - во мест";
                    label3.Location = new Point(275, 35); label3.Text = "Кол - во рядов";
                    label4.Location = new Point(410, 35); label4.Text = "Адрес";
                    label1.Visible = label2.Visible = label3.Visible = label4.Visible = true;
                    textBox1.Location = new Point(5, 50);
                    numericUpDown1.Location = new Point(150, 50);
                    numericUpDown2.Location = new Point(275, 50);
                    textBox2.Location = new Point(410, 50);
                    textBox1.Visible = numericUpDown1.Visible = numericUpDown2.Visible = textBox2.Visible = true;
                }
                else if (comboBox1.SelectedItem.Equals("Изменить"))
                {
                    label1.Location = new Point(5, 40);
                    label1.Text = "Выберете столбец:";
                    label1.Visible = true;
                    comboBox4.Location = new Point(120, 35);
                    comboBox4.Items.Clear();
                    for (int i = 1; i < dataGridView1.Columns.Count; ++i)
                        comboBox4.Items.Add(dataGridView1.Columns[i].HeaderText.ToString());
                    comboBox4.Visible = true;
                }
                else if (comboBox1.SelectedItem.Equals("Удалить"))
                {
                    MessageBox.Show("Выберете запись в таблице и нажмите\nКнопку 'Выполнить'\nБУДЬТЕ ВНИМАТЕЛЬНЫ!",
                        "Информация",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else if (selectTable.Equals("Мероприятие"))
            {
                comboBox2.DataSource = dataSource("USE TicketSale; SELECT * FROM[Тип мероприятия];");
                comboBox2.DisplayMember = "Тип";
                comboBox2.ValueMember = "Id";
                comboBox3.DataSource = dataSource("USE TicketSale; SELECT * FROM[Место проведения];");
                comboBox3.DisplayMember = "Наименование";
                comboBox2.ValueMember = "Id";
                if (comboBox1.SelectedItem.Equals("Добавить"))
                {
                    label1.Location = new Point(5, 35); label1.Text = "Тип мероприятия";
                    label2.Location = new Point(140, 35); label2.Text = "Название";
                    label3.Location = new Point(275, 35); label3.Text = "Цена";
                    label4.Location = new Point(410, 35); label4.Text = "Продолжительность";
                    label5.Location = new Point(545, 35); label5.Text = "Место проведения";
                    label6.Location = new Point(680, 35); label6.Text = "Дата проведения";
                    label1.Visible = label2.Visible = label3.Visible = true;
                    label4.Visible = label5.Visible = label6.Visible = true;
                    comboBox2.Location = new Point(5, 50);
                    textBox7.Location = new Point(140, 50);
                    numericUpDown1.Location = new Point(275, 50);
                    numericUpDown2.Location = new Point(410, 50);
                    comboBox3.Location = new Point(545, 50);
                    dateTimePicker1.Location = new Point(680, 50);
                    comboBox2.Visible = textBox7.Visible = dateTimePicker1.Visible = true;
                    numericUpDown1.Visible = comboBox3.Visible = numericUpDown2.Visible = true;
                }
                else if (comboBox1.SelectedItem.Equals("Изменить"))
                {
                    label1.Location = new Point(5, 40);
                    label1.Text = "Выберете столбец:";
                    label1.Visible = true;
                    comboBox4.Location = new Point(120, 35);
                    comboBox4.Items.Clear();
                    for (int i = 1; i < dataGridView1.Columns.Count; ++i)
                        comboBox4.Items.Add(dataGridView1.Columns[i].HeaderText.ToString());
                    comboBox4.Visible = true;
                }
                else if (comboBox1.SelectedItem.Equals("Удалить"))
                {
                    MessageBox.Show("Выберете запись в таблице и нажмите\nКнопку 'Выполнить'\nБУДЬТЕ ВНИМАТЕЛЬНЫ!",
                        "Информация",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else if (selectTable.Equals("Клиент"))
            {
                comboBox2.DataSource = dataSource("USE TicketSale; SELECT * FROM[Мероприятие];");
                comboBox2.DisplayMember = "Название мероприятия";
                comboBox2.ValueMember = "Id";
                if (comboBox1.SelectedItem.Equals("Добавить"))
                {
                    label1.Location = new Point(5, 35);
                    label2.Location = new Point(140, 35);
                    label3.Location = new Point(275, 35);
                    label4.Location = new Point(410, 35);
                    label5.Location = new Point(545, 35);
                    label1.Text = "Фамилия"; label2.Text = "Имя";
                    label3.Text = "Отчество"; label4.Text = "Мероприятие";
                    label5.Text = "Место";
                    label1.Visible = label2.Visible = label3.Visible = label4.Visible = label5.Visible = true;
                    textBox1.Location = new Point(5, 50);
                    textBox2.Location = new Point(140, 50);
                    textBox3.Location = new Point(275, 50);
                    comboBox2.Location = new Point(410, 50);
                    numericUpDown1.Location = new Point(545, 50);
                    textBox1.Visible = textBox2.Visible = textBox3.Visible = comboBox2.Visible = true;
                    numericUpDown1.Visible = true;
                }
                else if (comboBox1.SelectedItem.Equals("Изменить"))
                {
                    label1.Location = new Point(5, 40);
                    label1.Text = "Выберете столбец:";
                    label1.Visible = true;
                    comboBox4.Location = new Point(120, 35);
                    comboBox4.Items.Clear();
                    for (int i = 1; i < dataGridView1.Columns.Count; ++i)
                        comboBox4.Items.Add(dataGridView1.Columns[i].HeaderText.ToString());
                    comboBox4.Visible = true;
                }
                else if (comboBox1.SelectedItem.Equals("Удалить"))
                {
                    MessageBox.Show("Выберете запись в таблице и нажмите\nКнопку 'Выполнить'\nБУДЬТЕ ВНИМАТЕЛЬНЫ!",
                        "Информация",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void VisibleAndEnabled()
        {
            label1.Visible = false; label1.Enabled = true;
            label2.Visible = false; label2.Enabled = true;
            label3.Visible = false; label3.Enabled = true;
            label4.Visible = false; label4.Enabled = true;
            label5.Visible = false; label5.Enabled = true;
            label6.Visible = false; label6.Enabled = true;
            textBox1.Visible = false; textBox1.Enabled = true; textBox1.Text = "";
            textBox2.Visible = false; textBox2.Enabled = true; textBox2.Text = "";
            textBox3.Visible = false; textBox3.Enabled = true; textBox3.Text = "";
            textBox4.Visible = false; textBox4.Enabled = true; textBox4.Text = "";
            textBox5.Visible = false; textBox5.Enabled = true; textBox5.Text = "";
            textBox6.Visible = false; textBox6.Enabled = true; textBox6.Text = "";
            textBox7.Visible = false; textBox7.Enabled = true; textBox7.Text = "";
            comboBox2.Visible = false; comboBox2.Enabled = true;
            comboBox3.Visible = false; comboBox3.Enabled = true;
            comboBox4.Visible = false; comboBox4.Enabled = true;
            numericUpDown1.Visible = false; numericUpDown1.Enabled = true;
            numericUpDown2.Visible = false; numericUpDown2.Enabled = true;
            dateTimePicker1.Visible = false; dateTimePicker1.Enabled = true;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            VisibleAndEnabled();
            label1.Visible = true;
            comboBox4.Visible = true;
            if (selectTable.Equals("Тип мероприятия"))
            {
                if (comboBox4.SelectedItem.Equals("Тип"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    textBox2.Location = new Point(120, 110);
                    textBox2.Visible = true;
                    label2.Location = new Point(5, 75);
                    label2.Text = "Текущее значение:";
                    label2.Visible = true;
                    label3.Location = new Point(5, 112);
                    label3.Text = "Новое значение:";
                    label3.Visible = true;
                }
            }
            else if (selectTable.Equals("Мероприятие"))
            {
                label2.Location = new Point(5, 75);
                label2.Text = "Текущее значение:";
                label2.Visible = true;
                label3.Location = new Point(5, 112);
                label3.Text = "Новое значение:";
                label3.Visible = true;
                if (comboBox4.SelectedItem.Equals("Тип"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    comboBox2.Location = new Point(120, 110);
                    comboBox2.Visible = true;
                    comboBox2.DataSource = dataSource("USE TicketSale; SELECT * FROM[Тип мероприятия];");
                    comboBox2.DisplayMember = "Тип";
                    comboBox2.ValueMember = "Id";
                }
                else if (comboBox4.SelectedItem.Equals("Название мероприятия"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    textBox2.Location = new Point(120, 110);
                    textBox2.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Дата"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    dateTimePicker1.Location = new Point(120, 110);
                    dateTimePicker1.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Продолжительность"))
                {
                    numericUpDown1.Location = new Point(120, 75);
                    numericUpDown1.Visible = true;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Location = new Point(120, 110);
                    numericUpDown2.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Место проведения"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    comboBox2.Location = new Point(120, 110);
                    comboBox2.Visible = true;
                    comboBox2.DataSource = dataSource("USE TicketSale; SELECT * FROM[Место проведения];");
                    comboBox2.DisplayMember = "Наименование";
                    comboBox2.ValueMember = "Id";
                }
                else if (comboBox4.SelectedItem.Equals("Цена"))
                {
                    numericUpDown1.Location = new Point(120, 75);
                    numericUpDown1.Visible = true;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Location = new Point(120, 110);
                    numericUpDown2.Visible = true;
                }
            }
            else if (selectTable.Equals("Место проведения"))
            {
                label2.Location = new Point(5, 75);
                label2.Text = "Текущее значение:";
                label2.Visible = true;
                label3.Location = new Point(5, 112);
                label3.Text = "Новое значение:";
                label3.Visible = true;
                if (comboBox4.SelectedItem.Equals("Наименование"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    textBox2.Location = new Point(120, 110);
                    textBox2.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Количество мест"))
                {
                    numericUpDown1.Location = new Point(120, 75);
                    numericUpDown1.Visible = true;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Location = new Point(120, 110);
                    numericUpDown2.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Количество рядов"))
                {
                    numericUpDown1.Location = new Point(120, 75);
                    numericUpDown1.Visible = true;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Location = new Point(120, 110);
                    numericUpDown2.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Адрес"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    textBox2.Location = new Point(120, 110);
                    textBox2.Visible = true;
                }
            }
            else if (selectTable.Equals("Клиент"))
            {
                label2.Location = new Point(5, 75);
                label2.Text = "Текущее значение:";
                label2.Visible = true;
                label3.Location = new Point(5, 112);
                label3.Text = "Новое значение:";
                label3.Visible = true;
                if (comboBox4.SelectedItem.Equals("Фамилия"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    textBox2.Location = new Point(120, 110);
                    textBox2.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Имя"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    textBox2.Location = new Point(120, 110);
                    textBox2.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Отчество"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    textBox2.Location = new Point(120, 110);
                    textBox2.Visible = true;
                }
                else if (comboBox4.SelectedItem.Equals("Название мероприятия"))
                {
                    textBox1.Location = new Point(120, 75);
                    textBox1.Visible = true;
                    textBox1.Enabled = false;
                    comboBox2.Location = new Point(120, 110);
                    comboBox2.Visible = true;
                    comboBox2.DataSource = dataSource("USE TicketSale; SELECT * FROM[Мероприятие];");
                    comboBox2.DisplayMember = "Название мероприятия";
                    comboBox2.ValueMember = "Id";
                }
                else if (comboBox4.SelectedItem.Equals("Место"))
                {
                    numericUpDown1.Location = new Point(120, 75);
                    numericUpDown1.Visible = true;
                    numericUpDown1.Enabled = false;
                    numericUpDown2.Location = new Point(120, 110);
                    numericUpDown2.Visible = true;
                }
            }
            MessageBox.Show("Выберете строку!", "Внимание",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            select = dataGridView1.SelectedRows;
            if(selectTable != null)
            {
                if(comboBox4.SelectedItem != null)
                {
                    if (selectTable.Equals("Тип мероприятия"))
                    {
                        typeEvent.Id = Convert.ToInt32(select[0].Cells[0].Value);
                        textBox1.Text = select[0].Cells[1].Value.ToString();
                    }
                    else if (selectTable.Equals("Место проведения"))
                    {
                        venue.Id = Convert.ToInt32(select[0].Cells[0].Value);
                        if (comboBox4.SelectedItem.Equals("Наименование"))
                        {
                            textBox1.Text = select[0].Cells[1].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Количество мест"))
                        {
                            numericUpDown1.Value = Convert.ToInt32(select[0].Cells[2].Value);
                        }
                        else if (comboBox4.SelectedItem.Equals("Количество рядов"))
                        {
                            numericUpDown1.Value = Convert.ToInt32(select[0].Cells[3].Value);
                        }
                        else if (comboBox4.SelectedItem.Equals("Адрес"))
                        {
                            textBox1.Text = select[0].Cells[4].Value.ToString();
                        }
                    }
                    else if (selectTable.Equals("Мероприятие"))
                    {
                        _event.Id = Convert.ToInt32(select[0].Cells[0].Value);
                        if (comboBox4.SelectedItem.Equals("Тип"))
                        {
                            textBox1.Text = select[0].Cells[1].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Название мероприятия"))
                        {
                            textBox1.Text = select[0].Cells[2].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Дата"))
                        {
                            textBox1.Text = select[0].Cells[3].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Продолжительность"))
                        {
                            numericUpDown1.Text = select[0].Cells[4].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Место проведения"))
                        {
                            textBox1.Text = select[0].Cells[5].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Цена"))
                        {
                            numericUpDown1.Text = select[0].Cells[6].Value.ToString();
                        }
                    }
                    else if (selectTable.Equals("Клиент"))
                    {
                        client.Id = Convert.ToInt32(select[0].Cells[0].Value);
                        if (comboBox4.SelectedItem.Equals("Фамилия"))
                        {
                            textBox1.Text = select[0].Cells[1].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Имя"))
                        {
                            textBox1.Text = select[0].Cells[2].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Отчество"))
                        {
                            textBox1.Text = select[0].Cells[3].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Название мероприятия"))
                        {
                            textBox1.Text = select[0].Cells[4].Value.ToString();
                        }
                        else if (comboBox4.SelectedItem.Equals("Место"))
                        {
                            numericUpDown1.Value = Convert.ToInt32(select[0].Cells[5].Value);
                        }
                    }
                }
            }
        }
    }
}
