using System.Windows;

namespace photodev
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

         private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainImage.Source = App.OpenFile(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2");
        }
    }
}
