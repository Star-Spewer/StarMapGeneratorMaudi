using System.Numerics;

namespace StarMapGeneratorMaudi
{
    /// <summary>
    /// Methods to generate a list of normal distributed Vector3 objects
    /// </summary>
    public static class NormalDistributedGenerator
    {
        /// <summary>
        /// Optimized by just using a single list instead of multiple layers.
        /// </summary>
        /// <param name="numOfStars">The number of stars you want to generate</param>
        /// <param name="minDistance">Minimum distance between points</param>
        /// <param name="minCenterDistance">Minimum distance between points and (0,0,0)</param>
        /// <param name="size">Standard deviation used for distribution</param>
        /// <param name="thickness">Axial compression along the z-axis</param>
        public static List<Vector3> GeneratorSmall(
                int numOfStars,
                float minDistance,
                float minCenterDistance,
                float size,
                float thickness)
        {
            List<Vector3> list = new()
            {
                new Vector3(0, 0, 0)
            };
            RandomGaussDistributor rand = new();
            for (int i = 0; i < numOfStars; i++)
            {
                float curMinDistance = 0;
                Vector3 curPoint;
                do
                {
                    curPoint = new Vector3(rand.GiveNumbers(3)) * size;
                    curPoint.Z *= thickness;
                    if (curPoint.Length() < minCenterDistance)
                        continue;
                    curMinDistance = 2 * minDistance;
                    float holder = list.AsParallel()
                        .Select(vector => Vector3.Distance(vector, curPoint))
                        .Min();
                    curMinDistance = Math.Min(curMinDistance, holder);

                } while (curMinDistance < minDistance);
                list.Add(curPoint);
            }
            list.RemoveAt(0);
            return list;
        }
        /// <summary>
        /// Optimized by sorting stars into smaller lists based on minCenterDistance and minDistance.
        /// </summary>
        /// <param name="numOfStars">The number of stars you want to generate</param>
        /// <param name="minDistance">Minimum distance between points</param>
        /// <param name="minCenterDistance">Minimum distance between points and (0,0,0)</param>
        /// <param name="size">Standard deviation used for distribution</param>
        /// <param name="thickness">Axial compression along the z-axis</param>
        public static List<Vector3> GeneratorBig(
                int numOfStars,
                float minDistance,
                float minCenterDistance,
                float size,
                float thickness)
        {
            List<List<Vector3>> layeredList = new();

            int layers = (int)((size * 5 - minCenterDistance) / minDistance);
            for (int i = 0; i < layers; i++) { layeredList.Add(new List<Vector3>()); }
            RandomGaussDistributor rand = new();
            for (int i = 0; i < numOfStars; i++)
            {
                float curMinDistance = 0;
                Vector3 curPoint;
                int layer = 0;

                do
                {
                    curPoint = new Vector3(rand.GiveNumbers(3)) * size;
                    curPoint.Z *= thickness;
                    if (curPoint.Length() < minCenterDistance)
                        continue;
                    curMinDistance = 2 * minDistance;
                    layer = Math.Max(0, Math.Min(layers - 1, (int)((curPoint.Length() - minCenterDistance) / minDistance)));

                    for (int j = Math.Max(layer - 1, 0); j <= Math.Min(layer + 1, layers - 1); j++)
                    {
                        if (layeredList[j].Count > 0)
                        {
                            float holder = layeredList[j].AsParallel()
                                .Select(vector => Vector3.Distance(vector, curPoint))
                                .Min();
                            curMinDistance = Math.Min(curMinDistance, holder);
                        }
                        //Console.WriteLine($"\r{i} {layer}");
                    }
                } while (curMinDistance < minDistance);
                layeredList[layer].Add(curPoint);
            }

            return layeredList.SelectMany(list => list).ToList();
        }
        /// <summary>
        /// No constraints like minimum distance to each other or the center.
        /// </summary>
        /// <param name="numOfStars">The number of stars you want to generate</param>
        /// <param name="size">Standard deviation used for distribution</param>
        /// <param name="thickness">Axial compression along the z-axis</param>
        /// <returns></returns>
        public static List<Vector3> GeneratorUnconstrained(int numOfStars, float size, float thickness)
        {
            Vector3[] list = new Vector3[numOfStars];
            var rand = new RandomGaussDistributor();
            for (int i = 0; i < numOfStars; i++)
            {
                Vector3 curPoint = new Vector3(rand.GiveNumbers(3)) * size;
                curPoint.Z *= thickness;
                list[i] = curPoint;
            }

            return list.ToList();
        }
    }
}
