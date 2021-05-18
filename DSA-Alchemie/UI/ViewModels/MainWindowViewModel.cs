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
            _app = Application.Current as App;
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

        private readonly App _app;
        private Rezept _rezept = new();

        public delegate void RezeptChangedHandler(object sender, Rezept newRezept);

        public event RezeptChangedHandler OnRezeptChanged;

        #endregion Members

        public RezeptViewModel TestRezepte { get; set; }

        private readonly ExtendedObserableCollection<string> _rezepte = new();

        public ExtendedObserableCollection<string> Rezepte
        {
            get { return _rezepte; }
        }

        private readonly ExtendedObserableCollection<string> _gruppen = new();

        public ExtendedObserableCollection<string> Gruppen
        {
            get { return _gruppen; }
        }

        private string _selectedGruppe;

        public string SelectedGruppe
        {
            get { return _selectedGruppe; }
            set
            {
                if (SetValue(ref _selectedGruppe, value))
                {
                    var rezeptGruppe = _rezept?.Gruppe;
                    var oldRezept = _selectedRezept;
                    var filtered = _app.RezepteDB.RezepteGruppen[_selectedGruppe];
                    _rezepte.RebuildWithRange(filtered);
                    SelectedRezept = rezeptGruppe == _selectedGruppe ? oldRezept : filtered[0];
                }
            }
        }

        private string _selectedRezept;

        public string SelectedRezept
        {
            get { return _selectedRezept; }
            set
            {
                if (SetValue(ref _selectedRezept, value))
                {
                    _rezept = value == null ? null : _app.RezepteDB.Rezepte[_selectedRezept];
                    if (_rezept != null && _app.Trank != null && !_rezept.Equals(_app.Trank.Rezept))
                    {
                        Seite = _rezept.Seite;
                        _app.Trank.Rezept = _rezept;
                        OnRezeptChanged.Invoke(this, _rezept);
                    }
                }
            }
        }

        private int _seite;

        public int Seite
        {
            get => _seite;
            set => SetValue(ref _seite, value);
        }

        public void Attach_Rezepte(Database DB)
        {
            Attach_Rezepte(DB.Gruppen, DB.RezepteGruppen[DB.AllKey]);
        }

        public void Attach_Rezepte(IReadOnlyList<string> gruppen, IReadOnlyList<string> rezepte)
        {
            _gruppen.AddRange(gruppen);
            _rezepte.AddRange(rezepte);
            SelectedGruppe = gruppen[0];
            SelectedRezept = rezepte[0];
            Seite = (int)_rezept?.Seite;
        }
    }
}