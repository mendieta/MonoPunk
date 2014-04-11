//
//  Tween.cs
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
	public class Tween
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
		public const  uint ONESHOT = 2;
		/**
		 * If the tween should update.
		 */
		public bool Active;
		/**
		 * Tween completion callback.
		 */
		public Action Complete;
		/**
		 * Length of time to wait before starting this tween.
		 */
		public double Delay = 0;

		/**
		 * Constructor. Specify basic information about the Tween.
		 * @param	duration		Duration of the tween (in seconds or frames).
		 * @param	type			Tween type, one of Tween.PERSIST (default), Tween.LOOPING, or Tween.ONESHOT.
		 * @param	complete		Optional callback for when the Tween completes.
		 * @param	ease			Optional easer function to apply to the Tweened value.
		 */
		public Tween (double duration, uint type = 0, Action complete = null, Func<double, double> ease = null)
		{
			_target = duration;
			_type = type;
			this.Complete = complete;
			_ease = ease;
		}

		/**
		 * Updates the Tween, called by World.
		 */
		public void Update ()
		{
			double dt = MP.TimeInFrames ? 1 : MP.Elapsed;
			if (Delay > 0) {
				Delay -= dt;

				if (Delay > 0) {
					return;
				} else {
					_time -= Delay;
				}
			} else {
				_time += dt;
			}

			_t = _time / _target;
			if (_time >= _target) {
				_t = 1;
				_finish = true;
			}
			if (_ease != null)
				_t = _ease (_t);
		}

		/**
		 * Starts the Tween, or restarts it if it's currently running.
		 */
		public void Start ()
		{
			_time = 0;
			if (_target == 0) {
				Active = false;
				return;
			}
			Active = true;
		}

		/**
		* Immediately stops the Tween and removes it from its Tweener without calling the complete callback.
		*/
		public void Cancel ()
		{
			Active = false;
			if (_parent != null)
				_parent.RemoveTween (this);
		}

		/** @private Called when the Tween completes. */
		internal void Finish ()
		{
			switch (_type) {
			case PERSIST:
				_time = _target;
				Active = false;
				break;
			case LOOPING:
				_time %= _target;
				_t = _time / _target;
				if (_ease != null)
					_t = _ease (_t);
				Start ();
				break;
			case ONESHOT:
				_time = _target;
				Active = false;
				_parent.RemoveTween (this);
				break;
			}
			_finish = false;
			if (Complete != null)
				Complete ();
		}

		/**
		 * The completion percentage of the Tween.
		 */
		public double Percent {
			get { return _time / _target; }
			set { _time = _target * value; }
		}

		/**
		 * The current time scale of the Tween (after easer has been applied).
		 */
		public double Scale { get { return _t; } }
		// Tween information.
		/** @private */ private uint _type;
		/** @private */ protected Func<double, double> _ease;
		/** @private */ protected double _t = 0;
		// Timing information.
		/** @private */ protected double _time;
		/** @private */ protected double _target;
		// List information.
		/** @private */ internal double _finish;
		/** @private */ internal Tweener _parent;
		/** @private */ internal Tween _prev;
		/** @private */ internal Tween _next;
	}
}

