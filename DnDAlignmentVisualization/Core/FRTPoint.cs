using SFML.System;

namespace DnDAlignmentVisualization.Core
{
    public class FRTPoint
    {
        public Vector2f Position { get; set; }
        public string Name { get; set; }
        public float ToleranceRadius { get; set; } = 15f;

        public FRTPoint(float x, float y, string name)
        {
            Position = new Vector2f(x, y);
            Name = name;
        }

        public bool IsInTolerance(Vector2f point)
        {
            float dx = point.X - Position.X;
            float dy = point.Y - Position.Y;
            return dx * dx + dy * dy <= ToleranceRadius * ToleranceRadius;
        }
    }
}