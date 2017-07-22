using System;
using System.Drawing;

namespace HundredsGame
{
	class Circle
	{
		public Rectangle MyCircle { get; set; }
		public static int BallCount = 0;

		// Container is the object that the Circle is within (generally the form/border)
		Rectangle Container { get; set; }

		public static Random RandGen = new Random();

		//calculate the radius from the diameter
		public float Radius
		{
			get
			{
				return MyCircle.Width / 2;
			}

		}

		Color randColor;

		public int Score { get; set; }
		double tempscore;

		public bool CursorInsideCircle { get; set; }

		// the center coordinate of the circle
		public Point Center
		{
			get
			{
				return new Point((int)(MyCircle.X + Radius), (int)(MyCircle.Y + Radius));
			}
		}

		// x and y move speeds
		public int XSpeed { get; set; }
		public int YSpeed { get; set; }

		//public Circle(int x, int y, int diameter, Random rand, int speed = 1)
		public Circle(int x, int y, int diameter, int speed = 1)
		{
			CursorInsideCircle = false;
			tempscore = 0;
			Score = 0;
			Container = Rectangle.Empty;
			MyCircle = new Rectangle(x, y, diameter, diameter);
			XSpeed = YSpeed = speed;
			Move();
			// create a random rgb color
			randColor = Color.FromArgb(RandGen.Next(256), RandGen.Next(256), RandGen.Next(256));
			//Radius = diameter / 2;
			Console.WriteLine($"Ball #{++BallCount} at {Center.ToString()}");
		}

		// swap the current ball x and y speeds with the provided ball's speed
		public void SwapSpeeds(Circle c)
		{
			int tempx = this.XSpeed;
			int tempy = this.YSpeed;

			this.YSpeed = c.YSpeed;
			this.XSpeed = c.XSpeed;

			c.YSpeed = tempy;
			c.XSpeed = tempx;
		}

		// return true if the circles are colliding
		public bool IsColliding(Circle c)
		{
			// if the square of the radii is larger than the distance, then the two circles are inside each other
			float xdiff = this.Center.X - c.Center.X;
			float ydiff = this.Center.Y - c.Center.Y;

			float sumRadius = this.Radius + c.Radius;
			float sqrRadius = sumRadius * sumRadius;

			float distSquare = (xdiff * xdiff) + (ydiff * ydiff);

			if (distSquare <= sqrRadius)
			{
				return true;
			}
			return false;
		}

		// Move the circle 
		public void Move()
		{
			// check the left/right/top/bottom bounds
			// reverse ball direction if it goes past any of them
			if (MyCircle.Right > Container.Right)
			{
				XSpeed = -Math.Abs(XSpeed);
			}
			else if (MyCircle.Left < Container.Left)
			{
				XSpeed = Math.Abs(XSpeed);
			}
			else if (MyCircle.Top < Container.Top)
			{
				YSpeed = Math.Abs(YSpeed);
			}
			else if (MyCircle.Bottom > Container.Bottom)
			{
				YSpeed = -Math.Abs(YSpeed);
			}

			// set the circles position to new point
			Point newLocation = new Point(MyCircle.Location.X + XSpeed, MyCircle.Location.Y + YSpeed);
			MyCircle = new Rectangle(newLocation, MyCircle.Size);
		}

		// The container will be the border generally
		public void SetContainer(Border container)
		{
			Container = container.Rect;
		}

		// Increase the size of the ball if the cursor position is inside the circle position
		public void Grow(Point cursorLocation)
		{
			CursorInsideCircle = false;
			if (Container.Contains(MyCircle))
			{
				if (MyCircle.Contains(cursorLocation))
				{
					CursorInsideCircle = true;
					Size newSize = new Size(MyCircle.Size.Width + 1, MyCircle.Size.Height + 1);
					MyCircle = new Rectangle(MyCircle.Location, newSize);
					tempscore += 0.20;
					Score = (int)Math.Floor(tempscore);
				}
			}

		}

		// return true if the circle collides with another circle
		public bool RectangleCollision(Circle c)
		{
			return (c.MyCircle.IntersectsWith(this.MyCircle));
		}

		// draw the circle shape with the score in the center
		public void DrawCircle(Graphics g)
		{
			g.FillEllipse(new SolidBrush(randColor), MyCircle);
			g.DrawString(Score.ToString(), new Font("Helvetica", 14), new SolidBrush(Color.Yellow), Center);

		}
	}
}
