using SFML.Graphics;

namespace DnDAlignmentVisualization.Utils
{
    public static class ColorUtils
    {
        public static Color GetColorForAlignment(float x, float y)
        {
            return CustomColors.GetColorForAlignment(x, y);
        }

        public static Color GetTransparentColor(Color baseColor, byte alpha = 100)
        {
            return CustomColors.GetTransparentColor(baseColor, alpha);
        }

        public static Color GetColorForFRT(string frtName)
        {
            if (CustomColors.FRTColors.ContainsKey(frtName))
                return CustomColors.FRTColors[frtName];

            return Color.White;
        }
    }
}