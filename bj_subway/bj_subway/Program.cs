using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace bj_subway
{
	class Program
	{
		static void Main(string[] args)
		{
			Map map;
			try
			{
				map = new Map("beijing-subway.txt");
				while (true)
				{
					string inStr = Console.In.ReadLine();
					map.printLine(inStr);
				}
			}
			catch (System.Exception ex)
			{
				return;
			}
		}

	}
}

