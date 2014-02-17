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

        public void runTests()
        {
            Quaternion quatA = new Quaternion(1,2,1,2);
            Quaternion quatB = new Quaternion(1,-1,0,2);
            Quaternion result = new Quaternion(0,0,0,0);

            // test inverse
            TLog.Text += "test inverse\n";
            TLog.Text += quatA + "^-1\n";
            result = quatA.inverse();
            TLog.Text += "= " + result + "\n\n";

            quatA = new Quaternion(3, 2, 1, 0);

            // test addition
            TLog.Text += "test add\n";
            TLog.Text += quatA + " + " + quatB + "\n";
            result = quatA.add(quatB);
            TLog.Text += "= " + result + "\n\n";

            // test subtraction
            TLog.Text += "test subtract\n";
            TLog.Text += quatA + " - " + quatB + "\n";
            result = quatA.subtract(quatB);
            TLog.Text += "= " + result + "\n\n";

            quatB = new Quaternion(0,1,-2,0);

            // test multiplication
            TLog.Text += "test multiply\n";
            TLog.Text += quatA + " * " + quatB + "\n";
            result = quatA.multiply(quatB);
            TLog.Text += "= " + result + "\n\n";

            // test division
            TLog.Text += "test divide\n";
            TLog.Text += quatA + " / " + quatB + "\n";
            result = quatA.divide(quatB);
            TLog.Text += "inverse of quatB: " + quatB.inverse() + "\n";
            TLog.Text += "= " + result + "\n\n";

            quatA = new Quaternion(1,2,1,2);

            // test magnitude
            TLog.Text += "test magnitude\n";
            TLog.Text += quatA + "\n";
            TLog.Text += "= " + quatA.magnitude() + "\n\n";

            // test rotation
            Quaternion point = new Quaternion(0, 5, 0, 0);
            Double angle = Math.PI / 2;
            Quaternion axis = new Quaternion(0, 0, 0, 1);

            TLog.Text += "test quaternion rotation\n";
            TLog.Text += "point: " + point + "\n";
            TLog.Text += "angle: " + angle + "\n";
            TLog.Text += "axis: " + axis + "\n";
            result = Quaternion.rotatePointByAngleAboutAxisVector(point, angle, axis);
            TLog.Text += "= " + result + "\n\n";

            // verify rotation manually
            TLog.Text += "test quaternion rotation (manual functions check)\n";
            quatA = new Quaternion(Math.Sqrt(2) / 2, 0, 0, Math.Sqrt(2) / 2);
            Quaternion quatP = new Quaternion(0, 5, 0, 0);

            TLog.Text += "-- inverse\n";
            TLog.Text += quatA + "^-1\n";
            result = quatA.inverse();
            TLog.Text += result + "\n\n";

            TLog.Text += "-- multiply\n";
            TLog.Text += quatA + " * " + quatP + "\n\n";
            result = quatA.multiply(quatP);
            result = result.multiply(quatA.inverse());
            TLog.Text += "= " + result + "\n\n";
        }

        private void Operation(String op)
        {
            Quaternion quatA = new Quaternion(0,0,0,0);
            Quaternion quatB = new Quaternion(0,0,0,0);
            Quaternion quatResult = new Quaternion(0,0,0,0);
            Double doubleResult = 0.0f;

            switch (op)
            {
                case "add":
                    quatA = parseStringToQuat(OPQuatABox.Text);
                    quatB = parseStringToQuat(OPQuatBBox.Text);
                    quatResult = quatA.add(quatB);
                    OPResultBox.Text = quatResult.ToString();
                    break;
                case "subtract":
                    quatA = parseStringToQuat(OPQuatABox.Text);
                    quatB = parseStringToQuat(OPQuatBBox.Text);
                    quatResult = quatA.subtract(quatB);
                    OPResultBox.Text = quatResult.ToString();
                    break;
                case "multiply":
                    quatA = parseStringToQuat(OPQuatABox.Text);
                    quatB = parseStringToQuat(OPQuatBBox.Text);
                    quatResult = quatA.multiply(quatB);
                    OPResultBox.Text = quatResult.ToString();
                    break;
                case "divide":
                    quatA = parseStringToQuat(OPQuatABox.Text);
                    quatB = parseStringToQuat(OPQuatBBox.Text);
                    quatResult = quatA.divide(quatB);
                    OPResultBox.Text = quatResult.ToString();
                    break;
                case "inverse":
                    quatA = parseStringToQuat(IMQuatBox.Text);
                    quatResult = quatA.inverse();
                    IMResultBox.Text = quatResult.ToString();
                    break;
                case "magnitude":
                    quatA = parseStringToQuat(IMQuatBox.Text);
                    doubleResult = quatA.magnitude();
                    IMResultBox.Text = doubleResult.ToString();
                    break;
                case "rotate":
                    quatA = parseStringToQuat(RQuatABox.Text);
                    quatB = parseStringToQuat(RQuatBBox.Text);
                    double angleNumerator = 1.0f;

                    if (!string.IsNullOrWhiteSpace(RAngleNumerator.Text))
                        angleNumerator = Convert.ToDouble(RAngleNumerator.Text);

                    double piInNumerator = 1.0f;
                    
                    if (RAngleIncludePIInNumerator.IsChecked.Value)
                        piInNumerator = Math.PI;

                    double angleDenominator = Convert.ToDouble(RAngleDenominator.Text);
                    double angle = (angleNumerator * piInNumerator) / angleDenominator;

                    quatResult = Quaternion.rotatePointByAngleAboutAxisVector(quatA, angle, quatB);
                    RResultBox.Text = quatResult.ToString();
                    break;
                default:
                    break;
            }
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

        private void InverseButtonClick(object sender, RoutedEventArgs e)
        {
            Operation("inverse");
        }

        private void MagnitudeButtonClick(object sender, RoutedEventArgs e)
        {
            Operation("magnitude");
        }

        private void RotateButtonClick(object sender, RoutedEventArgs e)
        {
            Operation("rotate");
        }

        private void RunTestsButtonClick(object sender, RoutedEventArgs e)
        {
            runTests();
        }

        private Quaternion parseStringToQuat(String input)
        {
            String[] inputString = input.Split(',');
            Quaternion result = new Quaternion(Convert.ToDouble(inputString[0]), Convert.ToDouble(inputString[1]), Convert.ToDouble(inputString[2]), Convert.ToDouble(inputString[3]));  
            return result;
        }
    }
}
