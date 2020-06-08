using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Colorful;

namespace NitroChecker
{
	// Token: 0x02000004 RID: 4
	internal class Program
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002794 File Offset: 0x00000994
		[STAThread]
		private static void Main(string[] args)
		{
			Program.Menu(args);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000027A0 File Offset: 0x000009A0
		private static void Menu(string[] args)
		{

			Colorful.Console.WriteAscii("Stan Generator", Color.FromArgb(251, 51, 0));

		

			Colorful.Console.Title = (Colorful.Console.Title = "[Nitro Generator & Checker] | Make by Stanley#0001");
			Thread.Sleep(250);
			Colorful.Console.WriteLine("[1] Cree des codes nitros", Color.Orange);
			Colorful.Console.WriteLine("[2] Check les codes nitros", Color.Orange);
			Colorful.Console.WriteLine();
			Colorful.Console.Write("", Color.DarkOrange);
			string text = System.Console.ReadLine();
			string text2 = text;
			if (text2 != null)
			{
				if (!(text2 == "1"))
				{
					if (text2 == "2")
					{
						Colorful.Console.WriteLine();
						Colorful.Console.Write("Combien de", Color.Orange);
						Colorful.Console.Write("THREADS", Color.Orange);
						Colorful.Console.Write(" voulez-vous utilisez", Color.Orange);
						Colorful.Console.Write(": ", Color.Orange);
						try
						{
							CheckerHelper.threads = int.Parse(Colorful.Console.ReadLine());
						}
						catch
						{
							CheckerHelper.threads = 100;
						}
						for (;;)
						{
							Colorful.Console.Write("> Quel type de ", Color.Orange);
							Colorful.Console.Write("PROXIES ", Color.White);
							Colorful.Console.Write("[HTTP, SOCKS4, SOCKS5, NO]", Color.DarkMagenta);
							Colorful.Console.Write(": ", Color.DarkMagenta);
							CheckerHelper.proxytype = Colorful.Console.ReadLine();
							CheckerHelper.proxytype = CheckerHelper.proxytype.ToUpper();
							bool flag = CheckerHelper.proxytype == "HTTP" || CheckerHelper.proxytype == "SOCKS4" || CheckerHelper.proxytype == "SOCKS5" || CheckerHelper.proxytype == "NO";
							if (flag)
							{
								break;
							}
							Colorful.Console.Write("> Choisis un format valide.\n\n", Color.Red);
							Thread.Sleep(2000);
						}
						Task.Factory.StartNew(delegate()
						{
							CheckerHelper.UpdateTitle();
						});
						Colorful.Console.WriteLine();
						CheckerHelper.accounts = new List<string>(File.ReadAllLines("codes.txt"));
						CheckerHelper.LoadCodes();
						Colorful.Console.Write("> ");
						Colorful.Console.Write(CheckerHelper.total, Color.DarkMagenta);
						Colorful.Console.WriteLine(" Codes nitros ajouté\n");
						OpenFileDialog openFileDialog = new OpenFileDialog();
						bool flag2 = CheckerHelper.proxytype != "NO";
						if (flag2)
						{
							string fileName;
							do
							{
								Colorful.Console.WriteLine("Choisissez vos proxys", Color.DarkMagenta);
								Thread.Sleep(500);
								openFileDialog.Title = "Select Proxy List";
								openFileDialog.DefaultExt = "txt";
								openFileDialog.Filter = "Text files|*.txt";
								openFileDialog.RestoreDirectory = true;
								openFileDialog.ShowDialog();
								fileName = openFileDialog.FileName;
							}
							while (!File.Exists(fileName));
							CheckerHelper.proxies = new List<string>(File.ReadAllLines(fileName));
							CheckerHelper.LoadProxies(fileName);
							Colorful.Console.Write("> ");
							Colorful.Console.Write(CheckerHelper.proxytotal, Color.DarkMagenta);
							Colorful.Console.WriteLine(" Proxies ajouté\n");
						}
						for (int i = 1; i <= CheckerHelper.threads; i++)
						{
							new Thread(new ThreadStart(CheckerHelper.Check)).Start();
						}
						Colorful.Console.ReadLine();
						Environment.Exit(0);
					}
				}
				else
				{
					Colorful.Console.WriteLine();
					Colorful.Console.Write("Combien de ", Color.Orange);
					Colorful.Console.Write("CODES", Color.Orange);
					Colorful.Console.Write(" voulez vous générez", Color.Orange);
					Colorful.Console.Write(": ", Color.Orange);
					try
					{
						GenerateHelper.amount = int.Parse(Colorful.Console.ReadLine());
					}
					catch
					{
						GenerateHelper.amount = 50000;
					}
					Task.Factory.StartNew(delegate()
					{
						GenerateHelper.Generate();
					}).Wait();
					Colorful.Console.WriteLine("Générez avec succès, retour au hub...", Color.Green);
					Thread.Sleep(2000);
					Program.Main(args);
					System.Console.ReadLine();
				}
			}
		}
	}
}
