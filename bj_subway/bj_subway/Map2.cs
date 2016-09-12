using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bj_subway
{
	partial class Map
	{

		private List<int> shortestRoute;
		private int length;

		public void findShortest(string station)
		{
			shortestRoute = new List<int>();
			length = 0;

			if (!name2Index.ContainsKey(station))
			{
				Console.Out.WriteLine("不存在该站");
				return;
			}

			bool[] mark = new bool[1000];
			int indexOfStation = name2Index[station];

			List<int> route = new List<int>();
			foreach (var i in connectionList[indexOfStation])
			{
				_find(i, 1, route, mark);
				for (int j = 0; j < 1000; j++)
					mark[j] = false;
			}


		}

		private void _find(int index, int len, List<int> route, bool[] mark)
		{
			//keep going until there comes an intersection
			while (true)
			{
				int count = 0;
				int next = 0;
				foreach (var i in connectionList[index])
				{
					if (mark[i] == false)
					{
						count++;
						next = i;
					}
				}
				if (count == 1)
				{
					index = next;
					mark[index] = true;
					route.Add(index);
					continue;
				}
				else
				{
					if (count == 0) //
					{
						List<int> choice = new List<int>();
						for (int i = 0; i < size; i++)
						{
							if (mark[i] == false)
								choice.Add(i);
						}

						if (choice.Count > 0)
						{
							int lenBackup = len;
							List<int> routeBackup = new List<int>(route);
							bool[] markBackup = new bool[1000];
							for (int i = 0; i < 1000; i++)
								markBackup[i] = mark[i];

							foreach (var i in choice)
							{
								///////////////////////////////////////////////
								///////////////////////////////////////////////
								//......
								///////////////////////////////////////////////
								///////////////////////////////////////////////

								_find(i, len, route, mark);
								route = new List<int>(routeBackup);
								len = lenBackup;
								for (int j = 0; j < 1000; j++)
									mark[j] = markBackup[j];
							}

						}
						else
						{
							//every station has been visited
							if (len < length)
							{
								length = len;
								shortestRoute = new List<int>(route);
							}
						}

					}
					else
					{
						//intersection
						int lenBackup = len;
						List<int> routeBackup = new List<int>(route);
						bool[] markBackup = new bool[1000];
						for (int i = 0; i < 1000; i++)
							markBackup[i] = mark[i];
						foreach (var i in connectionList[index])
						{
							_find(i, len, route, mark);
							route = new List<int>(routeBackup);
							len = lenBackup;
							for (int j = 0; j < 1000; j++)
								mark[j] = markBackup[j];
						}
					}
				}
			}
		}

		private int find(int id1,int id2)
		{
			return 0;
		}

	}
}
