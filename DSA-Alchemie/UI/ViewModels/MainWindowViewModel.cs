﻿using Alchemie.Core;
using Alchemie.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        private Rezept rezept_ = new Rezept();

        public delegate void RezeptChangedHandler(object sender, Rezept newRezept);

        public event RezeptChangedHandler OnRezeptChanged;

        #endregion Members

        public RezeptViewModel TestRezepte { get; set; }

        private readonly ExtendedObserableCollection<string> rezepte_ = new ExtendedObserableCollection<string>();

        public ExtendedObserableCollection<string> Rezepte
        {
            get { return rezepte_; }
        }

        private readonly ExtendedObserableCollection<string> gruppen_ = new ExtendedObserableCollection<string>();

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
                List<string> filtered = app_.RezepteDB.RezepteGruppen[selectedGruppe_];
                rezepte_.Clear();
                rezepte_.AddRange(filtered);
                SelectedRezept = filtered[0];
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
                Rezept rezept = app_.RezepteDB.Rezepte[selectedRezept_];
                if (app_.Trank != null && !app_.Trank.IsSameBase(rezept))
                {
                    app_.Trank.Rezept = rezept;
                    rezept_ = rezept;
                    Seite = rezept_.Seite;

                    OnRezeptChanged.Invoke(this, rezept);
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

        public void Attach_Rezepte(List<string> gruppen, List<string> rezepte)
        {
            gruppen_.AddRange(gruppen);
            rezepte_.AddRange(rezepte);
            SelectedGruppe = gruppen[0];
            SelectedRezept = rezepte[0];
        }
    }
}