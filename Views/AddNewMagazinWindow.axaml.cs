using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Center.ModelsDTO;
using Center.ViewModels;
using System.IO;

namespace Center;

public partial class AddNewMagazinWindow : Window
{
    public AddNewMagazinWindow(MainWindowViewModel model)
    {
        InitializeComponent();
        DataContext = new AddNewMagazinWindowViewModel(model);
    }

    private async void ChangePhoto(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var context = (AddNewMagazinWindowViewModel)button.DataContext;

        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filters.Add(new FileDialogFilter() { Name = "Images", Extensions = { "jpg", "png", "jpeg" } });

        string[] result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            using (FileStream fs = File.OpenRead(result[0]))
            {
                Avalonia.Media.Imaging.Bitmap bp = new Avalonia.Media.Imaging.Bitmap(fs);

                using (MemoryStream ms = new MemoryStream())
                {
                    bp.Save(ms);
                    context.Magazin.Photo = ms.ToArray();
                }
            }
        }
    }
}