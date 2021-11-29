using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TwinSouls.Tools
{
	public static class Utils
	{
		public static Color32 GenerateColorFromSeed(string seed)
		{
			System.Random random = new System.Random(seed.GetHashCode());

			return new Color32(
				(byte)random.Next(0, 255),
				(byte)random.Next(0, 255),
				(byte)random.Next(0, 255),
				255
			);
		}
	}
}
