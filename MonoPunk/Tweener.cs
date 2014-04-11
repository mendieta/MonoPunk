//
//  Tweener.cs
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

namespace MonoPunk
{
	/**
	 * Updateable Tween container.
	 */
	public class Tweener
	{
		/**
		 * Persistent Tween type, will stop when it finishes.
		 */
		public const uint PERSIST = 0;
		/**
		 * Looping Tween type, will restart immediately when it finishes.
		 */
		public const uint LOOPING = 1;
		/**
		 * Oneshot Tween type, will stop and remove itself from its core container when it finishes.
		 */
		public const uint ONESHOT = 2;
		/**
		 * If this object should update.
		 */
		public Boolean Active = true;
		/**
		 * If the Tweener should clear on removal. For Entities, this is when they are
		 * removed from a World, and for World this is when the active World is switched.
		 */
		public Boolean AutoClear = false;

		/**
		 * Constructor.
		 */
		public Tweener ()
		{
		}

		/**
		 * Updates the Tween container.
		 */
		virtual public void Update ()
		{

		}

		/**
		 * Adds a new Tween.
		 * @param	t			The Tween to add.
		 * @param	start		If the Tween should call start() immediately.
		 * @return	The added Tween.
		 */
		public Tween AddTween (Tween t, bool start = false)
		{
			if (t._parent)
				throw new System.ArgumentException ("Cannot add a Tween object more than once.");
			t._parent = this;
			t._next = _tween;
			if (_tween)
				_tween._prev = t;
			_tween = t;
			if (start)
				_tween.Start ();
			return t;
		}

		/**
		 * Removes a Tween.
		 * @param	t		The Tween to remove.
		 * @return	The removed Tween.
		 */
		public Tween RemoveTween (Tween t)
		{
			if (t._parent != this)
				throw new System.ArgumentException ("Core object does not contain Tween.");
			if (t._next)
				t._next._prev = t._prev;
			if (t._prev)
				t._prev._next = t._next;
			else
				_tween = t._next;
			t._next = t._prev = null;
			t._parent = null;
			t.Active = false;
			return t;
		}

		/**
		 * Removes all Tweens.
		 */
		public void ClearTweens ()
		{
			Tween t = _tween;
			Tween n;
			while (t) {
				n = t._next;
				RemoveTween (t);
				t = n;
			}
		}

		/** 
		 * Updates all contained tweens.
		 */
		public void UpdateTweens ()
		{
			Tween t = _tween;
			Tween n;
			while (t) {
				n = t._next;
				if (t.Active) {
					t.Update ();
					if (t._finish)
						t.Finish ();
				}
				t = n;
			}
		}
		// List information.
		/** @private */ internal Tween _tween;
	}
}

