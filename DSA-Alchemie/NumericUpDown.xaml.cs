using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DSA_Alchemie
{
    /// <summary>
    /// Interaktionslogik für NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        //style Logic here
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnChanged(DependencyPropertyChangedEventArgs e)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(e.Property.Name)); }
        }
        private static void PropertyChangedCallback_(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown s = sender as NumericUpDown;
            if (s != null) { s.OnChanged(e); }
        }
        //private Visibility buttonvisibility_;
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
        public bool NotifyOnValueChanged { set; get; } = false;
        //normal Code begins here
        private static void ValuePropertyChangedCallback_(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown s = sender as NumericUpDown;
            s.value_ = Math.Max(s.Min, Math.Min(s.Max, (int)e.NewValue));
            s.textBox.TextChanged -= s.TextBox_TextChanged;
            s.textBox.Text = s.value_.ToString();
            s.textBox.TextChanged += s.TextBox_TextChanged;
            s.textBox.CaretIndex = s.textBox.Text.Length;
            if (s != null) { s.OnChanged(e); }
        }
        private int value_;
        public int Value
        {
            get { return (int)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0, ValuePropertyChangedCallback_));
        private static void MinMaxPropertyChangedCallback_(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown s = sender as NumericUpDown;
            s.Value = Math.Max(s.Min, Math.Min(s.Max, s.Value));
            if (s != null) { s.OnChanged(e); }
        }
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
        public Func<int, int> IncreaseFunc { set; get; }
        public Func<int, int> DecreaseFunc { set; get; }
        public bool AllowCopyPaste = true;
        private readonly Regex regexFull = new Regex("([-]?[0-9]+)");
        private readonly Regex regexQuick = new Regex("^[-+]");
        bool success = false;
        public NumericUpDown()
        {
            InitializeComponent();
            //textBox.PreviewTextInput += NumberValidation;
        }
        public NumericUpDown(Func<int, int> Increase, Func<int, int> Decrease)
        {
            this.IncreaseFunc = Increase;
            this.DecreaseFunc = Decrease;
            InitializeComponent();
            //textBox.PreviewTextInput += NumberValidation;
        }
        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            success = (IncreaseFunc != null) ? Int32.TryParse(textBox.Text, out value_) : false;
            if (success)
            {
                Value = IncreaseFunc(value_);
            }
        }
        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            success = (DecreaseFunc != null) ? Int32.TryParse(textBox.Text, out value_) : false;
            if (success)
            {
                Value = DecreaseFunc(value_);
            }
        }
        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!AllowCopyPaste &&
                (e.Command == ApplicationCommands.Paste ||
                 e.Command == ApplicationCommands.Copy ||
                 e.Command == ApplicationCommands.Copy)) { e.Handled = true; }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox origin = sender as TextBox;
            e.Handled = true;
            long value;
            string text = origin.Text;
            origin.TextChanged -= TextBox_TextChanged;
            var matchQuick = regexQuick.Match(text);
            if (matchQuick.Success && text.Length == 1) { origin.Text = matchQuick.Value; }
            else if (text.Length == 0) { origin.Text = ""; }
            else
            {
                var matchFull = regexFull.Match(text);
                if (matchFull.Success)
                {
                    Int64.TryParse(matchFull.Value, out value);
                    textBox.Text = value_.ToString();
                    origin.TextChanged += TextBox_TextChanged;
                    Value = (int)Math.Max(Math.Min(value, Max), Min);
                }
                else { origin.Text = value_.ToString(); }
            }
            origin.TextChanged += TextBox_TextChanged;
        }
    }
    class VisibilityToColumnConverter : IValueConverter
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
    class VisibilityToWidthConverter : IValueConverter
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
}
