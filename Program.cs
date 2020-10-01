using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "JORGE-W-01";
            builder.UserID = "sa";
            builder.Password = "Sa2020.";
            builder.InitialCatalog = "ClinicaMedicaDb";
            try
            {
                Console.WriteLine("\n1 - Inicio, Obtenemos el archivo de Azure");
                var rootDir = AppDomain.CurrentDomain.BaseDirectory;
                Console.WriteLine(rootDir);
                string remoteUri = "https://storage10082020.blob.core.windows.net/y9ne9ilzmfld/";
                string fileName = "Stock.CSV", myStringWebResource = null;
                WebClient myWebClient = new WebClient();
                myStringWebResource = remoteUri + fileName;
                Console.WriteLine("Descargando Archivo \"{0}\" Desde el origen \"{1}\" .......\n\n", fileName, myStringWebResource);
                myWebClient.DownloadFile(myStringWebResource, fileName);
                Console.WriteLine("Descarga exitosa del archivo \"{0}\" de la URL \"{1}\"", fileName, myStringWebResource);
                Console.WriteLine("\nSe descargo con exito el archivo en la carpeta:\n\t" + rootDir);
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\n2 - Eliminar data en la tabla (Truncate) antes de insertar los nuevos datos:");
                    Console.WriteLine("=========================================\n");
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("TRUNCATE TABLE valores");
                    String sql = sb.ToString();
                    using SqlCommand command = new SqlCommand(sql, connection);
                    command.CommandTimeout = 0;
                    using SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                    connection.Open();
                    Console.WriteLine("\n3 - Insertar datos nuevos en la tabla:");
                    Console.WriteLine("=========================================\n");
                    StringBuilder sb2 = new StringBuilder();
                    /*string[] separators = {";"};
                    string[] Valores = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);*/
                    //sb.Append("INSERT INTO valores(valor1,valor2,valor3,valor4) VALUES('"+ Valores[0]+"','"+ Valores[1]+"','"+ Valores[2]+"','"+ Valores[3]+"');");
                    sb2.Append("BULK INSERT valores FROM 'C:\\Users\\jorge\\source\\repos\\ConsoleApp\\bin\\Debug\\netcoreapp3.1\\Stock.CSV' WITH(FIRSTROW = 2, DATAFILETYPE = 'char', FIELDTERMINATOR = ';', ROWTERMINATOR = '\n', batchSize = 10000); ");
                    String sql2 = sb2.ToString();
                    using SqlCommand command2 = new SqlCommand(sql2, connection);
                    command2.CommandTimeout = 0;
                    using SqlDataReader reader3 = command2.ExecuteReader();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            /*WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead("https://storage10082020.blob.core.windows.net/y9ne9ilzmfld/Stock.CSV");
            try
            {
               using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    Console.WriteLine(reader);
                    int sum = 0;
                while (!reader.EndOfStream)
                {
                    string text = reader.ReadLine();
                    Console.WriteLine("\n2 - Leemos los datos en Azure:");
                    Console.WriteLine("=========================================\n");
                    sum++;
                    Console.WriteLine("NRO ->"+sum+"..."+text); 
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        Console.WriteLine("\n3 - Insertar datos nuevos en la tabla:");
                        Console.WriteLine("=========================================\n");
                        connection.Open();
                        StringBuilder sb = new StringBuilder();
                        string[] separators = {";"};
                        string[] Valores = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        //sb.Append("INSERT INTO valores(valor1,valor2,valor3,valor4) VALUES('"+ Valores[0]+"','"+ Valores[1]+"','"+ Valores[2]+"','"+ Valores[3]+"');");
                        sb.Append("BULK INSERT valores FROM 'C:\\Users\\jorge\\source\\repos\\ConsoleApp\\bin\\Debug\\netcoreapp3.1\\Stock2.CSV' WITH(FIRSTROW = 2, DATAFILETYPE = 'char', FIELDTERMINATOR = ';', ROWTERMINATOR = '\n', batchSize = 100000); ");
                        String sql = sb.ToString();
                        using SqlCommand command = new SqlCommand(sql, connection);                       
                        using SqlDataReader reader3 = command.ExecuteReader();
                        connection.Close();
                    }
                }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }*/
            Console.WriteLine("\n4 - Proceso completo. Presionar enter.");
            Console.ReadLine();
        }
    }
}