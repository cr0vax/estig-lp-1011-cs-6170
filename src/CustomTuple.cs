
using System;
using System.Collections.Generic;

namespace rebides
{
	public class CustomTuple : IComparable<CustomTuple>
	{
		List<string> strData = new List<string>();			// stores string columns
		//int iCounter;										// stores integer columns
		//Boolean blnHasCounter;								// defines if it has integer columns
		
		/*
		 * Class constructor without counter
		 * 
		 * lData - list containing a list with string columns
		 * */
		public CustomTuple (List<string> lData)
		{
			foreach (string item in lData) {
				this.strData.Add(item);
			}
		}

		/*
		 * Class constructor with counter
		 * 
		 * lData 		- list containing a list with string columns
		 * iCounter 	- integer value containing counter for this line
		 * */
		/*
		public CustomTuple (List<string> lData, int iCounter)
		{
			foreach (string item in lData) {
				this.strData.Add(item);
			}
			this.iCounter = iCounter;
			this.blnHasCounter = true;
		}
		*/
		public List<string> getData()
		{
			return strData;
		}
		
		/*
		 * To String override function
		 * */
		public override string ToString() 
		{
			string returnString = "";
			
			//Console.WriteLine("Total strData: " + strData.Count);
			
			// add string columns to return string
			for (int i = 0; i < strData.Count; i++)
			{
				//Console.WriteLine("Coluna: " + strData[i]);
				returnString = returnString + strData[i] + ";";
			}
			
			//return string
			return returnString;
		}
		
		#region IComparable<CustomTuple> implementation
		public int CompareTo (CustomTuple other)
		{
			int iCurrentColumn = 0;
			int iCompareResult = 0;
				    
			while ( iCompareResult == 0 && iCurrentColumn < strData.Count )
			{
				iCompareResult = this.strData[iCurrentColumn].CompareTo(other.strData[iCurrentColumn]);
				iCurrentColumn++;
			}
			
			return iCompareResult;
		}
		
		#endregion
	}
}
