using System.Collections.Generic;

namespace Snake
{
    class Snake
    {
        public List<Point> Location = new List<Point>()
        {
            new Point { X = 2 * Form1.CircleDiameter, Y = 3 * Form1.CircleDiameter },
            new Point { X = 1 * Form1.CircleDiameter, Y = 3 * Form1.CircleDiameter },
            new Point { X = 0, Y = 3 * Form1.CircleDiameter }
        };

        public List<TailPoint> TailLocation = new List<TailPoint>();
        public int TailLength { get; set; } = 0;
        public int Length { get; set; } = 3;
        public Directions Direction { get; set; } = Directions.Right;

        public void MoveSnake()
        {
            if (Direction == Directions.Left)
                Location.Insert(0, new Point { X = Location[0].X - Form1.CircleDiameter, Y = Location[0].Y });
            if (Direction == Directions.Right)
                Location.Insert(0, new Point { X = Location[0].X + Form1.CircleDiameter, Y = Location[0].Y });
            if (Direction == Directions.Up)
                Location.Insert(0, new Point { X = Location[0].X, Y = Location[0].Y - Form1.CircleDiameter });
            if (Direction == Directions.Down)
                Location.Insert(0, new Point { X = Location[0].X, Y = Location[0].Y + Form1.CircleDiameter });

            for (int i = Length - 1; i < 0; i--)
                Location.Insert(i, Location[i - 1]);
        }

        public void CheckGrowSnake()
        {
            bool flag = false;
            TailPoint tailPoint = new TailPoint();
            foreach (TailPoint element in TailLocation)
            {
                element.LengthToAppear--;
                if (element.LengthToAppear == 0)
                {
                    Length++;
                    Location.Add(new Point { X = element.X, Y = element.Y });
                    flag = true;
                    tailPoint = element;
                    
                }
            }
            if (flag)
                TailLocation.Remove(tailPoint);
        }

        public void Default()
        {
            Location.Clear();
            TailLocation.Clear();
            Length = 3;
            TailLength = 0;

            Location.Add(new Point { X = 2 * Form1.CircleDiameter, Y = 3 * Form1.CircleDiameter });
            Location.Add(new Point { X = 1 * Form1.CircleDiameter, Y = 3 * Form1.CircleDiameter });
            Location.Add(new Point { X = 0, Y = 3 * Form1.CircleDiameter });

            Direction = Directions.Right;
        }
    }
}
