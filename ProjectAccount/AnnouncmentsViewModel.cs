using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections;
using Prism.Mvvm;

namespace ProjectAccount
{
    public class AnnouncmentsViewModel : BindableBase
    {

        private string _login;
        public AnnouncmentsViewModel(string login, string occupation)
        {
            _announcments = ModelWorker2.SearchAnnouncement(idnew, occupation, login);
            _occupation = occupation;
            _login = login;
        }

        private DelegateCommand SearchAnnouncment;
        public DelegateCommand searchAnnouncment =>
            SearchAnnouncment ?? (SearchAnnouncment = new DelegateCommand(Convert));
        private string _occupation;
        public void Convert()
        {
            _announcments = ModelWorker2.SearchAnnouncement(idnew, _occupation, _login);

        }
        private DelegateCommand FilterAnnouncment;
        public DelegateCommand filterAnnouncment =>
            FilterAnnouncment ?? (FilterAnnouncment = new DelegateCommand(Convert2));

        public void Convert2()
        {
            _announcments = new List<Announcment>(ModelWorker2.FilterAnnouncments(_occupation, _filter, _type, _login));
        }

        public void AcceptOffer(Announcment an, string login, int deals, decimal price)
        {
            if (price >= 0)
            {
                ModelWorker2.AddOffer(an, _login, deals, price);
                _announcments = ModelWorker2.SearchAnnouncement(idnew, _occupation, _login);
            }
            else
            {
                MessageBox.Show("Wrong price!");
            }
        }
        private string _filter;
        public string filter
        {
            get => _filter;
            set
            {
                _filter = value;
            }
        }
        private string _type;
        public string type
        {
            get => _type;
            set
            {
                _type = value;
            }
        }
        private List<Announcment> announcments;

        public List<Announcment> _announcments
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
            }
        }
        static public string ReturnOccup(string name)
        {
            return ModelWorker2.ReturnOccupation(name);
        }
        public static void add_success()
        {
            MessageBox.Show("New offer has been added!");
        }

        public static void add_problem()
        {
            MessageBox.Show("Failed to add a new offer!");
        }

        public static void CatchException(Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
}
