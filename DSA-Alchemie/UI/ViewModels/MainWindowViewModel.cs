using Alchemie.Core;
using Alchemie.Models;
using System.Collections.Generic;
using System.Windows;

namespace Alchemie.UI.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        #region Construction

        public MainWindowViewModel()
        {
            app_ = Application.Current as App;
        }

        public MainWindowViewModel(Database DB) : this()
        {
            Attach_Rezepte(DB);
        }

        public MainWindowViewModel(List<string> gruppen, List<string> rezepte) : this()
        {
            Attach_Rezepte(gruppen, rezepte);
        }

        #endregion Construction

        #region Members

        private readonly App app_;
        private Rezept rezept_ = new();

        public delegate void RezeptChangedHandler(object sender, Rezept newRezept);

        public event RezeptChangedHandler OnRezeptChanged;

        #endregion Members

        public RezeptViewModel TestRezepte { get; set; }

        private readonly ExtendedObserableCollection<string> rezepte_ = new();

        public ExtendedObserableCollection<string> Rezepte
        {
            get { return rezepte_; }
        }

        private readonly ExtendedObserableCollection<string> gruppen_ = new();

        public ExtendedObserableCollection<string> Gruppen
        {
            get { return gruppen_; }
        }

        private string selectedGruppe_;

        public string SelectedGruppe
        {
            get { return selectedGruppe_; }
            set
            {
                selectedGruppe_ = value;
                var rezeptGruppe = rezept_?.Gruppe;
                var oldRezept = selectedRezept_;
                var filtered = app_.RezepteDB.RezepteGruppen[selectedGruppe_];
                rezepte_.RebuildWithRange(filtered);
                SelectedRezept = rezeptGruppe == selectedGruppe_ ? oldRezept : filtered[0];
                RaisePropertyChange();
            }
        }

        private string selectedRezept_;

        public string SelectedRezept
        {
            get { return selectedRezept_; }
            set
            {
                selectedRezept_ = value;
                rezept_ = value == null ? null : app_.RezepteDB.Rezepte[selectedRezept_];
                if (rezept_ != null && app_.Trank != null && !rezept_.Equals(app_.Trank.Rezept))
                {
                    Seite = rezept_.Seite;
                    app_.Trank.Rezept = rezept_;
                    OnRezeptChanged.Invoke(this, rezept_);
                }
                RaisePropertyChange();
            }
        }

        private int seite_;

        public int Seite
        {
            get { return seite_; }
            set { seite_ = value; RaisePropertyChange(); }
        }

        public void Attach_Rezepte(Database DB)
        {
            Attach_Rezepte(DB.Gruppen, DB.RezepteGruppen[DB.AllKey]);
        }

        public void Attach_Rezepte(IReadOnlyList<string> gruppen, IReadOnlyList<string> rezepte)
        {
            gruppen_.AddRange(gruppen);
            rezepte_.AddRange(rezepte);
            SelectedGruppe = gruppen[0];
            SelectedRezept = rezepte[0];
            Seite = (int)rezept_?.Seite;
        }
    }
}