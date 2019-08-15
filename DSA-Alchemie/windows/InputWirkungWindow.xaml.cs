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

namespace DSA_Alchemie.window
{
    /// <summary>
    /// Interaktionslogik für InputWirkung.xaml
    /// </summary>
    public partial class InputWirkungWindow : Window
    {
        public string[] Wirkung { private set; get; } = new string[7];
        public InputWirkungWindow()
        {
            InitializeComponent();
        }
        public static RoutedCommand Exit_RoutedCommand = new RoutedCommand();
        private void Exit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Exit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Wirkung[0] = M_IN.Text;
            Wirkung[1] = A_IN.Text;
            Wirkung[2] = B_IN.Text;
            Wirkung[3] = C_IN.Text;
            Wirkung[4] = D_IN.Text;
            Wirkung[5] = E_IN.Text;
            Wirkung[6] = F_IN.Text;
            this.Close();
        }
    }
}
