using DnDAlignmentVisualization.Core;
using DnDAlignmentVisualization.Utils;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;

namespace DnDAlignmentVisualization.Rendering
{
    public class FRTRenderer
    {
        private readonly RenderWindow _window;
        private readonly GridRenderer _gridRenderer;
        private bool _showLabels = false;
        private Font _font;

        public FRTRenderer(RenderWindow window, GridRenderer gridRenderer)
        {
            _window = window;
            _gridRenderer = gridRenderer;
            try
            {
                _font = new Font("arial.ttf");
            }
            catch
            {
                _font = null;
            }
        }

        public void ToggleLabels(bool show)
        {
            _showLabels = show;
        }

        public void DrawFRTPoints(List<FRTPoint> frtPoints, FRTPoint activeFRT)
        {
            foreach (var frt in frtPoints)
            {
                DrawFRTPoint(frt, frt == activeFRT);
            }
        }

        private void DrawFRTPoint(FRTPoint frt, bool isActive)
        {
            var screenPos = _gridRenderer.WorldToScreen(frt.Position);

            var toleranceCircle = new CircleShape(frt.ToleranceRadius * GridRenderer.CellSize)
            {
                Position = new Vector2f(
                    screenPos.X - frt.ToleranceRadius * GridRenderer.CellSize,
                    screenPos.Y - frt.ToleranceRadius * GridRenderer.CellSize
                ),
                FillColor = ColorUtils.GetTransparentColor(ColorUtils.GetColorForFRT(frt.Name)),
                OutlineColor = Color.Black,
                OutlineThickness = 1f
            };
            _window.Draw(toleranceCircle);

            var frtCircle = new CircleShape(8f)
            {
                Position = new Vector2f(screenPos.X - 8, screenPos.Y - 8),
                FillColor = ColorUtils.GetColorForFRT(frt.Name),
                OutlineColor = isActive ? Color.Red : Color.Black,
                OutlineThickness = isActive ? 3f : 2f
            };
            _window.Draw(frtCircle);

            // Подпись
            if (_showLabels && _font != null)
            {
                string label = GetShortLabel(frt.Name);
                Text text = new Text(label, _font, 12);
                text.FillColor = Color.Black;
                text.Position = new Vector2f(screenPos.X + 10, screenPos.Y - 6);
                _window.Draw(text);
            }
        }

        private string GetShortLabel(string name)
        {
            switch (name)
            {
                case "Lawful Good": return "LG";
                case "Lawful Neutral": return "LN";
                case "Lawful Evil": return "LE";
                case "Neutral Good": return "NG";
                case "True Neutral": return "TN";
                case "Neutral Evil": return "NE";
                case "Chaotic Good": return "CG";
                case "Chaotic Neutral": return "CN";
                case "Chaotic Evil": return "CE";
                default: return "";
            }
        }

        public void DrawPlayer(Player player)
        {
            var screenPos = _gridRenderer.WorldToScreen(player.Position);

            var playerCircle = new CircleShape(6f)
            {
                Position = new Vector2f(screenPos.X - 6, screenPos.Y - 6),
                FillColor = Color.Black,
                OutlineColor = Color.White,
                OutlineThickness = 2f
            };
            _window.Draw(playerCircle);
        }
    }
}