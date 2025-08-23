using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectAccount
{
    public class Worker_RegistrationVM : INotifyDataErrorInfo
    {
        public RelayCommand AddWorkerCommand { get; set; }

        private string _name;
        [Required(ErrorMessage = "Name is required!")]
        public string name
        {
            get { return _name; }

            set
            {
                _name = value;
                Validate(nameof(name), value);
            }
        }
        private string _login;
        [Required(ErrorMessage = "Login is required!")]
        public string login
        {
            get { return _login; }

            set
            {
                _login = value;
                Validate(nameof(login), value);
            }
        }
        private string _pswrd;
        [Required(ErrorMessage = "Password is required!")]
        public string pswrd
        {
            get { return _pswrd; }

            set
            {
                _pswrd = value;
                Validate(nameof(pswrd), value);
            }
        }
        private string _occupation;
        [Required(ErrorMessage = "Occupation is required!")]
        public string occupation
        {
            get { return _occupation; }

            set
            {
                _occupation = value;
                Validate(nameof(occupation), value);
            }
        }
        public Worker_RegistrationVM()
        {
            AddWorkerCommand = new RelayCommand(AddUser, CanAddUser);

        }
        private bool CanAddUser(object obj)
        {
            //return true;
            return Validator.TryValidateObject(this, new ValidationContext(this), null);
        }

        private void AddUser(object obj)
        {
            try
            {
                if (name != null && login != null && pswrd != null &&
                    name != "" && login != "" && pswrd != "")
                {
                    ModelWorker2.AddUser(name = name, login = login, pswrd = pswrd, occupation = occupation);
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
            AddWorkerCommand.RaiseCanExecuteChanged();
        }
        public bool HasErrors => _propertyError.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public static void Success()
        {
            MessageBox.Show("Success!");
        }

        public static void Problem()
        {
            MessageBox.Show("Failure!");
        }

        public static void CatchException(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        public static void ExistingLogin()
        {
            MessageBox.Show("This login does already exist!");
        }

        public static void ExistingPassword()
        {
            MessageBox.Show("This password does already exist!");
        }
    }
}

