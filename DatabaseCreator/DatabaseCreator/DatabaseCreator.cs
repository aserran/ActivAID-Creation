using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DatabaseCreator
{
    class DatabaseCreator
    {
        //public static string DB_NAME = Environment.GetEnvironmentVariable("DBNAME");
        public static string DB_NAME = "ActivAID DB";
        //public static string DB_SERVER = Environment.GetEnvironmentVariable("SERVER");
        public static string DB_SERVER = "localhost\\SQLEXPRESS";
        public static string DB_PATH = "C:\\Program Files\\Microsoft SQL Server\\MSSQL12.SQLEXPRESS\\MSSQL\\DATA\\";
        /* Currently needs a data folder in C drive
         * need to replace with location of future ActivAID files*/
        //public static string DB_PATH = "C:\\Program Files\\Microsoft SQL Server\\MSSQL12.SQLEXPRESS\\MSSQL\\DATA";
        private string dbLocation;
        private SqlConnectionStringBuilder builder;
        private SqlConnection conn;
        private string elementsQuery = "CREATE TABLE Elements(elementId int NOT NULL IDENTITY (1000,1), fileId int, blockNumber int, data varchar(MAX));";
        private string filesQuery = "CREATE TABLE Files(fileId int NOT NULL IDENTITY (1,1), filePath varchar(MAX), filename varchar(MAX));";
        private string hyperlinksQuery = "CREATE TABLE Hyperlinks(hyperlinkId int NOT NULL IDENTITY (100,10), fileId int, filePath varchar(MAX), filename varchar(MAX), text varchar(MAX));";
        private string imagesQuery = "CREATE TABLE Images(imageId int NOT NULL IDENTITY (1,1), elementId int, elementImg varchar(MAX));";

        public DatabaseCreator()
        {
            bool result = CreateDatabase();
            if (result)
            {
                CreateTables();
            }
            else
            {
                Console.WriteLine("Database not created.");
            }
        }

        public bool CreateDatabase()
        {
            bool status = true;
            string sqlCreateString;
            conn = new SqlConnection("Server=localhost\\SQLEXPRESS;Integrated security=SSPI;database=master;");

            sqlCreateString = "CREATE DATABASE ["
                                + DB_NAME + "] ON PRIMARY "
                                + " (NAME = [" + DB_NAME + "_Data], "
                                + " FILENAME = '" + DB_PATH + DB_NAME + ".mdf', "
                                + "SIZE = 5MB,"
                                + " FILEGROWTH = 10%) "
                                + " LOG ON (NAME = [" + DB_NAME + "_Log], "
                                + " FILENAME = '" + DB_PATH + DB_NAME + "Log.ldf', "
                                + " SIZE = 1MB, "
                                + " FILEGROWTH = 10%) ";
            //sqlCreateString = "CREATE DATABASE " + DB_NAME + ";";
            SqlCommand comm = new SqlCommand(sqlCreateString, conn);
            try
            {
                conn.Open();
                comm.ExecuteNonQuery();
            }
            catch (System.Exception)
            {
                throw;
                Console.WriteLine("Database string not right.");
                status = false;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
            return status;                
        }

        private void CreateTables()
        {
            dbLocation = "Server=.\\SQLEXPRESS;Database = "+ DB_NAME + ";Integrated Security=true";
            builder = new SqlConnectionStringBuilder();
            //builder.DataSource = 
            using (conn = new SqlConnection(dbLocation))
            {
                conn.Open();
                using (SqlCommand cmd1 = new SqlCommand(elementsQuery, conn)) { cmd1.ExecuteNonQuery(); }
                using (SqlCommand cmd2 = new SqlCommand(filesQuery, conn)) { cmd2.ExecuteNonQuery(); }
                using (SqlCommand cmd3 = new SqlCommand(hyperlinksQuery, conn)) { cmd3.ExecuteNonQuery(); }
                using (SqlCommand cmd4 = new SqlCommand(imagesQuery, conn)) { cmd4.ExecuteNonQuery(); }
            }
            conn.Close();
        }

    }
}
