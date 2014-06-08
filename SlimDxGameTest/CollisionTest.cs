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
            SlimDxGame.Collision.Shape.Circle circle1 = new SlimDxGame.Collision.Shape.Circle()
            {
                Center = new Vector2(0.0f, 0.0f),
                Radius = 1.0f
            };
            SlimDxGame.Collision.Shape.Circle circle2 = new SlimDxGame.Collision.Shape.Circle()
            {
                Center = new Vector2(1.0f, 0.0f),
                Radius = 1.0f
            };

            Assert.IsTrue(circle1.Hit(circle2));
        }

        [TestMethod]
        public void CircleHitLine()
        {
            SlimDxGame.Collision.Shape.Circle circle = new SlimDxGame.Collision.Shape.Circle()
            {
                Center = new Vector2(0.0f, 0.0f),
                Radius = 1.0f
            };
            SlimDxGame.Collision.Shape.Line line = new SlimDxGame.Collision.Shape.Line()
            {
                StartingPoint = new Vector2(1.0f, 1.0f),
                TerminalPoint = new Vector2(-1.0f, -1.0f)
            };

            Assert.IsTrue(circle.Hit(line));
        }

        [TestMethod]
        public void CircleDoesNotHitLine()
        {
            SlimDxGame.Collision.Shape.Circle circle = new SlimDxGame.Collision.Shape.Circle()
            {
                Center = new Vector2(0.0f, 0.0f),
                Radius = 1.0f
            };
            SlimDxGame.Collision.Shape.Line line = new SlimDxGame.Collision.Shape.Line()
            {
                StartingPoint = new Vector2(-5.0f, 0.5f),
                TerminalPoint = new Vector2(5.0f, 0.5f)
            };

            Assert.IsFalse(circle.Hit(line));
        }

        [TestMethod]
        public void CircleDoesNotHitCircle()
        {
            SlimDxGame.Collision.Shape.Circle circle1 = new SlimDxGame.Collision.Shape.Circle()
            {
                Center = new Vector2(0.0f, 0.0f),
                Radius = 1.0f
            };
            SlimDxGame.Collision.Shape.Circle circle2 = new SlimDxGame.Collision.Shape.Circle()
            {
                Center = new Vector2(3.0f, 0.0f),
                Radius = 1.0f
            };

            Assert.IsFalse(circle1.Hit(circle2));
        }

        [TestMethod]
        public void PointHitLine()
        {
            SlimDxGame.Collision.Shape.Point point = new SlimDxGame.Collision.Shape.Point()
            {
                Position = new Vector2(0.0f, 0.0f)
            };
            SlimDxGame.Collision.Shape.Line line = new SlimDxGame.Collision.Shape.Line()
            {
                StartingPoint = new Vector2(-1.0f, 1.0f),
                TerminalPoint = new Vector2(1.0f, -1.0f)
            };

            Assert.IsTrue(point.Hit(line));
            Assert.IsTrue(line.Hit(point));
        }

        [TestMethod]
        public void PointDoesNotHitLine()
        {
            SlimDxGame.Collision.Shape.Point point = new SlimDxGame.Collision.Shape.Point()
            {
                Position = new Vector2(0.0f, 0.0f)
            };
            SlimDxGame.Collision.Shape.Line line = new SlimDxGame.Collision.Shape.Line()
            {
                StartingPoint = new Vector2(1.0f, 1.0f),
                TerminalPoint = new Vector2(1.0f, -1.0f)
            };

            Assert.IsFalse(point.Hit(line));
            Assert.IsFalse(line.Hit(point));
        }
    }
}
