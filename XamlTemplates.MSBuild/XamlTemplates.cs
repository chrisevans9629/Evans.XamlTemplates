﻿using System;
using System.IO;
using System.Reflection;
using Evans.XamlTemplates;
using Evans.XamlTemplates.Generator;
using Microsoft.Build.Framework;

namespace XamlTemplates.MSBuild
{
    public class XamlTemplates : Microsoft.Build.Utilities.Task
    {
        public string Filter { get; set; } = "*.taml";
        public string Folder { get; set; } = "";
        public string Namespace { get; set; } =
            Assembly.GetEntryAssembly()?.GetName().Name ?? "Template";
        public override bool Execute()
        {
            try
            {
                var templator = new Templator();
                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), Filter);
                foreach (var file in files)
                {
                    Log.LogMessage(MessageImportance.High, $"Found file {file}");

                    var content = File.ReadAllText(file);

                    var result = templator.Generate(content, Namespace);

                    foreach (var generatedType in result)
                    {
                        Log.LogMessage(MessageImportance.High, $"Generated '{generatedType.CSharp.FileName}'");
                        Log.LogMessage(MessageImportance.High, $"Generated '{generatedType.Xaml.FileName}'");

                        var x = generatedType.Xaml;
                        var c = generatedType.CSharp;

                        var directory = Path.Combine(Directory.GetCurrentDirectory(), Folder);

                        if (Directory.Exists(directory) != true)
                        {
                            Directory.CreateDirectory(directory);
                        }

                        File.WriteAllText(Path.Combine(directory,x.FileName), x.Content);
                        File.WriteAllText(Path.Combine(directory, c.FileName), c.Content);
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }
           
            return true;
        }
    }
}
