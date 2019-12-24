using System.Data.SqlClient;

namespace TicketSale
{
    class Client
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public int idEvent { get; set; }

        public int Location { get; set; }

        public void insertInDatabase(SqlConnection connection)
        {
            string cmd = "USE TicketSale; INSERT INTO [Клиент] " +
                        "VALUES('" + FirstName + "', '" + Name + "', '" +
                        LastName + "', " + idEvent + ", " + Location + ");";
            SqlCommand sqlCommand = new SqlCommand(cmd, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
        }

        public void deleteDataInDB(SqlConnection connection, object value)
        {
            Id = (int)value;
            string cmd = "USE TicketSale; DELETE FROM[Клиент] WHERE Id = " + value + ";";
            SqlCommand sqlCommand = new SqlCommand(cmd, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
        }
    }
}
