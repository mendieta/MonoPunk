//
//  Entity.cs
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoMac.ObjCRuntime;

namespace MonoPunk
{
	public class Entity:Tweener
	{
		public Boolean Visible = true;
		public Boolean Active = true;
		// this should be movet to tweener
		public Boolean Collidable = true;
		public Vector2 Position;

		public float X {
			get { return Position.X; }
			set { Position.X = value; }
		}

		public float Y {
			get { return Position.Y; }
			set { Position.Y = value; }			
		}

		public Vector2 Size;

		public float Width {
			get { return Size.X; }
			set { Size.X = value; }
		}

		public float Height {
			get { return Size.Y; }
			set { Size.Y = value; }
		}

		/**
		 * X origin of the Entity's hitbox.
		 */
		public int OriginX;
		/**
		 * Y origin of the Entity's hitbox.
		 */
		public int OriginY;

		public Entity (float x = 0, float y = 0, Graphic graphic = null)
		{
			Position = new Vector2 (x, y);

			this.Graphic = graphic;
		}

		public SpriteBatch renderTarget;

		/**
		 * Override this, called when the Entity is added to a World.
		 */
		virtual public void Added ()
		{

		}

		/**
		 * Override this, called when the Entity is removed from a World.
		 */
		virtual public void Removed ()
		{

		}

		/**
		 * Updates the Entity.
		 */
		override public void Update ()
		{

		}

		/**
		 * Renders the Entity. If you override this for special behaviour,
		 * remember to call super.render() to render the Entity's graphic.
		 */
		virtual public void Render ()
		{
			if (_graphic != null && _graphic.Visible) {
				_graphic.Render (renderTarget != null ? renderTarget : MP.Buffer, Position, MP.Camera);
			}
		}

		/**
		 * Checks for a collision against an Entity type.
		 * @param	type		The Entity type to check for.
		 * @param	x			Virtual x position to place this Entity.
		 * @param	y			Virtual y position to place this Entity.
		 * @return	The first Entity collided with, or null if none were collided.
		 */
		public Entity Collide (String type, float x, float y)
		{
			if (!_world)
				return null;

			Entity e = _world._typeFirst [type];
			if (!e)
				return null;

			_x = this.X; 
			_y = this.Y;
			this.X = x; 
			this.Y = y;

			if (!_mask) {
				while (e) {
					if (e.Collidable && e != this
					    && x - OriginX + Width > e.X - e.OriginX
					    && y - OriginY + Height > e.Y - e.OriginY
					    && x - OriginX < e.X - e.OriginX + e.Width
					    && y - OriginY < e.Y - e.OriginY + e.Height) {
						if (!e._mask || e._mask.Collide (HITBOX)) {
							this.X = _x; 
							this.Y = _y;
							return e;
						}
					}
					e = e._typeNext;
				}
				this.X = _x;
				this.Y = _y;
				return null;
			}

			while (e) {
				if (e.Collidable && e != this
				    && x - OriginX + Width > e.X - e.OriginX
				    && y - OriginY + Height > e.Y - e.OriginY
				    && x - OriginX < e.X - e.OriginX + e.Width
				    && y - OriginY < e.Y - e.OriginY + e.Height) {
					if (_mask.Collide (e._mask ? e._mask : e.HITBOX)) {
						this.X = _x;
						this.Y = _y;
						return e;
					}
				}
				e = e._typeNext;
			}
			this.X = _x;
			this.Y = _y;
			return null;
		}

		/**
		 * Checks for collision against multiple Entity types.
		 * @param	types		An Array or Vector of Entity types to check for.
		 * @param	x			Virtual x position to place this Entity.
		 * @param	y			Virtual y position to place this Entity.
		 * @return	The first Entity collided with, or null if none were collided.
		 */
		public Entity CollideTypes (String[] types, float x, float y)
		{
			if (!_world) {
				return null;
			}

			Entity e;
			foreach (String type in types) {
				if ((e = Collide (type, x, y))) {
					return e;
				}
			}

			return null;
		}

