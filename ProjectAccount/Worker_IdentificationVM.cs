using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectAccount
{
    public class Worker_IdentificationVM : INotifyDataErrorInfo
    {
        public RelayCommand IdentifyUserCommand { get; set; }

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

        private bool check { get; set; }
        private bool callMehtod { get; set; }

        public Worker_IdentificationVM()
        {
            IdentifyUserCommand = new RelayCommand(IdentifyUser, CanIdentify);
        }
        public bool CheckUser()
        {
            bool check1 = false;
            object oobb = null;
            callMehtod = false;
            IdentifyUser(oobb);
            try
            {
                if (check)
                    check1 = true;
                else
                    check1 = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            callMehtod = true;
            return check1;
        }
        private bool CanIdentify(object obj)
        {
            //return true;
            return Validator.TryValidateObject(this, new ValidationContext(this), null);
        }

        private void IdentifyUser(object obj)
        {
            check = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(pswrd) && callMehtod != true)
                {
                    if (ModelWorker2.AccessUser(login, pswrd))
                        check = true;
                    else
                        check = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool HasErrors => _propertyError.Count > 0;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

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
            IdentifyUserCommand.RaiseCanExecuteChanged();
        }

        public static void Success()
        {
            MessageBox.Show("Success!");
        }

        public static void Problem()
        {
            MessageBox.Show("User doesn't exist!");
        }
    }
}
