using SFML.Graphics;
using System.Collections.Generic;

namespace DnDAlignmentVisualization.Utils
{
    public static class CustomColors
    {
        // Базовые цвета для осей
        private static readonly Color GoodColor = new Color(0, 255, 0);     // Зеленый
        private static readonly Color EvilColor = new Color(255, 0, 0);     // Красный
        private static readonly Color NeutralColor = new Color(128, 128, 128); // Серый

        // Яркости для разных уровней законопослушности
        private static readonly float LawfulBrightness = 1.0f;    // Яркий
        private static readonly float NeutralBrightness = 0.7f;   // Средний
        private static readonly float ChaoticBrightness = 0.4f;   // Темный

        public static Dictionary<string, Color> FRTColors = new Dictionary<string, Color>
        {
            // Lawful (яркие)
            ["Lawful Good"] = ApplyBrightness(GoodColor, LawfulBrightness),      
            ["Lawful Neutral"] = ApplyBrightness(NeutralColor, LawfulBrightness),
            ["Lawful Evil"] = ApplyBrightness(EvilColor, LawfulBrightness),      

            // Neutral (средняя яркость)
            ["Neutral Good"] = ApplyBrightness(GoodColor, NeutralBrightness),    
            ["True Neutral"] = ApplyBrightness(NeutralColor, NeutralBrightness), 
            ["Neutral Evil"] = ApplyBrightness(EvilColor, NeutralBrightness),    

            // Chaotic (темные)
            ["Chaotic Good"] = ApplyBrightness(GoodColor, ChaoticBrightness),     
            ["Chaotic Neutral"] = ApplyBrightness(NeutralColor, ChaoticBrightness),
            ["Chaotic Evil"] = ApplyBrightness(EvilColor, ChaoticBrightness)     
        };

        private static Color ApplyBrightness(Color baseColor, float brightness)
        {
            return new Color(
                (byte)(baseColor.R * brightness),
                (byte)(baseColor.G * brightness),
                (byte)(baseColor.B * brightness)
            );
        }

        public static Color GetColorForAlignment(float x, float y)
        {
            Color moralColor;
            if (x < -33) moralColor = GoodColor;
            else if (x > 33) moralColor = EvilColor;
            else moralColor = NeutralColor;

            float brightness;
            if (y > 33) brightness = LawfulBrightness;      
            else if (y < -33) brightness = ChaoticBrightness; 
            else brightness = NeutralBrightness;             

            return ApplyBrightness(moralColor, brightness);
        }

        public static Color GetTransparentColor(Color baseColor, byte alpha = 100)
        {
            return new Color(baseColor.R, baseColor.G, baseColor.B, alpha);
        }
    }
}