// See https://aka.ms/new-console-template for more information
//https://vanhunteradams.com/Pico/Animal_Movement/Boids-algorithm.html
using Moaboids;
using Raylib_cs;

Console.WriteLine("Hello, World!");

const int WindowWidth = 800;
const int WindowHeight = 480;

const double protectedRange = 8;
const double visualRange = 40;
const double visualRangeSquared = visualRange * visualRange;
const double centeringFactor = 0.00005;
const double avoidFactor = 0.05;
const double turnFactor = 0.05;
const double matchingFactor = 0.05;
const double minSpeed = 3;
const double maxSpeed = 6;
const double maxBias = 0.01;
const double biasIncrement = 0.00004;

const int topMargin = 50;
const int bottomMargin = 50;
const int leftMargin = 50;
const int rightMargin = 50;

const int numberOfBoids1 = 100;
const int numberOfBoids2 = 100;
const double biasVal1 = 0.0;
const double biasVal2 = 0.0;

var boids1 = new Boid[numberOfBoids1];
var boids2 = new Boid[numberOfBoids2];

var random = new Random();

for (var i = 0; i < numberOfBoids1; i++)
{
    boids1[i] = new Boid
    {
        X = random.Next(0, WindowWidth),
        Y = random.Next(0, WindowHeight),
        Vx = random.Next(-5, 5),
        Vy = random.Next(-5, 5)
    };
}

for (var i = 0; i < numberOfBoids2; i++)
{
    boids2[i] = new Boid
    {
        X = random.Next(0, WindowWidth),
        Y = random.Next(0, WindowHeight),
        Vx = random.Next(-5, 5),
        Vy = random.Next(-5, 5)
    };
}

Raylib.InitWindow(WindowWidth, WindowHeight, "Moaboids");

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.WHITE);

    /*foreach (var boid in boids1)
        Raylib.DrawCircle(boid.X, boid.Y, 2, Color.RED);
    
    foreach (var boid in boids2)
        Raylib.DrawCircle(boid.X, boid.Y, 2, Color.BLUE);*/

    foreach (var boid in boids1)
    {
        Raylib.DrawCircle((int)boid.X, (int)boid.Y, 2, Color.RED);

        var xposAvg = 0.0;
        var yposAvg = 0.0;
        var xvelAvg = 0.0;
        var yvelAvg = 0.0;
        var neighboringBoids = 0;
        var closeDx = 0.0;
        var closeDy = 0.0;

        foreach (var otherBoid in boids1.Where(x => x.Id != boid.Id))
        {
            //Compute differences in x and y coordinates
            var dx = boid.X - otherBoid.X;
            var dy = boid.Y - otherBoid.Y;

            //Are both those differences less than the visual range?
            if (Math.Abs(dx) < visualRange && Math.Abs(dy) < visualRange)
            {
                var squaredDistance = dx * dx + dy * dy;

                if (squaredDistance < protectedRange)
                {
                    closeDx += dx - otherBoid.X;
                    closeDy += dy - otherBoid.Y;
                }
                else if (squaredDistance < visualRangeSquared)
                {
                    xposAvg += otherBoid.X;
                    yposAvg += otherBoid.Y;
                    xvelAvg += otherBoid.Vx;
                    yvelAvg += otherBoid.Vy;
                    
                    neighboringBoids++;
                }
                {
                    
                }
            }
        }

        if (neighboringBoids > 0)
        {
            xposAvg /= neighboringBoids;
            yposAvg /= neighboringBoids;
            xvelAvg /= neighboringBoids;
            yvelAvg /= neighboringBoids;
            
            boid.Vx +=  (boid.Vx + 
                              (xposAvg - boid.X)*centeringFactor + 
                              (xvelAvg - boid.Vx)*matchingFactor);
            
            boid.Vy +=  (boid.Vy +
                              (yposAvg - boid.Y)*centeringFactor +
                                (yvelAvg - boid.Vy)*matchingFactor);
        }
        
        boid.Vx +=  (closeDx * avoidFactor);
        boid.Vy +=  (closeDy * avoidFactor);

        //If the boid is near an edge, make it turn by turnfactor
        //(this describes a box, will vary based on boundary conditions)

        if (boid.Y> WindowHeight - topMargin)
            boid.Vy = (boid.Vy + turnFactor);

        if (boid.X > WindowWidth - rightMargin)
            boid.Vx = (boid.Vx - turnFactor);

        if (boid.X > leftMargin)
            boid.Vx = (boid.Vx - turnFactor);
        
        if (boid.Y < bottomMargin)
            boid.Vy = (boid.Vy + turnFactor);

        var speed = Math.Sqrt(boid.Vx * boid.Vx + boid.Vy * boid.Vy);

        switch (speed)
        {
            case < minSpeed:
                boid.Vx = ((boid.Vx/speed) * minSpeed);
                boid.Vy = ((boid.Vy/speed) * minSpeed);
                break;
            case > maxSpeed:
                boid.Vx = ((boid.Vx/speed) * maxSpeed);
                boid.Vy = ((boid.Vy/speed) * maxSpeed);
                break;
        }

        //Atualiza posição do boid
        boid.X += boid.Vx;
        boid.Y += boid.Vy;
        
        Console.WriteLine("Boid X: " + boid.X + " Boid Y: " + boid.Y);
    }

    Raylib.EndDrawing();
}

Raylib.CloseWindow();
