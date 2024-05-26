using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Center.ModelsDTO;
using Center.ViewModels;

namespace Center.Views;

public partial class DeleteWindow : Window
{
    public DeleteWindow(MainWindowViewModel mainWindowViewModel, MagazinDTO magazinDTO)
    {
        InitializeComponent();
        DataContext = new DeleteWindowViewModel(mainWindowViewModel, magazinDTO);
    }
    private void Close(object sender, RoutedEventArgs e)
    {
        Close();
    }
}