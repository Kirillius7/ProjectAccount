using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ProjectAccount
{
    public class ModelWorker2
    {
        public static void AddUser(string name, string login, string pswrd, string occupation)
        {
            Connection c = new Connection();
            c.OpenConnection();

            string selectLogin = "select worker_login from account_project.workers where worker_login = '" + login + "'";

            MySqlCommand ex1 = new MySqlCommand(selectLogin, c.con);

            string selectPswrd = "select worker_pswrd from account_project.workers where worker_pswrd = '" + pswrd + "'";

            MySqlCommand ex2 = new MySqlCommand(selectPswrd, c.con);
            string lg = " ";
            string pd = " ";
            if (ex1.ExecuteScalar() != null)
                lg = ex1.ExecuteScalar().ToString();

            if (ex2.ExecuteScalar() != null)
                pd = ex2.ExecuteScalar().ToString();

            c.CloseConnection();
            try
            {
                if (lg == login)
                {
                    Worker_RegistrationVM.ExistingLogin();
                }
                if (pd == pswrd)
                {
                    Worker_RegistrationVM.ExistingPassword();
                }
                else if (lg != login && pd != pswrd)
                {
                    c.OpenConnection();

                    string insertQuery = "INSERT INTO account_project.workers(worker_name, worker_login, worker_pswrd, worker_occupation, num_deals) VALUES ('" + name + "','" + login + "','" + pswrd + "','" + occupation + "','" + 0 + "')";

                    MySqlCommand exec = new MySqlCommand(insertQuery, c.con);
                    if (exec.ExecuteNonQuery() > -1)
                    {
                        Worker_RegistrationVM.Success();
                    }
                    else
                    {
                        Worker_RegistrationVM.Problem();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Worker_RegistrationVM.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }
        }

        public static bool AccessUser(string login, string pswrd)
        {
            Connection c = new Connection();
            c.OpenConnection();
            string comText = "SELECT * FROM account_project.workers WHERE worker_login = '" + login + "' AND worker_pswrd = '" + pswrd + "'";
            bool truth = false;
            MySqlCommand cmd = c.con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = comText;
            cmd.ExecuteNonQuery();
            int i = 0;
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            i = Convert.ToInt32(dt.Rows.Count.ToString());
            if (i == 0)
                Worker_IdentificationVM.Problem();
            else
            {
                Worker_IdentificationVM.Success();
                truth = true;
            }
            c.CloseConnection();
            return truth;
        }
        public static ObservableCollection<Announcment> _DataBaseC = new ObservableCollection<Announcment>();

        public static List<Announcment> SearchAnnouncement(string type, string id, string login)
        {
            Connection c = new Connection();
            string insertQuery = " ";
            c.OpenConnection();
            if (string.IsNullOrWhiteSpace(type))
            {
                //insertQuery = "SELECT * FROM account_project.datatest";
                insertQuery = "SELECT * FROM account_project.datatest WHERE type_product = '" + id + "'";
            }
            else
            {
                int _id = 0;
                try
                {
                    _id = Convert.ToInt32(type);
                    insertQuery = "SELECT * FROM account_project.datatest WHERE id LIKE '" + _id + "' AND type_product = '" + id + "'";
                }
                catch
                {
                    MessageBox.Show("Only integer is allowed!");
                    //insertQuery = "SELECT * FROM account_project.datatest";
                    insertQuery = "SELECT * FROM account_project.datatest WHERE type_product = '" + id + "'";
                }

            }
            //_Deals.Clear();
            List<Announcment> deal = new List<Announcment>();

            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Announcment dl;
                dl = new Announcment();
                dl.id = Convert.ToInt32(reader[0]);
                dl.nameprod = reader[1].ToString();
                dl.nameprob = reader[2].ToString();
                dl.typeprob = reader[3].ToString();
                dl.state = reader[4].ToString();
                dl.urgency = reader[5].ToString();

                deal.Add(dl);
            }
            c.CloseConnection();
            c.OpenConnection();
            string insertQuery2 = "SELECT * FROM account_project.workersoffers WHERE login_worker = '" + login + "'";

            List<Announcment> deal2 = new List<Announcment>();

            MySqlCommand cmd2 = new MySqlCommand(insertQuery2, c.con);

            MySqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                Announcment dl2;
                dl2 = new Announcment();
                dl2.id = Convert.ToInt32(reader2[0]);
                dl2.nameprod = reader2[1].ToString();
                dl2.nameprob = reader2[2].ToString();
                dl2.typeprob = reader2[3].ToString();
                dl2.state = reader2[4].ToString();
                dl2.urgency = reader2[5].ToString();

                deal2.Add(dl2);
            }
            c.CloseConnection();

            //List<Announcment> distinctList = deal.Except(deal2).ToList();
            List<Announcment> distinctList = new List<Announcment>();
            if (deal2.Count != 0)
            {
                distinctList = GetDistinctData(deal, deal2, d => d.id);
            }
            else
            {
                distinctList = new List<Announcment>(deal);
            }
            return distinctList;
        }
        public static List<Announcment> GetDistinctData(List<Announcment> firstList, List<Announcment> secondList, Func<Announcment, int> idSelector)
        {
            HashSet<int> secondIds = new HashSet<int>(secondList.Select(idSelector));

            List<Announcment> distinctData = firstList.Where(d => !secondIds.Contains(idSelector(d))).ToList();

            return distinctData;
        }
        public static List<Announcment> FilterAnnouncments(string occupation, string filter, string type, string login)
        {
            Connection c = new Connection();
            string insertQuery = " ";

            c.OpenConnection();
            if ((string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(type)) ||
                (string.IsNullOrWhiteSpace(filter)) || (string.IsNullOrWhiteSpace(type)))
            {
                //insertQuery = "SELECT * FROM account_project.datatest";
                insertQuery = "SELECT * FROM account_project.datatest WHERE type_product = '" + occupation + "'";
            }
            else
            {
                try
                {
                    string _filter = filter switch
                    {
                        //"nameprod" => "name_product",
                        //"nameprob" => "name_problem",
                        //"state" => "state_product",
                        //"urgency" => "urgency",
                        "Name" => "name_product",
                        "Problem" => "name_problem",
                        "State" => "state_product",
                        "Urgency" => "urgency"
                    };
                    insertQuery = _filter switch
                    {
                        "name_product" => "SELECT * FROM account_project.datatest WHERE name_product LIKE '" + type + "' AND type_product = '" + occupation + "'",
                        "name_problem" => "SELECT * FROM account_project.datatest WHERE name_problem LIKE '" + type + "'AND type_product = '" + occupation + "'",                        
                        "state_product" => "SELECT * FROM account_project.datatest WHERE state_product LIKE '" + type + "'AND type_product = '" + occupation + "'",
                        "urgency" => "SELECT * FROM account_project.datatest WHERE urgency LIKE '" + type + "' AND type_product = '" + occupation + "'"
                    };

                }
                catch
                {
                    MessageBox.Show("An error has happened!");
                    insertQuery = "SELECT * FROM account_project.datatest";
                }
            }
            //_Deals.Clear();
            List<Announcment> deal = new List<Announcment>();

            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Announcment dl;
                dl = new Announcment();
                dl.id = Convert.ToInt32(reader[0]);
                dl.nameprod = reader[1].ToString();
                dl.nameprob = reader[2].ToString();
                dl.typeprob = reader[3].ToString();
                dl.state = reader[4].ToString();
                dl.urgency = reader[5].ToString();

                deal.Add(dl);
            }
            c.CloseConnection();

            //return deal;
            c.OpenConnection();
            string insertQuery2 = "SELECT * FROM account_project.workersoffers WHERE login_worker = '" + login + "'";

            List<Announcment> deal2 = new List<Announcment>();

            MySqlCommand cmd2 = new MySqlCommand(insertQuery2, c.con);

            MySqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                Announcment dl2;
                dl2 = new Announcment();
                dl2.id = Convert.ToInt32(reader2[0]);
                dl2.nameprod = reader2[1].ToString();
                dl2.nameprob = reader2[2].ToString();
                dl2.typeprob = reader2[3].ToString();
                dl2.state = reader2[4].ToString();
                dl2.urgency = reader2[5].ToString();

                deal2.Add(dl2);
            }
            c.CloseConnection();

            //List<Announcment> distinctList = deal.Except(deal2).ToList();
            List<Announcment> distinctList = new List<Announcment>();
            if (deal2.Count != 0)
            {
                distinctList = GetDistinctData(deal, deal2, d => d.id);
            }
            else
            {
                distinctList = new List<Announcment>(deal);
            }

            return distinctList;
        }
        public static List<AnnouncmentOffers> SearchOffer(string login, string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";
            c.OpenConnection();
            if (string.IsNullOrWhiteSpace(id))
            {
                //insertQuery = "SELECT * FROM account_project.datatest";
                insertQuery = "SELECT * FROM account_project.workersoffers WHERE login_worker = '" + login + "'";
            }
            else
            {
                int _id = 0;
                try
                {
                    _id = Convert.ToInt32(id);
                    insertQuery = "SELECT * FROM account_project.workersoffers WHERE login_worker LIKE '" + login + "' AND id = '" + _id + "'";
                }
                catch
                {
                    MessageBox.Show("Only integer is allowed!");
                    //insertQuery = "SELECT * FROM account_project.datatest";
                    insertQuery = "SELECT * FROM account_project.workersoffers WHERE login_worker = '" + login + "'";
                }

            }
            //_Deals.Clear();
            List<AnnouncmentOffers> deal = new List<AnnouncmentOffers>();

            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                AnnouncmentOffers dl;
                dl = new AnnouncmentOffers();
                dl.id = Convert.ToInt32(reader[0]);
                dl.nameprod = reader[1].ToString();
                dl.nameprob = reader[2].ToString();
                dl.typeprob = reader[3].ToString();
                dl.state = reader[4].ToString();
                dl.urgency = reader[5].ToString();
                dl.price = Convert.ToDecimal(reader[8]);
                deal.Add(dl);
            }
            c.CloseConnection();
            return deal;
        }
        public static List<AnnouncmentOffers> FilterOffers(string login, string filter, string type)
        {
            Connection c = new Connection();
            string insertQuery = " ";

            c.OpenConnection();
            if ((string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(type)) ||
                (string.IsNullOrWhiteSpace(filter)) || (string.IsNullOrWhiteSpace(type)))
            {
                //insertQuery = "SELECT * FROM account_project.datatest";
                insertQuery = "SELECT * FROM account_project.workersoffers WHERE login_worker = '" + login + "'";
            }
            else
            {
                try
                {
                    string _filter = filter switch
                    {
                        //"nameprod" => "name_product",
                        //"nameprob" => "name_problem",
                        //"state" => "state_product",
                        //"urgency" => "urgency",
                        "Name" => "name_product",
                        "Problem" => "name_problem",
                        "State" => "state_product",
                        "Urgency" => "urgency"
                    };
                    insertQuery = _filter switch
                    {
                        "name_product" => "SELECT * FROM account_project.workersoffers WHERE login_worker LIKE '" + login + "' AND name_product = '" + type + "'",
                        "name_problem" => "SELECT * FROM account_project.workersoffers WHERE login_worker LIKE '" + login + "'AND name_problem = '" + type + "'",
                        "state_product" => "SELECT * FROM account_project.workersoffers WHERE login_worker LIKE '" + login + "'AND state_product = '" + type + "'",
                        "urgency" => "SELECT * FROM account_project.workersoffers WHERE login_worker LIKE '" + login + "' AND urgency = '" + type + "'"
                    };

                }
                catch
                {
                    MessageBox.Show("An error has happened!");
                    insertQuery = "SELECT * FROM account_project.datatest";
                }
            }
            //_Deals.Clear();
            List<AnnouncmentOffers> deal = new List<AnnouncmentOffers>();

            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                AnnouncmentOffers dl;
                dl = new AnnouncmentOffers();
                dl.id = Convert.ToInt32(reader[0]);
                dl.nameprod = reader[1].ToString();
                dl.nameprob = reader[2].ToString();
                dl.typeprob = reader[3].ToString();
                dl.state = reader[4].ToString();
                dl.urgency = reader[5].ToString();
                dl.price = Convert.ToDecimal(reader[8]);
                deal.Add(dl);
            }
            c.CloseConnection();

            return deal;
        }
        public static string ReturnOccupation(string login)
        {
            Connection c = new Connection();
            c.OpenConnection();
            string insertQuery = "SELECT * FROM account_project.workers WHERE worker_login = '" + login + "'";
            _DataBaseC.Clear();
            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);
            MySqlDataReader reader = cmd.ExecuteReader();
            string occupation = " ";
            while (reader.Read())
            {
                occupation = reader[3].ToString();
            }

            c.CloseConnection();
            return occupation;
        }
        public static int ReturnDeals(string login)
        {
            Connection c = new Connection();
            c.OpenConnection();
            string insertQuery = "SELECT num_deals FROM account_project.workers WHERE worker_login = '" + login + "'";
            _DataBaseC.Clear();
            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);
            MySqlDataReader reader = cmd.ExecuteReader();
            int occupation = 0;
            while (reader.Read())
            {
                occupation = Convert.ToInt32(reader[0]);
            }

            c.CloseConnection();
            return occupation;
        }
        public static void AddOffer(Announcment an, string login, int deals, decimal price)
        {
            Connection c = new Connection();
            c.OpenConnection();
            if (an.nameprod is not null && login is not null)
            {
                try
                {
                    c.OpenConnection();

                    string insertQuery = "INSERT INTO account_project.workersoffers" +
                    "(id, name_product, name_problem, type_product, state_product, urgency, login_worker, deals, price) VALUES " +
                    "('" + an.id + "','" + an.nameprod + "','" + an.nameprob + "','" + an.typeprob + "','" + an.state +
                    "','" + an.urgency + "','" + login + "','" + deals + "','" + price + "')";

                    MySqlCommand exec = new MySqlCommand(insertQuery, c.con);
                    if (exec.ExecuteNonQuery() > -1)
                    {
                        AnnouncmentsViewModel.add_success();
                    }
                    else
                    {
                        AnnouncmentsViewModel.add_problem();
                    }
                    c.CloseConnection();
                }
                catch (Exception ex)
                {
                    AnnouncmentsViewModel.CatchException(ex);
                }
            }
            else
            {
                MessageBox.Show("Choose an announcment!");
            }
        }
        public static void DeleteOffer(int id, string login_worker)
        {

            Connection c = new Connection();
            c.OpenConnection();
            string insertQuery = "DELETE FROM account_project.workersoffers WHERE id LIKE '" + id + "' " +
                "AND login_worker = '" + login_worker + "'";
            try
            {
                MySqlCommand exec = new MySqlCommand(insertQuery, c.con);
                if (exec.ExecuteNonQuery() > -1)
                {
                    WorkListOffersVM.remove_success();
                }
                else
                {
                    WorkListOffersVM.remove_problem();
                }
            }
            catch (Exception ex)
            {
                WorkListOffersVM.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }

        }
        public static string ReturnName(string login)
        {
            Connection c = new Connection();
            c.OpenConnection();
            string insertQuery = "SELECT * FROM account_project.workers WHERE worker_login = '" + login + "'";
            _DataBaseC.Clear();
            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);
            MySqlDataReader reader = cmd.ExecuteReader();
            string occupation = " ";
            while (reader.Read())
            {
                occupation = reader[0].ToString();
            }

            c.CloseConnection();
            return occupation;
        }

        public static List<Deals> SearchDeal(string login, string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";
            c.OpenConnection();
            if (string.IsNullOrWhiteSpace(id))
            {
                insertQuery = "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "'";
            }
            else
            {
                int _id = 0;
                try
                {
                    _id = Convert.ToInt32(id);
                    insertQuery = "SELECT * FROM account_project.deals WHERE id LIKE '" + id + "' AND login_worker = '" + login + "'";
                }
                catch
                {
                    //MessageBox.Show("Only integer is allowed!");
                    AdminWorkingSpaceVM.CatchExceptionInput();
                    insertQuery = "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "'";
                }

            }
            //_Deals.Clear();
            List<Deals> deal = new List<Deals>();

            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Deals an;
                an = new Deals();
                an.id = Convert.ToInt32(reader[0].ToString());
                an.nameprod = reader[1].ToString();
                an.nameprob = reader[2].ToString();
                an.typeprob = reader[3].ToString();
                an.state = reader[4].ToString();
                an.urgency = reader[5].ToString();
                an.price = Convert.ToDecimal(reader[7]);
                an.date = reader[8].ToString();

                deal.Add(an);
            }
            c.CloseConnection();

            return deal;
        }
        public static List<Deals> FilterDeals(string login, string type, string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";

            c.OpenConnection();
            if ((string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(type)) ||
                (string.IsNullOrWhiteSpace(id)) || (string.IsNullOrWhiteSpace(type)))
            {
                insertQuery = "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "'";
            }
            else
            {
                try
                {
                    string _type = type switch
                    {
                        //"nameprod" => "name_product",
                        //"nameprob" => "name_problem",
                        //"state" => "state_product",
                        //"urgency" => "urgency",
                        "Name" => "name_product",
                        "Problem" => "name_problem",
                        "State" => "state_product",
                        "Urgency" => "urgency",
                        "Date" => "date"
                    };
                    insertQuery = _type switch
                    {
                        "name_product" => "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "' AND name_product = '" + id + "'",
                        "name_problem" => "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "' AND name_problem = '" + id + "'",
                        "state_product" => "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "' AND state_product = '" + id + "'",
                        "urgency" => "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "' AND urgency = '" + id + "'",
                        "date" => "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "' AND date_deal = '" + id + "'"
                    };

                }
                catch (Exception ex)
                {
                    AdminWorkingSpaceVM.CatchException(ex);
                    insertQuery = "SELECT * FROM account_project.deals WHERE login_worker = '" + login + "'";
                }
            }
            //_Deals.Clear();
            List<Deals> deal = new List<Deals>();

            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Deals an;
                an = new Deals();
                an.id = Convert.ToInt32(reader[0].ToString());
                an.nameprod = reader[1].ToString();
                an.nameprob = reader[2].ToString();
                an.typeprob = reader[3].ToString();
                an.state = reader[4].ToString();
                an.urgency = reader[5].ToString();
                an.price = Convert.ToDecimal(reader[7]);
                an.date = reader[8].ToString();

                deal.Add(an);
            }
            c.CloseConnection();

            return deal;
        }
    }
}
