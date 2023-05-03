using Tracker.Global;



namespace Tracker.T
{

    public class Filter
    {
        private bool isSmooth;
        public double time;
        private double[] Vector1 = new double[2];
        private double[] Vector2 = new double[2];
        private double filterTime;
        public enum Mode
        {
            initialise,
            normal,
            update
        }

        //vector should be of type tFloatType, which is float restricted to 6 digits

        private static Filter instance = null;
        private static readonly object padlock = new object();
        Filter() { }

        public static Filter Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Filter();
                    }
                    return instance;
                } 
            }
        }


        private struct TargetType
        {
           // private double Range;
            //private double Bearing;
            public double Distance;
            public double Angle;

        }

        private TargetType TargetData = new TargetType(); //should this be the case....

        private void Convert_Cartesian_To_Polar(in double x, in double y, out double angle, out double distance, in double scale)
        {
            double temp_x = x * scale;
            double temp_y = y * scale;

            distance = Math.Sqrt(Math.Pow(temp_x, 2) + Math.Pow(temp_y, 2));

            if (x >= -1.0 * Types.V && x <= Types.V)
            {
                if (y >= 0.0)
                {
                    angle = 0.0;
                }
                else
                {
                    angle = Math.PI;
                }
            }
            else
            {
                angle = Math.Atan(y / x);
            }

            while (angle < 0.0)
            {
                angle += 2.0 * Math.PI;
            }

            while (angle >= 2.0 * Math.PI)
            {
                angle -= 2.0 * Math.PI;
            }
        }
        public void Output(out Mail mail)//input is mailtype (how to write a record)????
        {
            double temp_x;
            double temp_y;
            mail = new Mail();

            //if (isSmooth == false)
            //{
                temp_x = Vector2[0];
                temp_y = Vector2[1];
                Convert_Cartesian_To_Polar(in temp_x, in temp_y, out TargetData.Angle, out TargetData.Distance, Types.KV);
                isSmooth = true;

                mail.Bearing = TargetData.Angle;
                mail.Range = TargetData.Distance;
            //}
                        
            

        }

        private void PositionUpdate(in double time, out double[] vector)
        {
            double[] temp_vector = new double[2];
            temp_vector = Matrices.MultiplyVectorScalar(time, Vector1);
            vector = Matrices.AddVector(temp_vector, Vector2);
        }

        public void CallPositionUpdate()
        {
            PositionUpdate(filterTime, out Vector2);
        }

        public void InitialiseData()
        {
            Vector1[0] = Types.ZeroMatrix[0, 0];
            Vector1[1] = Types.ZeroMatrix[0, 1]; //LINQ query it???
            Vector2[0] = Types.ZeroMatrix[0, 0];
            Vector2[1] = Types.ZeroMatrix[0, 1];

            isSmooth = false;
            filterTime = 0.0;

        }

        public void UpdateData(double t = 2.0, double v1 = 1.0, double v2 = 2.0 ) //should this be public or called through the UpdatePath method
        {
            filterTime += t;
            Vector1[0] += v1;
            Vector1[1] += v2;
        }
        public void UpdatePath(Mode mode)
        {
            switch (mode)
            {
                case Mode.initialise:
                    InitialiseData();
                    break;

                case Mode.update:
                    UpdateData(); //what about passing in ???
                    break;

                case Mode.normal:
                    break;
            }
        }
    }

}


