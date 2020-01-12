
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Evans.XamlTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryAndPicker: ContentView
    {
        public static BindableProperty labelProperty = 
            BindableProperty.Create(nameof(label), typeof(string), typeof(EntryAndPicker), default, BindingMode.TwoWay);
        public static BindableProperty TextProperty = 
            BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryAndPicker), default, BindingMode.TwoWay);
        public static BindableProperty DataProperty = 
            BindableProperty.Create(nameof(Data), typeof(IEnumerable<string>), typeof(EntryAndPicker), default, BindingMode.TwoWay);
        public static BindableProperty SelectedItemProperty = 
            BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(EntryAndPicker), default, BindingMode.TwoWay);

        public EntryAndPicker()
        {
            InitializeComponent();
            _Label.BindingContext = this;
            _Label.SetBinding(Label.TextProperty,nameof(label));
            _Entry.BindingContext = this;
            _Entry.SetBinding(Entry.TextProperty,nameof(Text));
            _Label1.BindingContext = this;
            _Label1.SetBinding(Label.TextProperty,nameof(Text));
            _Picker.BindingContext = this;
            _Picker.SetBinding(Picker.ItemsSourceProperty,nameof(Data));
            _Picker.SetBinding(Picker.SelectedItemProperty,nameof(SelectedItem));
            _Label2.BindingContext = this;
            _Label2.SetBinding(Label.TextProperty,nameof(SelectedItem));

        }
        public string label { get => (string)GetValue(labelProperty); set => SetValue(labelProperty, value); }
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        public IEnumerable<string> Data { get => (IEnumerable<string>)GetValue(DataProperty); set => SetValue(DataProperty, value); }
        public string SelectedItem { get => (string)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }

    }
}