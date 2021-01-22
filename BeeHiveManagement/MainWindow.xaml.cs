using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeeHiveManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Queen queen = new Queen();
        public MainWindow()
        {   
            InitializeComponent();
            reportTextBox.Text = queen.StatusReport;

        }

        private void assignJobButton_Click(object sender, RoutedEventArgs e)
        {
            queen.AssignBee(jobComboBox.Text);
            reportTextBox.Text = queen.StatusReport;
        }

        private void nextShiftButton_Click(object sender, RoutedEventArgs e)
        {
            queen.WorkTheNextShift();
            //queen.DoJob();
            reportTextBox.Text = queen.StatusReport;
        }

        private void difficultySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            difficultyNumber.Text = difficultySlider.Value.ToString();
          //  Bee.Difficulty = (float)difficultySlider.Value;
        }
    }
}
