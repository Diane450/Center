using Center.Services;
using ReactiveUI;
using System;
using Avalonia.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Center.ViewModels
{
    public class ReportWindowViewModel : ViewModelBase
    {
        public DateTime Today { get; set; } = DateTime.Now;

        private DateTime? _selectedDateStart = DateTime.Now;

        public DateTime? SelectedDateStart
        {
            get { return _selectedDateStart; }
            set
            {
                _selectedDateStart = this.RaiseAndSetIfChanged(ref _selectedDateStart, value);
            }
        }

        private DateTime? _selectedDateEnd = DateTime.Now;

        public DateTime? SelectedDateEnd
        {
            get { return _selectedDateEnd; }
            set
            {
                _selectedDateEnd = this.RaiseAndSetIfChanged(ref _selectedDateEnd, value);
            }
        }

        private string _message = null!;

        public string Message
        {
            get { return _message; }
            set { _message = this.RaiseAndSetIfChanged(ref _message, value); }
        }

        private ReportWindow _reportWindow = null!;

        public ReportWindow ReportWindow
        {
            get { return _reportWindow; }
            set { _reportWindow = this.RaiseAndSetIfChanged(ref _reportWindow, value); ; }
        }

        public ReportWindowViewModel(ReportWindow window)
        {
            ReportWindow = window;
        }

        public async Task CreateReport()
        {
            try
            {
                DateTime DateStart = (DateTime)SelectedDateStart!;
                DateTime DateEnd = (DateTime)SelectedDateEnd!;

                DateOnly[] range =
                [
                    new DateOnly (DateStart.Year, DateStart.Month, DateStart.Day),
                    new DateOnly (DateEnd.Year, DateEnd.Month, DateEnd.Day)
                ];
                Array.Sort(range);
                Report report = new(range);
                report.GetReportData();
                await report.CreateReport(ReportWindow);
            }
            catch
            {
                Message = "Ошибка соединения";
            }
        }
    }
}
