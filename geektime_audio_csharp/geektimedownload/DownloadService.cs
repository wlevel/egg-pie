using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace geektimedownload
{
	public static class DownloadService
	{
		public static async Task<IConfigurationBuilder> Download(this IConfigurationBuilder builder)
		{
			return await Download(builder, "");
		}


		public static async Task<IConfigurationBuilder> Download(this IConfigurationBuilder builder, string inputPath)
		{
			var filePaths = new List<string>();
			DirectoryInfo folder = new DirectoryInfo(inputPath);

			foreach (FileInfo file in folder.GetFiles("*.html"))
			{
				filePaths.Add(file.FullName);
			}
			var sw = Stopwatch.StartNew();
			var result = Parallel.ForEach(filePaths, currentFile =>
			{
				DownloadFile(currentFile).Wait();

			});
			Console.WriteLine($"Download complete for course: {Path.GetFileNameWithoutExtension(inputPath)} ! execution time {Math.Round(sw.Elapsed.TotalSeconds, 2)} seconds");

			return builder;
		}


		public static async Task DownloadFile(string filePath)
		{
			var matchs = await GetFile(filePath);
			int count = 0;
			foreach (var itemDic in matchs)
			{
				itemDic.TryGetValue("src", out string url);
				count++;
				string fileName = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath));
				if (count > 1)
				{
					fileName += count;
				}
				fileName += ".mp3";
				Uri uri = new Uri(url);
				await DownloadAsync(uri, fileName);
				Console.WriteLine($"Download {fileName}!");
			}
		}

		public static async Task<IList<IDictionary<string, string>>> GetFile(string inputPath)
		{
			string content = "";
			using (StreamReader file = new StreamReader(inputPath))
			{
				content = await file.ReadToEndAsync();
			}

			var matchs = ParseHtml(content, "<audio(?:\\s+([\\w\\-]+)(?:\\s*=\\s*(?<qouta>['\"]?)([^'\">]*)\\k<qouta>)?)*\\s*>");
			return matchs;
		}

		public static IList<IDictionary<string, string>> ParseHtml(string html, string patern)
		{
			IList<IDictionary<string, string>> tags = new List<IDictionary<string, string>>();
			//MitchellChu .NET Blog
			// 首先定义下正则表达式
			Regex regexParser = new Regex(patern, RegexOptions.IgnoreCase);
			// 1. 使用qouta的原因是因为引号要配对
			// 2. 引号在实际中，也有可能不存在，要加上?
			// 3. 只要属性名和属性值，因此其他的我们都忽略

			MatchCollection tagsMatched = regexParser.Matches(html);
			// 遍历所有的匹配HTML元素
			foreach (Match m in tagsMatched)
			{
				Dictionary<string, string> attrs = new Dictionary<string, string>();
				//将匹配到的HTML元素内的属性键值对存入字典中
				for (int i = 0; i < m.Groups[1].Captures.Count; i++)
				{
					attrs.Add(m.Groups[1].Captures[i].Value, m.Groups[2].Captures[i].Value);
				}
				tags.Add(attrs);
			}
			return tags;
		}


		public static async Task DownloadAsync(Uri requestUri, string filename)
		{
			using (var client = new HttpClient())
			using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
			using (
				Stream contentStream = await (await client.SendAsync(request)).Content.ReadAsStreamAsync(),
				stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, 3145728, true))
			{
				await contentStream.CopyToAsync(stream);
			}
		}


		public static IEnumerable<IEnumerable<TSource>> ForBatch<TSource>(this IEnumerable<TSource> source, int size)
		{
			for (int i = 0; i < source.Count(); i += size)
				yield return source.Skip(i).Take(size);
		}

	}
}
