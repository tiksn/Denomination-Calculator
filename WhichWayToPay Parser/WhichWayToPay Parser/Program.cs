using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WhichWayToPay_Parser
{
	class Program
	{
		static readonly string LandingPageURL = "http://www.whichwaytopay.com/world-currencies-by-country.asp";

		static void Main(string[] args)
		{
			Console.Title = "WhichWayToPay Parser";
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Clear();

			var currencies = ParseLandingPage(LandingPageURL);

			var currenciesJson = JsonConvert.SerializeObject(currencies);

			Console.WriteLine(currenciesJson);

			Console.WriteLine("Done.");
			Console.ReadLine();
		}

		private static object ParseLandingPage(string landingPageURL)
		{
			Console.WriteLine("Parsing: {0}", landingPageURL);
			var httpClient = new HttpClient();
			var document = new HtmlDocument();
			document.LoadHtml(httpClient.GetStringAsync(LandingPageURL).Result);

			var node = document.DocumentNode.ChildNodes[3].ChildNodes[3].ChildNodes[1].ChildNodes[5].ChildNodes[1].ChildNodes[3].ChildNodes[1];

			var currencyNodes = node.SelectNodes("div/ul/li/a");
			int counter = 0;

			foreach (var currencyNode in currencyNodes)
			{
				counter++;
				Console.Title = $"WhichWayToPay Parser { (int)(counter * 100d / currencyNodes.Count) }%";
				string href = currencyNode.GetAttributeValue("href", string.Empty);

				if (href.StartsWith("http://www.whichwaytopay.com/Costa-Rican-currency-Col"))
				{
					href = "http://www.whichwaytopay.com/Costa-Rican-currency-Colón-CRC.asp";
				}

				var country = ParseCurrencyPage(href);

				Console.WriteLine(JsonConvert.SerializeObject(country));
			}

			return null;
		}

		private static Currency ParseCurrencyPage(string currencyPageURL)
		{
			var result = new Currency();

			Console.WriteLine("Parsing: {0}", currencyPageURL);

			var httpClient = new HttpClient();
			var document = new HtmlDocument();
			document.LoadHtml(httpClient.GetStringAsync(currencyPageURL).Result);

			var node = document.DocumentNode.ChildNodes[3].ChildNodes[3].ChildNodes[1].ChildNodes[5].ChildNodes[1].ChildNodes[3].ChildNodes[1];

			var countryElement = node.ChildNodes[1];
			result.CountryName = countryElement.InnerText;
			result.CurrencyCode = node.ChildNodes[3].InnerText.Substring(1, 3);

			int currencyNameShift = 0;
			int notesShift = 0;
			int coinsShift = 0;

			if (node.ChildNodes[9].InnerText == "CURRENCY: ")
			{
				currencyNameShift = 2;
				notesShift = 2;
				coinsShift = 2;
			}
			else if (node.ChildNodes[11].InnerText.StartsWith("Please note the CFA Franc is tied to the Euro.  Please be aware that only currency issued by the Bank of West African States"))
			{
				currencyNameShift = 0;
				notesShift = 2;
				coinsShift = 2;
			}
			else if (currencyPageURL == "http://www.whichwaytopay.com/Cayman-Islands-currency-Dollar-KYD.asp")
			{
				currencyNameShift = 0;
				notesShift = 2;
				coinsShift = 2;
			}
			else if (currencyPageURL == "http://www.whichwaytopay.com/Central-African-Repubic-currency-CFA-Franc-XAF.asp")
			{
				currencyNameShift = 0;
				notesShift = 2;
				coinsShift = 2;
			}
			else if (currencyPageURL == "http://www.whichwaytopay.com/Chad-currency-CFA-Franc-XAF.asp")
			{
				currencyNameShift = 0;
				notesShift = 2;
				coinsShift = 2;
			}
			else if (currencyPageURL == "http://www.whichwaytopay.com/East-Timor-currency-Dollar-USD.asp")
			{
				currencyNameShift = 0;
				notesShift = 2;
				coinsShift = 2;
			}
			else if (currencyPageURL == "http://www.whichwaytopay.com/El-Salvador-currency-Dollar-USD.asp")
			{
				currencyNameShift = 0;
				notesShift = 2;
				coinsShift = 2;
			}
			else if (currencyPageURL == "http://www.whichwaytopay.com/French-Polynesia-currency-French-Pacific-Franc-XPF.asp")
			{
				currencyNameShift = 0;
				notesShift = 2;
				coinsShift = 2;
			}
			result.CurrencyName = node.ChildNodes[9 + currencyNameShift].InnerText;

			var notes = node.ChildNodes[13 + notesShift].InnerText;

			bool startsWithNote = false;

			if (notes.StartsWith("NOTE: "))
			{
				notes = notes.Substring(6);
				startsWithNote = true;
			}

			if (notes.StartsWith(result.CurrencyCode, StringComparison.OrdinalIgnoreCase))
				notes = notes.Substring(3);

			if (result.CurrencyCode == "ALL")
				notes = notes.Substring(3);

			var notesParts = GetNotesOrCoins(notes, result.CurrencyCode, currencyPageURL);

			result.Notes.AddRange(notesParts);

			if (startsWithNote)
			{
				var coins = node.ChildNodes[15 + coinsShift].InnerText;

				if (coins.StartsWith("COIN: "))
				{
					coins = coins.Substring(6);
				}

				coins = coins.Substring(3);

				//var coinParts = GetNotesOrCoins(coins);

				//result.Coins.AddRange(coinParts);
			}

			return result;
		}

		private static List<double> GetNotesOrCoins(string notesOrCoins, string currencyCode, string currencyPageURL)
		{
			double multiplier = 1.0;

			var result = new List<double>();

			if (notesOrCoins.EndsWith("."))
				notesOrCoins = notesOrCoins.Substring(0, notesOrCoins.Length - 1);

			if (notesOrCoins.EndsWith(". "))
				notesOrCoins = notesOrCoins.Substring(0, notesOrCoins.Length - 2);

			var noteParts = notesOrCoins.Split(new string[] { ", ", "and" }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var notePart in noteParts)
			{
				double noteMultiplier = 1.0;
				string noteString = notePart.Replace(",", "");

				noteString = noteString.Trim();
				//noteString = noteString.Replace("centimes", "");

				if (noteString.EndsWith("$"))
				{
					noteString = noteString.Substring(0, noteString.Length - 1);
				}
				else if (noteString.StartsWith("€"))
				{
					noteString = noteString.Substring(1);
				}
				else if (noteString.EndsWith("€"))
				{
					noteString = noteString.Substring(0, noteString.Length - 1);
				}
				else if (noteString.Length > 0 && (int)noteString[0] == 65533)
				{
					noteString = noteString.Substring(1);
				}
				else if (noteString.Contains("$"))
				{
					noteString = noteString.Substring(noteString.IndexOf("$") + 1);
				}
				else if (noteString.EndsWith(currencyCode, StringComparison.OrdinalIgnoreCase))
				{
					noteString = noteString.Substring(0, noteString.Length - currencyCode.Length);
				}
				else if (noteString.StartsWith("p."))
				{
					noteString = noteString.Substring(2);
				}
				else if (noteString.StartsWith("Nu"))
				{
					noteString = noteString.Substring(2);
				}
				else if (noteString.StartsWith("Bs"))
				{
					noteString = noteString.Substring(2);
				}
				else if (noteString.StartsWith("KM"))
				{
					noteString = noteString.Substring(2);
				}
				else if (noteString.StartsWith("Kn"))
				{
					noteString = noteString.Substring(2);
				}
				else if (noteString.StartsWith("kr"))
				{
					noteString = noteString.Substring(2);
				}
				else if (noteString.StartsWith("CR"))
				{
					noteString = noteString.Substring(2);
				}
				else if (noteString.StartsWith("P"))
				{
					noteString = noteString.Substring(1);
				}
				else if (noteString.StartsWith("CFA"))
				{
					noteString = noteString.Substring(3);
				}
				else if (noteString.EndsWith("chiao/jiao"))
				{
					noteString = noteString.Substring(0, noteString.Length - "chiao/jiao".Length);
				}
				else if (noteString.EndsWith("Kc"))
				{
					noteString = noteString.Substring(0, noteString.Length - "Kc".Length);
				}

				int prefixShift = 0;

				while (noteString.Length > prefixShift)
				{
					//int code = (int)noteString[prefixShift];

					//if ((code >= 65 && code <= 90) || (code >= 97 && code <= 122))
					//	prefixShift++;
					//else
					//	break;

					int code = (int)noteString[prefixShift];

					if (code < 48 || code > 57)
						prefixShift++;
					else
						break;
				}

				noteString = noteString.Substring(prefixShift);

				if (noteString.EndsWith("cents"))
				{
					noteMultiplier = 0.01;
					noteString = noteString.Substring(0, noteString.Length - "cents".Length);
				}
				else if (noteString.EndsWith("pfenings"))
				{
					noteMultiplier = 0.01;
					noteString = noteString.Substring(0, noteString.Length - "pfenings".Length);
				}
				else if(noteString.EndsWith("piastres"))
				{
					noteMultiplier = 0.01;
					noteString = noteString.Substring(0, noteString.Length - "piastres".Length);
				}

				noteString = noteString.Trim();

				Debug.WriteLine(currencyPageURL);

				result.Add(double.Parse(noteString) * multiplier * noteMultiplier);
			}

			return result;
		}
	}
}
