 /*
 * **********************************************
 * Class responsable to generate lists
 * 
 * Author: Bruno Moreira
 * Date  : 2011/06/18
 * **********************************************
 */

 using System;
 using System.Linq;
 using rebides;
 
 public class Rebides
 {
    public static void Main(string[] args)
    {
		// Delete file
		System.IO.File.Delete("lists.txt");
		
		Lists l = new Lists();
		
		Console.WriteLine("PROCESSO INICIADO");
		
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
		
		//The set of teachers that changed from one establishment to another one per year
		//l.make_stcfetapy();
		
		// list of establishments per year
		//l.make_liepy();
		
		// list of holders of a degree per year
		//l.make_lhdpy();
		
		// personnel leaving the institution per year
		//l.make_pltipy();
		
		// new personnel in the institution per year
		//l.make_npitipy();
		
		// number of teachers migrating from one establishment to another
		//l.make_ntmfeta();
		
		// number of teachers promoted to the next category each year per establishment
		//l.make_ntptncpype();
		
		
		Console.WriteLine("PROCESSO CONCLUÍDO");
		
		l = null;
    }
 }