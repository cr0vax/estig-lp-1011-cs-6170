 using System;
 using System.Data;
 using Mono.Data.SqliteClient;
 
 public class Test
 {
    public static void Main(string[] args)
    {
       string connectionString = "URI=file:rebides.db,version=3";
       IDbConnection dbcon;
       dbcon = (IDbConnection) new SqliteConnection(connectionString);
       dbcon.Open();
       IDbCommand dbcmd = dbcon.CreateCommand();
       // requires a table to be created named employee
       // with columns firstname and lastname
       // such as,
       //        CREATE TABLE employee (
       //           firstname varchar(32),
       //           lastname varchar(32));
       string sql =
          "SELECT nome_completo " +
          "FROM docentes";
       dbcmd.CommandText = sql;
       IDataReader reader = dbcmd.ExecuteReader();
       while(reader.Read()) {
            string FirstName = reader.GetString (0);
            // string LastName = reader.GetString (1);
            Console.WriteLine("Name: " +
                FirstName);
       }
       // clean up
       reader.Close();
       reader = null;
       dbcmd.Dispose();
       dbcmd = null;
       dbcon.Close();
       dbcon = null;
    }
 }