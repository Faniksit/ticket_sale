using System.Data.SqlClient;

namespace TicketSale
{
    class Venue
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountSeats { get; set; }

        public int CountRow { get; set; }

        public string Address { get; set; }

        public void insertInDatabase(SqlConnection connection)
        {
            string cmd = "USE TicketSale;INSERT INTO [Место проведения] " +
                        "VALUES('" + Name + "', " + CountSeats + ", " + CountRow + ", '" + Address + "');";
            SqlCommand sqlCommand = new SqlCommand(cmd, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
        }

        public void deleteDataInDB(SqlConnection connection, object value)
        {
            Id = (int)value;
            string cmd = "USE TicketSale; DELETE FROM[Место проведения] WHERE Id = " + value + ";";
            SqlCommand sqlCommand = new SqlCommand(cmd, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
        }

        public override string ToString() { return Name; }
    }
}
