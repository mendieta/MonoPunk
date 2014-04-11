//
//  Engine.cs
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
using Microsoft.Xna.Framework.Graphics;

namespace MonoPunk
{
	public class Engine:Game
	{
		GraphicsDeviceManager graphics;
		public static Engine currentEngine;
		private float frameRateSum = 0;
		private int frameRateCount = 0;

		public Engine (int width, int height, string assetDirectory = "./", int frameRate = 60, Boolean fixedFrameRate = false) : base ()
		{
			MP.Width = width;
			MP.Height = height;
			MP.HalfWidth = width / 2;
			MP.HalfHeight = height / 2;
			MP.AssignedFrameRate = frameRate;
			MP.Fixed = fixedFrameRate;
			MP.TimeInFrames = fixedFrameRate;

			//TODO Implement control over user resizing
			Window.AllowUserResizing = true;

			graphics = new GraphicsDeviceManager (this);
			graphics.SynchronizeWithVerticalRetrace = true;
			graphics.PreferredBackBufferWidth = MP.Width;
			graphics.PreferredBackBufferHeight = MP.Height;
			graphics.ApplyChanges ();

			//TODO Implement fullscreen code
			/*graphics.IsFullScreen = true;
			graphics.ApplyChanges ();*/
		
			Content.RootDirectory = assetDirectory;
			Engine.currentEngine = this;
		}

		public static Texture2D EmbeddFile (String filename)
		{
			return currentEngine.Content.Load<Texture2D> (filename);
		}

		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures
			MP.Buffer = new SpriteBatch (GraphicsDevice);
			// TODO: use this.Content to load your game content here
			//eater.LoadGraphic(Content,"smiley.png");
			//MP.CurrentWorld.Add (eater);
			//myWorld.addRender(eater);
			//myWorld.addUpdate(eater);
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			MP.Buffer.Begin ();
			Render ();
			MP.Buffer.End ();

			base.Draw (gameTime);
		}

		protected override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
		}

		public void Render ()
		{

		}

		private void checkWorld ()
		{

		}
	}
}

