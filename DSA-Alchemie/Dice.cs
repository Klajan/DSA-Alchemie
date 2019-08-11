using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA_Alchemie
{
    public class Dice
    {
        public readonly int Min;
        public readonly int Max;
        private readonly Random rnd;

        public Dice(int min, int max, ref Random rnd)
        {
            this.Min = min; this.Max = max; this.rnd = rnd;
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
            get { return diceList_; }
            set { diceList_ = value; RaisePropertyChange("DiceList"); }
        }


        public DiceContainer(string name, Dice dice, int ammount)
        {
            this.Name = name; this.Dice = dice; this.Ammount = ammount;
            DiceList = new List<int>(ammount);
            for (int i = 0; i < ammount; i++) { DiceList.Add(0); }
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
