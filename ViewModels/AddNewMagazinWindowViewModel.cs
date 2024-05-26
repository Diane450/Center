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
    public class AddNewMagazinWindowViewModel : ViewModelBase
    {
        private MagazinDTO _magazin = null!;

        public MagazinDTO Magazin
        {
            get { return _magazin; }
            set { _magazin = this.RaiseAndSetIfChanged(ref _magazin, value); }
        }

        private decimal? _price;

        public decimal? Price
        {
            get { return _price; }
            set { _price = this.RaiseAndSetIfChanged(ref _price, value); }
        }

        private int? _count;

        public int? Count
        {
            get { return _count; }
            set { _count = this.RaiseAndSetIfChanged(ref _count, value); }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value);; }
        }

        public List<Worker> Creators { get; set; } = null!;

        private Worker _selectedCreator = null!;

        public Worker SelectedCreator
        {
            get { return _selectedCreator; }
            set { _selectedCreator = this.RaiseAndSetIfChanged(ref _selectedCreator, value); }
        }

        private bool _isButtonEnable;
        public bool IsButtonEnable
        {
            get { return _isButtonEnable; }
            set { _isButtonEnable = this.RaiseAndSetIfChanged(ref _isButtonEnable, value); }
        }

        public MainWindowViewModel Model { get; set; }

        public AddNewMagazinWindowViewModel(MainWindowViewModel model)
        {
            Model = model;
            Magazin = new MagazinDTO();

            Creators = DBCall.GetCreators();
            SelectedCreator = Creators[0];

            this.WhenAnyValue(
               x => x.Magazin.Title,
               x => x.Magazin.Photo,
               x => x.Count,
               x=>x.Price).Subscribe(_ => IsButtonEnabled());
        }

        private void IsButtonEnabled()
        {
            IsButtonEnable = !string.IsNullOrEmpty(Magazin.Title) && Magazin.Photo != null && Count != null && Price != null;
        }

        public void Add()
        {
            try
            {
                Magazin.Price = (decimal)Price!;
                Magazin.CreationDate = DateOnly.FromDateTime(DateTime.Now);
                Magazin.Count = (int)Count!;
                Magazin.Creator = SelectedCreator;
                DBCall.Add(Magazin);
                Model.Magazins.Add(Magazin);
                Model.Filter();
                Message = "Добавлено успешно";
            }
            catch (Exception)
            {
                Message = "Не удалось добавить журнал";
            }
        }
    }
}
