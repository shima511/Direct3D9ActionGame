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

    public class Line : Object.Base.Model, IShape
    {
        /// <summary>
        /// 始点の値を取得または設定します。
        /// </summary>
        public Vector2 StartingPoint { get; set; }
        /// <summary>
        /// 終点の値を取得又は設定します。
        /// </summary>
        public Vector2 TerminalPoint { get; set; }

        /// <summary>
        /// 線分の切片を取得します。
        /// </summary>
        public float Intercept {
            get { return StartingPoint.Y; }
            private set { }
        }

        /// <summary>
        /// 線分の係数を取得します。
        /// </summary>
        public float Coefficient {
            get { return (TerminalPoint.Y - StartingPoint.Y) / (TerminalPoint.X - StartingPoint.X); }
            private set { }
        }

        /// <summary>
        /// 線分の傾きを取得します。
        /// </summary>
        /// 
        public float Slope
        {
            get { return (float)Math.Atan(Coefficient); }
            private set { }
        }
        /// <summary>
        /// 始点から終点へのベクトルを取得します。
        /// </summary>
        /// 
        public Vector2 Vector
        {
            get { return new Vector2(TerminalPoint.X - StartingPoint.X, TerminalPoint.Y - StartingPoint.Y); }
            private set { }
        }

        private const float Allowable = 0.02f;

        public Line()
        {
            this.ModelAsset = AssetFactory.ModelFactory.CreateBasicModel(AssetFactory.ModelType.Box);
        }

        /// <summary>
        /// x成分からy座標を求めます
        /// </summary>
        /// <param name="x">x座標の値</param>
        /// <returns>y座標の値</returns>
        public float GetYAxisFromXAxis(float x)
        {
            return Intercept + Coefficient * (x - StartingPoint.X);
        }

        public bool Hit(Line line)
        {
            var tc = (StartingPoint.X - TerminalPoint.X) * (line.StartingPoint.Y - StartingPoint.Y) + (StartingPoint.Y - TerminalPoint.Y) * (StartingPoint.X - line.StartingPoint.X);
            var td = (StartingPoint.X - TerminalPoint.X) * (line.TerminalPoint.Y - StartingPoint.Y) + (StartingPoint.Y - TerminalPoint.Y) * (StartingPoint.X - line.TerminalPoint.X);
            var ta = (line.StartingPoint.X - line.TerminalPoint.X) * (StartingPoint.Y - line.StartingPoint.Y) + (line.StartingPoint.Y - line.TerminalPoint.Y) * (line.StartingPoint.X - StartingPoint.X);
            var tb = (line.StartingPoint.X - line.TerminalPoint.X) * (TerminalPoint.Y - line.StartingPoint.Y) + (line.StartingPoint.Y - line.TerminalPoint.Y) * (line.StartingPoint.X - TerminalPoint.X);
            if (tc * td < 0 && ta * tb < 0) return true;
            return false;
        }

        public bool Hit(Point point)
        {
            var line_length = (StartingPoint -TerminalPoint).Length();
            var to_line_length = (point.Location - StartingPoint).Length();
            var x_dot = (TerminalPoint.X - StartingPoint.X) * (point.Location.X - StartingPoint.X);
            var y_dot = (TerminalPoint.Y - StartingPoint.Y) * (point.Location.Y - StartingPoint.Y);
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

        public override void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            var center = new SlimDX.Vector2((TerminalPoint.X - StartingPoint.X) / 2, (TerminalPoint.Y - StartingPoint.Y) / 2);
            _position.X = StartingPoint.X + center.X;
            _position.Y = StartingPoint.Y + center.Y;
            _scale.Y = 0.1f;
            _scale.X = (float)Math.Sqrt(Math.Pow(TerminalPoint.X - StartingPoint.X, 2) + (float)Math.Pow(TerminalPoint.Y - StartingPoint.Y, 2));
            _rotation.Z = Slope;
            base.Draw3D(dev);
        }
    }

    public class Point : Object.Base.Model, IShape
    {
        /// <summary>
        /// 点の位置を取得または設定します。
        /// </summary>
        public Vector2 Location { get; set; }
        private const float Allowable = 0.02f;

        public bool Hit(Line line)
        {
            var line_length = (line.StartingPoint - line.TerminalPoint).Length();
            var to_line_length = (Location - line.StartingPoint).Length();
            var x_dot = (line.TerminalPoint.X - line.StartingPoint.X) * (Location.X - line.StartingPoint.X);
            var y_dot = (line.TerminalPoint.Y - line.StartingPoint.Y) * (Location.Y - line.StartingPoint.Y);
            bool face_same_direction = x_dot + y_dot - Allowable <= line_length * to_line_length && x_dot + y_dot + Allowable >= line_length * to_line_length;
            bool on_the_line = line_length >= to_line_length;
            return face_same_direction && on_the_line;
        }

        public bool Hit(Point point)
        {
            bool x_hit = point.Location.X - Allowable <= Location.X && point.Location.X + Allowable >= Location.X;
            bool y_hit = point.Location.Y - Allowable <= Location.Y && point.Location.Y + Allowable >= Location.Y;
            return x_hit && y_hit;
        }

        public bool Hit(Circle circle)
        {
            return Math.Pow((circle.Center.X - Location.X), 2) + Math.Pow((circle.Center.Y - Location.Y), 2) <= Math.Pow(circle.Radius, 2);
        }

#if DEBUG
        public Point()
        {
            this.ModelAsset = AssetFactory.ModelFactory.CreateBasicModel(AssetFactory.ModelType.Box);
        }
#endif

        public override void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            _position = new Vector3(Location.X, Location.Y, 0.0f);
            _scale.X = 0.5f;
            _scale.Y = 0.5f;
            base.Draw3D(dev);
        }
    }

    public class Circle : Object.Base.Model, IShape
    {
        /// <summary>
        /// 円の中心を取得または設定します。
        /// </summary>
        public Vector2 Center { get; set; }
        /// <summary>
        /// 円の半径を取得または設定します。
        /// </summary>
        public float Radius { get; set; }

#if DEBUG
        public Circle()
        {
            this.ModelAsset = AssetFactory.ModelFactory.CreateBasicModel(AssetFactory.ModelType.Sphere);
        }
#endif

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
            return Math.Pow((Center.X - point.Location.X), 2) + Math.Pow((Center.Y - point.Location.Y), 2) <= Math.Pow(Radius, 2);
        }

        public bool Hit(Circle circle)
        {
            return Math.Pow((circle.Center.X - Center.X), 2) + Math.Pow((circle.Center.Y - Center.Y), 2) <= Math.Pow(circle.Radius + Radius, 2);
        }

        public override void Draw3D(SlimDX.Direct3D9.Device dev)
        {
            _position = new Vector3(Center.X, Center.Y, 0.0f);
            base.Draw3D(dev);
        }
    }

}
