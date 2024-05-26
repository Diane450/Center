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
        var dataContext = (MainWindowViewModel)button.DataContext!;

        var window = new AddNewMagazinWindow(dataContext);
        window.ShowDialog(this);
    }

    private void Delete(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var DataContext = (MainWindowViewModel)this.DataContext!;
        var magazin = (MagazinDTO)button.DataContext!;

        var window = new DeleteWindow(DataContext, magazin);
        window.ShowDialog(this);
    }

    private void Edit(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var dataContext = (MagazinDTO)button.DataContext!;
        var editWindow = new EditWindow(dataContext);
        editWindow.ShowDialog(this);
    }

    private void OpenReportWindow(object sender, RoutedEventArgs e)
    {
        var window = new ReportWindow();
        window.ShowDialog(this);
    }

}