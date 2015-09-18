using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhichWayToPay_Parser
{
	class Currency
	{
		public Currency()
		{
			Notes = new List<int>();
			Coins = new List<int>();
		}

		public string CountryName { get; set; }

		public string CurrencyName { get; set; }

		public string CurrencyCode { get; set; }

		public List<int> Notes { get; private set; }

		public List<int> Coins { get; private set; }
	}
}