		/**
		 * Checks if this Entity collides with a specific Entity.
		 * @param	e		The Entity to collide against.
		 * @param	x		Virtual x position to place this Entity.
		 * @param	y		Virtual y position to place this Entity.
		 * @return	The Entity if they overlap, or null if they don't.
		 */
		public Entity CollideWith (Entity e, float x, float y)
		{
			_x = this.X; 
			_y = this.Y;
			this.X = x; 
			this.Y = y;

			if (e.Collidable
			    && x - OriginX + Width > e.X - e.OriginX
			    && y - OriginY + Height > e.Y - e.OriginY
			    && x - OriginX < e.X - e.OriginX + e.Width
			    && y - OriginY < e.Y - e.OriginY + e.Height) {
				if (!_mask) {
					if (!e._mask || e._mask.Collide (HITBOX)) {
						this.X = _x;
						this.Y = _y;
						return e;
					}
					this.X = _x;
					this.Y = _y;
					return null;
				}
				if (_mask.Collide (e._mask ? e._mask : e.HITBOX)) {
					this.X = _x;
					this.Y = _y;
					return e;
				}
			}
			this.X = _x;
			this.Y = _y;
			return null;
		}

		/**
		 * Checks if this Entity overlaps the specified rectangle.
		 * @param	x			Virtual x position to place this Entity.
		 * @param	y			Virtual y position to place this Entity.
		 * @param	rX			X position of the rectangle.
		 * @param	rY			Y position of the rectangle.
		 * @param	rWidth		Width of the rectangle.
		 * @param	rHeight		Height of the rectangle.
		 * @return	If they overlap.
		 */
		public Boolean CollideRect (float x, float y, float rX, float rY, float rWidth, float rHeight)
		{
			if (x - OriginX + Width >= rX && y - OriginY + Height >= rY
			    && x - OriginX <= rX + rWidth && y - OriginY <= rY + rHeight) {
				if (!_mask)
					return true;
				_x = this.X;
				_y = this.Y;
				this.X = x;
				this.Y = y;
				MP.Entity.X = rX;
				MP.Entity.Y = rY;
				MP.Entity.Width = rWidth;
				MP.Entity.Height = rHeight;
				if (_mask.Collide (MP.Entity.HITBOX)) {
					this.X = _x;
					this.Y = _y;
					return true;
				}
				this.X = _x;
				this.Y = _y;
				return false;
			}
			return false;
		}

		/**
		 * Checks if this Entity overlaps the specified position.
		 * @param	x			Virtual x position to place this Entity.
		 * @param	y			Virtual y position to place this Entity.
		 * @param	pX			X position.
		 * @param	pY			Y position.
		 * @return	If the Entity intersects with the position.
		 */
		public Boolean CollidePoint (float x, float y, float pX, float pY)
		{
			if (pX >= x - OriginX && pY >= y - OriginY
			    && pX < x - OriginX + Width && pY < y - OriginY + Height) {
				if (!_mask)
					return true;
				_x = this.X;
				_y = this.Y;
				this.X = x;
				this.Y = y;
				MP.Entity.X = pX;
				MP.Entity.Y = pY;
				MP.Entity.Width = 1;
				MP.Entity.Height = 1;
				if (_mask.Collide (MP.Entity.HITBOX)) {
					this.X = _x;
					this.Y = _y;
					return true;
				}
				this.X = _x;
				this.Y = _y;
				return false;
			}
			return false;
		}

