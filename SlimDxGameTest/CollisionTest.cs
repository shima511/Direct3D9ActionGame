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

        [TestMethod]
        public void CircleHitLine()
        {
            SlimDxGame.Collision.Shape.Circle circle = new SlimDxGame.Collision.Shape.Circle();
            SlimDxGame.Collision.Shape.Line line = new SlimDxGame.Collision.Shape.Line();

            circle.Center = new Vector2(0.0f, 0.0f);
            circle.Radius = 1.0f;

            line.StartingPoint = new Vector2(1.0f, 1.0f);
            line.TerminalPoint = new Vector2(-1.0f, -1.0f);

            Assert.IsTrue(circle.Hit(line));
        }

        [TestMethod]
        public void CircleDoesNotHitLine()
        {
            SlimDxGame.Collision.Shape.Circle circle = new SlimDxGame.Collision.Shape.Circle();
            SlimDxGame.Collision.Shape.Line line = new SlimDxGame.Collision.Shape.Line();

            circle.Center = new Vector2(0.0f, 0.0f);
            circle.Radius = 1.0f;

            line.StartingPoint = new Vector2(-5.0f, 0.5f);
            line.TerminalPoint = new Vector2(5.0f, 0.5f);

            Assert.IsFalse(circle.Hit(line));
        }

        [TestMethod]
        public void CircleDoesNotHitCircle()
        {
            SlimDxGame.Collision.Shape.Circle circle1 = new SlimDxGame.Collision.Shape.Circle();
            SlimDxGame.Collision.Shape.Circle circle2 = new SlimDxGame.Collision.Shape.Circle();

            circle1.Center = new Vector2(0.0f, 0.0f);
            circle1.Radius = 1.0f;

            circle2.Center = new Vector2(3.0f, 0.0f);
            circle2.Radius = 1.0f;

            Assert.IsFalse(circle1.Hit(circle2));
        }
    }
}
