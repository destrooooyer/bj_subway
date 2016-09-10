using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bj_subway
{
	class Map
	{
		private int[,] connectionList;
		private Dictionary<string, int> name2Index;
		private Dictionary<int, string> index2Name;

		private List<Line> lines;

		public Map(string path)
		{
			lines = new List<Line>();
			name2Index = new Dictionary<string, int>();
			index2Name = new Dictionary<int, string>();

			//input from file
			try
			{
				StreamReader sr = new StreamReader(path);
				string lineNameIn;
				string stationNameIn;

				while ((lineNameIn = sr.ReadLine()) != null)
				{
					stationNameIn = sr.ReadLine();
					lines.Add(new Line(lineNameIn, stationNameIn));
					
					sr.ReadLine();
				}
				//Console.Out.WriteLine(in_str);
			}
			catch (Exception e)
			{
				Console.Out.WriteLine("输入文件\"beijing - subway.txt\"不存在或存在问题，按任意键结束");
				Console.ReadKey();
				throw e;
			}

			//prepare connection list
			int index = 0;
			foreach (var i in lines)
			{
				List<string> stations = i.getStations();
				for (int j = 0; j < stations.Count; j++)
				{
					//give each station an id number
					if (!name2Index.ContainsKey(stations[j]))
					{
						name2Index.Add(stations[j], index);
						index2Name.Add(index, stations[j]);
						index++;
					}

					//connection list
					if (j == stations.Count - 1)    //last station
					{

					}
					else
					{

					}
				}
			}


		}

		public void printLine(string inStr)
		{
			if (inStr == "14号线")
			{
				foreach (var i in lines)
				{
					if (i.getLineName() == "14号线西" || i.getLineName() == "14号线东")
					{
						//Console.Out.WriteLine(i);
						foreach (var j in i.getStations())
						{
							Console.Out.Write(j + " ");
						}
					}
				}
				Console.Out.WriteLine();
			}
			else
			{
				foreach (var i in lines)
				{
					if (i.getLineName() == inStr)
					{
						//Console.Out.WriteLine(i);
						foreach (var j in i.getStations())
						{
							Console.Out.Write(j + " ");
						}

						Console.Out.WriteLine();
					}
				}
			}
		}
	}
}
