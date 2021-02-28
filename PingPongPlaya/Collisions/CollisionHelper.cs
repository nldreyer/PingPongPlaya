using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionExample.Collisions
{
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects a collision between two BoundingCircles
        /// </summary>
        /// <param name="a">The first bounding circle</param>
        /// <param name="b">The second bounding circle</param>
        /// <returns>True if colliding</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >=
                Math.Pow(a.Center.X - b.Center.X, 2) +
                Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects a collision between two BoundingRectangles
        /// </summary>
        /// <param name="a">The first bounding rectangle</param>
        /// <param name="b">The second bounding rectangle</param>
        /// <returns>True if colliding</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right ||
                     a.Top > b.Bottom || a.Bottom < b.Top);
        }

        /// <summary>
        /// Detects a collision between a rectangle and a circle
        /// </summary>
        /// <param name="c">The BoundingCircle</param>
        /// <param name="r">The BoundingRectangle</param>
        /// <returns>True if colliding</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);

            return Math.Pow(c.Radius, 2) >=
                Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2);
        }

        /// <summary>
        /// Detects a collision between a rectangle and a circle
        /// </summary>
        /// <param name="r">The BoundingRectangle</param>
        /// <param name="c">The BoundingCircle</param>
        /// <returns>True if colliding</returns>
        public static bool Collides(BoundingRectangle r, BoundingCircle c) => Collides(c, r);

        /// <summary>
        /// Detects if a circle has gone left, right, or below the viewport and provides a new direction for the circle
        /// </summary>
        /// <param name="c">Circle to check</param>
        /// <param name="g">Game to reference for viewport</param>
        /// <param name="v">Output vector for new circle direction</param>
        /// <returns></returns>
        public static bool OffScreenBounce(BoundingCircle c, Game g, out Vector2 v)
        {
            if (c.Center.X < g.GraphicsDevice.Viewport.Bounds.Left ||
                c.Center.X + (c.Radius * 2) > g.GraphicsDevice.Viewport.Bounds.Right)
            {
                v = new Vector2((float)-1, 1);
                return true;
            }
            if (c.Center.Y + c.Radius > g.GraphicsDevice.Viewport.Bounds.Bottom)
            {
                v = new Vector2(0, 0);
                return true;
            }
            v = new Vector2(1, 1);
            return false;
        }
    }
}
