/*
 * **********************************************
 * Class responsable to generate DB queries
 * 
 * Author: Bruno Moreira
 * Date  : 2011/06/18
 * **********************************************
 */

using System;
using System.Data;
using Mono.Data.SqliteClient;

namespace rebides
{
	public class DataBase
	{
		const string DATABASE_NAME = "pristine.db";		// database name
		IDbConnection dbcon;							// connection to database
	       
		/*
		 * Constructor for database class
		 * */
		public DataBase ()
		{
			// create connection
			createDataBaseConnection();
		}
		
		/*
		 * Create connection to database
		 * */
		private void createDataBaseConnection()
		{
			// build database connection string
			string connectionString = "URI=file:" + DATABASE_NAME + ",version=3";
			
			// create connection
			this.dbcon = (IDbConnection) new SqliteConnection(connectionString);
		}
		
		
		/*
		 * Run command against the database
		 * */
		public IDbCommand runCommand(string strSQL)
		{
			// open database connection
			this.dbcon.Open();
			
			// run command against database
			IDbCommand dbcmd = dbcon.CreateCommand();
			dbcmd.CommandText = strSQL;
			
			// return command
			return dbcmd;			
		}
	}
}
