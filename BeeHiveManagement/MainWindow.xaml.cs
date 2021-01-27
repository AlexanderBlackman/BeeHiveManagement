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
using System.Windows.Threading;

namespace BeeHiveManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Queen queen;
 
        private DispatcherTimer timer = new DispatcherTimer();
        public MainWindow()
        {   
            InitializeComponent();
            queen = Resources["queen"] as Queen;
        //    reportTextBox.Text = queen.StatusReport;
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1.5);
            timer.Start();

        }

        private void assignJobButton_Click(object sender, RoutedEventArgs e)
        {
            queen.AssignBee(jobComboBox.Text);
  //          reportTextBox.Text = queen.StatusReport;
        }

        private void nextShiftButton_Click(object sender, RoutedEventArgs e)
        {
            queen.WorkTheNextShift();
            //queen.DoJob();
     //       reportTextBox.Text = queen.StatusReport;
        }

        private void difficultySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            difficultyNumber.Text = difficultySlider.Value.ToString();
          //  Bee.Difficulty = (float)difficultySlider.Value;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            nextShiftButton_Click(this, new RoutedEventArgs());
        }
    }
}
