using System.Data.SqlClient;

namespace TicketSale
{
    class TypeEvent
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public void insertInDatabase(SqlConnection connection)
        {
            string cmd = "USE TicketSale;INSERT INTO [Тип мероприятия] VALUES('" + Type + "');";
            SqlCommand sqlCommand = new SqlCommand(cmd, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
        }

        public void deleteDataInDB(SqlConnection connection, object value)
        {
            Id = (int)value;
            string cmd = "USE TicketSale; DELETE FROM[Тип мероприятия] WHERE Id = " + value + ";";
            SqlCommand sqlCommand = new SqlCommand(cmd, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
        }

        public override string ToString() { return Type; }
    }
}
