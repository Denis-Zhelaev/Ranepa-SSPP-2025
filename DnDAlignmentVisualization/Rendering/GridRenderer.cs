using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DnDAlignmentVisualization.Rendering
{
    public class GridRenderer
    {
        private readonly RenderWindow _window;
        public static int CellSize { get; } = 3;

        private readonly Color _gridColor = new Color(200, 200, 200);
        private readonly Color _axisColor = new Color(100, 100, 100);

        public GridRenderer(RenderWindow window)
        {
            _window = window;
        }

        public Vector2f WorldToScreen(Vector2f worldPos)
        {
            float centerX = _window.Size.X / 2f;
            float centerY = _window.Size.Y / 2f;

            return new Vector2f(
                centerX + worldPos.X * CellSize,
                centerY - worldPos.Y * CellSize
            );
        }

        public void Draw()
        {
            DrawGrid();
            DrawAxes();
        }

        private void DrawGrid()
        {
            float centerX = _window.Size.X / 2f;
            float centerY = _window.Size.Y / 2f;

            for (int x = -100; x <= 100; x += 10)
            {
                float screenX = centerX + x * CellSize;
                float thickness = (x % 50 == 0) ? 2f : 1f; 
                Color color = (x % 50 == 0) ? _axisColor : _gridColor;

                var line = new Vertex[]
                {
                    new Vertex(new Vector2f(screenX, 0), color),
                    new Vertex(new Vector2f(screenX, _window.Size.Y), color)
                };
                _window.Draw(line, PrimitiveType.Lines);
            }

            for (int y = -100; y <= 100; y += 10)
            {
                float screenY = centerY - y * CellSize;
                float thickness = (y % 50 == 0) ? 2f : 1f;
                Color color = (y % 50 == 0) ? _axisColor : _gridColor;

                var line = new Vertex[]
                {
                    new Vertex(new Vector2f(0, screenY), color),
                    new Vertex(new Vector2f(_window.Size.X, screenY), color)
                };
                _window.Draw(line, PrimitiveType.Lines);
            }
        }

        private void DrawAxes()
        {
            float centerX = _window.Size.X / 2f;
            float centerY = _window.Size.Y / 2f;

            var xAxis = new Vertex[]
            {
                new Vertex(new Vector2f(0, centerY), Color.Black),
                new Vertex(new Vector2f(_window.Size.X, centerY), Color.Black)
            };

            var yAxis = new Vertex[]
            {
                new Vertex(new Vector2f(centerX, 0), Color.Black),
                new Vertex(new Vector2f(centerX, _window.Size.Y), Color.Black)
            };

            _window.Draw(xAxis, PrimitiveType.Lines);
            _window.Draw(yAxis, PrimitiveType.Lines);

            DrawArrow(new Vector2f(_window.Size.X - 20, centerY - 10), new Vector2f(_window.Size.X, centerY), Color.Black);
            DrawArrow(new Vector2f(centerX - 10, 20), new Vector2f(centerX, 0), Color.Black);
        }

        private void DrawArrow(Vector2f p1, Vector2f p2, Color color)
        {
            var line = new Vertex[]
            {
                new Vertex(p1, color),
                new Vertex(p2, color)
            };
            _window.Draw(line, PrimitiveType.Lines);

            Vector2f direction = p2 - p1;
            float length = (float)System.Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            direction = direction / length;

            Vector2f perpendicular = new Vector2f(-direction.Y, direction.X);

            var arrowHead = new Vertex[]
            {
                new Vertex(p2, color),
                new Vertex(p2 - direction * 10 + perpendicular * 5, color),
                new Vertex(p2 - direction * 10 - perpendicular * 5, color),
                new Vertex(p2, color)
            };
            _window.Draw(arrowHead, PrimitiveType.LineStrip);
        }
    }
}