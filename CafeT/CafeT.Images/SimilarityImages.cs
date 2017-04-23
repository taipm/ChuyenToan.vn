namespace EyeOpen.Imaging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a comparable images class.
    /// </summary>
    public class SimilarityImages : IComparer<SimilarityImages>, IComparable
    {
        private readonly ComparableImage source;
        private readonly ComparableImage destination;
        private readonly double similarity;

        public SimilarityImages(ComparableImage source, ComparableImage destination, double similarity)
        {
            this.source = source;
            this.destination = destination;
            this.similarity = similarity;
        }

        public ComparableImage Source
        {
            get
            {
                return source;
            }
        }

        public ComparableImage Destination
        {
            get
            {
                return destination;
            }
        }

        public double Similarity
        {
            get
            {
                return Math.Round(similarity * 100, 1);
            }
        }

        public static int operator !=(SimilarityImages value, SimilarityImages compare)
        {
            return value.CompareTo(compare);
        }

        public static int operator <(SimilarityImages value, SimilarityImages compare)
        {
            return value.CompareTo(compare);
        }

        public static int operator ==(SimilarityImages value, SimilarityImages compare)
        {
            return value.CompareTo(compare);
        }

        public static int operator >(SimilarityImages value, SimilarityImages compare)
        {
            return value.CompareTo(compare);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1} --> {2}", source.File.Name, destination.File.Name, similarity);
        }

        #region IComparer<SimilarityImages> Members

        public int Compare(SimilarityImages x, SimilarityImages y)
        {
            return x.similarity.CompareTo(y.similarity);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            SimilarityImages other = (SimilarityImages)obj;
            return this.Compare(this, other);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (SimilarityImages)obj;

            var equals = Source.File.FullName.Equals(other.Source.File.FullName, StringComparison.InvariantCultureIgnoreCase);

            if (!equals)
            {
                return false;
            }

            equals = Destination.File.FullName.Equals(other.Destination.File.FullName, StringComparison.InvariantCultureIgnoreCase);
            
            if (!equals)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return string.Format("{0};{1}", Source.File.FullName, Destination.File.FullName).GetHashCode();
        }
    }
}