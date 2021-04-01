using System;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SDT_Project.Models
{
    /// <summary>
    /// Логика взаимодействия для CardModel.xaml
    /// </summary>
    public partial class CardModel : UserControl
    {
        // Using a DependencyProperty backing store for Angle.
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(CardModel), new UIPropertyMetadata(0.0));

        public static readonly DependencyProperty DistanceProperty =
            DependencyProperty.Register("Distance", typeof(Point3D), typeof(CardModel), new UIPropertyMetadata(new Point3D(0, -6, 3.5)));

        public static readonly DependencyProperty FrontBrushProperty =
            DependencyProperty.Register("FrontBrush", typeof(Brush), typeof(CardModel), new UIPropertyMetadata(new ImageBrush(new BitmapImage(new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\img.png")))));

        public static readonly DependencyProperty BackBrushProperty =
            DependencyProperty.Register("BackBrush", typeof(Brush), typeof(CardModel), new UIPropertyMetadata(new ImageBrush(new BitmapImage(new Uri(@"C:\Users\bril1\source\repos\SDT-Project\SDT-Project\images\img2.png")))));

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public Point3D Distance
        {
            get { return (Point3D)GetValue(DistanceProperty); }
            set { SetValue(DistanceProperty, value); }
        }

        public Brush FrontBrush { set; get; }

        public Brush BackBrush { set; get; }

        public CardModel()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
            this.MouseUp += new MouseButtonEventHandler(OnMouseUp);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseWheel += new MouseWheelEventHandler(OnMouseWheel);
        }

        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(this);
        }

        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured == this)
            {
                // Get the current mouse position relative to the volume control
                System.Windows.Point currentLocation = Mouse.GetPosition(this);

                // We want to rotate around the center of the knob, not the top corner
                System.Windows.Point knobCenter = new System.Windows.Point(this.ActualHeight / 2, this.ActualWidth / 2);

                // Calculate an angle
                double radians = Math.Atan((currentLocation.Y - knobCenter.Y) /
                                           (currentLocation.X - knobCenter.X));
                this.Angle = radians * 180 / Math.PI;

                // Apply a 180 degree shift when X is negative so that we can rotate
                // all of the way around
                if (currentLocation.X - knobCenter.X < 0)
                {
                    this.Angle += 180;
                }
            }
        }

        public void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.Distance = new Point3D(0, -6, this.Distance.Z - e.Delta / 300.0);
        }
    }
}
