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
        private string _idnew;
        public string idnew
        {
            get => _idnew;
            set
            {
                _idnew = value;
            }
        }

        public AdminListOffersVM()
        {
            //_announcments = new List<AnnouncmentOffers>(ModelManager1.ReturnOffers(_idnew));
            _announcments = new List<AnnouncmentOffers>(ModelManager1.SearchOffer(_idnew));
        }    
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
