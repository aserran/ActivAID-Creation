using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DatabaseCreator
{
    class DatabaseCreator
    {
        //public static string DB_NAME = Environment.GetEnvironmentVariable("DBNAME");
        public static string DB_NAME = "ActivAID DB";
        public static string serverName = Environment.GetEnvironmentVariable("SERVER");
        private string dbLocation;
        private SqlConnectionStringBuilder builder;
        private SqlConnection conn;
        private SqlCommand comm;
        private string elementsQuery = "CREATE TABLE Elements(elementId int NOT NULL IDENTITY (1000,1), fileId int, blockNumber int, data varchar(MAX));";
        private string filesQuery = "CREATE TABLE Files(fileId int NOT NULL IDENTITY (1,1), filePath varchar(MAX), filename varchar(MAX));";
        private string hyperlinksQuery = "CREATE TABLE Hyperlinks(hyperlinkId int NOT NULL IDENTITY (100,10), fileId int, filePath varchar(MAX), filename varchar(MAX), text varchar(MAX));";
        private string imagesQuery = "CREATE TABLE Images(imageId int NOT NULL IDENTITY (1,1), elementId int, elementImg varchar(MAX));";

        public DatabaseCreator()
        {
            // Create SqlConnection
            conn = new SqlConnection("Server="+serverName+";Integrated security=SSPI;database=master;");
            conn.Open();

            bool result = CreateDatabase();
            if (result)
                CreateTables();
            else
                Console.WriteLine("Database not created.");

            closeConnection();
        }

        private void closeConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();

            conn.Dispose();
        }

        public bool CreateDatabase()
        {
            bool status = true;

            string sqlCreateString = "CREATE DATABASE [" + DB_NAME + "]";
            comm = new SqlCommand(sqlCreateString, conn);
            try
            {
                comm.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                if (e.Number == 1801)   // Database already exist error
                {
                    dropDatabase();
                    status = CreateDatabase();
                    return status;
                }
                else {
                    MessageBox.Show(e.ToString(), "Unable to Create Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    status = false;
                }
            }
            // Connection gets closed when constuctor finishes
            return status;                
        }

        private void dropDatabase()
        {
            string sqlCreateString;
            try
            {
                sqlCreateString = "DROP DATABASE [" + DB_NAME + "]";
                comm = new SqlCommand(sqlCreateString, conn);
                comm.ExecuteNonQuery();
            }
            catch (System.Exception se)
            {
                MessageBox.Show(se.ToString(), "Unable to Create Database", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CreateTables()
        {
            dbLocation = "Server="+serverName+";Database = "+ DB_NAME + ";Integrated Security=true";
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
