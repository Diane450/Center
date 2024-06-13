using Center.Models;
using Center.ModelsDTO;
using Center.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Center.ViewModels
{
    public class DeleteWindowViewModel(MainWindowViewModel model, MagazinDTO magazin) : ViewModelBase
    {
        public MainWindowViewModel Model { get; set; } = model;

        public MagazinDTO Magazin { get; set; } = magazin;


        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        private bool _isButtonEnable = true;

        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set { _isButtonEnable = this.RaiseAndSetIfChanged(ref _isButtonEnable, value); }
        }

        public void Delete()
        {
            try
            {
                DBCall.DeleteMagazin(Magazin);
                Model.Magazins.Remove(Magazin);
                Model.Filter();
                Message = "Успешно удалено";
                IsButtonEnable = false;
            }
            catch (Exception)
            {
                Message = "Не удалось удалить препарат";
            }
        }
    }
}
