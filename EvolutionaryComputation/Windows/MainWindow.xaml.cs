using EvolutionaryComputation.App_Code;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace EvolutionaryComputation.Windows
{
    public partial class MainWindow : Window
    {
        private Core core;
        public MainWindow()
        {
            InitializeComponent();

            core = new Core();

            

            Debug.WriteLine(this.DataContext);
        }

        private void buttonInitialize_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;

            int numberOfPopulation = int.Parse(textBoxPopulation.Text);
            int bits = int.Parse(textBoxBits.Text);
            double mutation = double.Parse(textBoxMutation.Text);
            double noCrossover = double.Parse(textBoxNoCrossover.Text);
            var parents = this.core.Initialize(numberOfPopulation, bits, mutation, noCrossover);
            this.DataContext = core;
            this.textBoxGenerations.Text = this.core.generations.ToString();
            Cursor = Cursors.Arrow;
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;

            int steps = int.Parse(textBoxSteps.Text);

            this.core.Next(steps);

            this.textBoxGenerations.Text = this.core.generations.ToString();

            Cursor = Cursors.Arrow;
        }
    }
}
