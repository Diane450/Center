using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Center.ViewModels;

namespace Center;

public partial class ReportWindow : Window
{
    public ReportWindow()
    {
        InitializeComponent();
        DataContext = new ReportWindowViewModel();
    }
}