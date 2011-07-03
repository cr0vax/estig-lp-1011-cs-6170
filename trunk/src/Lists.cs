/*
 * **********************************************
 * Class responsable to generate lists
 * 
 * Author: Bruno Moreira
 * Date  : 2011/06/18
 * **********************************************
 */

using System.IO;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Mono.Data.SqliteClient;
using rebides;

namespace rebides
{

	public class Lists
	{	
		DataBase dbRebides = new DataBase();
		// open file
		File outputFile = new File();
		
		// add title to lists
		private void add_title ( string strTitleToBeAdded, File outputFile )
		{ 
			string title_bar = "";
			
			strTitleToBeAdded = "#" + strTitleToBeAdded + "#";
			for ( int i = 0; i < strTitleToBeAdded.Length; i++ )
			{
				title_bar = title_bar + "#";
			}
			
			outputFile.write(title_bar);
			outputFile.write(strTitleToBeAdded);
			outputFile.write(title_bar);
		}
		
		private void write_list_to_file( Dictionary<int, List<CustomTuple>> list, string title, bool is_counter )
		{

			// write list title to file
			this.add_title(title, outputFile);
			
			// add list to file
			List<string> old_tuple_data = new List<string>();
			
			foreach ( KeyValuePair<int, List<CustomTuple>> dict_row in list )
			{				
				// write year
				outputFile.write("[200" + dict_row.Key + "]");
				
				// get row data
				foreach ( CustomTuple tuple_row in dict_row.Value )
				{
					// get tuple data
					List<string> tuple_data = tuple_row.getData();
					
					// check each column of tuple data
					for ( int v = 0; v < tuple_data.Count; v++ )
					{
						// check if it's counter and it's the last 2 columns
						if ( tuple_data.Count - 2 == v && is_counter )
						{
							// write last 2 columns
							outputFile.write(" " + tuple_data[v] + ";" + tuple_data[v + 1]);
							
							// go to last index
							v++;
						}
						else
						{
							// check if there is older data saved
							if ( old_tuple_data.Capacity > 0 )
							{
								// check if older data is different from actual data
								if ( old_tuple_data[v] != tuple_data[v] )
								{
									// check if it's the last column
									if ( v == tuple_data.Count -1 )
									{
										outputFile.write(" " + tuple_data[v]);
									}
									else
									{
										outputFile.write("»" + tuple_data[v]);
									}
								}	
							}
							else
							{
								// check if it's the last column
								if ( v == tuple_data.Count -1 )
								{
									outputFile.write(" " + tuple_data[v]);
								}
								else
								{
									outputFile.write("»" + tuple_data[v]);
								}
							}
						}
					}
					
					// save old tuple data
					old_tuple_data = tuple_data;
				}
				
				// clear old tuple data
				old_tuple_data = new List<string>();
			}
			
			// flushes file
			outputFile.flush();
		}
		
		
		// Total number of teachers in the higher education system per year
		public void make_tnthespy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<string> tempColumnList = new List<string>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			
			Console.WriteLine("Total number of teachers in the higher education system per year");
			
			for (int i = 0; i < 10; i++)
			{
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT count(distinct docente_id) " +
						"FROM informatica_registodocencia " +
						"WHERE ano ="+i+";");
				IDataReader reader = dbcmd.ExecuteReader();
					
				// add teacher names to tuple
				reader.Read();
				tempColumnList.Add(reader.GetString(0));
			
		    	// add tuple to temp list
				tempList.Add(new CustomTuple(tempColumnList));
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear custom tuple
				tempColumnList.Clear();
				tempList.Clear();
			}
			
