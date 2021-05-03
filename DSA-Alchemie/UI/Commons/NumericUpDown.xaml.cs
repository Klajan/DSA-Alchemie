using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Alchemie.UI.Commons
{
#pragma warning disable IDE0038 // Use pattern matching

    /// <summary>
    /// Interaktionslogik für NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnChanged(DependencyPropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.Property.Name));
        }

        private static void PropertyChangedCallback_(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is NumericUpDown)
            {
                ((NumericUpDown)sender).OnChanged(e);
            }
        }

        #region Properties

        #region DependencyProperties

        public bool IsReadOnly
        {
            get { return (bool)this.GetValue(IsReadOnlyProperty); }
            set { this.SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false, PropertyChangedCallback_));

        public Visibility ButtonVisibility
        {
            get { return (Visibility)this.GetValue(ButtonVisibilityProperty); }
            set { this.SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ButtonVisibilityProperty =
            DependencyProperty.Register(nameof(ButtonVisibility), typeof(Visibility), typeof(NumericUpDown), new PropertyMetadata(Visibility.Visible, PropertyChangedCallback_));

        private int intvalue_;

        [Browsable(true)]
        public int IntValue
        {
            get { return (int)this.GetValue(IntValueProperty); }
            set { this.SetValue(IntValueProperty, value); }
        }

        public static readonly DependencyProperty IntValueProperty =
            DependencyProperty.Register(nameof(IntValue), typeof(int), typeof(NumericUpDown), new PropertyMetadata(0, IntValuePropertyChangedCallback_));

        public int Max
        {
            get { return (int)this.GetValue(MaxProperty); }
            set { this.SetValue(MaxProperty, value); }
        }

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register(nameof(Max), typeof(int), typeof(NumericUpDown), new PropertyMetadata(Int32.MaxValue, MinMaxPropertyChangedCallback_));

        public int Min
        {
            get { return (int)this.GetValue(MinProperty); }
            set { this.SetValue(MinProperty, value); }
        }

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register(nameof(Min), typeof(int), typeof(NumericUpDown), new PropertyMetadata(Int32.MinValue, MinMaxPropertyChangedCallback_));

        #endregion DependencyProperties

        public Func<int, int> IncreaseFunc { set; get; } = (value) => value + 1;
        public Func<int, int> DecreaseFunc { set; get; } = (value) => value - 1;
        public bool AllowCopyPaste { set; get; } = true;
        public bool NotifyOnValueChanged { set; get; }

        #endregion Properties

        //private readonly Regex _regexFull = new Regex("([-]?[0-9]+)");
        private readonly Regex _regexQuick = new ("^[-+]");

        private bool _handleTextChanged = true;

        #region Construction

        public NumericUpDown()
        {
            InitializeComponent();
        }

        public NumericUpDown(Func<int, int> Increase, Func<int, int> Decrease)
        {
            this.IncreaseFunc = Increase;
            this.DecreaseFunc = Decrease;
            InitializeComponent();
        }

        #endregion Construction

        #region CallbackMethods

        private static void IntValuePropertyChangedCallback_(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown s = sender as NumericUpDown;
            s.intvalue_ = Math.Max(s.Min, Math.Min(s.Max, (int)e.NewValue));

            //var carret = s.textBox.CaretIndex;

            s._handleTextChanged = false;
            s.TextBox.Text = s.intvalue_.ToString(CultureInfo.CurrentCulture);
            s._handleTextChanged = true;

            //s.textBox.CaretIndex = carret;
            s.TextBox.CaretIndex = s.TextBox.Text.Length;
            if (s != null) { s.OnChanged(e); }
        }

        private static void MinMaxPropertyChangedCallback_(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown s = sender as NumericUpDown;
            s.IntValue = Math.Max(s.Min, Math.Min(s.Max, s.IntValue));
            if (s != null) { s.OnChanged(e); }
        }

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!AllowCopyPaste &&
                (e.Command == ApplicationCommands.Paste ||
                 e.Command == ApplicationCommands.Copy ||
                 e.Command == ApplicationCommands.Copy)) { e.Handled = true; }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_handleTextChanged) return;

            _handleTextChanged = false;
            TextBox origin = sender as TextBox;
            e.Handled = true;
            if (origin.Text.Length == 0) { origin.Text = ""; }
            else
            {
                Match _matchQuick = _regexQuick.Match(origin.Text);
                if (origin.Text.Length == 1 && _matchQuick.Success) { origin.Text = _matchQuick.Value; }
                else if (Int32.TryParse(origin.Text, out int value))
                {
                    IntValue = value;
                }
                else { origin.Text = intvalue_.ToString(CultureInfo.CurrentCulture); }
            }
            _handleTextChanged = true;
        }

        #endregion CallbackMethods

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (IncreaseFunc != null)
            {
                IntValue = IncreaseFunc(intvalue_);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (DecreaseFunc != null)
            {
                IntValue = DecreaseFunc(intvalue_);
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }

    #region Converters

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class VisibilityToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Collapsed) { return new GridLength(0); }
            else { return new GridLength(1, GridUnitType.Star); }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class VisibilityToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Collapsed) { return (double)0; }
            else { return (double)10; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1812:Avoid uninstantiated internal classes", Justification = "instantiated in xaml")]
    internal class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    #endregion Converters
}