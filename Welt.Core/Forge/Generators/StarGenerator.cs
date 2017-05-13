using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Core.Forge.Generators
{
    public class StarGenerator
    {
        private const double L_SIZE = 14.08;
        public static char GetStarClassificationFromTemperature(int temp)
        {
            if (temp >= 30000) return 'O';
            if (temp >= 10000) return 'B';
            if (temp >= 7500) return 'A';
            if (temp >= 6000) return 'F';
            if (temp >= 5200) return 'G';
            if (temp >= 3700) return 'K';
            if (temp >= 2400) return 'M';
            throw new ArgumentOutOfRangeException(nameof(temp), "Temperature must be at least 2400");
        }

        private delegate Star StarGenerationCallback();
        private static Dictionary<double, StarGenerationCallback> m_RandomizedGen = new Dictionary<double, StarGenerationCallback>
        {
            [.7645] = GenerateMType,
            [.121] = GenerateKType,
            [.076] = GenerateGType,
            [.03] = GenerateFType,
            [.006] = GenerateAType,
            [.0013] = GenerateBType,
            [.0012] = GenerateOType
        };

        public static Star GenerateRandomStar()
        {
            var c = FastMath.NextRandomBetween(0, 1);
            var v = 0d;
            foreach (var val in m_RandomizedGen)
            {
                if (c >= v && c < val.Key + v)
                {
                    return val.Value();
                }
                else
                {
                    v += val.Key;
                }
            }
            // this should basically never happen unless our universe (real one) collapsed all laws of physics
            throw new Exception("ABORT. Seek shelter.");
        }

        /// <summary>
        ///     Generates an O Type star (blue [super]giant)
        /// </summary>
        /// <returns></returns>
        public static Star GenerateOType()
        {
            var name = FastMath.NextRandom(1000, 9999).ToString();
            var temp = FastMath.NextRandom(30000, 60000);
            var classification = $"O{GetClassification(30000, 60000, temp)}";
            var size = (int)(FastMath.NextRandomBetween(6.6f, 10) * L_SIZE);
            var color = FastMath.NextRandom(Color.SteelBlue, Color.Blue);
            return new Star($"{classification} {name}", temp, color, size);
        }

        /// <summary>
        ///     Generates a B Type star (white-blue [super]giant)
        /// </summary>
        /// <returns></returns>
        public static Star GenerateBType()
        {
            var name = FastMath.NextRandom(1000, 9999).ToString();
            var temp = FastMath.NextRandom(10000, 30000);
            var classification = $"B{GetClassification(10000, 30000, temp)}";
            var size = (int)(FastMath.NextRandomBetween(1.8f, 6.6f) * L_SIZE);
            var color = FastMath.NextRandom(Color.Blue, Color.DeepSkyBlue);
            return new Star($"{classification} {name}", temp, color, size);
        }

        /// <summary>
        ///     Generates an A Type star (white/blue main-sequence)
        /// </summary>
        /// <returns></returns>
        public static Star GenerateAType()
        {
            var name = FastMath.NextRandom(1000, 9999).ToString();
            var temp = FastMath.NextRandom(7500, 10000);
            var classification = $"A{GetClassification(7500, 10000, temp)}";
            var size = (int)(FastMath.NextRandomBetween(1.4f, 1.8f) * L_SIZE);
            var color = FastMath.NextRandom(Color.LightSkyBlue, Color.LightBlue);
            return new Star($"{classification} {name}", temp, color, size);
        }

        /// <summary>
        ///     Generates an F Type star (white main-sequence)
        /// </summary>
        /// <returns></returns>
        public static Star GenerateFType()
        {
            var name = FastMath.NextRandom(1000, 9999).ToString();
            var temp = FastMath.NextRandom(6000, 7500);
            var classification = $"F{GetClassification(6000, 7500, temp)}";
            var size = (int)(FastMath.NextRandomBetween(1.15f, 1.4f) * L_SIZE);
            var color = FastMath.NextRandom(Color.WhiteSmoke, Color.White);
            return new Star($"{classification} {name}", temp, color, size);
        }

        /// <summary>
        ///     Generates a G Type star (white-yellow supergiant)
        /// </summary>
        /// <returns></returns>
        public static Star GenerateGType()
        {
            var name = FastMath.NextRandom(1000, 9999).ToString();
            var temp = FastMath.NextRandom(5200, 6000);
            var classification = $"G{GetClassification(5200, 6000, temp)}";
            var size = (int)(FastMath.NextRandomBetween(0.96f, 1.15f) * L_SIZE);
            var color = FastMath.NextRandom(Color.LightYellow, Color.FloralWhite);
            return new Star($"{classification} {name}", temp, color, size);
        }

        /// <summary>
        ///     Generates a K Type star (yellow-orange main-sequence)
        /// </summary>
        /// <returns></returns>
        public static Star GenerateKType()
        {
            var name = FastMath.NextRandom(1000, 9999).ToString();
            var temp = FastMath.NextRandom(3700, 5200);
            var classification = $"K{GetClassification(3700, 5200, temp)}";
            var size = (int)(FastMath.NextRandomBetween(0.7f, 0.96f) * L_SIZE);
            var color = FastMath.NextRandom(Color.LightGoldenrodYellow, Color.LightYellow);
            return new Star($"{classification} {name}", temp, color, size);
        }

        /// <summary>
        ///     Generates an M Type star (light-orange-red main-sequence)
        /// </summary>
        /// <returns></returns>
        public static Star GenerateMType()
        {
            var name = FastMath.NextRandom(1000, 9999).ToString();
            var temp = FastMath.NextRandom(2400, 3700);
            var classification = $"M{GetClassification(2400, 3700, temp)}";
            var size = (int)(FastMath.NextRandomBetween(0.1f, 0.7f) * L_SIZE);
            var color = FastMath.NextRandom(Color.OrangeRed, Color.FromNonPremultiplied(255, 190, 50, 255));
            return new Star($"{classification} {name}", temp, color, size);
        }

        /// <summary>
        ///     Returns an int between 0 and 9 for a star type based on temperature, 0 being the coolest
        ///     and 9 being the hottest.
        /// </summary>
        /// <param name="tempMin"></param>
        /// <param name="tempMax"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public static int GetClassification(float tempMin, float tempMax, float temp)
        {
            return (int)((temp - tempMin) / ((tempMax - tempMin) / 10));
        }
    }
}
