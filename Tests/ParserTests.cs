﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Evans.XamlTemplates;
using Evans.XamlTemplates.Generator;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ParserTests
    {
        string code = @"
@LabelEntry(string label,string Text)
{
	<StackLayout>
		<Label Text=""@label""/>
		<Entry Text=""@Text""/>
	</StackLayout>
}
";

        string codeMissingBracket = @"
@LabelEntry(string label,string Text)
{
	<StackLayout>
		<Label Text=""@label""/>
		<Entry Text=""@Text""/>
	</StackLayout>
";


        [Test]
        public void GetTokens()
        {
            var tokens = parser.GetTokens(code);
            tokens.Should().NotBeEmpty();
        }

        string codeMissingParenthesis = @"
@LabelEntry(string label,string Text
{
	<StackLayout>
		<Label Text=""@label""/>
		<Entry Text=""@Text""/>
	</StackLayout>
}
";

        private string codeMultipleTypes = @"
@LabelEntry(string label,string Text)
{
	<StackLayout>
		<Entry Text=""@Text""/>
		<Entry Text=""@Text""/>
		<Entry Text=""@Text""/>
	</StackLayout>
}";

        Templator templator = new Templator();

       [Test]
        public void MultipleControlTypes_Should_ChangeName()
        {
            var result = templator.Generate(codeMultipleTypes, "test").First();

            result.CSharp.Content.Should().Contain("_Entry");
            result.Xaml.Content.Should().Contain("_Entry");

            result.CSharp.Content.Should().Contain("_Entry1");
            result.Xaml.Content.Should().Contain("_Entry1");
            result.CSharp.Content.Should().Contain("_Entry2");
            result.Xaml.Content.Should().Contain("_Entry2");
        }


        Generator gen = new Generator();

        [Test]
        public void Ast_MissingParenthesis_Should_ThrowException()
        {
            var tokens = parser.GetTokens(codeMissingParenthesis);
            var program = Assert.Throws<CompileException>(() => tamlAst.Evaluate(tokens));
        }

        [Test]
        public void Ast_MissingBracket_Should_ThrowException()
        {
            var tokens = parser.GetTokens(codeMissingBracket);
            var program = Assert.Throws<CompileException>(()=> tamlAst.Evaluate(tokens));
        }
        TamlParser parser = new TamlParser();

        TamlAst tamlAst = new TamlAst();
        [Test]
        public void GenerateTree()
        {
            var tokens = parser.GetTokens(code);
            var program = tamlAst.Evaluate(tokens);
        }

        [Test]
        public void xProp_Should_GenerateTree()
        {
            var parser = new TamlParser();

            var tamlAst = new TamlAst();

            string code = @"
@LabelEntry(string label,string Text)
{
    <Label 
        xmlns:x=""http://schemas.microsoft.com/winfx/2009/xaml"" 
        x:Name=""test""/>
}";

            var tokens = parser.GetTokens(code);

            var program = tamlAst.Evaluate(tokens);

            program.Templates.First().Body.Controls.First().ControlProperties.Last().Name.Should().Be("x:Name");

        }



        [Test]
        public void GenerateFiles()
        {


            var tokens = parser.GetTokens(code);

            var program = tamlAst.Evaluate(tokens);

            var result = gen.Generate(program);

            result.First().Xaml.FileName.Should().Be("LabelEntry.xaml");
            result.First().CSharp.FileName.Should().Be("LabelEntry.xaml.cs");

            
            Console.WriteLine(result.First().Xaml.Content);
            Console.WriteLine(result.First().CSharp.Content);
        }

        private string advanced = @"
@EntryAndPicker(string label,string Text, IEnumerable<string> Data, string SelectedItem)
{
<StackLayout>
    <Label Text=""@label""/>
    <Entry Text=""@Text""/>
    <Label Text=""Result:""/>
    <Label Text=""@Text""/>
    <Picker ItemsSource=""@Data"" SelectedItem=""@SelectedItem""/>
    <Label Text=""@SelectedItem""/>
</StackLayout>
}";
        [Test]
        public void GenerateFiles_Advanced()
        {
            var result = GeneratedTypes();

            result.First().Xaml.FileName.Should().Be("EntryAndPicker.xaml");
            result.First().CSharp.FileName.Should().Be("EntryAndPicker.xaml.cs");


            Console.WriteLine(result.First().Xaml.Content);
            Console.WriteLine(result.First().CSharp.Content);
        }

        private IEnumerable<GeneratedType> GeneratedTypes()
        {
            var parser = new TamlParser();

            var tamlAst = new TamlAst();

            var gen = new Generator();

            gen.Namespace = "Evans.XamlTemplates";

            var tokens = parser.GetTokens(advanced);

            var program = tamlAst.Evaluate(tokens);

            var result = gen.Generate(program);
            return result;
        }

        [Test]
        public void WriteToFile()
        {
            var path = @"..\..\..\..\Evans.XamlTemplates\Evans.XamlTemplates";
            var result = GeneratedTypes();

            foreach (var generatedType in result)
            {
                File.WriteAllText(Path.Combine(path,generatedType.CSharp.FileName), generatedType.CSharp.Content);
                File.WriteAllText(Path.Combine(path,generatedType.Xaml.FileName), generatedType.Xaml.Content);
            }
        }
    }

    
}