using Avalonia.Controls;
using Avalonia.Interactivity;
using Center.Models;
using Center.ModelsDTO;
using Center.Services;
using System;

namespace Center.Views
{
    public partial class AuthorizeWindow : Window
    {
        public AuthorizeWindow()
        {
            InitializeComponent();
        }

        private void Authorize(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox code = this.Find<TextBox>("Code")!;
                TextBox login = this.Find<TextBox>("Login")!;
                Worker worker = DBCall.Authorize(login.Text!, code.Text!);
                if (worker != null)
                {
                    CurrentUser.Worker = worker;
                    var window = new MainWindow();
                    window.Show();
                    Close();
                }
                else
                {
                    Label ErrorLabel = this.Find<Label>("ErrorLabel")!;
                    ErrorLabel.IsVisible = true;
                    ErrorLabel.Content = "Неправильный код";
                }
            }
            catch (Exception)
            {
                Label ErrorLabel = this.Find<Label>("ErrorLabel")!;
                ErrorLabel.IsVisible = true;
                ErrorLabel.Content = "Ошибка соединения";
            }
        }
    }
}