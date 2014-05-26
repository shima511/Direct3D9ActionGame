using System;
using System.Collections.Generic;
using SlimDX;

namespace SlimDxGame.Collision.Shape
{
    interface IShape
    {
        bool Hit(Line line);
        bool Hit(Point point);
        bool Hit(Circle circle);
    }

    class Line : IShape
    {
        /// <summary>
        /// 始点
        /// </summary>
        public Vector2 StartingPoint { get; set; }
        /// <summary>
        /// 終点
        /// </summary>
        public Vector2 TerminalPoint { get; set; }
        private const float Allowable = 0.0002f;

        public bool Hit(Line line)
        {
            return false;
        }

        public bool Hit(Point point)
        {
            var line_length = (StartingPoint -TerminalPoint).Length();
            var to_line_length = (point.Position - StartingPoint).Length();
            var x_dot = (TerminalPoint.X - StartingPoint.X) * (point.Position.X - StartingPoint.X);
            var y_dot = (TerminalPoint.Y - StartingPoint.Y) * (point.Position.Y - StartingPoint.Y);
            bool face_same_direction = x_dot + y_dot - Allowable <= line_length * to_line_length && x_dot + y_dot + Allowable >= line_length * to_line_length;
            bool on_the_line = line_length >= to_line_length;
            return face_same_direction && on_the_line;
        }

        public bool Hit(Circle circle)
        {
            var start_to_center_vector = new Vector2(circle.Center.X - StartingPoint.X, circle.Center.Y - StartingPoint.Y);
            var line_vector = new Vector2(TerminalPoint.X - StartingPoint.X, TerminalPoint.Y - StartingPoint.Y);
            var line_length = (TerminalPoint - StartingPoint).Length();
            var distance = start_to_center_vector.X * line_vector.Y - start_to_center_vector.Y * line_vector.X;
            if (distance > circle.Radius)
            {
                return false;
            }
            else
            {

                var terminal_to_center_vector = new Vector2(circle.Center.X - TerminalPoint.X, circle.Center.Y - TerminalPoint.Y);
                if (Vector2.Dot(start_to_center_vector, line_vector) * Vector2.Dot(terminal_to_center_vector, line_vector) <= 0)
                {
                    return true;
                }
                else if (circle.Radius > start_to_center_vector.Length() || circle.Radius > terminal_to_center_vector.Length())
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Point : IShape
    {
        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 Position { get; set; }
        private const float Allowable = 0.0002f;

        public bool Hit(Line line)
        {
            var line_length = (line.StartingPoint - line.TerminalPoint).Length();
            var to_line_length = (Position - line.StartingPoint).Length();
            var x_dot = (line.TerminalPoint.X - line.StartingPoint.X) * (Position.X - line.StartingPoint.X);
            var y_dot = (line.TerminalPoint.Y - line.StartingPoint.Y) * (Position.Y - line.StartingPoint.Y);
            bool face_same_direction = x_dot + y_dot - Allowable <= line_length * to_line_length && x_dot + y_dot + Allowable >= line_length * to_line_length;
            bool on_the_line = line_length >= to_line_length;
            return face_same_direction && on_the_line;
        }

        public bool Hit(Point point)
        {
            bool x_hit = point.Position.X - Allowable <= Position.X && point.Position.X + Allowable >= Position.X;
            bool y_hit = point.Position.Y - Allowable <= Position.Y && point.Position.Y + Allowable >= Position.Y;
            return x_hit && y_hit;
        }

        public bool Hit(Circle circle)
        {
            return Math.Pow((circle.Center.X - Position.X), 2) + Math.Pow((circle.Center.Y - Position.Y), 2) <= Math.Pow(circle.Radius, 2);
        }

    }

    class Circle : IShape
    {
        /// <summary>
        /// 中心
        /// </summary>
        public Vector2 Center { get; set; }
        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; set; }

        public bool Hit(Line line)
        {
            var start_to_center_vector = new Vector2(Center.X - line.StartingPoint.X, Center.Y - line.StartingPoint.Y);
            var line_vector = new Vector2(line.TerminalPoint.X - line.StartingPoint.X, line.TerminalPoint.Y - line.StartingPoint.Y);
            var line_length = (line.TerminalPoint - line.StartingPoint).Length();
            var distance = start_to_center_vector.X * line_vector.Y - start_to_center_vector.Y * line_vector.X;
            if (distance > Radius)
            {
                return false;
            }
            else
            {
                
                var terminal_to_center_vector = new Vector2(Center.X - line.TerminalPoint.X, Center.Y - line.TerminalPoint.Y);
                if(Vector2.Dot(start_to_center_vector, line_vector) * Vector2.Dot(terminal_to_center_vector, line_vector) <= 0){
                    return true;
                }
                else if(Radius > start_to_center_vector.Length() || Radius > terminal_to_center_vector.Length())
                {
                    return true;
                }
            }
            return false;
        }

        public bool Hit(Point point)
        {
            return Math.Pow((Center.X - point.Position.X), 2) + Math.Pow((Center.Y - point.Position.Y), 2) <= Math.Pow(Radius, 2);
        }

        public bool Hit(Circle circle)
        {
            return Math.Pow((circle.Center.X - Center.X), 2) + Math.Pow((circle.Center.Y - Center.Y), 2) <= Math.Pow(circle.Radius + Radius, 2);
        }

    }

}
