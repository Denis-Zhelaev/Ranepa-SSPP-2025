using System.Collections.Generic;
using SFML.System;

namespace DnDAlignmentVisualization.Core
{
    public class AlignmentSystem
    {
        public List<FRTPoint> FRTPoints { get; private set; }
        public FRTPoint ActiveFRT { get; private set; }
        public Player Player { get; private set; }

        // УВЕЛИЧЕННЫЕ КОНСТАНТЫ ДЛЯ ЛУЧШЕЙ ВИДИМОСТИ
        private const float R = 3.0f; // Увеличил сопротивление в 10 раз
        private const float M = 5.0f; // Увеличил память в 5 раз

        public AlignmentSystem()
        {
            InitializeFRTPoints();
            Player = new Player(0, 0);
            ActiveFRT = FRTPoints[4]; // True Neutral (0, 0)
        }

        private void InitializeFRTPoints()
        {
            FRTPoints = new List<FRTPoint>
            {
                new FRTPoint(-100, 100, "Lawful Good"),
                new FRTPoint(0, 100, "Lawful Neutral"),
                new FRTPoint(100, 100, "Lawful Evil"),
                new FRTPoint(-100, 0, "Neutral Good"),
                new FRTPoint(0, 0, "True Neutral"),
                new FRTPoint(100, 0, "Neutral Evil"),
                new FRTPoint(-100, -100, "Chaotic Good"),
                new FRTPoint(0, -100, "Chaotic Neutral"),
                new FRTPoint(100, -100, "Chaotic Evil")
            };
        }

        public (Vector2f newPosition, Vector2f Dv, Vector2f Rv, Vector2f Mv) ProcessMoveCommand(int x, int y)
        {
            Vector2f decisionVector = new Vector2f(x, y);
            var result = CalculateNewPosition(decisionVector);
            Player.MoveTo(result.newPosition);
            UpdateActiveFRT();
            return result;
        }

        private (Vector2f newPosition, Vector2f Dv, Vector2f Rv, Vector2f Mv) CalculateNewPosition(Vector2f Dv)
        {
            Vector2f currentPos = Player.Position;
            Vector2f F = ActiveFRT.Position;

            Vector2f direction = new Vector2f(F.X - currentPos.X, F.Y - currentPos.Y);
            float distance = (float)System.Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            Vector2f Rv = new Vector2f(0, 0);
            Vector2f Mv = new Vector2f(0, 0);

            if (distance > 0)
            {
                Vector2f unitVector = new Vector2f(direction.X / distance, direction.Y / distance);


                float resistanceStrength = R * (1.0f - distance / 200.0f); 
                if (resistanceStrength < 0.1f) resistanceStrength = 0.1f; 

                Rv = new Vector2f(unitVector.X * resistanceStrength, unitVector.Y * resistanceStrength);

                float memoryStrength = M / (1 + distance * 0.5f); 

                if (Dv.X == 0 && Dv.Y == 0)
                {
                    memoryStrength *= 2.0f; 
                }

                Mv = new Vector2f(unitVector.X * memoryStrength, unitVector.Y * memoryStrength);
            }

            Vector2f totalMovement = Dv + Rv + Mv;
            float totalLength = (float)System.Math.Sqrt(totalMovement.X * totalMovement.X + totalMovement.Y * totalMovement.Y);
            if (totalLength > 50.0f) 
            {
                totalMovement = totalMovement / totalLength * 50.0f;
            }

            Vector2f newPos = new Vector2f(
                currentPos.X + totalMovement.X,
                currentPos.Y + totalMovement.Y
            );

            return (newPos, Dv, Rv, Mv);
        }

        private void UpdateActiveFRT()
        {
            foreach (var frt in FRTPoints)
            {
                if (frt.IsInTolerance(Player.Position))
                {
                    if (ActiveFRT != frt)
                    {
                        ActiveFRT = frt;
                        System.Console.WriteLine($"Активная ФРТ изменена на: {frt.Name}");
                    }
                    return;
                }
            }
        }

        public void Update()
        {
            Player.Update();
            UpdateActiveFRT();
        }
    }
}