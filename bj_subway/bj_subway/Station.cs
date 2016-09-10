using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bj_subway
{
	class Station
	{
		private string stationName;
		private bool isTransfer;
		private int index;

		public Station()
		{

		}

		public int getIndex()
		{
			return index;
		}

		public bool getIsTransfer()
		{
			return isTransfer;
		}

	}
}
