using System.Media;
using System.Windows;

namespace MetronomeApp
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public CustomMessageBox(string message, string title, bool isCancelVisible)
        {
            InitializeComponent();

            if (!isCancelVisible)
            {
                CancelButton.Visibility = Visibility.Collapsed;
            }

            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            MessageText.Text = message;
            Title = title;

            SystemSounds.Asterisk.Play();
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
