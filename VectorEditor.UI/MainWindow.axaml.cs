using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;

namespace VectorEditor.UI
{
    public partial class MainWindow : Window
    {
        private Button? _activeToolButton;
        private string _selectedColor="#000000";
        public MainWindow()
        {
            InitializeComponent();
        }

        void ToggleThemeChange(object? sender, RoutedEventArgs? e)
        {
            if (ActualThemeVariant == ThemeVariant.Dark)
            {
                RequestedThemeVariant = ThemeVariant.Light;
            }
            else
            {
                RequestedThemeVariant = ThemeVariant.Dark;
            }
        }

        void SelectTool(object? sender, RoutedEventArgs? e)
        {
            var clickedButton = sender as Button;
            if (clickedButton == null) return;
            if (_activeToolButton != null)
            {
                _activeToolButton.Classes.Remove("Selected");
            }
            clickedButton.Classes.Add("Selected");
            _activeToolButton = clickedButton;
        }

        void UpdateColor(string color)
        {
            _selectedColor = color;
            SelectedColor.Background = Brush.Parse(color);
        }
        
        void ToggleMenu(object? sender, RoutedEventArgs? e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }
    }
}