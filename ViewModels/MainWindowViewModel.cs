using Center.Models;
using Center.ModelsDTO;
using Center.Services;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace Center.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<MagazinDTO> Magazins { get; set; } = null!;


        private ObservableCollection<MagazinDTO> _filteredMagazin = null!;

        public ObservableCollection<MagazinDTO> FilteredMagazin
        {
            get { return _filteredMagazin; }
            set { _filteredMagazin = this.RaiseAndSetIfChanged(ref _filteredMagazin, value); }
        }

        private MagazinDTO _selectedMagazin = null!;

        public MagazinDTO SelectedMagazin
        {
            get { return _selectedMagazin; }
            set { _selectedMagazin = this.RaiseAndSetIfChanged(ref _selectedMagazin, value); }
        }

        public List<Worker> Creators { get; set; } = null!;


        private Worker _selectedCreator = null!;

        public Worker SelectedCreator
        {
            get { return _selectedCreator; }
            set { _selectedCreator = this.RaiseAndSetIfChanged(ref _selectedCreator, value); Filter(); }
        }

        private bool _isFilteredListNotNull = true;

        public bool IsFilteredListNotNull
        {
            get { return _isFilteredListNotNull; }
            set { _isFilteredListNotNull = this.RaiseAndSetIfChanged(ref _isFilteredListNotNull, value); }
        }

        public List<string> SortValues { get; set; } = null!;

        private string _selectedSortValue = null!;

        public string SelectedSortValue
        {
            get { return _selectedSortValue; }
            set { _selectedSortValue = this.RaiseAndSetIfChanged(ref _selectedSortValue, value); Sort(); }
        }

        public bool IsAdmin { get; set; }

        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        private string _searchingMagazin = null!;

        public string SearchingMagazin
        {
            get { return _searchingMagazin; }
            set { _searchingMagazin = this.RaiseAndSetIfChanged(ref _searchingMagazin, value); }
        }

        public MainWindowViewModel()
        {
            Magazins = DBCall.GetMagazines();
            FilteredMagazin = new ObservableCollection<MagazinDTO>(Magazins);
            MagazinDTO.IsAdmin = CurrentUser.Worker.User.RoleId == 1;
            SortValues = new List<string>
            {
                "от А до Я",
                "от Я до А",
                "количество: от меньшего к большему",
                "количество: от большего к меньшему",
                "дата: от раннего к позднему",
                "дата: от позднего к раннему",
            };
            SelectedSortValue = SortValues[0];

            Creators = new List<Worker>
            {
                new Worker
                {
                    Id=0,
                    FullName="Все"
                }
            };
            Creators.AddRange(DBCall.GetCreators());
            SelectedCreator = Creators[0];
            this.WhenAnyValue(x => x.SearchingMagazin).Subscribe(_ => Find());
            IsAdmin = CurrentUser.Worker.User.RoleId == 1;
        }

        private void Find()
        {
            if (!string.IsNullOrEmpty(SearchingMagazin))
            {
                FilteredMagazin = new ObservableCollection<MagazinDTO>(Magazins.Where(d => d.Title.ToLower().StartsWith(SearchingMagazin.ToLower())).ToList());
                if (FilteredMagazin.Count == 0 || FilteredMagazin == null)
                {
                    IsFilteredListNotNull = false;
                    Message = "Не найдено";
                }
                else
                {
                    IsFilteredListNotNull = true;
                    Message = "";
                    SelectedMagazin = FilteredMagazin[0];
                }
            }
            else
            {
                Filter();
            }
        }

        private void Sort()
        {
            if (SelectedSortValue == SortValues[0])
            {
                FilteredMagazin = new ObservableCollection<MagazinDTO>(FilteredMagazin.OrderBy(d => d.Title).ToList());
            }
            else if (SelectedSortValue == SortValues[1])
            {
                FilteredMagazin = new ObservableCollection<MagazinDTO>(FilteredMagazin.OrderByDescending(d => d.Title).ToList());
            }
            else if (SelectedSortValue == SortValues[2])
            {
                FilteredMagazin = new ObservableCollection<MagazinDTO>(FilteredMagazin.OrderBy(d => d.Count).ToList());
            }
            else if (SelectedSortValue == SortValues[3])
            {
                FilteredMagazin = new ObservableCollection<MagazinDTO>(FilteredMagazin.OrderByDescending(d => d.Count).ToList());
            }
            else if (SelectedSortValue == SortValues[4])
            {
                FilteredMagazin = new ObservableCollection<MagazinDTO>(FilteredMagazin.OrderBy(d => d.CreationDate).ToList());
            }
            else
            {
                FilteredMagazin = new ObservableCollection<MagazinDTO>(FilteredMagazin.OrderByDescending(d => d.CreationDate).ToList());
            }
            SelectedMagazin = FilteredMagazin[0];
        }

        public void Filter()
        {
            var filteredList = new List<MagazinDTO>(Magazins);

            if (SelectedCreator != Creators[0] && SelectedCreator != null)
            {
                filteredList = filteredList.Where(r => r.Creator.Id == SelectedCreator.Id).ToList();
            }

            FilteredMagazin.Clear();
            FilteredMagazin.AddRange(filteredList);

            if (FilteredMagazin.Any())
            {
                Sort();
                Message = "";
                IsFilteredListNotNull = true;
            }
            else
            {
                IsFilteredListNotNull = false;
                Message = "Не найдено";
            }
        }
    }
}
