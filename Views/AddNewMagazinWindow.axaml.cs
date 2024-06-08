using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Center.ModelsDTO;
using Center.ViewModels;
using System.Collections.Generic;
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
        var context = (AddNewMagazinWindowViewModel)button.DataContext!;

        var storageProvider = StorageProvider;
        var result = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Выбрать изображение",
            FileTypeFilter =
            [
                new FilePickerFileType("Images")
                {
                    Patterns = ["*.jpg", "*.png", "*.jpeg"]
                }
            ],
            AllowMultiple = false
        });

        if (result.Count > 0)
        {
            var selectedFile = result[0];
            await using var fs = await selectedFile.OpenReadAsync();
            var bitmap = new Bitmap(fs);

            using var ms = new MemoryStream();
            bitmap.Save(ms);
            context.Magazin.Photo = ms.ToArray();
        }
    }
}