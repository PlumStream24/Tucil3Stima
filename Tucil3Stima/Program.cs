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
            
            //read file
            System.IO.StreamReader file;
            try
            {
                file = new System.IO.StreamReader("test.txt");
            }
            catch (System.IO.IOException)
            {
                return;
            }

            int index = 0;
            String line = file.ReadLine();
            foreach (String s in line.Split(' '))
            {
                g.nodeName.Add(index, s);
                index++;
            }

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

            g.ReadMatrix(matrix);
            foreach (var stuff in g.edges)
            {
                Debug.WriteLine(stuff[0] + " " + stuff[1]);
            }

            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
    }
}
