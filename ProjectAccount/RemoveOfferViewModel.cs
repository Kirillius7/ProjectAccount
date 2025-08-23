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
using Prism.Mvvm;

namespace ProjectAccount
{
    public class RemoveOfferViewModel : BindableBase, INotifyDataErrorInfo
    {
        public RelayCommand RemoveOfferCommand { get; set; }
        private DelegateCommand SearchAnnouncment;
        public DelegateCommand searchAnnouncment =>
            SearchAnnouncment ?? (SearchAnnouncment = new DelegateCommand(Convert1));
        //public DelegateCommand updateData =>
        //    UpdateData ?? (UpdateData = new DelegateCommand(Convert1));

        //private DelegateCommand UpdateData;
        public void Convert1()
        {
            //_announcments = new List<Announcment>(ModelManager1.ReturnProposals(_idnew));
            _announcments = new List<Announcment>(ModelManager1.SearchAnnouncment(_idnew));

        }
        private DelegateCommand FilterAnnouncment;
        public DelegateCommand filterAnnouncment =>
            FilterAnnouncment ?? (FilterAnnouncment = new DelegateCommand(Convert2));

        public void Convert2()
        {
            //_announcments = new List<Announcment>(ModelManager1.ReturnProposals2(_filter, _type));
            _announcments = new List<Announcment>(ModelManager1.FilterAnnouncments(_filter, _type));
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
        private List<Announcment> announcments;

        public List<Announcment> _announcments
        {
            get => announcments;
            set => SetProperty(ref announcments, value);
        }

        private string _idnew;
        //[Required(ErrorMessage = "_idnew is required!")]
        public string idnew
        {
            get => _idnew;
            set
            {
                _idnew = value;
                Validate(nameof(idnew), value);
            }
        }
        public string _id;
        [Required(ErrorMessage = "Name is required!")]
        public string id
        {
            get { return _id; }

            set
            {
                _id = value;
                Validate(nameof(id), value);
            }
        }
        public bool HasErrors => _propertyError.Count > 0;

        public RemoveOfferViewModel()
        {
            RemoveOfferCommand = new RelayCommand(AddCmmdt, CanAddCmdt);
            //_announcments = new List<Announcment>(ModelManager1.ReturnProposals(_idnew));
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
                if (!string.IsNullOrWhiteSpace(_id))
                {
                    int idUnit = Convert.ToInt32(_id);
                    if(idUnit != 0)
                    {
                        MessageBox.Show("Submitted");
                        ModelManager1.DeleteOffer(idUnit);
                        _announcments = new List<Announcment>(ModelManager1.SearchAnnouncment(_idnew));
                    }
                }
                else
                {
                    MessageBox.Show("Fill all fields!");
                }
            }
            catch
            {
                MessageBox.Show("Wrong input!");
            }
        }

        //public void Login(string login)
        //{
        //    us_login = login;
        //}
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
            RemoveOfferCommand.RaiseCanExecuteChanged();
        }

        public static void Success()
        {
            MessageBox.Show("The announcment has been deleted!");
        }

        public static void Problem()
        {
            MessageBox.Show("Failed to delete the announcment!");
        }

        public static void SuccessfulRemove()
        {
            MessageBox.Show("The table of offers has been updated!");
        }

        public static void UnsuccessfulRemove()
        {
            MessageBox.Show("Failed to update the table of offers!");
        }

        public static void CatchException(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
