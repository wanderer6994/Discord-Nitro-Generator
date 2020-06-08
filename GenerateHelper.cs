using System;
using System.IO;
using System.Linq;
using Colorful;

namespace NitroChecker
{
	// Token: 0x02000003 RID: 3
	internal class GenerateHelper
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000026C0 File Offset: 0x000008C0
		public static void Generate()
		{
			string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567891234567891234567891234567891234567891234567";
			StreamWriter streamWriter = new StreamWriter("codes.txt");
			try
			{
				Colorful.Console.WriteLine("Génération...");
				for (int i = 0; i < GenerateHelper.amount; i++)
				{
					streamWriter.WriteLine(GenerateHelper.m2(16, charset));
				}
			}
			finally
			{
				((IDisposable)streamWriter).Dispose();
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000272C File Offset: 0x0000092C
		private static string m2(int length, string charset)
		{
			return new string(Enumerable.Repeat<string>(charset, length).Select(new Func<string, char>(GenerateHelper.m1)).ToArray<char>());
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002760 File Offset: 0x00000960
		private static char m1(string string_0)
		{
			return string_0[GenerateHelper.random.Next(string_0.Length)];
		}

		// Token: 0x04000010 RID: 16
		public static int amount;

		// Token: 0x04000011 RID: 17
		private static Random random = new Random();
	}
}
