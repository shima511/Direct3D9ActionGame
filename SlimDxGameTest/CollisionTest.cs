using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimDX;

namespace SlimDxGameTest
{
    [TestClass]
    public class CollisionTest
    {
        [TestMethod]
        public void CircleHitCircle()
        {
            SlimDxGame.Collision.Shape.Circle circle1 = new SlimDxGame.Collision.Shape.Circle();
            SlimDxGame.Collision.Shape.Circle circle2 = new SlimDxGame.Collision.Shape.Circle();

            circle1.Center = new Vector2(0.0f, 0.0f);
            circle1.Radius = 1.0f;

            circle2.Center = new Vector2(1.0f, 0.0f);
            circle2.Radius = 1.0f;

            Assert.IsTrue(circle1.Hit(circle2));
        }
    }
}
