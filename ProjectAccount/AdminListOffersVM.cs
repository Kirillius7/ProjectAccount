using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Xml.Linq;
using Prism.Commands;
using Prism.Mvvm;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectAccount
{
    public class AdminListOffersVM : BindableBase
    {
        private DelegateCommand SearchAnnouncment;
        public DelegateCommand searchAnnouncment =>
            SearchAnnouncment ?? (SearchAnnouncment = new DelegateCommand(Convert));
        public DelegateCommand updateData =>
            UpdateData ?? (UpdateData = new DelegateCommand(Convert));

        private DelegateCommand UpdateData;
        public void Convert()
        {
            //_announcments = new List<AnnouncmentOffers>(ModelManager1.ReturnOffers(_idnew));
            _announcments = new List<AnnouncmentOffers>(ModelManager1.SearchOffer(_idnew));
        }

        private DelegateCommand FilterAnnouncment;
        public DelegateCommand filterAnnouncment =>
            FilterAnnouncment ?? (FilterAnnouncment = new DelegateCommand(Convert2));

        public void Convert2()
        {
            //_announcments = new List<AnnouncmentOffers>(ModelManager1.ReturnOffers2(_filter, _type));
            _announcments = new List<AnnouncmentOffers>(ModelManager1.FilterOffers(_filter, _type));

        }
        public void DeclineOffer(AnnouncmentOffers announcmentOffers)
        {
            if (announcmentOffers is null)
            {
                MessageBox.Show("Choose an item!");
            }
            else
            {
                ModelManager1.DeleteOffer(announcmentOffers.id, announcmentOffers.worker_login);
                _announcments = new List<AnnouncmentOffers>(ModelManager1.FilterOffers(_filter, _type));
            }
        }
        public void AcceptOffer(AnnouncmentOffers announcmentOffers)
        {
            if (announcmentOffers is null)
            {
                MessageBox.Show("Choose an item!");
            }
            else
            {
                ModelManager1.DealOffer(announcmentOffers);
                _announcments = new List<AnnouncmentOffers>(ModelManager1.FilterOffers(_filter, _type));
            }
        }
        private List<AnnouncmentOffers> announcments;

        public List<AnnouncmentOffers> _announcments
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

        public AdminListOffersVM()
        {
            //_announcments = new List<AnnouncmentOffers>(ModelManager1.ReturnOffers(_idnew));
            _announcments = new List<AnnouncmentOffers>(ModelManager1.SearchOffer(_idnew));
        }
        private bool CanIdentify(object obj)
        {
            //return true;
            return Validator.TryValidateObject(this, new ValidationContext(this), null);
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
        }

        //public static void Deal_Success()
        //{
        //    MessageBox.Show("New deal has been successfully added!");
        //}

        //public static void DeleteAnnouncment_Success()
        //{
        //    MessageBox.Show("The table of announcments has been updated!");
        //}

        //public static void wDataUpdate_Success()
        //{
        //    MessageBox.Show("The table of workers has been updated!");
        //}

        //public static void DeleteOffer_Success()
        //{
        //    MessageBox.Show("The table of offers has been updated!");
        //}

        //public static void DeleteOffer_Problem()
        //{
        //    MessageBox.Show("Failed to update the table of offers!");
        //}

        public static void wDataUpdate_Problem()
        {
            MessageBox.Show("Failed to update the table of workers!");
        }

        public static void Deal_Problem()
        {
            MessageBox.Show("An error occured while adding a new deal!");
        }
        public static void DeleteAnnouncment_Problem()
        {
            MessageBox.Show("Failed to update the table of announcments!");
        }

        public static void CatchException(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        public static void SuccessfulTransaction()
        {
            MessageBox.Show("The deal has been successfully signed!");
        }
        public static void CatchExceptionInput()
        {
            MessageBox.Show("Only integer is allowed!");
        }
    }
}
