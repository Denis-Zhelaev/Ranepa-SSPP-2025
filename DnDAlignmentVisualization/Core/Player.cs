using SFML.System;

namespace DnDAlignmentVisualization.Core
{
    public class Player
    {
        public Vector2f Position { get; set; }
        public Vector2f TargetPosition { get; set; }
        public float MovementSpeed { get; set; } = 2f;

        public bool IsMoving
        {
            get
            {
                return Position.X != TargetPosition.X || Position.Y != TargetPosition.Y;
            }
        }

        public Player(float startX, float startY)
        {
            Position = new Vector2f(startX, startY);
            TargetPosition = Position;
        }

        public void Update()
        {
            if (IsMoving)
            {
                Vector2f direction = new Vector2f(
                    TargetPosition.X - Position.X,
                    TargetPosition.Y - Position.Y
                );

                float distance = (float)System.Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

                if (distance <= MovementSpeed)
                {
                    Position = TargetPosition;
                }
                else
                {
                    Position = new Vector2f(
                        Position.X + direction.X / distance * MovementSpeed,
                        Position.Y + direction.Y / distance * MovementSpeed
                    );
                }
            }
        }

        public void MoveTo(Vector2f newPosition)
        {
            float x = newPosition.X;
            float y = newPosition.Y;

            if (x < -100) x = -100;
            if (x > 100) x = 100;
            if (y < -100) y = -100;
            if (y > 100) y = 100;

            TargetPosition = new Vector2f(x, y);
        }
    }
}