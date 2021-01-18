using System;

namespace Alchemie.common
{
    public class Character : NotifyPropertyChanged
    {
        private int MU_ = 10;
        private int KL_ = 10;
        private int FF_ = 10;
        private int IN_ = 10;
        private int alchemie_ = 5;
        private int kochen_ = 5;
        private int labor_ = 5;
        private int laborQuality_ = 0;
        private bool allegorischeAnalyse_ = false;
        private bool chymischeHochzeit_ = false;
        private bool mandriconsBindung_ = false;

        //Properties begin here

        public int MU { get { return MU_; } set { MU_ = Math.Max(value, 1); RaisePropertyChange("MU"); } }
        
        public int KL { get { return KL_; } set { KL_ = Math.Max(value, 1); RaisePropertyChange("KL"); } }
        
        public int FF { get { return FF_; } set { FF_ = Math.Max(value, 1); RaisePropertyChange("FF"); } }
        
        public int IN { get { return IN_; } set { IN_ = Math.Max(value, 1); RaisePropertyChange("IN"); } }
        
        public int Alchemie { get { return alchemie_; } set { alchemie_ = value; RaisePropertyChange("Alchemie"); } }
        
        public int Kochen { get { return kochen_; } set { kochen_ = value; RaisePropertyChange("Kochen"); } }
        
        public int Labor { get { return labor_; } set { labor_ = Math.Max(Math.Min(value, 2), 0); RaisePropertyChange("Labor"); } }
        
        public int LaborQuality { get { return laborQuality_; } set { laborQuality_ = Math.Max(Math.Min(value, +3), -7); RaisePropertyChange("LaborQuality"); } }
        
        public bool AllegorischeAnalyse { get => allegorischeAnalyse_; set { allegorischeAnalyse_ = value; RaisePropertyChange("AllegorischeAnalyse"); } }
        
        public bool ChymischeHochzeit { get => chymischeHochzeit_; set { chymischeHochzeit_ = value; RaisePropertyChange("ChymischeHochzeit"); } }
        
        public bool MandriconsBindung { get => mandriconsBindung_; set { mandriconsBindung_ = value; RaisePropertyChange("MandriconsBindung"); } }
        
        //Functions begin here

        public Character()
        {
            
        }
    }
}
