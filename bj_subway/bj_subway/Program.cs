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
			string in_str = "";
			List<string> line_name = new List<string>();
			List<List<string>> stop_name = new List<List<string>>();

			try
			{
				StreamReader sr = new StreamReader("beijing-subway.txt");

				while ((in_str = sr.ReadLine()) != null)
				{
					line_name.Add(in_str);
					in_str = sr.ReadLine();
					stop_name.Add(new List<string>(Regex.Split(in_str, " ")));

					in_str = sr.ReadLine();
				}
				//Console.Out.WriteLine(in_str);
			}
			catch (Exception e)
			{
				Console.Out.WriteLine("beijing-subway.txt的格式不正确，按任意键结束");
				Console.ReadKey();
				return;
			}

			while (true)
			{
				in_str = Console.In.ReadLine();
				int index = 0;
				if (in_str == "14号线")
				{
					foreach (var i in line_name)
					{
						if (i == "14号线西" || i == "14号线东")
						{
							//Console.Out.WriteLine(i);
							foreach (var j in stop_name[index])
							{
								Console.Out.Write(j + " ");
							}
						}
						index++;
					}
					Console.Out.WriteLine();
				}
				else
				{
					foreach (var i in line_name)
					{
						if (i == in_str)
						{
							//Console.Out.WriteLine(i);
							foreach (var j in stop_name[index])
							{
								Console.Out.Write(j + " ");
							}

							Console.Out.WriteLine();
						}
						index++;
					}
				}
			}

			Map map = new Map("beijing-subway.txt");

		}

	}
}

