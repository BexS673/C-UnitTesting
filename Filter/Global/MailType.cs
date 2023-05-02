﻿namespace Tracker.Global
{
    public record Mail
    {
        private double range;
        private double bearing;
        //private int mailId;

        //public int MailId
        //{
        //    get
        //    {
                      
        //        return mailId;
        //    }
        //    set
        //    {
        //        mailId++;
        //    }
        //}
        public double Range
        {
            get
            {
                return Math.Round(range, 6);
            }
            set
            {
                if (value >= 0 && value <= Types.Max)
                {
                    Math.Round(range, 6);
                    range = value;
                }
                //else raise an exception???
            }
        }
        public double Bearing
        {
            get
            {
                return Math.Round(bearing, 6);
            }
            set
            {
                Math.Round(bearing, 6);
                 bearing = value;
            }
        }

    }

}
