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
				Console.Out.WriteLine("输入文件存在问题，按任意键结束");
				throw e;
			}

			//prepare connection list
			int index = 0;
			foreach (var i in lines)
			{
				List<string> stations=i.getStations();
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
					if(j==stations.Count-1)	//last station
					{

					}
					else
					{

					}
				}
			}


		}

	}
}
