using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Center.ViewModels;

namespace Center.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void Add(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var dataContext = (MainWindowViewModel)button.DataContext;

        var window = new AddNewMagazinWindow(dataContext);
        window.ShowDialog(this);
    }

}