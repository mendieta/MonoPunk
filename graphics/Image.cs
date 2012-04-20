using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dabrorius.MonoPunk
{
	public class Image : Graphic
	{
				/**
		 * Rotation of the image, in degrees.
		 */
		public float Angle = 0;
		
		/**
		 * Scale of the image, affects both x and y scale.
		 */
		public float Scale = 1;
		
		/**
		 * X scale of the image.
		 */
		public float ScaleX = 1;
		
		/**
		 * Y scale of the image.
		 */
		public float ScaleY = 1;
		
		/**
		 * X origin of the image, determines transformation point.
		 */
		public float OriginX = 0;
		
		/**
		 * Y origin of the image, determines transformation point.
		 */
		public float OriginY = 0;
		
		private Texture2D texture;
		
		public Image (Texture2D source)
		{
			clipRect = new Rectangle(0,0,source.Width,source.Height);;
			texture = source;
		}
		
		public Image (Texture2D source, Rectangle clipRect )
		{
			this.clipRect = clipRect;
			texture = source;
		}
		
		override public void Render(SpriteBatch target, Vector2 point, Vector2 camera)
		{
			if( texture == null) return;
			point.X += OriginX;
			point.Y += OriginY;
			Vector2 scale = new Vector2(ScaleX * Scale,ScaleY*Scale);
			target.Draw( texture, point, clipRect, Color.White, Angle,
        				scale, 1.0f, SpriteEffects.None, 0f );
			
		}
		
		private Rectangle clipRect;

	}
}

