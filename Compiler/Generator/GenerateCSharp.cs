﻿using System;
using System.Linq;
using System.Reflection;

namespace Evans.XamlTemplates.Generator
{
    public class GenerateCSharp
    {
        public string AssemblyName { get; set; } = Assembly.GetCallingAssembly().GetName().Name;

        private string CSharpTemplate => @"
//WARNING! THIS CODE IS GENERATED BY XAML TEMPLATES.  DO NOT CHANGE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
" + GenerateUsingStatements() + @"
namespace " + AssemblyName + @"
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class " + Template?.ClassName + @": ContentView
    {
" + GenerateBindableProperties() + @"
        public " + Template?.ClassName + @"()
        {
            InitializeComponent();
" + GenerateConstructor() + @"
        }
" + GenerateProperties() + @"
    }
}";

        private string GenerateUsingStatements()
        {
            //if (Template?.Body == null) return "";

            //var assemblies = Template.Body.GetAllAssemblies().Where(p => p.Contains("clr-namespace:"));
            //var str = "";
            //foreach (var assembly in assemblies)
            //{
            //    str += $"using {assembly.Split(':')[1]};{Environment.NewLine}";
            //}
            //return str;
            return "";
        }

        string GetNamespace(string namespaceXml)
        {
            if (namespaceXml.Contains("clr-namespace:"))
                return namespaceXml.Replace("{","").Replace("}","").Split(':',';')[1] + ".";
            return "";
        }

        private string GenerateConstructor()
        {
            if (Template == null) return "";
            if (NameGenerator == null) return "";
            var str = "";
            foreach (var control in NameGenerator.NamedControls)
            {
                str += $"            {control.Key}.BindingContext = this;{Environment.NewLine}";

                foreach (var property in control.Value.ControlProperties.Where(p => p.IsParameter))
                {
                    str += $"            {control.Key}.SetBinding({GetNamespace(control.Value.Namespace)}{control.Value.Name.LocalName}.{property.Name}Property,nameof({property.Value.Substring(1)}));{Environment.NewLine}";
                }
            }

            foreach (var parameter in Template.Parameters.Where(p=>p.DefaultValue != null))
            {
                str += $"            {parameter.Name} = {parameter.DefaultValue};{Environment.NewLine}";
            }

            return str;
        }
        private string GenerateProperties()
        {
            if (Template == null) return "";
            var parameters = Template.Parameters;
            var str = "";

            foreach (var parameter in parameters)
            {
                str += @"        public " + parameter.Type + @" " + parameter.Name + @" { get => (" + parameter.Type + @")GetValue(" + parameter.Name + @"Property); set => SetValue(" + parameter.Name + @"Property, value); }
";
            }

            return str;
        }

        string GenerateBindableProperties()
        {
            if (Template == null) return "";
            var parameters = Template.Parameters;
            var str = "";

            foreach (var parameter in parameters)
            {
                str += $@"        public static BindableProperty {parameter.Name}Property = 
            BindableProperty.Create(nameof({parameter.Name}), typeof({parameter.Type}), typeof({Template?.ClassName}), default, BindingMode.TwoWay);
";
            }

            return str;
        }

        public Template? Template { get; set; }
        public NameGenerator? NameGenerator { get; set; }
        public GeneratedFile Generate(Template template, NameGenerator nameGenerator)
        {
            Template = template;
            NameGenerator = nameGenerator;
            var name = template.ClassName;
            var file = new GeneratedFile();

            file.FileName = name + ".xaml.cs";
            file.Content = CSharpTemplate;
            return file;
        }
    }
}