using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bj_subway
{
	class Line
	{
		private bool isRound;
		private string lineName;
		private List<string> stations;

		public Line(string lineNameIn, string stationNameIn)
		{
			lineName = lineNameIn;
			try
			{
				stations = new List<string>(Regex.Split(stationNameIn, " "));
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
		}

		public bool hasStation(string str)
		{
			return false;
		}

		public List<string> getStations()
		{
			return stations;
		}

	}
}
