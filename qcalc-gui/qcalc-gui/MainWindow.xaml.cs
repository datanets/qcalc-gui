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
using qcalc;

namespace qcalc_gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Operation(String op)
        {
            Quaternion quatA = new Quaternion(0,0,0,0);
            Quaternion quatB = new Quaternion(0,0,0,0);
            Quaternion result = new Quaternion(0,0,0,0);

            quatA = parseStringToQuat(QuatABox.Text);
            quatB = parseStringToQuat(QuatBBox.Text);

            switch (op)
            {
                case "add":
                    result = quatA.add(quatB);
                    break;
                case "subtract":
                    result = quatA.subtract(quatB);
                    break;
                case "multiply":
                    result = quatA.multiply(quatB);
                    break;
                case "divide":
                    result = quatA.divide(quatB);
                    break;
                default:
                    break;
            }

            ResultBox.Text = result.ToString();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            Operation("add");
        }

        private void SubtractButtonClick(object sender, RoutedEventArgs e)
        {
            Operation("subtract");
        }

        private void MultiplyButtonClick(object sender, RoutedEventArgs e)
        {
            Operation("multiply");
        }

        private void DivideButtonClick(object sender, RoutedEventArgs e)
        {
            Operation("divide");
        }

        private Quaternion parseStringToQuat(String input)
        {
            String[] inputString = input.Split(',');
            Quaternion result = new Quaternion(Convert.ToDouble(inputString[0]), Convert.ToDouble(inputString[1]), Convert.ToDouble(inputString[2]), Convert.ToDouble(inputString[3]));  
            return result;
        }
    }
}
