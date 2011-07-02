 /*
 * **********************************************
 * Class responsable to generate lists
 * 
 * Author: Bruno Moreira
 * Date  : 2011/06/18
 * **********************************************
 */

 using System;
 using rebides;
 
 public class Rebides
 {
    public static void Main(string[] args)
    {
		Lists l = new Lists();
		
		// Total number of teachers in the higher education system per year
		//l.make_tnthespy();
		
		// Total number of teachers per establishment and per year
		//l.make_tntpepy();
		
		// Total number of teachers per degree and per year
		//l.make_tntpdpy();
		
		// Total number of teachers per degree, per establishment and per year
		//l.make_tntpdpepy();
		
		// Number of holders of a doctorate degree, per establishment and per year
		//l.make_nhddpepy();
		
		// The set of holders of a doctorate degree per establishment and per year
		//l.make_shfdpepy();
		
		//TODO: The set of teachers that changed from one establishment to another one per year
		//l.make_stcfetapy();
		
		// list of establishments per year
		//l.make_liepy();
		
		// list of holders of a degree per year
		//l.make_lhdpy();
		
		// personnel leaving the institution per year
		//l.make_pltipy();
		
		// TODO: VALIDAR new personnel in the institution per year
		//l.make_npitipy();
		
		// TODO: number of teachers migrating from one establishment to another
		
		// TODO: number of teachers promoted to the next category each year per establishment
		/*
		
		reader.Close();
		reader = null;
		dbcmd.Dispose();
		dbcmd = null;
		dbRebides = null;
		*/
		Console.WriteLine("CONCLUÍDO");
    }
 }