//
//  Graphic.cs
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
	/**
	 * Base class for all graphical types that can be drawn by Entity.
	 */
	public class Graphic
	{
		/**
		 * If the graphic should update.
		 */
		public Boolean Active = false;
		/**
		 * If the graphic should render.
		 */
		public Boolean Visible = true;
		/**
		 * X offset.
		 */
		public float X = 0;
		/**
		 * Y offset.
		 */
		public float Y = 0;
		/**
		 * X scrollfactor, effects how much the camera offsets the drawn graphic.
		 * Can be used for parallax effect, eg. Set to 0 to follow the camera,
		 * 0.5 to move at half-speed of the camera, or 1 (default) to stay still.
		 */
		public float ScrollX = 1;
		/**
		 * Y scrollfactor, effects how much the camera offsets the drawn graphic.
		 * Can be used for parallax effect, eg. Set to 0 to follow the camera,
		 * 0.5 to move at half-speed of the camera, or 1 (default) to stay still.
		 */
		public float ScrollY = 1;
		/**
		 * If the graphic should render at its position relative to its parent Entity's position.
		 */
		public Boolean Relative = true;

		/**
		 * Constructor.
		 */
		public Graphic ()
		{
		}

		/**
		 * Updates the graphic.
		 */
		virtual public void update ()
		{

		}

		virtual public void Render (SpriteBatch target, Vector2 point, Vector2 camera)
		{
			// to override
		}

		/** @private Callback for when the graphic is assigned to an Entity. */
		virtual public Action Assign {
			get{ return _assign; }
			set{ _assign = value; }

		}
		// Graphic information.
		/** @private */ internal Action _assign;
		/** @private */ protected Vector2 _point = new Vector2 ();
	}
}

