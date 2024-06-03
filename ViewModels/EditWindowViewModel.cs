using Center.Models;
using Center.ModelsDTO;
using Center.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Center.ViewModels
{
    public class EditWindowViewModel : ViewModelBase
    {
        public MagazinDTO CurrentMagazin { get; set; }

        private MagazinDTO _magazin = null!;

        public MagazinDTO Magazin
        {
            get { return _magazin; }
            set { _magazin = value; }
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

        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); ; }
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
        public EditWindowViewModel(MagazinDTO magazinDTO)
        {
            CurrentMagazin = magazinDTO;

            Magazin = new MagazinDTO
            {
                Id = magazinDTO.Id,
                Title = magazinDTO.Title,
                CreationDate = magazinDTO.CreationDate,
                Price = magazinDTO.Price,
                Count = magazinDTO.Count,
                Photo = magazinDTO.Photo,
                Creator = magazinDTO.Creator
            };

            Count = magazinDTO.Count;
            Price = magazinDTO.Price;

            Creators = DBCall.GetCreators();

            SelectedCreator = Creators.First(m => m.Id == Magazin.Creator.Id);

            this.WhenAnyValue(
               x => x.Magazin.Title,
               x => x.Magazin.Photo,
               x => x.Count,
               x => x.Price).Subscribe(_ => IsButtonEnabled());
        }

        private void IsButtonEnabled()
        {
            IsButtonEnable = !string.IsNullOrEmpty(Magazin.Title) && Magazin.Photo != null && Count != null && Price != null;
        }

        public void Edit()
        {
            try
            {
                Magazin.Price = (decimal)Price!;
                Magazin.Count = (int)Count!;
                Magazin.Creator = SelectedCreator;
                DBCall.Edit(Magazin);

                if (CurrentMagazin.Count > Magazin.Count)
                    DBCall.IssueMagazin(Magazin.Id, CurrentUser.Worker.Id, CurrentMagazin.Count - Magazin.Count);
                else if (CurrentMagazin.Count < Magazin.Count)
                    DBCall.ReceiveMagazin(Magazin.Id, CurrentUser.Worker.Id, Magazin.Count - CurrentMagazin.Count);

                CurrentMagazin.Title = Magazin.Title;
                CurrentMagazin.Photo = Magazin.Photo;
                CurrentMagazin.Price = Magazin.Price;
                CurrentMagazin.Creator = Magazin.Creator;
                CurrentMagazin.Count = Magazin.Count;

                Message = "Данные обновлены";
            }
            catch (Exception)
            {
                Message = "Не удалось применить изменения";
            }
        }
    }
}
