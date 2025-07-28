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

namespace SF6CharacterDatabaseApp
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

        private void OnCharacterSelectClick(object sender, RoutedEventArgs e)
        {
            // 仮のウィンドウ。後ほど CharacterSelectWindow.xaml を作成
            MessageBox.Show("キャラ選択画面を表示（後で実装）");
        }

        private void OnSettingsClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("設定画面は未実装です。");
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}