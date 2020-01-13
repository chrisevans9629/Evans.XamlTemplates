
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
    public partial class CodeEditor: ContentView
    {
        public static BindableProperty CodeProperty = 
            BindableProperty.Create(nameof(Code), typeof(string), typeof(CodeEditor), default, BindingMode.TwoWay);

        public CodeEditor()
        {
            InitializeComponent();
            _Label.BindingContext = this;
            _Label.SetBinding(Label.TextProperty,nameof(Code));

        }
        public string Code { get => (string)GetValue(CodeProperty); set => SetValue(CodeProperty, value); }

    }
}