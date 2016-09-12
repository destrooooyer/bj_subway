using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bj_subway
{
    partial class Map
    {
        private List<List<int>> connectionList;
        private Dictionary<string, int> name2Index;
        private Dictionary<int, string> index2Name;
        private List<Line> lines;
        private List<List<int>> lineOfStation;
        private int size;

        public Map(string path)
        {
            connectionList = new List<List<int>>();
            lines = new List<Line>();
            name2Index = new Dictionary<string, int>();
            index2Name = new Dictionary<int, string>();
            lineOfStation = new List<List<int>>();
            size = 0;

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
                    size = index;

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

        public void printShortestC2(string station1, string station2)
        {
            //staion1 or station2 does not exist
            if (!name2Index.ContainsKey(station1) || !name2Index.ContainsKey(station2))
            {
                Console.Out.WriteLine("起始站或目标站不存在");
                return;
            }

            int id1 = name2Index[station1];
            int id2 = name2Index[station2];
            bool[] bo = new bool[1000];         //whether a station is visited
            List<int>[] from = new List<int>[1000];         //a note: from which station did I come to this station
            int[] len = new int[1000];
            List<int> que = new List<int>();    //the queue of spfa
            int front = 0, rear = 1;
            List<int> route = new List<int>();


            que.Add(id1);
            bo[id1] = true;
            while (front != rear)
            {
                int head = que[front];
                foreach (var i in connectionList[head])
                {
                    if (bo[i] == false)
                    {
                        bo[i] = true;
                        que.Add(i);
                        from[i] = new List<int>();
                        from[i].Add(head);
                        rear++;
                        len[i] = len[head] + 1;
                    }
                    else if (len[i] == len[head] + 1 && !from[i].Contains(head))
                    {
                        que.Add(i);
                        rear++;
                        from[i].Add(head);
                    }
                }
                front++;
            }
            route.Add(id2);
            List<int> routeTemp = new List<int>();
            findLeastTransfer(id1, id2, from, new List<int>(), route, new int());

        }

        private void findLeastTransfer(int id1, int id2, List<int>[] from, List<int> route, List<int> routeFinal, int countFinal)
        {

            if (id2 == id1)
            {

                int count = 0;
                for (int i = route.Count - 1; i >= 0; i--)
                {
                    Console.Out.Write(index2Name[route[i]]);
                    if (i != 0 && i != route.Count - 1)
                    {
                        foreach (var k in lineOfStation[route[i + 1]])
                        {
                            if (lineOfStation[route[i]].Contains(k))
                            {
                                if (!lineOfStation[route[i - 1]].Contains(k))
                                {
                                    foreach (var m in lineOfStation[route[i]])
                                    {
                                        if (lineOfStation[route[i - 1]].Contains(m))
                                        {
                                            Console.Out.Write(" 在此站换乘" + lines[m].getLineName());
                                            count++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Console.Out.WriteLine();
                }

                if (routeFinal.Count == 0 || count < countFinal)
                {
                    routeFinal = new List<int>(route);
                }

                return;
            }

            foreach (var i in from[id2])
            {
                //Console.Out.WriteLine(index2Name[i]);
                route.Add(i);
                findLeastTransfer(id1, i, from, route, routeFinal, countFinal);
                route.RemoveAt(route.Count - 1);
            }
        }


        public void printShortestB(string station1, string station2)
        {
            //staion1 or station2 does not exist
            if (!name2Index.ContainsKey(station1) || !name2Index.ContainsKey(station2))
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
            bo[id1] = true;
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
                                    foreach (var k in lineOfStation[route[j + 1]])
                                    {
                                        foreach (var l in lineOfStation[route[j]])
                                        {
                                            if (k == l)
                                            {
                                                if (!lineOfStation[route[j - 1]].Contains(k))
                                                {
                                                    foreach (var m in lineOfStation[route[j]])
                                                    {
                                                        if (lineOfStation[route[j - 1]].Contains(m))
                                                            Console.Out.Write(" 在此站换乘" + lines[m].getLineName());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                Console.Out.WriteLine();


                                // 									int tempFlag = 0;
                                // 									foreach (var k in lineOfStation[route[j - 1]])
                                // 									{
                                // 										foreach (var l in lineOfStation[route[j + 1]])
                                // 										{
                                // 											if (k == l)
                                // 											{
                                // 												tempFlag = 1;
                                // 												break;
                                // 											}
                                // 										}
                                // 										if (tempFlag == 1)
                                // 											break;
                                // 									}
                                // 									if (tempFlag == 0)
                                // 									{
                                // 										int tempFlag2 = 0;
                                // 										foreach (var k in lineOfStation[route[j - 1]])
                                // 										{
                                // 											foreach (var l in lineOfStation[route[j]])
                                // 											{
                                // 												if (k == l)
                                // 												{
                                // 													Console.Out.Write(" 在此站换乘" + lines[k].getLineName());
                                // 													tempFlag2 = 1;
                                // 													break;
                                // 												}
                                // 											}
                                // 											if (tempFlag2 == 1)
                                // 												break;
                                // 										}
                                // 									}
                                // 								}
                                // 								Console.Out.WriteLine();
                            }

                            Console.Out.Write("共");
                            Console.Out.Write(route.Count - 1);
                            Console.Out.WriteLine("站");
                            return;
                        }

                    }
                }
                front++;
            }

        }

        public void printBetween(string station1, string station2, ref int x)
        {
            //get line number first
            foreach (var i in lineOfStation[name2Index[station1]])
            {
                foreach (var j in lineOfStation[name2Index[station2]])
                {
                    if (i == j)
                    {
                        if (lines[i].getStations().IndexOf(station1) < lines[i].getStations().IndexOf(station2))
                        {
                            for (int k = lines[i].getStations().IndexOf(station1) + 1; k < lines[i].getStations().IndexOf(station2); k++)
                            {
                                Console.Out.WriteLine(lines[i].getStations()[k]);
                                x++;
                            }
                        }
                        else
                        {
                            for (int k = lines[i].getStations().IndexOf(station1) - 1; k > lines[i].getStations().IndexOf(station2); k--)
                            {
                                Console.Out.WriteLine(lines[i].getStations()[k]);
                                x++;
                            }
                        }
                        return;
                    }
                }
            }
        }

        private void findShortest(int id1, int id2, List<int>[] from, List<int> route,ref List<int> routeFinal, ref int countFinal)
        {
            if (id2 == id1)
            {
                //                 foreach (var i in route)
                //                     Console.Out.WriteLine(index2Name[i]);
                //                 Console.WriteLine();
                int countTemp = 0;
                for (int i = 1; i < route.Count; i++)
                {
                    foreach (var j in lineOfStation[route[i]])
                    {
                        if (lineOfStation[route[i - 1]].Contains(j))
                        {
                            List<string> stations = lines[j].getStations();
                            if (stations[0] == stations[stations.Count - 1])
                            {
                                int temp = Math.Abs(stations.IndexOf(index2Name[route[i]]) - stations.IndexOf(index2Name[route[i - 1]]));
                                if (temp < stations.Count - temp - 1)
                                    countTemp += temp;
                                else
                                    countTemp += stations.Count - temp - 1;
                            }
                            else
                            {
                                countTemp += Math.Abs(stations.IndexOf(index2Name[route[i]]) - stations.IndexOf(index2Name[route[i - 1]]));
                            }
                            break;
                        }
                    }
                }

                if (countTemp < countFinal)
                {
                    countFinal = countTemp;
                    routeFinal = new List<int>(route);
                }

                return;
            }

            foreach (var i in from[id2])
            {
                //Console.Out.WriteLine(index2Name[i]);
                route.Add(i);
                findShortest(id1, i, from, route,ref routeFinal, ref countFinal);
                route.RemoveAt(route.Count - 1);
            }
        }

        public void printShortestC(string station1, string station2)
        {

            //staion1 or station2 does not exist
            if (!name2Index.ContainsKey(station1) || !name2Index.ContainsKey(station2))
            {
                Console.Out.WriteLine("起始站或目标站不存在");
                return;
            }
            int id1 = name2Index[station1];
            int id2 = name2Index[station2];
            bool[] bo = new bool[1000];         //whether a line is visited
            List<int>[] from = new List<int>[1000];         //a note: from which station did I come to this station
            List<int> que = new List<int>();    //the queue of spfa
            int front = 0, rear = 1;
            int[] len = new int[1000];

            for (int i = 0; i < name2Index.Count; i++)
            {
                from[i] = new List<int>();
                len[i] = -1;
            }

            que.Add(id1);

            while (front < rear)
            {
                foreach (var i in lineOfStation[que[front]])
                {
                    foreach (var j in lines[i].getStations())
                    {
                        if (j == station2)
                        {
                            if (len[name2Index[j]] == -1)
                            {
                                from[name2Index[j]].Add(que[front]);
                                len[name2Index[j]] = len[que[front]] + 1;
                            }
                            else if (len[name2Index[j]] == len[que[front]] + 1 && !from[name2Index[j]].Contains(que[front]))
                            {
                                from[name2Index[j]].Add(que[front]);
                            }
                        }

                        foreach (var k in lineOfStation[name2Index[j]])
                        {
                            if (!lineOfStation[que[front]].Contains(k))
                            {
                                if (len[name2Index[j]] == -1)
                                {
                                    que.Add(name2Index[j]);
                                    len[name2Index[j]] = len[que[front]] + 1;
                                    from[name2Index[j]].Add(que[front]);
                                    rear++;
                                }
                                else if (len[name2Index[j]] == len[que[front]] + 1 && !from[name2Index[j]].Contains(que[front]))
                                {
                                    que.Add(name2Index[j]);
                                    from[name2Index[j]].Add(que[front]);
                                    rear++;
                                }
                                break;
                            }
                        }
                    }
                }
                front++;
            }
            List<int> routeTemp = new List<int>();
            List<int> route = new List<int>();
            int count = 2147483600;
            routeTemp.Add(id2);

            findShortest(id1, id2, from, routeTemp,ref route, ref count);

            for (int i = route.Count - 1; i >= 0; i--)
            {
                Console.Out.Write(index2Name[route[i]]);
                if (i != 0 && i != route.Count - 1)
                {
                    foreach (var m in lineOfStation[route[i]])
                    {
                        foreach (var n in lineOfStation[route[i - 1]])
                        {
                            if (m == n)
                                Console.Out.Write(" 在此站换乘" + lines[m].getLineName());
                        }
                    }
                }
                Console.Out.WriteLine();

                if (i != 0)
                {
                    foreach (var j in lineOfStation[route[i]])
                    {
                        if (lineOfStation[route[i - 1]].Contains(j))
                        {
                            List<string> stations = lines[j].getStations();
                            int index1, index2;
                            index1 = stations.IndexOf(index2Name[route[i]]);
                            index2 = stations.IndexOf(index2Name[route[i - 1]]);
                            if (stations[0] == stations[stations.Count - 1])
                            {
                                int temp = Math.Abs(index2 - index1);
                                if (temp < stations.Count - temp - 1)
                                {
                                    if (index1 > index2)
                                    {
                                        for (int k = index1 - 1; k > index2; k--)
                                            Console.WriteLine(stations[k]);
                                    }
                                    else
                                    {
                                        for (int k = index1 + 1; k < index2; k++)
                                            Console.WriteLine(stations[k]);
                                    }
                                }
                                else
                                {
                                    if (index1 > index2)
                                    {
                                        for (int k = index1 + 1; k < stations.Count; k++)
                                            Console.WriteLine(stations[k]);
                                        for (int k = 1; k < index2; k++)
                                            Console.WriteLine(stations[k]);
                                    }
                                    else
                                    {
                                        for (int k = index1 - 1; k >= 0; k--)
                                            Console.WriteLine(stations[k]);
                                        for (int k = stations.Count - 2; k > index2; k--)
                                            Console.WriteLine(stations[k]);
                                    }
                                }
                            }
                            else
                            {
                                if (index1 > index2)
                                {
                                    for (int k = index1 - 1; k > index2; k--)
                                        Console.WriteLine(stations[k]);
                                }
                                else
                                {
                                    for (int k = index1 + 1; k < index2; k++)
                                        Console.WriteLine(stations[k]);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            Console.Out.Write("共");
            Console.Out.Write(count + route.Count - 1);
            Console.Out.WriteLine("站");
        }


        public void printShortestC3(string station1, string station2)
        {
            //staion1 or station2 does not exist
            if (!name2Index.ContainsKey(station1) || !name2Index.ContainsKey(station2))
            {
                Console.Out.WriteLine("起始站或目标站不存在");
                return;
            }
            int id1 = name2Index[station1];
            int id2 = name2Index[station2];
            bool[] bo = new bool[1000];         //whether a line is visited
            int[] from = new int[1000];         //a note: from which station did I come to this station
            List<int> que = new List<int>();    //the queue of spfa
            int front = 0, rear = 1;

            que.Add(id1);
            foreach (var i in lineOfStation[id1])
            {
                bo[i] = true;
            }
            while (front < rear)
            {
                foreach (var i in lineOfStation[que[front]])
                {
                    foreach (var j in lines[i].getStations())
                    {
                        if (j == station2)
                        {
                            from[name2Index[j]] = que[front];
                            int iter = id2;
                            List<int> route = new List<int>();
                            while (iter != id1)
                            {
                                route.Add(iter);
                                iter = from[iter];
                            }
                            route.Add(iter);

                            int count = 0;
                            for (int k = route.Count - 1; k >= 0; k--)
                            {
                                Console.Out.Write(index2Name[route[k]]);
                                if (k != 0 && k != route.Count - 1)
                                {
                                    foreach (var m in lineOfStation[route[k]])
                                    {
                                        foreach (var n in lineOfStation[route[k - 1]])
                                        {
                                            if (m == n)
                                                Console.Out.Write(" 在此站换乘" + lines[m].getLineName());
                                        }
                                    }
                                }
                                Console.Out.WriteLine();

                                if (k > 0)
                                {
                                    int x = 0;
                                    printBetween(index2Name[route[k]], index2Name[route[k - 1]], ref x);
                                    count += x;
                                }
                            }
                            Console.Out.Write("共");
                            Console.Out.Write(count + route.Count - 1);
                            Console.Out.WriteLine("站");
                            return;
                        }

                        foreach (var k in lineOfStation[name2Index[j]])
                        {
                            if (bo[k] == false)
                            {
                                bo[k] = true;
                                que.Add(name2Index[j]);
                                from[name2Index[j]] = que[front];
                                rear++;
                                break;
                            }
                        }
                    }
                }
                front++;
            }
        }




    }
}