			write_list_to_file(l, "Total number of teachers in the higher education system per year", true);
		}
		
		// Total number of teachers per establishment and per year
		public void make_tntpepy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader reader;
			
			Console.WriteLine("Total number of teachers per establishment and per year");
			
			for (int i = 0; i < 10; i++)
			{
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT e.designacao, count(distinct rd.docente_id) " +
						"FROM informatica_registodocencia rd " +
				        "INNER JOIN informatica_estabelecimento e on " + 
				        	"e.id = rd.estabelecimento_id " +
						" WHERE rd.ano =" + i +
				        " GROUP BY rd.estabelecimento_id;");
				
				reader = dbcmd.ExecuteReader();
				
				while(reader.Read()) {
					
				    // get tuple string columns
					for ( int u = 0; u < reader.FieldCount; u++ )
					{
						tempColumnList.Add(reader.GetString(u));
					}

					// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
					
					// prepare tuples and lists for next reader
					tempColumnList.Clear();
				}
	
				// sort list
				tempList.Sort();

				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			
			// write to file
			write_list_to_file(l, "Total number of teachers per establishment and per year", true);
		}
		
		// Total number of teachers per degree and per year
		public void make_tntpdpy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader reader;
			
			Console.WriteLine("Total number of teachers per degree and per year");
			
			for (int i = 0; i < 10; i++)
			{
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT g.designacao, count(distinct rd.docente_id) " +
						"FROM informatica_registodocencia rd " +
				        "INNER JOIN informatica_grau g on " + 
				        	"g.id = rd.grau_id " +
						"WHERE rd.ano =" + i +
				        " GROUP BY rd.grau_id;");
				reader = dbcmd.ExecuteReader();
				
				while(reader.Read()) {
					
				    // get tuple string columns
					for ( int u = 0; u < reader.FieldCount; u++ )
					{
						tempColumnList.Add(reader.GetString(u));
					}

					// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
					
					// prepare tuples and lists for next reader
					tempColumnList.Clear();
				}
	
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i,  new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			
			// write list to file
			write_list_to_file(l, "Total number of teachers per degree and per year", true);
		}
		
		// Total number of teachers per degree, per establishment and per year
		public void make_tntpdpepy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader reader;
			
			Console.WriteLine("Total number of teachers per degree, per establishment and per year");
			
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
				reader = dbcmd.ExecuteReader();
				
				while(reader.Read()) {
					
				    // get tuple string columns
					for ( int u = 0; u < reader.FieldCount; u++ )
					{
						tempColumnList.Add(reader.GetString(u));
					}

					// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
					
					// prepare tuples and lists for next reader
					tempColumnList.Clear();
				}
	
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			
			// write list to file
			this.write_list_to_file(l, "Total number of teachers per degree, per establishment and per year", true);
		}
		
		// Number of holders of a doctorate degree, per establishment and per year
		public void make_nhddpepy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader reader;
			
			Console.WriteLine("Number of holders of a doctorate degree, per establishment and per year");
			
			for (int i = 0; i < 10; i++)
			{
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT e.designacao, count(distinct rd.docente_id) " +
						"FROM informatica_registodocencia rd " +
				        "INNER JOIN informatica_grau g on " + 
				        	"g.id = rd.grau_id AND " +
				            "g.designacao LIKE ('Do%') " +
						"INNER JOIN informatica_estabelecimento e on " + 
				        	"e.id = rd.estabelecimento_id " +
						"WHERE rd.ano =" + i +
				        " GROUP BY rd.estabelecimento_id;");
				reader = dbcmd.ExecuteReader();
				
				while(reader.Read()) {
					
				    // get tuple string columns
					for ( int u = 0; u < reader.FieldCount; u++ )
					{
						tempColumnList.Add(reader.GetString(u));
					}

					// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
					
					// prepare tuples and lists for next reader
					tempColumnList.Clear();
				}
	
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			
			// write list to file
			this.write_list_to_file(l, "Number of holders of a doctorate degree, per establishment and per year", true);
		}
		
		
		// The set of holders of a doctorate degree per establishment and per year
		public void make_shfdpepy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader reader;
			
			Console.WriteLine("The set of holders of a doctorate degree per establishment and per year");
			
			for (int i = 0; i < 10; i++)
			{
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT e.designacao, d.nome " +
						"FROM informatica_registodocencia rd " +
				        "INNER JOIN informatica_grau g on " + 
				        	"g.id = rd.grau_id AND " +
				            "g.designacao LIKE ('Do%') " +
						"INNER JOIN informatica_estabelecimento e on " + 
				        	"e.id = rd.estabelecimento_id " +
						"INNER JOIN informatica_docente d on " +
							"d.id = rd.docente_id " +
						"WHERE rd.ano =" + i);
				
				reader = dbcmd.ExecuteReader();
				
				while(reader.Read()) {
					
				    // get tuple string columns
					for ( int u = 0; u < reader.FieldCount; u++ )
					{
						tempColumnList.Add(reader.GetString(u));
					}

					// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
					
					// prepare tuples and lists for next reader
					tempColumnList.Clear();
				}
	
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			
			// write list to file
			this.write_list_to_file(l, "The set of holders of a doctorate degree per establishment and per year", false);
		}
		
		//The set of teachers that changed from one establishment to another one per year
		public void make_stcfetapy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> teachers_before = new List<CustomTuple>();
			List<CustomTuple> teachers_after = new List<CustomTuple>();
			
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader teachers_yearbefore_reader;
			IDataReader teachers_yearafter_reader;
			IDbCommand dbcmd;
			
			Console.WriteLine("The set of teachers that changed from one establishment to another one per year");
			
			for (int i = 1; i < 10; i++)
			{
				// query database for the teachers of the year before
				dbcmd = dbRebides.runCommand(
					"SELECT distinct rd.docente_id, e.codp, d.nome " +
					"FROM informatica_registodocencia rd " +
				    "INNER JOIN informatica_estabelecimento e on " +
				       "rd.estabelecimento_id = e.id " +
				    "INNER JOIN informatica_docente d on " +
				       "rd.docente_id = d.id " +
					"WHERE rd.ano=" + (i - 1));
				
				teachers_yearbefore_reader = dbcmd.ExecuteReader();
				
			    // add teachers of year before the one being treated to the dictionary
				while (teachers_yearbefore_reader.Read())
				{
					// get tuple string columns
					for ( int u = 0; u < teachers_yearbefore_reader.FieldCount; u++ )
					{
						tempColumnList.Add(teachers_yearbefore_reader.GetString(u));
					}
					
					// item to list
					teachers_before.Add(new CustomTuple(tempColumnList));
					
					// clear tempColumnList
					tempColumnList.Clear();
				}
				
				// query database for the teachers of current year
				dbcmd = dbRebides.runCommand(
					"SELECT distinct rd.docente_id, e.codp, d.nome " +
					"FROM informatica_registodocencia rd " +
				    "INNER JOIN informatica_estabelecimento e on " +
				       "rd.estabelecimento_id = e.id " +
					"INNER JOIN informatica_docente d on " +
				       "rd.docente_id = d.id " +
					"WHERE rd.ano=" + (i));
				
				teachers_yearafter_reader = dbcmd.ExecuteReader();
				
				// add teachers of year being treated to the dictionary
				while (teachers_yearafter_reader.Read())
				{
					// get tuple string columns
					for ( int u = 0; u < teachers_yearafter_reader.FieldCount; u++ )
					{
						tempColumnList.Add(teachers_yearafter_reader.GetString(u));
					}
					
					// item to list
					teachers_after.Add(new CustomTuple(tempColumnList));
					
					// clear tempColumnList
					tempColumnList.Clear();
				}
				
				var query = from a in teachers_after
				            join b in teachers_before
				            	on a.getData()[0] equals b.getData()[0]
							where a.getData()[1] != b.getData()[1]
				            select a.getData()[2];
				
				var q = query.Distinct();
				
				// add counter to list
				foreach( string tuple in q )
				{
					// add teacher names to tuple
					tempColumnList.Add(tuple);
				
			    	// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
					
					// clear column list
					tempColumnList.Clear();
				}
				
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				teachers_after.Clear();
				teachers_before.Clear();
				tempList.Clear();
			}
				
			// write list to file
			this.write_list_to_file(l, "The set of teachers that changed from one establishment to another one per year", false);
		}
		
		// list of establishments per year
		public void make_liepy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader reader;
			
			Console.WriteLine("list of establishments per year");
			
			for (int i = 0; i < 10; i++)
			{
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT e.designacao " +
						"FROM informatica_registodocencia rd " +
				        "INNER JOIN informatica_estabelecimento e on " + 
				        	"e.id = rd.estabelecimento_id " +
						"WHERE rd.ano =" + i);
				
				reader = dbcmd.ExecuteReader();
				
				while(reader.Read()) {
					
				    // get tuple string columns
					for ( int u = 0; u < reader.FieldCount; u++ )
					{
						tempColumnList.Add(reader.GetString(u));
					}

					// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
					
					// prepare tuples and lists for next reader
					tempColumnList.Clear();
				}
	
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			
			// write list to file
			this.write_list_to_file(l, "list of establishments per year", false);
		}
		
		// list of holders of a degree per year
		public void make_lhdpy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader reader;
			
			Console.WriteLine("list of holders of a degree per year");
			
			for (int i = 0; i < 10; i++)
			{
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT g.designacao, d.nome " +
						"FROM informatica_registodocencia rd " +
				        "INNER JOIN informatica_grau g on " + 
				        	"g.id = rd.grau_id " +
						"INNER JOIN informatica_docente d on " + 
				        	"d.id = rd.docente_id " +
						"WHERE rd.ano =" + i);
				
				reader = dbcmd.ExecuteReader();
				
				while(reader.Read()) {
					
				    // get tuple string columns
					for ( int u = 0; u < reader.FieldCount; u++ )
					{
						tempColumnList.Add(reader.GetString(u));
					}

					// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
					
					// prepare tuples and lists for next reader
					tempColumnList.Clear();
				}
	
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			
			// write list to file
			this.write_list_to_file(l, "list of holders of a degree per year", false);
		}
		
		// personnel leaving the institution per year
		public void make_pltipy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader estabelecimento_reader;
			IDataReader personnel_reader;
			int id_estabelecimento;
			
			Console.WriteLine("personnel leaving the institution per year");
			
			for (int i = 1; i < 10; i++)
			{
				// get each distinct establishment per year
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT distinct codp " + 
				        "FROM informatica_estabelecimento " +
				        "WHERE ano = " + ( i-1 ) + " LIMIT 10");
				
				estabelecimento_reader = dbcmd.ExecuteReader();

				while(estabelecimento_reader.Read()) {
					
					// get tuple string columns
					id_estabelecimento = estabelecimento_reader.GetInt32(0);
					
					// builds the query
					IDbCommand pdbcmd = dbRebides.runCommand(
						"SELECT distinct e.designacao, d.nome " +
						"FROM informatica_registodocencia rd " +
						"INNER JOIN informatica_docente d on " +
							"d.id = rd.docente_id " +
						"INNER JOIN informatica_estabelecimento e on " +
							"e.id = rd.estabelecimento_id AND " +
					        "e.codP = " + id_estabelecimento +
						" WHERE " +
					        "rd.ano = " + (i - 1) + " AND " +
							"d.ano = " + (i - 1) + " AND " +
					        "d.id not in ( " +
								"SELECT d.id " +
									"FROM informatica_registodocencia rd " +
									"INNER JOIN informatica_docente d on " +
										"d.id = rd.docente_id " +
					                "INNER JOIN informatica_estabelecimento e on " +
					                    "e.id = rd.estabelecimento_id AND " +
					                    "e.codP = " + id_estabelecimento +
									" WHERE " +
										"rd.ano=" + i + ")");
					
					personnel_reader = pdbcmd.ExecuteReader();

					while(personnel_reader.Read()) {
						
					    // get tuple string columns
						for ( int u = 0; u < personnel_reader.FieldCount; u++ )
						{
							tempColumnList.Add(personnel_reader.GetString(u));
						}
	
						// add tuple to temp list
						tempList.Add(new CustomTuple(tempColumnList));
						
						// prepare tuples and lists for next reader
						tempColumnList.Clear();
					}					
				}
				
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			// write list to file
			this.write_list_to_file(l, "personnel leaving the institution per year", false);
		}
		
		// new personnel in the institution per year
		public void make_npitipy()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader estabelecimento_reader;
			IDataReader personnel_reader;
			int id_estabelecimento;
			
			Console.WriteLine("new personnel in the institution per year");
			
			for (int i = 1; i < 10; i++)
			{
				// get each distinct establishment per year
				IDbCommand dbcmd = dbRebides.runCommand(
				        "SELECT distinct codp " + 
				        "FROM informatica_estabelecimento " +
				        "WHERE ano = " + i + " LIMIT 100");
				
				estabelecimento_reader = dbcmd.ExecuteReader();

				while(estabelecimento_reader.Read()) {
					
					// get tuple string columns
					id_estabelecimento = estabelecimento_reader.GetInt32(0);
					
					// builds the query
					IDbCommand pdbcmd = dbRebides.runCommand(
						"SELECT distinct e.designacao, d.nome " +
						"FROM informatica_registodocencia rd " +
						"INNER JOIN informatica_docente d on " +
							"d.id = rd.docente_id " +
						"INNER JOIN informatica_estabelecimento e on " +
							"e.id = rd.estabelecimento_id AND " +
					        "e.codP = " + id_estabelecimento +
						" WHERE " +
					        "rd.ano = " + i + " AND " +
							"d.ano = " + i + " AND " +
					        "d.id not in ( " +
								"SELECT d.id " +
									"FROM informatica_registodocencia rd " +
									"INNER JOIN informatica_docente d on " +
										"d.id = rd.docente_id " +
					                "INNER JOIN informatica_estabelecimento e on " +
					                    "e.id = rd.estabelecimento_id AND " +
					                    "e.codP = " + id_estabelecimento +
									" WHERE " +
										"rd.ano=" + (i + 1) + ")");
					
					personnel_reader = pdbcmd.ExecuteReader();

					while(personnel_reader.Read()) {
						
					    // get tuple string columns
						for ( int u = 0; u < personnel_reader.FieldCount; u++ )
						{
							tempColumnList.Add(personnel_reader.GetString(u));
						}
	
						// add tuple to temp list
						tempList.Add(new CustomTuple(tempColumnList));
						
						// prepare tuples and lists for next reader
						tempColumnList.Clear();
					}					
				}
				
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				tempList.Clear();
			}
			// write list to file
			this.write_list_to_file(l, "new personnel in the institution per year", false);
		}
		
		// number of teachers promoted to the next category each year per establishment
		public void make_ntptncpype()
		{
			Dictionary<int, List<CustomTuple>> l = new Dictionary<int, List<CustomTuple>>();
			List<CustomTuple> teachers_before = new List<CustomTuple>();
			List<CustomTuple> teachers_after = new List<CustomTuple>();
			
			List<CustomTuple> tempList = new List<CustomTuple>();
			List<string> tempColumnList = new List<string>();
			IDataReader teachers_yearbefore_reader;
			IDataReader teachers_yearafter_reader;
			IDbCommand dbcmd;
			
			Console.WriteLine("number of teachers promoted to the next category each year per establishment");
			
			for (int i = 1; i < 10; i++)
			{
				// 1º seleccionar os professores do ano i
				dbcmd = dbRebides.runCommand(
					"SELECT distinct rd.docente_id, e.codp, rd.categoria_id " +
					"FROM informatica_registodocencia rd " +
				    "INNER JOIN informatica_estabelecimento e on " +
				       "rd.estabelecimento_id = e.id AND " +
				        "rd.ano = " + (i - 1 ) +
					" WHERE rd.ano=" + (i - 1));
				
				teachers_yearbefore_reader = dbcmd.ExecuteReader();
				
			    // adiciona os docentes no dicionário
				while (teachers_yearbefore_reader.Read())
				{
					// get tuple string columns
					for ( int u = 0; u < teachers_yearbefore_reader.FieldCount; u++ )
					{
						tempColumnList.Add(teachers_yearbefore_reader.GetString(u));
					}
					
					// item to list
					teachers_before.Add(new CustomTuple(tempColumnList));
					
					// clear tempColumnList
					tempColumnList.Clear();
				}
				
				// 2º seleccionar os professores do ano i + 1
				dbcmd = dbRebides.runCommand(
					"SELECT distinct rd.docente_id, e.codp, rd.categoria_id, e.designacao " +
					"FROM informatica_registodocencia rd " +
				    "INNER JOIN informatica_estabelecimento e on " +
				       "rd.estabelecimento_id = e.id AND " +
				       "rd.ano = " + i +
					" WHERE rd.ano=" + i);
				
				teachers_yearafter_reader = dbcmd.ExecuteReader();
				
				// adiciona os docentes no dicionário
				while (teachers_yearafter_reader.Read())
				{
					// get tuple string columns
					for ( int u = 0; u < teachers_yearafter_reader.FieldCount; u++ )
					{
						tempColumnList.Add(teachers_yearafter_reader.GetString(u));
					}
					
					// item to list
					teachers_after.Add(new CustomTuple(tempColumnList));
					
					// clear tempColumnList
					tempColumnList.Clear();
				}
				
				var query = from a in teachers_after
				            join b in teachers_before
				            	on a.getData()[0] equals b.getData()[0]
							where a.getData()[1] == b.getData()[1] && Int32.Parse(a.getData()[2]) > Int32.Parse(b.getData()[2])
				            select a.getData()[3];
				
				var final = query.Distinct();
				
				foreach ( string establishment in final )
				{
					tempColumnList.Add(establishment);
					tempColumnList.Add((from est in query
										where est == establishment
										select est).Count().ToString());
					
					// add tuple to temp list
					tempList.Add(new CustomTuple(tempColumnList));
				}
				
				// sort list
				tempList.Sort();
				
				// add list to year dictionary
				l.Add(i, new List<CustomTuple>(tempList));
				
				// clear variables
				teachers_after.Clear();
				teachers_before.Clear();
				tempList.Clear();
				query = null;
			}
				
			// write list to file
			this.write_list_to_file(l, "number of teachers promoted to the next category each year per establishment", true);
		}
	}
}
