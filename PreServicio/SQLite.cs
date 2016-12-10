using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreServicio
{
    class SQLite
    {
        private SQLiteConnection connection;
        private SQLiteCommand command;
        public SQLite(string bd) {
            connection = new SQLiteConnection(string.Format(@"Data Source={0};Version=3;",bd));
            connection.Open();
        }

        private void insert(string consulta)
        {
            command = new SQLiteCommand("DELETE FROM LAST",connection);
            command.ExecuteNonQuery();
            command.Dispose();
            command = new SQLiteCommand(consulta, connection);
            command.ExecuteNonQuery();

        }

        private SQLiteDataReader select(string consulta)
        {
            command = new SQLiteCommand(consulta, connection);
            return command.ExecuteReader();
        }

    }
}
