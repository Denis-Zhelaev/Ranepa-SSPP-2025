using SFML.Graphics;
using DnDAlignmentVisualization.Core;

namespace DnDAlignmentVisualization.Rendering
{
    public class AlignmentRenderer
    {
        private readonly RenderWindow _window;
        private readonly GridRenderer _gridRenderer;
        private readonly FRTRenderer _frtRenderer;

        public AlignmentRenderer(RenderWindow window)
        {
            _window = window;
            _gridRenderer = new GridRenderer(window);
            _frtRenderer = new FRTRenderer(window, _gridRenderer);
        }

        public FRTRenderer GetFRTRenderer()
        {
            return _frtRenderer;
        }

        public void Draw(AlignmentSystem alignmentSystem)
        {
            _window.Clear(Color.White);

            _gridRenderer.Draw();
            _frtRenderer.DrawFRTPoints(alignmentSystem.FRTPoints, alignmentSystem.ActiveFRT);
            _frtRenderer.DrawPlayer(alignmentSystem.Player);

            _window.Display();
        }
    }
}