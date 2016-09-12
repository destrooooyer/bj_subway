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

				if (args.Length == 3)
				{
					if (args[0] == "-b")
					{
						map.printShortestB(args[1], args[2]);
					}
					else if (args[0] == "-c")
					{
						map.printShortestC(args[1], args[2]);
					}
				}

				while (true)
				{
					string inStr = Console.In.ReadLine();
					map.printLine(inStr);
				}
			}
			catch (System.Exception ex)
			{
                Console.Out.WriteLine(ex);
				return;
			}
		}

	}
}

