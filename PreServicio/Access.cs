using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreServicio
{
    class Access
    {
        private OleDbConnection connection;
        public Access(string db){
            connection = new OleDbConnection(string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", db));
            connection.Open();
        }

        public OleDbDataReader query(string consulta){
            OleDbDataReader reader = null;
            OleDbCommand command = new OleDbCommand(consulta,this.connection);
            reader = command.ExecuteReader();
            return reader;       
        }
    }
}
