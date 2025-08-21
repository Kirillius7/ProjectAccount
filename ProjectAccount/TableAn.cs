using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectAccount
{
    public class TableAn
    {
        public ICommand ShowWindowCommand { get; set; }
        public ObservableCollection<Announcment> Announcments { get; set; }

        public TableAn()
        {
            Announcments = ModelManager1.ReturnAnnouncements();
            ShowWindowCommand = new RelayCommand(ShowWindow, CanShowWindow);
        }

        private bool CanShowWindow(object obj)
        {
            return true;
        }
        //public void TransferData(string name)
        //{
        //    ModelManager1.ReturnUserDB(name);
        //}
        private void ShowWindow(object obj) { }
    }
}
