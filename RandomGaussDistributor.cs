namespace StarMapGeneratorMaudi
{
    /// <summary>
    /// A generator for random gauss distributed floats
    /// </summary>
    class RandomGaussDistributor
    {
        private Random rand;
        /// <summary>
        /// Generator uses clock for seed
        /// </summary>
        public RandomGaussDistributor() { rand = new Random(); }
        /// <summary>
        /// Generator uses the given seed
        /// </summary>
        /// <param name="seed"></param>
        public RandomGaussDistributor(int seed)
        {
            rand = new Random(seed);
        }
        /// <summary>
        /// generates the next standard distributed number
        /// </summary>
        /// <returns>float</returns>
        private float GenerateNumber()
        {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            float result = (float)randStdNormal;
            return result;
        }
        /// <summary>
        /// returns a standard distributed float
        /// </summary>
        /// <returns>float</returns>
        public float GiveNumber()
        {

            return GenerateNumber();
        }
        /// <summary>
        /// returns an array of standard distributed floats
        /// </summary>
        /// <returns>float[x]</returns>
        public float[] GiveNumbers(int x)
        {
            float[] answer = new float[x];
            for (int i = 0; i < x; i++)
            {
                answer[i] = GenerateNumber();
            }
            return answer;
        }
    }
}