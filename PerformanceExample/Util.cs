namespace PerformanceExample
{
    public static class Util
    {
        /// <summary>
        /// Determines the minimum of the specified parameters.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The minimum of the parameters.</returns>
        /// <remarks>
        /// We could (should) use Math.Min instead, but it does an additional check for NaN which we don't have the time for.
        /// See https://stackoverflow.com/a/30740585/6748306
        /// </remarks>
        public static double Min(double left, double right) => left < right ? left : right;

        /// <summary>
        /// Determines the maximum of the specified parameters.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The maximum of the parameters.</returns>
        /// <remarks>
        /// We could (should) use Math.Max instead, but it does an additional check for NaN which we don't have the time for.
        /// See https://stackoverflow.com/a/30740585/6748306
        /// </remarks>
        public static double Max(double left, double right) => left > right ? left : right;
    }
}
