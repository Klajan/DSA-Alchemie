using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Alchemie.UI
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
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false, PropertyChangedCallback_));

        public Visibility ButtonVisibility
        {
            get { return (Visibility)this.GetValue(ButtonVisibilityProperty); }
            set { this.SetValue(ButtonVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ButtonVisibilityProperty =
            DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(NumericUpDown), new PropertyMetadata(Visibility.Visible, PropertyChangedCallback_));

        private int value_;

        public int Value
        {
            get { return (int)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0, ValuePropertyChangedCallback_));

        public int Max
        {
            get { return (int)this.GetValue(MaxProperty); }
            set { this.SetValue(MaxProperty, value); }
        }

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(int), typeof(NumericUpDown), new PropertyMetadata(Int32.MaxValue, MinMaxPropertyChangedCallback_));

        public int Min
        {
            get { return (int)this.GetValue(MinProperty); }
            set { this.SetValue(MinProperty, value); }
        }

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(int), typeof(NumericUpDown), new PropertyMetadata(Int32.MinValue, MinMaxPropertyChangedCallback_));

        #endregion DependencyProperties

        public Func<int, int> IncreaseFunc { set; get; } = (value) => value + 1;
        public Func<int, int> DecreaseFunc { set; get; } = (value) => value - 1;
        public bool AllowCopyPaste { set; get; } = true;
        public bool NotifyOnValueChanged { set; get; } = false;

        #endregion Properties

        private readonly Regex _regexFull = new Regex("([-]?[0-9]+)");
        private readonly Regex _regexQuick = new Regex("^[-+]");

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

        private static void ValuePropertyChangedCallback_(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown s = sender as NumericUpDown;
            s.value_ = Math.Max(s.Min, Math.Min(s.Max, (int)e.NewValue));

            //var carret = s.textBox.CaretIndex;

            s._handleTextChanged = false;
            s.textBox.Text = s.value_.ToString(CultureInfo.CurrentCulture);
            s._handleTextChanged = true;

            //s.textBox.CaretIndex = carret;
            s.textBox.CaretIndex = s.textBox.Text.Length;
            if (s != null) { s.OnChanged(e); }
        }

        private static void MinMaxPropertyChangedCallback_(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown s = sender as NumericUpDown;
            s.Value = Math.Max(s.Min, Math.Min(s.Max, s.Value));
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
            string text = origin.Text;
            Match _matchQuick = _regexQuick.Match(text);
            if (text.Length == 0) { origin.Text = ""; }
            else if (text.Length == 1 && _matchQuick.Success) { origin.Text = _matchQuick.Value; }
            else
            {
                if (Int64.TryParse(text, out long value))
                {
                    textBox.Text = value_.ToString(CultureInfo.CurrentCulture);
                    Value = (int)Math.Max(Math.Min(value, Max), Min);
                }
                else { origin.Text = value_.ToString(CultureInfo.CurrentCulture); }
            }

            _handleTextChanged = true;
        }

        #endregion CallbackMethods

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            if (IncreaseFunc != null)
            {
                Value = IncreaseFunc(value_);
            }
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            if (DecreaseFunc != null)
            {
                Value = DecreaseFunc(value_);
            }
        }
    }

    #region Converters

    internal class VisibilityToColumnConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Collapsed) { return new GridLength(0); }
            else { return new GridLength(1, GridUnitType.Star); }
        }

        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class VisibilityToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Collapsed) { return (double)0; }
            else { return (double)10; }
        }

        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class BoolInverterConverter : IValueConverter
    {
        public object Convert(object value, Type type, object paramater, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type type, object paramater, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    #endregion Converters
}