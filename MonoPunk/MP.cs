//
//  MP.cs
//
//  Author:
//       Ricardo Mendieta <mendieta@foostudio.mx>
//
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoPunk
{
	public static class MP
	{
		/**
	 	* The current screen buffer, drawn to in the render loop.
	 	*/
		public static SpriteBatch Buffer;
		public static double Elapsed;
		public static int Width;
		public static float HalfWidth;
		public static int Height;
		public static float HalfHeight;
		public static Boolean Fixed;
		public static int AssignedFrameRate;
		public static Boolean TimeInFrames;
		public static Vector2 Camera;

		public static float FElapsed {
			get { return (float)Elapsed; }
		}

		public static World CurrentWorld {
			get { return currentWorld; }
			set {
				if (currentWorld != value) {
					nextWorld = value;
				}
			}

		}

		public static float Degs2Rad (float degrees)
		{
			return (degrees / 180 * ((float)Math.PI));
		}

		public static int Rand (int amount)
		{
			return random.Next (0, amount);
		}

		internal static World currentWorld;
		internal static World nextWorld;
		private static Random random = new Random ();
		/** @private */ public static Point Point = new Point ();
		/** @private */ public static Point Point2 = new Point ();
		/** @private */ public static Entity Entity;
	}
}

