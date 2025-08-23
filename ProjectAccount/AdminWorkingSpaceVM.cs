using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using System.Windows;
using Prism.Mvvm;

namespace ProjectAccount
{
    public class AdminWorkingSpaceVM : BindableBase
    {
        private DelegateCommand SearchAnnouncment;
        public DelegateCommand searchAnnouncment =>
            SearchAnnouncment ?? (SearchAnnouncment = new DelegateCommand(Convert));

        public void Convert()
        {
            //_announcments = new List<Deals>(ModelManager1.ReturnDeals(_idnew));
            _announcments = new List<Deals>(ModelManager1.SearchDeal(_idnew));

        }
        private DelegateCommand FilterAnnouncment;
        public DelegateCommand filterAnnouncment =>
            FilterAnnouncment ?? (FilterAnnouncment = new DelegateCommand(Convert2));

        public void Convert2()
        {
            //_announcments = new List<Deals>(ModelManager1.ReturnDeals2(_filter, _type));
            _announcments = new List<Deals>(ModelManager1.FilterDeals(_filter, _type));
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
        private List<Deals> announcments;

        public List<Deals> _announcments
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