		/**
		 * Populates an array with all collided Entities of a type.
		 * @param	type		The Entity type to check for.
		 * @param	x			Virtual x position to place this Entity.
		 * @param	y			Virtual y position to place this Entity.
		 * @param	array		The Array or Vector object to populate.
		 * @return	The array, populated with all collided Entities.
		 */
		public void CollideInto (String type, float x, float y, Entity[] array)
		{
			if (!_world)
				return;

			Entity e = _world._typeFirst [type];
			if (!e)
				return;

			_x = this.X; 
			_y = this.Y;
			this.X = x;
			this.Y = y;
			uint n = array.Length;

			if (!_mask) {
				while (e) {
					if (e.Collidable && e != this
					    && x - OriginX + Width > e.X - e.OriginX
					    && y - OriginY + Height > e.Y - e.OriginY
					    && x - OriginX < e.X - e.OriginX + e.Width
					    && y - OriginY < e.Y - e.OriginY + e.Height) {
						if (!e._mask || e._mask.Collide (HITBOX))
							array [n++] = e;
					}
					e = e._typeNext;
				}
				this.X = _x; 
				this.Y = _y;
				return;
			}

			while (e) {
				if (e.Collidable && e != this
				    && x - OriginX + Width > e.X - e.OriginX
				    && y - OriginY + Height > e.Y - e.OriginY
				    && x - OriginX < e.X - e.OriginX + e.Width
				    && y - OriginY < e.Y - e.OriginY + e.Height) {
					if (_mask.Collide (e._mask ? e._mask : e.HITBOX))
						array [n++] = e;
				}
				e = e._typeNext;
			}
			this.X = _x;
			this.Y = _y;
			return;
		}

		/**
		 * Populates an array with all collided Entities of multiple types.
		 * @param	types		An array of Entity types to check for.
		 * @param	x			Virtual x position to place this Entity.
		 * @param	y			Virtual y position to place this Entity.
		 * @param	array		The Array or Vector object to populate.
		 * @return	The array, populated with all collided Entities.
		 */
		public void CollideTypesInto (String[] types, float x, float y, Entity[] array)
		{
			if (!_world) {
				return;
			}
			foreach (String type in types) {
				CollideInto (type, x, y, array);
			}
		}

		/**
		 * If the Entity collides with the camera rectangle.
		 */
		public Boolean OnCamera {
			get { return CollideRect (X, Y, _world.Camera.X, _world.Camera.Y, MP.Width, MP.Height); }
		}

		/**
		 * The World object this Entity has been added to.
		 */
		public World World {
			get{ return _world; }
		}

		/**
		 * Half the Entity's width.
		 */
		public float HalfWidth { 
			get{ return Width / 2; } 
		}

		/**
		 * Half the Entity's height.
		 */
		public float HalfHeight { 
			get{ return Height / 2; } 
		}

		/**
		 * The center x position of the Entity's hitbox.
		 */
		public float CenterX { 
			get{ return X - OriginX + Width / 2; } 
		}

		/**
		 * The center y position of the Entity's hitbox.
		 */
		public float CenterY { 
			get{ return Y - OriginY + Height / 2; }
		}

		/**
		 * The leftmost position of the Entity's hitbox.
		 */
		public float Left {
			get{ return X - OriginX; }
		}

		/**
		 * The rightmost position of the Entity's hitbox.
		 */
		public float Right { 
			get{ return X - OriginX + Width; } 
		}

		/**
		 * The topmost position of the Entity's hitbox.
		 */
		public float Top { 
			get{ return Y - OriginY; }
		}

		/**
		 * The bottommost position of the Entity's hitbox.
		 */
		public float Bottom { 
			get{ return Y - OriginY + Height; }
		}

		/**
		 * The rendering layer of this Entity. Higher layers are rendered first.
		 */
		public int Layer {
			get{ return _layer; }
			set {
				if (_layer == value)
					return;
				if (!_world) {
					_layer = value;
					return;
				}
				_world.RemoveRender (this);
				_layer = value;
				_world.AddRender (this);
			}
		}

		/**
		 * The collision type, used for collision checking.
		 */
		public string type {
			get{ return _type; }
			set {
				if (_type == value)
					return;
				if (!_world) {
					_type = value;
					return;
				}
				if (_type)
					_world.RemoveType (this);
				_type = value;
				if (value)
					_world.AddType (this);
			}
		}

		/**
		 * An optional Mask component, used for specialized collision. If this is
		 * not assigned, collision checks will use the Entity's hitbox by default.
		 */
		public Mask Mask {
			get{ return _mask; }
			set {
				if (_mask == value)
					return;
				if (_mask)
					_mask.AssignTo (null);
				_mask = value;
				if (value)
					_mask.AssignTo (this);
			}
		}

