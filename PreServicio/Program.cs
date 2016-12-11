using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.OleDb;
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
        private static string add = ConfigurationManager.AppSettings["add"].ToString();
        private static string del = ConfigurationManager.AppSettings["del"].ToString();
        private static string check = ConfigurationManager.AppSettings["check"].ToString();
        private static string json;
        static void Main(string[] args)
        {
            db = ConfigurationManager.AppSettings["database"].ToString();
            connString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}",db);
            List<Dictionary<string,string>> valores = new List<Dictionary<string,string>>();
            Access adb = new Access(connString);
            try {
               string ultimo = getUltimo();
               if (!string.IsNullOrEmpty((json = adb.usuarios(ultimo))))
               {
                   post(add, json);
               }
               /*if (!string.IsNullOrEmpty((json = adb.check(""))))
               {
                   post(check, json);
               }*/
               if (!string.IsNullOrEmpty((json = adb.del(ultimo))))
               {
                   post(del, json);
               }
               adb.insertAhora();                
                    
            }catch(Exception e){
                Console.WriteLine(e.ToString());
            }

        }

        public static void post(string url, string json){
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                //Console.WriteLine(result);
            }
        }


        public static string getUltimo()
        {
            string ultimo = "'01/01/2000 00:00:00'";
            try
            {
                string text = System.IO.File.ReadAllText("last.txt");
                return text;
            }
            catch (Exception e)
            {
            }
            return ultimo;
        }

    }
}
