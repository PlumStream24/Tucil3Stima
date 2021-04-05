using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Tucil3Stima
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Graph g = new Graph();
            List<List<String>> matrix = new List<List<String>>();
            
            // Read file
            System.IO.StreamReader file;
            try
            {
                file = new System.IO.StreamReader("test.txt");
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
            List<String> pain = new List<String>();
            pain.Add("S");
            String end = "F";
            if (g.manhattan.ContainsKey((pain.First(), end)))
            {
                (List<String>, int) pain2 = g.AllStar((pain, g.manhattan[(pain.Last(), end)]), end, new List<(List<String>, int)>());
            }
            else
            {
                Debug.WriteLine("Shit dont exist my guy");
            }
            //g.AStar("S", "a");

            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
