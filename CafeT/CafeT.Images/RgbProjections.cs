namespace EyeOpen.Imaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents RGB projections, horizontal and vertical.
    /// </summary>
    public class RgbProjections
    {
        private readonly double[] horizontalProjection;
        private readonly double[] verticalProjection;

        internal RgbProjections(double[][] projections)
            : this(projections[0], projections[1])
        {
        }

        internal RgbProjections(double[] horizontalProjection, double[] verticalProjection)
        {
            this.horizontalProjection = horizontalProjection;
            this.verticalProjection = verticalProjection;
        }

        public double[] HorizontalProjection
        {
            get 
            { 
                return horizontalProjection; 
            }            
        }

        public double[] VerticalProjection
        {
            get 
            { 
                return verticalProjection; 
            }            
        }

        /// <summary>
        /// Calculate the similarity between two RGB projections, horizontal and vertical.
        /// </summary>
        /// <param name="compare">The RGB projection to compare with.</param>
        /// <returns>Return the max similarity value betweem horizontal and vertical RGB projections.</returns>
        public double CalculateSimilarity(RgbProjections compare) 
        {
            var horizontalSimilarity = CalculateProjectionSimilarity(horizontalProjection, compare.horizontalProjection);
            var verticalSimilarity = CalculateProjectionSimilarity(verticalProjection, compare.verticalProjection);
            return Math.Max(horizontalSimilarity, verticalSimilarity);            
        }

        /// <summary>
        /// Calculate the similarity to another RGB projection.
        /// </summary>
        /// <param name="source">The source RGB projection.</param>
        /// <param name="compare">The RGB projection to compare with.</param>
        /// <returns>Return a value from 0 to 1 that is the similarity.</returns>
        private static double CalculateProjectionSimilarity(double[] source, double[] compare)
        {
            if (source.Length != compare.Length)
            {
                throw new ArgumentException();
            }

            var frequencies = new Dictionary<double, int>();

            ////Calculate frequencies
            for (var i = 0; i < source.Length; i++) 
            {
                var difference = source[i] - compare[i];
                difference = Math.Round(difference, 2);
                difference = Math.Abs(difference);
                if (frequencies.ContainsKey(difference))
                {
                    frequencies[difference] = frequencies[difference] + 1;
                }
                else
                {
                    frequencies.Add(difference, 1);
                }
            }

            var deviation = frequencies.Sum(value => (value.Key * value.Value));

            ////Calculate "weighted mean"
            ////http://en.wikipedia.org/wiki/Weighted_mean
            deviation /= source.Length;
            
            ////Maximize scale
            deviation = (0.5 - deviation) * 2;

            return deviation;
        }
    }
}