		/**
		 * Graphical component to render to the screen.
		 */
		public Graphic Graphic {
			get{ return _graphic; }
			set {
				if (_graphic == value)
					return;
				_graphic = value;
				if (value && value._assign != null)
					value._assign ();
			}
		}

		/**
		 * Adds the graphic to the Entity via a Graphiclist.
		 * @param	g		Graphic to add.
		 */
		public Graphic AddGraphic (Graphic g)
		{
			if (this.Graphic is Graphiclist)
				(this.Graphic as Graphiclist).Add (g);
			else {
				Graphiclist list = new Graphiclist ();
				if (this.Graphic) {
					list.Add (this.Graphic);
				}
				list.Add (g);
				Graphic = list;
			}
			return g;
		}

		/**
		 * Sets the Entity's hitbox properties.
		 * @param	width		Width of the hitbox.
		 * @param	height		Height of the hitbox.
		 * @param	originX		X origin of the hitbox.
		 * @param	originY		Y origin of the hitbox.
		 */
		public void SetHitbox (int width = 0, int height = 0, int originX = 0, int originY = 0)
		{
			this.Width = width;
			this.Height = height;
			this.OriginX = originX;
			this.OriginY = originY;
		}

		/**
		 * Sets the Entity's hitbox to match that of the provided object.
		 * @param	o		The object defining the hitbox (eg. an Image or Rectangle).
		 */
		public void SetHitboxTo (Object o)
		{
			if (o is Texture2D) {
				SetHitbox ((o as Texture2D).Width, (o as Texture2D).Height, 0, 0);
			} else if (o is Rectangle) {
				SetHitbox ((o as Rectangle).Width, (o as Rectangle).Height, (o as Rectangle).X, (o as Rectangle).Y);
			}
			/*if (o is Texture2D || o is Rectangle) SetHitbox(o.Width, o.Height, -o.X, -o.Y);
			else
			{
				if (o.hasOwnProperty("width")) width = o.width;
				if (o.hasOwnProperty("height")) height = o.height;
				if (o.hasOwnProperty("originX") && !(o is Graphic)) originX = o.originX;
				else if (o.hasOwnProperty("x")) originX = -o.x;
				if (o.hasOwnProperty("originY") && !(o is Graphic)) originY = o.originY;
				else if (o.hasOwnProperty("y")) originY = -o.y;
			}*/
		}

		/**
		 * Sets the origin of the Entity.
		 * @param	x		X origin.
		 * @param	y		Y origin.
		 */
		public void setOrigin (int x = 0, int y = 0)
		{
			OriginX = x;
			OriginY = y;
		}

		/**
		 * Center's the Entity's origin (half width &amp; height).
		 */
		public void centerOrigin ()
		{
			OriginX = Width / 2.0f;
			OriginY = Height / 2.0f;
		}
		// Entity information.
		/** @private */ //internal var _class:Class;
		/** @private */ internal World _world;
		/** @private */ internal String _type;
		/** @private */ internal String _name;
		/** @private */ internal int _layer;
		/** @private */ internal Entity _updatePrev;
		/** @private */ internal Entity _updateNext;
		/** @private */ internal Entity _renderPrev;
		/** @private */ internal Entity _renderNext;
		/** @private */ internal Entity _typePrev;
		/** @private */ internal Entity _typeNext;
		/** @private */ internal Entity _recycleNext;
		// Collision information.
		/** @private */ private const Mask HITBOX = new Mask ();
		/** @private */ private Mask _mask;
		/** @private */ private float _x;
		/** @private */ private float _y;
		/** @private */ private float _moveX = 0;
		/** @private */ private float _moveY = 0;
		// Rendering information.
		/** @private */ internal Graphic _graphic;
		/** @private */ private Point _point = MP.Point;
		/** @private */ private Point _camera = MP.Point2;
	}
}

