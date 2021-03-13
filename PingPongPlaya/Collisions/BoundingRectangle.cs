using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace PingPongPlaya.Collisions
{
    /// <summary>
    /// A bounding rectangle for collision detection
    /// </summary>
    public struct BoundingRectangle
    {
        /// <summary>
        /// X position of BoundingRectangle
        /// </summary>
        public float X => RectangleBounds.X;

        /// <summary>
        /// Y position of BoundingRectangle
        /// </summary>
        public float Y => RectangleBounds.Y;

        /// <summary>
        /// Height of BoundingRectangle
        /// </summary>
        public float Width => RectangleBounds.Width;

        /// <summary>
        /// Width of BoundingRectangle
        /// </summary>
        public float Height => RectangleBounds.Height;

        /// <summary>
        /// Left side of BoundingRectangle
        /// </summary>
        public float Left => X;

        /// <summary>
        /// Right side of BoundingRectangle
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        /// Top of BoundingRectangle
        /// </summary>
        public float Top => Y;
        
        /// <summary>
        /// Bottom of BoundingRectangle
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        /// Rectangle containing all information required
        /// </summary>
        public Rectangle RectangleBounds;

        /// <summary>
        /// Constructor for BoundingRectangle with X and Y coordinates
        /// </summary>
        /// <param name="x">X-Value of rectangle</param>
        /// <param name="y">Y-Value of rectangle</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            RectangleBounds = new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        /// <summary>
        /// Constructor for BoundingRectangle with a vector2
        /// </summary>
        /// <param name="position">Locatioon of rectangle</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        public BoundingRectangle(Vector2 position, float width, float height)
        {
            RectangleBounds = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
        }

        /// <summary>
        /// Collision check with another rectangle
        /// </summary>
        /// <param name="other">The BoundingRectangle to check against</param>
        /// <returns>True if collision, false otherwise</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// Collision check with a circle
        /// </summary>
        /// <param name="other">The BoundingCircle to check against</param>
        /// <returns>True if collision, false otherwise</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
    }
}
