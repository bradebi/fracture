namespace UTK.Math
{
    public static class Angle
    {
        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static double ToRadians(double angle)
        {
            return angle * (System.Math.PI / 180d);
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static double ToDegrees(double radians)
        {
            return radians * (180d / System.Math.PI);
        }

        /// <summary>
        /// Angle is in Degrees
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Point2d Rotate2D(Point2d origin, Point2d point, double angle)
        {
            angle = ToRadians(angle);

            Point2d offset = point - origin;
            Point2d rotatedPoint = Point2d.zero;

            //Rotate points
            rotatedPoint.x = offset.x* System.Math.Cos(angle) - offset.y * System.Math.Sin(angle);
            rotatedPoint.y = offset.y * System.Math.Cos(angle) + offset.x * System.Math.Sin(angle);

            return (rotatedPoint + origin);
        }
    }
}
