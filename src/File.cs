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
		TextWriter file;

		public File ()
		{
			 this.file = new StreamWriter("lists.txt");
		}
		
		public void write(string stringToWrite)
		{
			// write a line of text to the file
			this.file.WriteLine(stringToWrite);
		}
		
		public void close()
		{
			this.file.Close();
		}
	}
}
