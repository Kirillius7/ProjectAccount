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
        private string _login;
        public WorkListOffersVM(string login)
        {
            _announcments = ModelWorker2.SearchOffer(login, idnew);
            _login = login;
        }

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
            _announcments = new List<AnnouncmentOffers>(ModelWorker2.FilterOffers(_login, _filter, _type));
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
        static public string ReturnOccup(string name)
        {
            return ModelWorker2.ReturnOccupation(name);
        }     
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
