using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhichWayToPay_Parser
{
	public class CurrenciesContext : DbContext
	{
		public CurrenciesContext(DbContextOptions options) : base(options)
		{

		}

		DbSet<CurrencyModel> Currencies { get; set; }

		DbSet<CountryModel> Countries { get; set; }
	}
}
