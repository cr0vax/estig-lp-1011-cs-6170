/*
 * **********************************************
 * Class responsable to write information to file
 * 
 * Author: Bruno Moreira
 * Date  : 2011/06/18
 * **********************************************
 */

using System;
using System.IO;

namespace rebides
{


	public class File
	{
		TextWriter lists_file_writer;
		
		public File ()
		{
			const string FILE_NAME = "lists.txt";
				
			System.IO.File.Delete(FILE_NAME);
			this.lists_file_writer = new StreamWriter(FILE_NAME, true);
		}
		
		public void write(string stringToWrite)
		{
			
			// write a line of text to the file
			this.lists_file_writer.WriteLine(stringToWrite);
			
			//this.close();
		}
		public void close()
		{
			this.lists_file_writer.Close();
		}
		
		public void flush()
		{
			this.lists_file_writer.Flush();
			//this.lists_file_writer.
		}
	}
}
