using System;

namespace AssemblyCSharp
{
	public class LoadMapEvent : NetworkEvent
	{
		/*

		In the future, the content field should contain the id of the map.
		id that doesn't exist at the moment, but I assume that the basic map is gonna be 0x0
		The constructor will take some kind of enum with map values and translate that into 
		the corresponding byte.
		
		*/
		public LoadMapEvent (byte map) : base (0x0, map, true)
		{
		}
	}
}

