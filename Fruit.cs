using System;
using System.Collections.Generic;

namespace Snake
{
    class Fruit
    {
        Random random = new Random();
        public Point Location { get; set; } = new Point
        {
            X = Form1.CircleDiameter * new Random().Next(0, Form1.FormWidth / Form1.CircleDiameter),
            Y = Form1.CircleDiameter * new Random().Next(0, Form1.FormHeight / Form1.CircleDiameter)
        };

        public void Create()
        {
            Location.X = Form1.CircleDiameter * random.Next(0, Form1.FormWidth / Form1.CircleDiameter);
            Location.Y = Form1.CircleDiameter * random.Next(0, Form1.FormHeight / Form1.CircleDiameter);
        }
    }
}
