using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WakeOnLan.Core;
namespace WakeOnLan.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnWakeClick(object sender, RoutedEventArgs e)
    {
        var mac = MacBox.Text.Trim();
        var broadcast = string.IsNullOrWhiteSpace(BroadcastBox.Text) ? "255.255.255.255" : BroadcastBox.Text.Trim();
        var portText = PortBox.Text.Trim();

        if (!int.TryParse(portText, out var port)) port = 9;

        try
        {
            // Disable UI
            IsEnabled = false;
            await WakeOnLanClient.SendMagicPacketAsync(mac, broadcast, port);
            MessageBox.Show($"Đã gửi Magic Packet tới {mac}\n{broadcast}:{port}", "Thành công",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsEnabled = true;
        }
    }
}