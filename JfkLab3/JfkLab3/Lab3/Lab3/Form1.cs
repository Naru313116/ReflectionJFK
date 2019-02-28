using JfkCallable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DescriptionAttribute = JfkCallable.DescriptionAttribute;

namespace JfkWindow
{
    public partial class Form1 : Form
    {
        private Assembly assembly;

        public Form1()
        {
            InitializeComponent();
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            RefreshLoadFile();

            using (var openAssemblyDialog = new FolderBrowserDialog())
            {
                if (openAssemblyDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                else
                {
                    string[] files = Directory.GetFiles(openAssemblyDialog.SelectedPath);

                    foreach (string file in files)
                    {
                        if (file.EndsWith("exe") || file.EndsWith("dll"))
                        {
                            assembly = Assembly.LoadFile(file.ToString());
                            PopulateTree();
                        }
                    }
                }
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            var parameters = ReadParameters();
            var selected = (Type)myTree.SelectedNode.Tag;

            if (typeof(ICallable).IsAssignableFrom(selected))
            {
                if (!(Activator.CreateInstance(selected) is ICallable callable))
                {
                    throw new InvalidOperationException();
                }

                DisplayResult(parameters, callable);
            }
        }
   
        private void MyTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (myTree.SelectedNode.Tag is Type selected)
            {
                if (selected.GetCustomAttribute<DescriptionAttribute>(true) != null)
                {
                    var descriptionAttribute = selected.GetCustomAttribute<DescriptionAttribute>();
                    descriptionText.Text = descriptionAttribute.Description + $", Autor: {descriptionAttribute.Copyright}";
                }

                else
                {
                    descriptionText.Text = string.Empty;
                }
            }

            else
            {
                descriptionText.Text = string.Empty;
            }
        }

        private static void AddModule(Module module, TreeNode parent)
        {
            var newNode = new TreeNode(module.Name) { Tag = module };
            parent.Nodes.Add(newNode);

            foreach (var type in module.GetTypes())
            {
                AddType(type, newNode);
            }
        }

        private static void AddType(Type type, TreeNode parent)
        {
            var newNode = new TreeNode(type.Name) { Tag = type };
            var memberTypeNode = new TreeNode(".ctors");
            TreeNode memberNode;

            foreach (var constructor in type.GetConstructors())
            {
                memberNode = new TreeNode(constructor.Name) { Tag = constructor };
                memberTypeNode.Nodes.Add(memberNode);
            }

            newNode.Nodes.Add(memberTypeNode);

            memberTypeNode = new TreeNode("Methods");
            foreach (var method in type.GetMethods())
            {
                var count = method.GetParameters().Length;
                var stringBuilder = new StringBuilder(method.Name).Append('(');
                foreach (var param in method.GetParameters())
                {
                    stringBuilder.Append(param.ParameterType);
                    if (param.Position < count - 1)
                    {
                        stringBuilder.Append(", ");
                    }
                }

                stringBuilder.Append(')');
                memberNode = new TreeNode(stringBuilder.ToString()) { Tag = method };
                memberTypeNode.Nodes.Add(memberNode);
            }

            newNode.Nodes.Add(memberTypeNode);
            memberTypeNode = new TreeNode("Properties");
            foreach (var property in type.GetProperties())
            {
                memberNode = new TreeNode(property.Name) { Tag = property };
                memberTypeNode.Nodes.Add(memberNode);
            }

            newNode.Nodes.Add(memberTypeNode);

            memberTypeNode = new TreeNode("Fields");
            foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
                        | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField))
            {
                var fieldInfo = field.FieldType.Name;
                fieldInfo += " " + field.Name;
                memberNode = new TreeNode(fieldInfo) { Tag = field };
                memberTypeNode.Nodes.Add(memberNode);
            }

            newNode.Nodes.Add(memberTypeNode);

            memberTypeNode = new TreeNode("Events");
            foreach (var @event in type.GetEvents())
            {
                var eventInfo = @event.Name;
                eventInfo += " Delegate Type=" + @event.EventHandlerType;
                memberNode = new TreeNode(eventInfo) { Tag = @event };
                memberTypeNode.Nodes.Add(memberNode);
            }

            newNode.Nodes.Add(memberTypeNode);

            parent.Nodes.Add(newNode);
        }

        private void PopulateTree()
        {
            myTree.Nodes.Clear();
            var newNode = new TreeNode(assembly.GetName().Name) { Tag = assembly };
            myTree.Nodes.Add(newNode);

            foreach (var module in assembly.GetModules())
            {
                AddModule(module, newNode);
            }
        }

        private List<string> ReadParameters()
        {
            var text = paramsText.Text;
            var parameters = text.Replace(" ", "").Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            
            return parameters;
        }

        private void DisplayResult(List<string> parameters, ICallable callable)
        {
            if (parameters.Count == 2)
            {
                if ((double.TryParse(parameters[0], out double a) && (double.TryParse(parameters[1], out double b))))
                {
                    if (IsValid(a, b))
                    {
                        resultText.Text = callable.Call(a, b).ToString();
                    }

                    else
                    {
                        resultText.Text = "Podałeś za duża lub za mała wartość parametru";
                    }
                }

                else
                {
                    resultText.Text = "Podałeś zły typ argumentów podaj argument typu double";
                }
            }

            else
            {
                resultText.Text = "Wpisałeś złą ilość argumentów. Podaj dwa argumenty oddzielone średnikiem";
            }
        }

        private bool IsValid(double a, double b)
        {
            return (a > 0 && a <= 10) && (b > 0 && b <= 10);
        }

        private void RefreshLoadFile()
        {
            myTree.Nodes.Clear();
            descriptionText.Clear();
            paramsText.Clear();
            resultText.Clear();
        }
    }
}
