using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Center.ModelsDTO;
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

    private void Delete(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var DataContext = (MainWindowViewModel)this.DataContext;
        var magazin = (MagazinDTO)button.DataContext;

        var window = new DeleteWindow(DataContext, magazin);
        window.ShowDialog(this);
    }


}