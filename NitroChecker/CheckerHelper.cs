using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Colorful;
using Leaf.xNet;

namespace NitroChecker
{
	// Token: 0x02000002 RID: 2
	internal class CheckerHelper
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void LoadCodes()
		{
			using (FileStream fileStream = File.Open("codes.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (BufferedStream bufferedStream = new BufferedStream(fileStream))
				{
					using (StreamReader streamReader = new StreamReader(bufferedStream))
					{
						while (streamReader.ReadLine() != null)
						{
							CheckerHelper.total++;
						}
					}
				}
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020EC File Offset: 0x000002EC
		public static void LoadProxies(string fileName)
		{
			using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (BufferedStream bufferedStream = new BufferedStream(fileStream))
				{
					using (StreamReader streamReader = new StreamReader(bufferedStream))
					{
						while (streamReader.ReadLine() != null)
						{
							CheckerHelper.proxytotal++;
						}
					}
				}
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002184 File Offset: 0x00000384
		public static void UpdateTitle()
		{
			for (;;)
			{
				CheckerHelper.CPM = CheckerHelper.CPM_aux;
				CheckerHelper.CPM_aux = 0;
				Colorful.Console.Title = string.Format("[Nitro Generator & Checker] | Checked: {0}/{1} | Hits: {2} | Bad: {3} | CPM: ", new object[]
				{
					CheckerHelper.check,
					CheckerHelper.total,
					CheckerHelper.hits,
					CheckerHelper.bad,
					CheckerHelper.err
				}) + (CheckerHelper.CPM * 60).ToString() + " | Crée par Stanley#0001";
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000222C File Offset: 0x0000042C
		public static void Check()
		{
			for (;;)
			{
				bool flag = CheckerHelper.proxyindex > CheckerHelper.proxies.Count<string>() - 2;
				if (flag)
				{
					CheckerHelper.proxyindex = 0;
				}
				try
				{
					Interlocked.Increment(ref CheckerHelper.proxyindex);
					using (HttpRequest httpRequest = new HttpRequest())
					{
						bool flag2 = CheckerHelper.accindex >= CheckerHelper.accounts.Count<string>();
						if (flag2)
						{
							CheckerHelper.stop++;
							break;
						}
						Interlocked.Increment(ref CheckerHelper.accindex);
						string text = CheckerHelper.accounts[CheckerHelper.accindex];
						try
						{
							bool flag3 = CheckerHelper.proxytype == "HTTP";
							if (flag3)
							{
								httpRequest.Proxy = HttpProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
								httpRequest.Proxy.ConnectTimeout = 5000;
							}
							bool flag4 = CheckerHelper.proxytype == "SOCKS4";
							if (flag4)
							{
								httpRequest.Proxy = Socks4ProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
								httpRequest.Proxy.ConnectTimeout = 5000;
							}
							bool flag5 = CheckerHelper.proxytype == "SOCKS5";
							if (flag5)
							{
								httpRequest.Proxy = Socks5ProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
								httpRequest.Proxy.ConnectTimeout = 5000;
							}
							bool flag6 = CheckerHelper.proxytype == "NO";
							if (flag6)
							{
								httpRequest.Proxy = null;
							}
							httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
							httpRequest.KeepAlive = true;
							httpRequest.IgnoreProtocolErrors = true;
							httpRequest.ConnectTimeout = 5000;
							httpRequest.UseCookies = true;
							httpRequest.AddHeader("Authorization", "mfa.TrYd-1Giyv9PUgpiv09tE9zoUfmYZIHjbvRx9K4bxl3TsaQ4h3Mjrj6NV0ro9ImI2q1fdXr104EC79H-NS0r");
							httpRequest.AddHeader("X-Fingerprint", "622832459796709396.no-ggtFhW5yweBngUZhaXThqlKk");
							string text2 = httpRequest.Get("https://discordapp.com/api/v6/entitlements/gift-codes/" + text + "?with_application=true&with_subscription_plan=true", null).ToString();
							bool flag7 = text2.Contains("Unknown Gift Code");
							if (flag7)
							{
								CheckerHelper.CPM_aux++;
								CheckerHelper.check++;
								CheckerHelper.bad++;
								Colorful.Console.WriteLine("[BAD] " + text, Color.DarkRed);
							}
							else
							{
								bool flag8 = text2.Contains("You are being rate limited");
								if (flag8)
								{
									CheckerHelper.accounts.Add(text);
								}
								else
								{
									bool flag9 = text2.Contains("You have tried to access a web page which is in violation of your internet usage policy");
									if (flag9)
									{
										CheckerHelper.accounts.Add(text);
									}
									else
									{
										bool flag10 = text2.Contains(", \"name\": \"Nitro\", \"summary\":");
										if (flag10)
										{
											CheckerHelper.CPM_aux++;
											CheckerHelper.check++;
											CheckerHelper.hits++;
											Colorful.Console.WriteLine("[GOOD] " + text, Color.DarkGreen);
											CheckerHelper.SaveData(text);
										}
										else
										{
											CheckerHelper.accounts.Add(text);
										}
									}
								}
							}
						}
						catch (Exception)
						{
							CheckerHelper.accounts.Add(text);
						}
					}
				}
				catch
				{
					Interlocked.Increment(ref CheckerHelper.err);
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002598 File Offset: 0x00000798
		public static void SaveData(string code)
		{
			try
			{
				using (StreamWriter streamWriter = File.AppendText("hits.txt"))
				{
					streamWriter.WriteLine("--------------------| Nitro Gift Code |----------------------");
					streamWriter.WriteLine("- Code: " + code);
					streamWriter.WriteLine("-------------------------------------------------------------");
					streamWriter.WriteLine();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002618 File Offset: 0x00000818
		private static string Parse(string source, string left, string right)
		{
			return source.Split(new string[]
			{
				left
			}, StringSplitOptions.None)[1].Split(new string[]
			{
				right
			}, StringSplitOptions.None)[0];
		}

		// Token: 0x04000001 RID: 1
		public static int total;

		// Token: 0x04000002 RID: 2
		public static int bad = 0;

		// Token: 0x04000003 RID: 3
		public static int hits = 0;

		// Token: 0x04000004 RID: 4
		public static int err = 0;

		// Token: 0x04000005 RID: 5
		public static int check = 0;

		// Token: 0x04000006 RID: 6
		public static int accindex = 0;

		// Token: 0x04000007 RID: 7
		public static List<string> proxies = new List<string>();

		// Token: 0x04000008 RID: 8
		public static string proxytype = "";

		// Token: 0x04000009 RID: 9
		public static int proxyindex = 0;

		// Token: 0x0400000A RID: 10
		public static int proxytotal = 0;

		// Token: 0x0400000B RID: 11
		public static int stop = 0;

		// Token: 0x0400000C RID: 12
		public static List<string> accounts = new List<string>();

		// Token: 0x0400000D RID: 13
		public static int CPM = 0;

		// Token: 0x0400000E RID: 14
		public static int CPM_aux = 0;

		// Token: 0x0400000F RID: 15
		public static int threads;
	}
}
