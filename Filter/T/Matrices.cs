namespace Tracker.T
{
    public class Matrices
    {

        public static double[] MultiplyVectorScalar(in double scalar, in double[] vector)
        {
            double[] temp_vector = new double[2];
            temp_vector[0] = scalar * vector[0]; //how to ensure that only types tfloattype are added to??
            temp_vector[1] = scalar * vector[1];

            return temp_vector;
        }

        public static double[] AddVector(in double[] v1, in double[] v2)
        {
            double[] temp_vector = new double[2];
            temp_vector[0] = v1[0] + v2[0];
            temp_vector[1] = v1[1] + v2[1];
            return temp_vector;

        }

        public static double[] SubtractVectors(in double[] v1, in double[] v2)
        {
            double[] temp_vector = new double[2];
            temp_vector[0] = v1[0] - v2[0];
            temp_vector[1] = v1[1] - v2[1];
            return temp_vector;

        }

        public static double[,] MatrixTranspose(in double[,] m1)
        {
            double[,] temp_matrix = new double[2, 2];
            temp_matrix[0, 0] = m1[0, 0];
            temp_matrix[0, 1] = m1[1, 0];
            temp_matrix[1, 0] = m1[0, 1];
            temp_matrix[1, 1] = m1[1, 1];
            return temp_matrix;
        }
    }
}
