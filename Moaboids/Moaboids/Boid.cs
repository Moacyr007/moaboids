namespace Moaboids;

public class Boid
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int X { get; set; }
    public int Y { get; set; }
    public int Vx { get; set; }
    public int Vy { get; set; }
}