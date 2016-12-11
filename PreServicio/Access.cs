using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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

        public string usuarios(string ultimo)
        {
            string[] apellido = null;
            string last = "";
            List<Dictionary<string,string>> valores = new List<Dictionary<string,string>>();
            try
            {
                string sub = string.Format("SELECT USERID FROM SERVERLOG WHERE EVENT='user append' and EVENTTIME > CDATE({0})",ultimo);
                var reader = query(string.Format("SELECT USERID, NAME, TITLE from  userinfo WHERE USERID IN ({0})",sub));
                while (reader.Read())
                {
                    if ((apellido = reader[1].ToString().Split(' ')).Length > 1)
                        last = apellido[1];
                    else
                        last = "";
                    valores.Add(new Dictionary<string, string>
                    {
                        {"reloj_id", reader[0].ToString()},
                        {"nombre", apellido[0]},
                        {"apellido", last},
                        {"cargo", reader[2].ToString()}
                            
                    });
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(valores);
                return json;
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return ""; 
            }
        }

        public string check(string last)
        {
            List<Dictionary<string, string>> valores = new List<Dictionary<string, string>>();
            try
            {
                var reader = query("SELECT USERID, CHECKTIME, CHECKTYPE from CHECKINOUT");
                while (reader.Read())
                {
                    valores.Add(new Dictionary<string, string>
                    {
                        {"reloj_id", reader[0].ToString()},
                        {"checktime", reader[1].ToString()},
                        {"checktype", reader[2].ToString()}
                    });
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(valores);
                return json;
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        public string del(string ultimo)
        {
            List<Dictionary<string, string>> valores = new List<Dictionary<string, string>>();
            try
            {
                var reader = query(string.Format("SELECT USERID FROM SERVERLOG WHERE EVENT='user delete' and EVENTTIME > CDATE({0})",ultimo));
                while (reader.Read())
                {
                    valores.Add(new Dictionary<string, string>
                    {
                        {"reloj_id", reader[0].ToString()}
                    });
                }
                JavaScriptSerializer js = new JavaScriptSerializer();
                string json = js.Serialize(valores);
                return json;
            }
            catch (Exception e) { return ""; }
        }

        public void insertAhora()
        {
            try {
                OleDbCommand command = new OleDbCommand("SELECT NOW()", this.connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    File.WriteAllText("last.txt", string.Format("{0}{1}{0}", "'", reader[0].ToString()));
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
 

    }
}
