 /*
 * **********************************************
 * Class responsable to generate lists
 * 
 * Author: Bruno Moreira
 * Date  : 2011/06/18
 * **********************************************
 */

 using System;
 using System.Data;
 using System.Collections.Generic;
 using Mono.Data.SqliteClient;
 using rebides;
 
 public class Rebides
 {
    public static void Main(string[] args)
    {
		DataBase dbRebides = new DataBase();
		File outputFile = new File();
		
		/*
		// Total number of teachers in the higher education system per year
		Dictionary<int, Lists> l = new Dictionary<int, Lists>();
		
		for (int i = 0; i < 10; i++)
		{
			IDbCommand dbcmd = dbRebides.runCommand(
			        "SELECT count(distinct docente_id) " +
					"FROM informatica_registodocencia " +
					"WHERE ano ="+i+";");
			IDataReader reader = dbcmd.ExecuteReader();
		
			List<string> tempList = new List<string>();
				
			while(reader.Read()) {
				tempList.Add(reader.GetString (0));
				l.Add(i, new Lists(tempList));
			}
		}
		
		// print out list
		for ( int i = 0; i < 10; i++)
		{
			Console.WriteLine("200{0};{1}", i, l[i].ToString());
		}
		*/
		// Total number of teachers per establishment and per year
		Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
		
		for (int i = 0; i < 10; i++)
		{
			IDbCommand dbcmd = dbRebides.runCommand(
			        "SELECT e.designacao, count(distinct rd.docente_id) " +
					"FROM informatica_registodocencia rd " +
			        "INNER JOIN informatica_estabelecimento e on " + 
			        	"e.id = rd.estabelecimento_id " +
					"WHERE rd.ano =" + i +
			        " GROUP BY rd.estabelecimento_id;");
			
			IDataReader reader = dbcmd.ExecuteReader();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			CustomTuple tempTuple;
		
			while(reader.Read()) {
				
			    // get tuple string columns
				for ( int u = 0; u < reader.FieldCount - 1; u++ )
				{	
					//Console.WriteLine(reader.GetString(u));));));
					tempColumnList.Add(reader.GetString(u));
				}
												
				//Console.WriteLine(reader.GetInt32(reader.FieldCount - 1));
				// create new tuple
				tempTuple = new CustomTuple(tempColumnList, reader.GetInt32(reader.FieldCount - 1));
				
				// add tuple to temp list
				tempList.Add(tempTuple);
				
				// prepare tuples and lists for next reader
				tempColumnList.Clear();
				tempTuple = null;
			}

			// sort list
			tempList.Sort();

			// add list to file
			outputFile.write("200" + i);
			foreach (CustomTuple row in tempList)
			{
				outputFile.write(row.ToString());
			}
			
			// add list to year dictionary
			l.Add(i, tempList);

		}
		
		/*
		// Total number of teachers per degree and per year
		for (int i = 0; i < 10; i++)
		{
			IDbCommand dbcmd = dbRebides.runCommand(
			        "SELECT g.designacao, count(distinct rd.docente_id) " +
					"FROM informatica_registodocencia rd " +
			        "INNER JOIN informatica_grau g on " + 
			        	"g.id = rd.grau_id " +
					"WHERE rd.ano =" + i +
			        " GROUP BY rd.grau_id;");
			IDataReader reader = dbcmd.ExecuteReader();
		
			while(reader.Read()) {
			    Console.WriteLine("200{0} :: {1} :: {2}", 
				                  i, 
				                  reader.GetString (0), 
				                  reader.GetString (1));
			}
		}
		
		// Total number of teachers per degree, per establishment and per year
		for (int i = 0; i < 10; i++)
		{
			IDbCommand dbcmd = dbRebides.runCommand(
			        "SELECT e.designacao, g.designacao, count(distinct rd.docente_id) " +
					"FROM informatica_registodocencia rd " +
			        "INNER JOIN informatica_grau g on " + 
			        	"g.id = rd.grau_id " +
					"INNER JOIN informatica_estabelecimento e on " + 
			        	"e.id = rd.estabelecimento_id " +
					"WHERE rd.ano =" + i +
			        " GROUP BY rd.estabelecimento_id, rd.grau_id;");
			IDataReader reader = dbcmd.ExecuteReader();
		
			while(reader.Read()) {
			    Console.WriteLine("200{0} :: {1} :: {2} :: {3}", 
				                  i, 
				                  reader.GetString (0), 
				                  reader.GetString (1), 
				                  reader.GetString (2));
			}
		}
		// Number of holders of a doctorate degree, per establishment and per year
		for (int i = 0; i < 10; i++)
		{
			IDbCommand dbcmd = dbRebides.runCommand(
			        "SELECT id " + 
			        "FROM informatica_grau " + 
			        "WHERE designacao LIKE ('Do%');");
			IDataReader reader = dbcmd.ExecuteReader();

			// Write to results to console
			while(reader.Read()) {
			    Console.WriteLine("200{0} :: {1}", 
				                  i, 
				                  reader.GetString (0));
			}
		}
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbRebides = null;
		*/
    }
 }