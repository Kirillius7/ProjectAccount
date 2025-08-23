using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using System.Security.Policy;

namespace ProjectAccount
{
    public class AddOfferViewModel : BindableBase, INotifyDataErrorInfo
    {
        public RelayCommand AddOfferCommand { get; set; }

        //private DelegateCommand UpdateData;

        //public DelegateCommand updateData => 
        //    UpdateData ?? (UpdateData = new DelegateCommand(Convert));

        private DelegateCommand SearchAnnouncment;
        public DelegateCommand searchAnnouncment =>
            SearchAnnouncment ?? (SearchAnnouncment = new DelegateCommand(Convert));

        public void Convert()
        {
            _announcments = new List<Announcment>(ModelManager1.SearchAnnouncment(_idnew));
        }

        private DelegateCommand FilterAnnouncment;
        public DelegateCommand filterAnnouncment =>
            FilterAnnouncment ?? (FilterAnnouncment = new DelegateCommand(Convert2));

        public void Convert2()
        {
            _announcments = new List<Announcment>(ModelManager1.FilterAnnouncments(_filter, _type));
        }

        private List<Announcment> announcments;

        public List<Announcment> _announcments
        {
            get => announcments;
            set => SetProperty(ref announcments, value);
        }
        public string us_login { get; set; }

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

        private string _nameprod;
        [Required(ErrorMessage = "Name is required!")]
        public string nameprod
        {
            get { return _nameprod; }

            set
            {
                _nameprod = value;
                Validate(nameof(nameprod), value);
            }
        }

        private string _nameprob;
        [Required(ErrorMessage = "Problem is required!")]
        public string nameprob
        {
            get { return _nameprob; }

            set
            {
                _nameprob = value;
                Validate(nameof(nameprob), value);
            }
        }


        private string _typeprod;
        [Required(ErrorMessage = "Type of product is required!")]
        public string typeprod
        {
            get { return _typeprod; }

            set
            {
                _typeprod = value;
                Validate(nameof(typeprod), value);
            }
        }
        private string _state;
        [Required(ErrorMessage = "State is required!")]
        public string state
        {
            get { return _state; }

            set
            {
                _state = value;
                Validate(nameof(state), value);
            }
        }

        private string _urgency;
        [Required(ErrorMessage = "Urgency is required!")]
        public string urgency
        {
            get { return _urgency; }

            set
            {
                _urgency = value;
                Validate(nameof(urgency), value);
            }
        }

        public bool HasErrors => _propertyError.Count > 0;

        public AddOfferViewModel()
        {
            AddOfferCommand = new RelayCommand(AddCmmdt, CanAddCmdt);
            _announcments = new List<Announcment>(ModelManager1.SearchAnnouncment(_idnew));

        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private bool CanAddCmdt(object obj)
        {
            return Validator.TryValidateObject(this, new ValidationContext(this), null);
        }

        private void AddCmmdt(object obj)
        {
            try
            {
                if(!string.IsNullOrWhiteSpace(_nameprod) && !string.IsNullOrWhiteSpace(_nameprob) &&
                        !string.IsNullOrWhiteSpace(_typeprod) && !string.IsNullOrWhiteSpace(_state) &&
                        !string.IsNullOrWhiteSpace(_urgency))
                {
                        MessageBox.Show("Submitted");
                        ModelManager1.AddOffer(_nameprod = _nameprod, _nameprob = _nameprob, _typeprod = _typeprod, state = state, _urgency = _urgency);
                        _announcments = new List<Announcment>(ModelManager1.SearchAnnouncment(_idnew));
                }
                else
                {
                    MessageBox.Show("Fill all fields!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Login(string login)
        {
            us_login = login;
        }
        private readonly Dictionary<string, List<string>> _propertyError = new Dictionary<string, List<string>>();
        public IEnumerable GetErrors(string propertyName)
        {
            if (_propertyError.ContainsKey(propertyName))
                return _propertyError[propertyName];
            else
                return Enumerable.Empty<string>();
        }

        //public void AddError(string propertyName, string ErrorName)
        //{
        //    if (_propertyError.ContainsKey(propertyName))
        //    {
        //        _propertyError.Add(propertyName, new List<string>());
        //    }

        //    _propertyError[propertyName].Add(ErrorName);
        //}
        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void Validate(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);
            try
            {

                //if (results.Any())
                //{
                //    _propertyError.Add(propertyName, results.Select(r => r.ErrorMessage).ToList());
                //    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                //}
                //else
                //{
                //    _propertyError.Remove(propertyName);
                //    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
                //}
                if (results.Any())
                {
                    _propertyError[propertyName] = results.Select(r => r.ErrorMessage).ToList();
                }
                else
                {
                    _propertyError.Remove(propertyName);
                }
                OnErrorsChanged(propertyName);
            }
            catch
            {
                MessageBox.Show("Fill the field!");
            }
            AddOfferCommand.RaiseCanExecuteChanged();
        }

        public static void Success()
        {
            MessageBox.Show("New announcment has been added!");
        }

        public static void Problem()
        {
            MessageBox.Show("Failed to add a new announcment!");
        }

        public static void CatchException(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        public static void CatchExceptionInput()
        {
            MessageBox.Show("Only integer is allowed!");
        }
    }
}
