
//WARNING! THIS CODE IS GENERATED BY XAML TEMPLATES.  DO NOT CHANGE
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
    public partial class DataGridSection: ContentView
    {
        public static BindableProperty HeaderTextProperty = 
            BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(DataGridSection), default, BindingMode.TwoWay);
        public static BindableProperty DataProperty = 
            BindableProperty.Create(nameof(Data), typeof(IEnumerable<object>), typeof(DataGridSection), default, BindingMode.TwoWay);

        public DataGridSection()
        {
            InitializeComponent();
            _Header.BindingContext = this;
            _Header.SetBinding(Evans.XamlTemplates.Header.TextProperty,nameof(HeaderText));
            _SfDataGrid.BindingContext = this;
            _SfDataGrid.SetBinding(Syncfusion.SfDataGrid.XForms.SfDataGrid.ItemsSourceProperty,nameof(Data));

        }
        public string HeaderText { get => (string)GetValue(HeaderTextProperty); set => SetValue(HeaderTextProperty, value); }
        public IEnumerable<object> Data { get => (IEnumerable<object>)GetValue(DataProperty); set => SetValue(DataProperty, value); }

    }
}