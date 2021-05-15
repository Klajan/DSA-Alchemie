using System.Windows;
using System.Windows.Input;

namespace Alchemie.UI.Windows
{
    /// <summary>
    /// Interaktionslogik für InputWirkung.xaml
    /// </summary>
    public partial class InputWirkungWindow : Window
    {
        public Models.Types.Wirkung Wirkung { private set; get; }

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
            Wirkung = new(M_IN.Text, A_IN.Text, B_IN.Text, C_IN.Text, D_IN.Text, E_IN.Text, F_IN.Text);
            this.Close();
        }
    }
}