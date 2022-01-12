using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class Vector3
	{
		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public float X { get; }
		public float Y { get; }
		public float Z { get; }

		public float ComputeDistanceSquared(Vector3 p)
		{
			float dX = X - p.X;
			float dY = Y - p.Y;
			float dZ = Z - p.Z;

			return dX * dX + dY * dY + dZ * dZ;
		}

		public override string ToString()
		{
			return $"{{{X}, {Y}, {Z}}}";
		}
	}
}
