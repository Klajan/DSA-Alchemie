using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Alchemie.common
{
    public class Dice
    {
        public readonly int Min;
        public readonly int Max;
        private static readonly Random rnd = new Random();

        public Dice(int min, int max)
        {
            this.Min = min; this.Max = max;
        }
        
        public int Roll()
        {
            return rnd.Next(Min, Max + 1);
        }
    }
    public class DiceContainer : NotifyPropertyChanged
    {
        public readonly string Name;
        public readonly Dice Dice;
        public int Ammount;
        private List<int> diceList_;
        public List<int> DiceList
        {
            get => diceList_;
            set { diceList_ = value; RaisePropertyChange("DiceList"); }
        }


        public DiceContainer(string name, Dice dice, int ammount)
        {
            this.Name = name; this.Dice = dice; this.Ammount = ammount;
            DiceList = new List<int>(ammount);
            for (int i = 0; i < ammount; i++) { DiceList.Add(0); }
        }
        public DiceContainer(DiceContainer prev)
        {
            this.Name = prev.Name;
            this.Dice = prev.Dice;
            this.Ammount = prev.Ammount;
            this.DiceList = prev.DiceList;
        }

        public void Roll()
        {
            var tmp = new List<int>(Ammount); ;
            for (int i = 0; i < Ammount; i++ )
            {
                tmp.Add(Dice.Roll());
            }
            DiceList = tmp;
        }
    }
}
