using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;



// TODO: If the mouse is inside the ball and there is collision, then game over

namespace HundredsGame
{
	public partial class Form1 : Form
	{
		const int NUMBALLS = 3;
		const int BALLSIZE = 50;

		Graphics g;
		Bitmap canvas;

		Border border;
		List<Circle> balls;

		Point cursorLocation;
		int collisionCount;
		bool currentlyColliding;

		public Form1()
		{
			InitializeComponent();
			g = this.CreateGraphics();
			canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			g = Graphics.FromImage(canvas);
		}

		void InitGame()
		{
			collisionCount = 0;
			currentlyColliding = false;
			cursorLocation = Point.Empty;
			border = new Border(0, 0, pictureBox1.Size.Width, pictureBox1.Size.Height);

			// initialize balls at random location and add balls to the list and set them to be within the form/border
			balls = new List<Circle>();
			for (int x = 0; x < NUMBALLS; x++)
			{
				int newx = Circle.RandGen.Next(50, pictureBox1.Width - 50);
				int newy = Circle.RandGen.Next(50, pictureBox1.Height - 50);
				balls.Add(new Circle(newx, newy, BALLSIZE));
				balls[x].SetContainer(border);
			}
			// if the balls that are spawned are inside each other, start the game again
			if (DidSpawnTrapped())
			{
				InitGame();
			}

		}


		private void Form1_Load(object sender, EventArgs e)
		{
			InitGame();
		}

		void UpdateCursorLocation()
		{
			cursorLocation = pictureBox1.PointToClient(Control.MousePosition);
			//this.Text = cursorLocation.ToString();
		}

		void CheckScore()
		{
			// sum up the scores of each balls and it adds up to hundred, game over
			int sum = 0;
			foreach (var ball in balls)
			{
				sum += ball.Score;
			}

			if (sum >= 100)
			{
				gameTimer.Stop();
				MessageBox.Show("Hundred!");
			}
		}

		void EraseDisplay()
		{
			g.Clear(Color.White);
		}

		bool DidSpawnTrapped()
		{
			for (int i = 0; i < balls.Count; i++)
			{
				for (int k = i + 1; k < balls.Count; k++)
				{
					if (balls[i].IsColliding(balls[k]))
					{
						return true;
					}
				}
			}

			return false;

		}
		void CheckCollision()
		{
			for (int i = 0; i < balls.Count; i++)
			{
				for (int k = i + 1; k < balls.Count; k++)
				{
					// draw a line from the center of one ball to another
					//g.DrawLine(new Pen(Color.Black, 3), balls[i].Center, balls[k].Center);

					if (balls[i].IsColliding(balls[k]))
					{
						currentlyColliding = true;

						if (balls[k].CursorInsideCircle || balls[i].CursorInsideCircle)
						{
							GameLost();
						}

						// count of collision
						Console.WriteLine($"collision {++collisionCount}");
						// swap the speeds of the two balls to make them bounce off
						balls[i].SwapSpeeds(balls[k]);

					}
					else
					{
						currentlyColliding = false;
					}
				}
			}

		}

		void GameLost()
		{
			gameTimer.Stop();
			MessageBox.Show("You lose!");
		}

		private void gameTimer_Tick(object sender, EventArgs e)
		{

			EraseDisplay();

			// win condition
			CheckScore();

			// update cursor location if within the bounds
			UpdateCursorLocation();

			//move ball to new position, grow ball if cursor inside and then draw it
			foreach (var ball in balls)
			{
				ball.Move();
				if (!currentlyColliding)
				{
					ball.Grow(cursorLocation);
				}

				ball.DrawCircle(g);
			}


			// Ball collisions
			CheckCollision();

			pictureBox1.Image = canvas;

		}

		void RestartGame()
		{
			InitGame();
			gameTimer.Start();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			// restart the game
			if (e.KeyCode == Keys.Space)
			{
				RestartGame();
			}
		}
	}
}
