using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tucil3Stima
{
    public partial class Form1 : Form
    {
        public Graph g;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Microsoft.Msagl.Drawing.Graph graph = gViewer1.Graph;
            //this bottom 2 loops are basically a REEESEEETTTT for the colours of the graph
            //colouring the nodes that are not the default colour
            foreach (Microsoft.Msagl.Drawing.Node element in graph.Nodes)
            {
                if (element.Attr.Color.A != 255 || element.Attr.Color.B != 0 || element.Attr.Color.G != 0 || element.Attr.Color.R != 0)
                {
                    element.Attr.Color = new Microsoft.Msagl.Drawing.Color((byte)0, (byte)0, (byte)0);
                }
            }
            //colouring the edges that are not the default colour
            foreach (Microsoft.Msagl.Drawing.Edge element in graph.Edges)
            {
                if (element.Attr.Color.A != 255 || element.Attr.Color.B != 0 || element.Attr.Color.G != 0 || element.Attr.Color.R != 0)
                {
                    element.Attr.Color = new Microsoft.Msagl.Drawing.Color((byte)0, (byte)0, (byte)0);
                }
            }

            if (g.manhattan.ContainsKey((textBox1.Text, textBox2.Text)))
            {
                //do the A* (AllStar method is about to be invoked)
                List<String> start = new List<String>();
                start.Add(textBox1.Text);
                String end = textBox2.Text;
                (List<String>, int) result = g.AllStar((start, g.manhattan[(start.Last(), end)]), end, new List<(List<String>, int)>());
                String writeThis = $"Computing path from {textBox1.Text} to {textBox2.Text}\n";
                for (int i = 0; i < result.Item1.Count()-1; i++)
                {
                    writeThis = writeThis + $"{result.Item1[i]} " + "-> ";
                }
                writeThis = writeThis + $"{result.Item1.Last()}\n";
                writeThis = writeThis + $"Path length : {result.Item2}";
                richTextBox1.Text = writeThis;

                //colouring motherfuckerss
                //colouring edges
                foreach (Microsoft.Msagl.Drawing.Edge element in graph.Edges)
                {
                    if (result.Item1.Contains(element.Source))
                    {
                        if (result.Item1.First() != element.Source)
                        {
                            if (result.Item1[result.Item1.IndexOf(element.Source) - 1] == element.Target)
                            {
                                element.Attr.Color = new Microsoft.Msagl.Drawing.Color((byte)255, (byte)0, (byte)0);
                            }
                        }
                        if (result.Item1.Last() != element.Source)
                        {
                            if (result.Item1[result.Item1.IndexOf(element.Source) + 1] == element.Target)
                            {
                                element.Attr.Color = new Microsoft.Msagl.Drawing.Color((byte)255, (byte)0, (byte)0);
                            }
                        }
                    }
                    else if (result.Item1.Contains(element.Target))
                    {
                        if (result.Item1.First() != element.Target)
                        {
                            if (result.Item1[result.Item1.IndexOf(element.Target) - 1] == element.Source)
                            {
                                element.Attr.Color = new Microsoft.Msagl.Drawing.Color((byte)255, (byte)0, (byte)0);
                            }
                        }
                        if (result.Item1.Last() != element.Target)
                        {
                            if (result.Item1[result.Item1.IndexOf(element.Target) + 1] == element.Source)
                            {
                                element.Attr.Color = new Microsoft.Msagl.Drawing.Color((byte)255, (byte)0, (byte)0);
                            }
                        }
                    }
                }

                //colouring verts/nodes
                foreach (Microsoft.Msagl.Drawing.Node element in graph.Nodes)
                {
                    if (result.Item1.Contains(element.Id))
                    {
                        element.Attr.Color = new Microsoft.Msagl.Drawing.Color((byte)255, (byte)0, (byte)0);
                    }
                }
            }
            else
            {
                richTextBox1.Text = "Cannot compute....ASSHOLE";
            }
            //assigning the changed graph to the modafuckin gViewer
            gViewer1.Graph = graph;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                String filename = openFileDialog1.FileName;
                FileInput.Text = filename;
            }

            List<List<String>> matrix = new List<List<String>>();
            this.g = new Graph();

            // Read file
            System.IO.StreamReader file;
            try
            {
                file = new System.IO.StreamReader(FileInput.Text);
            }
            catch (System.IO.IOException)
            {
                return;
            }

            // Make dictionary
            int index = 0;
            String line = file.ReadLine();
            foreach (String s in line.Split(' '))
            {
                g.nodeName.Add(index, s);
                index++;
            }

            // Make adjacency matrix
            index = 0;
            while (!file.EndOfStream)
            {
                line = file.ReadLine();
                matrix.Add(new List<String>());
                foreach (String s in line.Split(' '))
                {
                    matrix[index].Add(s);
                }
                index++;
            }

            // Move matrix to graph
            g.ReadMatrix(matrix);

            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph();
            List<(String, String)> made = new List<(String, String)>();
            foreach (String[] element in g.edges)
            {
                if (!made.Contains((element[1], element[0])))
                {
                    graph.AddEdge(element[0], element[2], element[1]).Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                    made.Add((element[0], element[1]));
                }
            }
            gViewer1.Graph = graph;

        }
    }
}
