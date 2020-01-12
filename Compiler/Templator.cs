﻿using System;
using System.Collections.Generic;
using System.Linq;
using Evans.XamlTemplates.Generator;

namespace Evans.XamlTemplates
{
    public class Examples
    {
        public const string Basic = @"
@LabelEntry(string label,string Text)
{
	<StackLayout>
		<Label Text=""@label""/>
		<Entry Text=""@Text""/>
	</StackLayout>
}";
    }

    public class CompileException : Exception
    {
        public CompileException()
        {
            
        }

        public CompileException(string message, int index) : base(message + $" at index {index}")
        {
            
        }
    }
    public class Templator
    {
        TamlParser parser = new TamlParser();
        TamlAst ast = new TamlAst();
        Generator.Generator gen = new Generator.Generator();
        public List<GeneratedType> Generate(string code, string Namespace)
        {
            gen.Namespace = Namespace;
            var tokens = parser.GetTokens(code);
            var t = ast.Evaluate(tokens);
            var checker= new SyntaxChecker(t);
            checker.CheckSyntax();
            return gen.Generate(t).ToList();
        }
    }
}