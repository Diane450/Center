using ReactiveUI;
using System;

namespace Center.ViewModels
{
    public class AuthorizeWindowViewModel : ViewModelBase
    {
        private string _code = null!;
        public string Code
        {
            get { return _code; }
            set { _code = this.RaiseAndSetIfChanged(ref _code, value); }
        }

        private string _login = null!;
        public string Login
        {
            get { return _login; }
            set { _login = this.RaiseAndSetIfChanged(ref _login, value); }
        }

        private bool _isAuthEnable;
        public bool IsAuthEnable
        {
            get { return _isAuthEnable; }
            set { _isAuthEnable = this.RaiseAndSetIfChanged(ref _isAuthEnable, value); }
        }

        public AuthorizeWindowViewModel()
        {
            this.WhenAnyValue(x => x.Code, x => x.Login).Subscribe(_ => Enable());
        }
        private void Enable()
        {
            IsAuthEnable = !string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Login);
        }
    }
}