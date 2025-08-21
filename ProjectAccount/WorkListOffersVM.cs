using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.Windows.Input;
using System.Windows;
using Prism.Mvvm;
using System.Security.Cryptography;

namespace ProjectAccount
{
    public class WorkListOffersVM : BindableBase
    {
        public ICommand ShowWindowWorkerAnnouncmentCommand { get; set; }
        //public ObservableCollection<Announcment> Announcments { get; set; }
        private string _login;
        public WorkListOffersVM(string login)
        {
            //_announcments = ModelWorker2.ReturnListOffers(login, idnew);
            _announcments = ModelWorker2.SearchOffer(login, idnew);
            _login = login;
            //ShowWindowWorkerAnnouncmentCommand = new RelayCommand(ShowWindow, CanShowWindow);
        }
        public DelegateCommand updateData =>
                UpdateData ?? (UpdateData = new DelegateCommand(Convert));

        private DelegateCommand UpdateData;
        private DelegateCommand SearchAnnouncment;
        public DelegateCommand searchAnnouncment =>
            SearchAnnouncment ?? (SearchAnnouncment = new DelegateCommand(Convert));
        private string _occupation;
        public void Convert()
        {
            _announcments = new List<AnnouncmentOffers>(ModelWorker2.SearchOffer(_login, idnew));
        }
        private DelegateCommand FilterAnnouncment;
        public DelegateCommand filterAnnouncment =>
            FilterAnnouncment ?? (FilterAnnouncment = new DelegateCommand(Convert2));

        public void Convert2()
        {
            //_announcments = new List<AnnouncmentOffers>(ModelWorker2.ReturnListOffers2(_login, _filter, _type));
            _announcments = new List<AnnouncmentOffers>(ModelWorker2.FilterOffers(_login, _filter, _type));
        }
        private string _filter;
        public string filter
        {
            get => _filter;
            set
            {
                _filter = value;
                Validate(nameof(filter), value);
            }
        }
        private string _type;
        public string type
        {
            get => _type;
            set
            {
                _type = value;
                Validate(nameof(type), value);
            }
        }
        private List<AnnouncmentOffers> announcments;

        public List<AnnouncmentOffers> _announcments
        {
            get => announcments;
            set => SetProperty(ref announcments, value);
        }

        private string _idnew;
        public string idnew
        {
            get => _idnew;
            set
            {
                _idnew = value;
                Validate(nameof(idnew), value);
            }
        }
        public void DeclineOffer(int id)
        {
            if (id == 0)
            {
                MessageBox.Show("Choose an item!");
            }
            else
            {
                ModelWorker2.DeleteOffer(id, _login);
                _announcments = ModelWorker2.SearchOffer(_login, idnew);
            }
        }
        private bool CanShowWindow(object obj)
        {
            return true;
        }

        private void ShowWindow(object obj) { }
        static public string ReturnOccup(string name)
        {
            return ModelWorker2.ReturnOccupation(name);
        }
        public void Validate(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);
            try
            {

                if (results.Any())
                {
                    _propertyError.Add(propertyName, results.Select(r => r.ErrorMessage).ToList());
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }
                else
                {
                    _propertyError.Remove(propertyName);
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                }
            }
            catch
            {
                MessageBox.Show("Fill the field!");
            }
        }
        private readonly Dictionary<string, List<string>> _propertyError = new Dictionary<string, List<string>>();
        public IEnumerable GetErrors(string propertyName)
        {
            if (_propertyError.ContainsKey(propertyName))
                return _propertyError[propertyName];
            else
                return Enumerable.Empty<string>();
        }

        public void AddError(string propertyName, string ErrorName)
        {
            if (_propertyError.ContainsKey(propertyName))
            {
                _propertyError.Add(propertyName, new List<string>());
            }

            _propertyError[propertyName].Add(ErrorName);
        }
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public static void remove_success()
        {
            MessageBox.Show("The offer has been removed!");
        }

        public static void remove_problem()
        {
            MessageBox.Show("Failed to remove the offer!");
        } 
        
        public static void CatchException(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
