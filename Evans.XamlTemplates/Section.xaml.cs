
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
    public partial class Section: ContentView
    {
        public static BindableProperty HeaderProperty = 
            BindableProperty.Create(nameof(Header), typeof(string), typeof(Section), default, BindingMode.TwoWay);
        public static BindableProperty SectionContentProperty = 
            BindableProperty.Create(nameof(SectionContent), typeof(object), typeof(Section), default, BindingMode.TwoWay);

        public Section()
        {
            InitializeComponent();
            _Label.BindingContext = this;
            _Label.SetBinding(Label.TextProperty,nameof(Header));
            _ContentView.BindingContext = this;
            _ContentView.SetBinding(ContentView.ContentProperty,nameof(SectionContent));

        }
        public string Header { get => (string)GetValue(HeaderProperty); set => SetValue(HeaderProperty, value); }
        public object SectionContent { get => (object)GetValue(SectionContentProperty); set => SetValue(SectionContentProperty, value); }

    }
}