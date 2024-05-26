using Center.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Center.ModelsDTO
{
    public class MagazinDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id { get; set; }

        private string _title = null!;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        private DateOnly _creationDate;

        public DateOnly CreationDate
        {
            get { return _creationDate; }
            set
            {
                _creationDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CreationDate)));
            }
        }


        private int _count;

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            }
        }

        private byte[] _photo = null!;

        public byte[] Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Photo)));
            }
        }

        private Worker _creator = null!;

        public Worker Creator
        {
            get { return _creator; }
            set
            {
                _creator = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Creator)));
            }
        }

        private decimal _price;

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Price)));
            }
        }
        public static bool IsAdmin { get; set; }
    }
}
