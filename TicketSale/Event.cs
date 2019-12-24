using System;
using System.Data.SqlClient;

namespace TicketSale
{
    class Event
    {
        public int Id { get; set; }

        public string NameEvent { get; set; }

        public int idTypeEvent { get; set; }

        public DateTime Date { get; set; }

        public int Length { get; set; }

        public int idVenue { get; set; }

        public int Price { get; set; }

        public void insertInDatabase(SqlConnection connection)
        {
            string cmd = "USE TicketSale; INSERT INTO [Мероприятие] " +
                        "VALUES(" + idTypeEvent + ", '" + NameEvent + "', '" + Date + "', " +
                        Length + ", " + idVenue + ", " + Price + ")";
            SqlCommand sqlCommand = new SqlCommand(cmd, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
        }

        public void deleteDataInDB(SqlConnection connection, object value)
        {
            Id = (int)value;
            string cmd = "USE TicketSale; DELETE FROM[Мероприятие] WHERE Id = " + value + ";";
            SqlCommand sqlCommand = new SqlCommand(cmd, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
        }

        public override string ToString() { return NameEvent; }
    }
}