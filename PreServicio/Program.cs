using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace PreServicio
{
    class Program
    {
        private static string db;
        private static string connString;
        static void Main(string[] args)
        {
            db = ConfigurationManager.AppSettings["database"].ToString();
            connString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}",db);
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create("http://pagina.com/webservice/getLastCheck");
            List<Dictionary<string,string>> valores = new List<Dictionary<string,string>>();
            try {
                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    connection.Open();
                    OleDbDataReader reader = null;
                    OleDbCommand command = new OleDbCommand("SELECT USERID, NAME, TITLE from  userinfo", connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        valores.Add(new Dictionary<string, string>
                        {
                            {"reloj_id", reader[0].ToString()},
                            {"nombre", reader[1].ToString()},
                            {"cargo", reader[2].ToString()}
                            
                        });
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string json = js.Serialize(valores);
                    Console.WriteLine(json);
                    /*if (!string.IsNullOrEmpty(json)) {
                        var request = (HttpWebRequest)WebRequest.Create("http://192.168.0.101:8080");
                        request.Method = "POST";
                        var requestBody = Encoding.UTF8.GetBytes(json);
                        request.ContentLength = requestBody.Length;
                        request.ContentType = "application/json";
                        using (var requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(requestBody, 0, requestBody.Length);
                        }
                        using (var respuesta = request.GetResponse())
                        {
                            using (var leeRespuesta = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252)))
                            {
                                var output = leeRespuesta.ReadToEnd();
                            }
                        }
                        
                    }*/
                }
                
                    
            }catch(Exception e){
                Console.WriteLine(e.ToString());
            }

        }
    }
}
