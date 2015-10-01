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
			Notes = new List<double>();
			Coins = new List<double>();
		}

		public string CountryName { get; set; }

		public string CurrencyName { get; set; }

		public string CurrencyCode { get; set; }

		public List<double> Notes { get; private set; }

		public List<double> Coins { get; private set; }
	}
}
