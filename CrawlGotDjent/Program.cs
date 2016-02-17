using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace CrawlGotDjent
{
	class Program
	{
		static void Main(string[] args)
		{
			string baseURL = "http://got-djent.com/front?page=";
			List<string> urls = new List<string>();
			List<string> masterList = File.ReadAllLines(@"E:\Music\got-djent master-list.txt").ToList();

			for (int x = 0; x < 50; x++)
			{
				HtmlDocument doc = new HtmlDocument();
				doc.Load(GetWebText(baseURL + x.ToString()));
				int addedCount = 0;

				foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
				{
					HtmlAttribute att = link.Attributes["href"];
					if ((att.Value.ToUpper().Contains("MEDIA") || (att.Value.ToUpper().Contains("ARTICLE"))) && urls.Contains(att.Value) == false && masterList.Contains("http://got-djent.com" + att.Value) == false)
					{
						urls.Add("http://got-djent.com" + att.Value);
						masterList.Add("http://got-djent.com" + att.Value);
						addedCount++;
					}
				}
				if (addedCount == 0)
				{ break; }
			}

			File.AppendAllLines(@"E:\Music\got-djent links.txt", urls);
			File.AppendAllLines(@"E:\Music\got-djent master-list.txt", urls);
		}


		private static Stream GetWebText(string url)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
			request.UserAgent = "Mardoch Crawling for New Tunes";
			WebResponse response = request.GetResponse();
			Stream stream = response.GetResponseStream();
			return stream;
		}
	}
}
