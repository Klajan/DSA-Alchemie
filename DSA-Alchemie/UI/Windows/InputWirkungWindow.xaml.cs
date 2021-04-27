using Alchemie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Alchemie.UI.Windows
{
    /// <summary>
    /// Interaktionslogik für InputWirkung.xaml
    /// </summary>
    public partial class InputWirkungWindow : Window
    {
        public Wirkung Wirkung { private set; get; }
        public InputWirkungWindow()
        {
            InitializeComponent();
        }
        public static RoutedCommand ExitCommand { private set; get; } = new RoutedCommand();
        private void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Wirkung = new Wirkung(M_IN.Text, A_IN.Text, B_IN.Text, C_IN.Text, D_IN.Text, E_IN.Text, F_IN.Text);
            this.Close();
        }
    }
}
