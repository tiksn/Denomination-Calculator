using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

			foreach (var currencyNode in node.SelectNodes("div/ul/li/a"))
			{
				string href = currencyNode.GetAttributeValue("href", string.Empty);

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
			result.CurrencyName = node.ChildNodes[9].InnerText;

			var notes = node.ChildNodes[13].InnerText;

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

			var notesParts = GetNotesOrCoins(notes);

			result.Notes.AddRange(notesParts);

			if (startsWithNote)
			{
				var coins = node.ChildNodes[15].InnerText;

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

		private static List<int> GetNotesOrCoins(string notesOrCoins)
		{
			var result = new List<int>();

			if (notesOrCoins.EndsWith("."))
				notesOrCoins = notesOrCoins.Substring(0, notesOrCoins.Length - 1);

			if (notesOrCoins.EndsWith(". "))
				notesOrCoins = notesOrCoins.Substring(0, notesOrCoins.Length - 2);

			var noteParts = notesOrCoins.Split(new string[] { ", ", "and" }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var notePart in noteParts)
			{
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

				result.Add(int.Parse(noteString));
			}

			return result;
		}
	}
}
