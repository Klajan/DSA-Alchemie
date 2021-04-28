using System;

namespace Alchemie.Models
{
    public class Dice
    {
        private readonly int min;
        private readonly int max;
        private static readonly Random rnd = new Random();

        public int Min => min;

        public int Max => max;

        public Dice(int min, int max)
        {
            this.min = min; this.max = max;
        }

        public int Roll()
        {
            return rnd.Next(Min, Max + 1);
        }
        public void Roll(ref int[] array)
        {
            if (array == null) return;
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Roll();
            }
        }
        public int[] Roll(int amount)
        {
            int[] arr = new int[amount];
            for (int i = 0; i < amount; i++)
            {
                arr[i] = Roll();
            }
            return arr;
        }
    }
}
