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
		private List<List<int>> connectionList;
		private Dictionary<string, int> name2Index;
		private Dictionary<int, string> index2Name;
		private List<Line> lines;
		private List<List<int>> lineOfStation;

		public Map(string path)
		{
			connectionList = new List<List<int>>();
			lines = new List<Line>();
			name2Index = new Dictionary<string, int>();
			index2Name = new Dictionary<int, string>();
			lineOfStation = new List<List<int>>();

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
					//and record the number of the line which the station is in
					if (!name2Index.ContainsKey(stations[j]))
					{
						name2Index.Add(stations[j], index);
						index2Name.Add(index, stations[j]);
						index++;
						connectionList.Add(new List<int>());
						lineOfStation.Add(new List<int>());
						lineOfStation[index - 1].Add(lines.IndexOf(i));
					}
					else
					{
						lineOfStation[name2Index[stations[j]]].Add(lines.IndexOf(i));
					}

					//connection list
					if (j == stations.Count - 1)    //last station
					{
						if (i.getIsRound()) //the last station is connected with the first station
						{
							connectionList[name2Index[stations[j]]].Add(name2Index[stations[0]]);
							connectionList[name2Index[stations[0]]].Add(name2Index[stations[j]]);
						}
					}
					else if (j != 0)    //not the first station
					{
						connectionList[name2Index[stations[j]]].Add(name2Index[stations[j - 1]]);
						connectionList[name2Index[stations[j - 1]]].Add(name2Index[stations[j]]);
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

		public void printShortestB(string station1, string station2)
		{
			//staion1 or station2 does not exist
			if (!name2Index.ContainsKey(station1) || !name2Index.ContainsKey(station1))
			{
				Console.Out.WriteLine("起始站或目标站不存在");
				return;
			}

			int id1 = name2Index[station1];
			int id2 = name2Index[station2];
			bool[] bo = new bool[1000];         //whether a station is visited
			int[] from = new int[1000];         //a note: from which station did I come to this station
			List<int> que = new List<int>();    //the queue of spfa
			int front = 0, rear = 1;

			que.Add(id1);
			while (front != rear)
			{
				int head = que[front];
				foreach (var i in connectionList[head])
				{
					if (bo[i] == false)
					{
						bo[i] = true;
						que.Add(i);
						from[i] = head;
						rear++;

						//the destination is found, print the route and end the loop
						if (i == id2)
						{
							int iter = id2;
							List<int> route = new List<int>();
							while (iter != id1)
							{
								route.Add(iter);
								iter = from[iter];
							}
							route.Add(iter);

							for (int j = route.Count - 1; j >= 0; j--)
							{
								Console.Out.Write(index2Name[route[j]]);
								if (j != 0 && j != route.Count - 1)
								{
									int tempFlag = 0;
									foreach (var k in lineOfStation[route[j - 1]])
									{
										foreach (var l in lineOfStation[route[j + 1]])
										{
											if (k == l)
											{
												tempFlag = 1;
												break;
											}
										}
										if (tempFlag == 1)
											break;
									}
									if (tempFlag == 0)
									{
										int tempFlag2 = 0;
										foreach (var k in lineOfStation[route[j - 1]])
										{
											foreach (var l in lineOfStation[route[j]])
											{
												if (k == l)
												{
													Console.Out.Write(" 在此站换乘" + lines[k].getLineName());
													tempFlag2 = 1;
													break;
												}
											}
											if(tempFlag2==1)
												break;
										}
									}
								}
								Console.Out.WriteLine();
							}

							return;
						}

					}
				}
				front++;
			}

		}



	}
}
