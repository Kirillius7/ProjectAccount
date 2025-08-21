using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace ProjectAccount
{
    class ModelManager1
    {
        public static bool AccessUser(string login, string pswrd)
        {
            Connection c = new Connection();
            c.OpenConnection();
            string comText = "SELECT * FROM account_project.ceo WHERE CEO_login = '" + login + "' AND CEO_password = '" + pswrd + "'";
            bool truth = false;
            try
            {
                MySqlCommand cmd = c.con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = comText;
                cmd.ExecuteNonQuery();
                int i = 0;
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                i = Convert.ToInt32(dt.Rows.Count.ToString());
                DateTime dtNow = DateTime.Now;
                if (i == 0)
                {
                    Admin_IdentificationVM.Problem();
                }
                else
                {
                    Admin_IdentificationVM.Success();
                    truth = true;
                }
            }
            catch(Exception ex)
            {
                Admin_IdentificationVM.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }
            
            return truth;
        }
        public static void AddOffer(string nameprod, string nameprob, string typeprod, string state, string urgency)
        {
            Connection c = new Connection();
            c.OpenConnection();
            string insertQuery = "INSERT INTO account_project.datatest(name_product, name_problem, type_product, state_product, urgency) VALUES " +
                "('" + nameprod + "','" + nameprob + "','" + typeprod + "','" + state + "','" + urgency + "')";
            
            DateTime dtNow = DateTime.Now;
            bool successfulOp = false;
            try
            {
                MySqlCommand exec1 = new MySqlCommand(insertQuery, c.con);
                if (exec1.ExecuteNonQuery() > -1)
                {
                    AddOfferViewModel.Success();
                    successfulOp = true;
                }
                else
                {
                    AddOfferViewModel.Problem();
                    string txt = $"Failed to add a new announcment at " + dtNow.ToString() + "\n";
                    File.AppendAllText("History.txt", txt);
                }
            }
            catch(Exception ex)
            {
                AddOfferViewModel.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }

            if (successfulOp)
            {
                c.OpenConnection();

                string query1 = "SELECT * FROM account_project.datatest ORDER BY id DESC LIMIT 1";
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query1, c.con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    int _id = 0;
                    while (reader.Read())
                    {
                        _id = Convert.ToInt32(reader[0].ToString());
                    }
                    string txt = $"A new announcment under id: {_id} (Name: {nameprod}, Problem: {nameprob}, Type: {typeprod}, State: {state}, Urgency: {urgency}) was added at " + dtNow.ToString() + "\n";
                    File.AppendAllText("History.txt", txt);
                }
                catch (Exception ex)
                {
                    AddOfferViewModel.CatchException(ex);
                }
                finally
                {
                    c.CloseConnection();
                }
            }
        }
        public static ObservableCollection<Announcment> _DataBaseC = new ObservableCollection<Announcment>();

        public static ObservableCollection<Announcment> ReturnAnnouncements()
        {
            Connection c = new Connection();
            c.OpenConnection();
            string insertQuery = "SELECT * FROM account_project.datatest";
            _DataBaseC.Clear();
            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);
            Announcment an;
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                an = new Announcment();
                an.id = Convert.ToInt32(reader[0].ToString());
                an.nameprod = reader[1].ToString();
                an.nameprob = reader[2].ToString();
                an.typeprob = reader[3].ToString();
                an.state = reader[4].ToString();
                an.urgency = reader[5].ToString();


                _DataBaseC.Add(an);
            }
            
            c.CloseConnection();
            return _DataBaseC;
        }
        public static void UpdateOffer(string id, string nameprod, string nameprob, string typeprod, string state, string urgency)
        {

            Connection c = new Connection();
            c.OpenConnection();

            string checkQuery = "SELECT EXISTS (SELECT 1 FROM account_project.datatest WHERE id LIKE '" + id + "')";
            MySqlCommand ch = new MySqlCommand(checkQuery, c.con);

            int rowCheck = (int)ch.ExecuteScalar();
            
            DateTime dtNow = DateTime.Now;
            if (rowCheck > 0)
            {

                string insertQuery = "UPDATE account_project.datatest SET name_product = '" + nameprod + "', name_problem = '" + nameprob + "', " +
                "type_product = '" + typeprod + "', state_product = '" + state + "', urgency = '" + urgency + "' WHERE id LIKE '" + id + "'";
                
                try
                {
                    MySqlCommand exec1 = new MySqlCommand(insertQuery, c.con);
                    if (exec1.ExecuteNonQuery() > -1)
                    {
                        EditOfferViewModel.Success();
                        string txt = $"The announcment under id {id} was updated to (Name: {nameprod}, Problem: {nameprob}, Type: {typeprod}, State: {state}, Urgency: {urgency}) at " + dtNow.ToString() + "\n";
                        File.AppendAllText("History.txt", txt);
                    }
                }
                catch (Exception ex)
                {
                    EditOfferViewModel.CatchException(ex);
                }
                finally
                {
                    c.CloseConnection();
                }

                c.OpenConnection();
                string checkQuery1 = "SELECT EXISTS (SELECT 1 FROM account_project.workersoffers WHERE id LIKE '" + id + "')";
                MySqlCommand ch1 = new MySqlCommand(checkQuery1, c.con);

                int rowCheck1 = (int)ch1.ExecuteScalar();
                if (rowCheck1 > 0)
                {
                    string insertQuery1 = "DELETE FROM account_project.workersoffers WHERE id LIKE '" + id + "'";

                    try
                    {
                        MySqlCommand exec1 = new MySqlCommand(insertQuery1, c.con);
                        if (exec1.ExecuteNonQuery() > -1)
                        {
                            RemoveOfferViewModel.SuccessfulRemove();
                        }
                        else
                        {
                            RemoveOfferViewModel.UnsuccessfulRemove();
                        }
                    }
                    catch (Exception ex)
                    {
                        RemoveOfferViewModel.CatchException(ex);
                    }
                    finally
                    {
                        c.CloseConnection();
                    }
                }
            }
            else
            {
                EditOfferViewModel.Problem();
                string txt = $"Failed to update the announcment under id {id} at " + dtNow.ToString() + "\n";
                File.AppendAllText("History.txt", txt);
            }
        }
        public static void DeleteOffer(int id)
        {
            Connection c = new Connection();
            c.OpenConnection();

            string checkQuery = "SELECT EXISTS (SELECT 1 FROM account_project.datatest WHERE id LIKE '" + id + "')";
            MySqlCommand ch = new MySqlCommand(checkQuery, c.con);

            int rowCheck = (int)ch.ExecuteScalar();
            DateTime dtNow = DateTime.Now;

            if (rowCheck > 0)
            {
                string insertQuery = "DELETE FROM account_project.datatest WHERE id LIKE '" + id + "'";

                try
                {
                    MySqlCommand exec = new MySqlCommand(insertQuery, c.con);
                    if (exec.ExecuteNonQuery() > -1)
                    {
                        RemoveOfferViewModel.Success();
                        string txt = $"The announcment under id {id} was deleted from database at " + dtNow.ToString() + "\n";
                        File.AppendAllText("History.txt", txt);
                    }
                }
                catch (Exception ex)
                {
                    RemoveOfferViewModel.CatchException(ex);
                }
                finally
                {
                    c.CloseConnection();
                }

                c.OpenConnection();
                string checkQuery1 = "SELECT EXISTS (SELECT 1 FROM account_project.workersoffers WHERE id LIKE '" + id + "')";
                MySqlCommand ch1 = new MySqlCommand(checkQuery1, c.con);

                int rowCheck1 = (int)ch1.ExecuteScalar();
                if (rowCheck1 > 0)
                {
                    string insertQuery1 = "DELETE FROM account_project.workersoffers WHERE id LIKE '" + id + "'";
                    try
                    {
                        MySqlCommand exec1 = new MySqlCommand(insertQuery1, c.con);
                        if (exec1.ExecuteNonQuery() > -1)
                        {
                            RemoveOfferViewModel.SuccessfulRemove();
                        }
                        else
                        {
                            RemoveOfferViewModel.UnsuccessfulRemove();
                        }
                    }
                    catch (Exception ex)
                    {
                        RemoveOfferViewModel.CatchException(ex);
                    }
                    finally
                    {
                        c.CloseConnection();
                    }
                }
            }
            else
            {
                RemoveOfferViewModel.Problem();
                string txt = $"Failed to delete the announcment under id {id} from database at " + dtNow.ToString() + "\n";
                File.AppendAllText("History.txt", txt);
            }
        }

        public static List<Announcment> SearchAnnouncment(string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";
            c.OpenConnection();
            if (string.IsNullOrWhiteSpace(id))
            {
                insertQuery = "SELECT * FROM account_project.datatest";
            }
            else
            {
                int _id = 0;
                try
                {
                    _id = Convert.ToInt32(id);
                    insertQuery = "SELECT * FROM account_project.datatest WHERE id LIKE '" + id + "'";
                }
                catch
                {                   
                    AddOfferViewModel.CatchExceptionInput();
                    insertQuery = "SELECT * FROM account_project.datatest";
                }
                
            }
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

            return deal;
        }
        public static List<Announcment> FilterAnnouncments(string type, string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";

            c.OpenConnection();
            if ((string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(type)) || 
                (string.IsNullOrWhiteSpace(id)) || (string.IsNullOrWhiteSpace(type)))
            {
                insertQuery = "SELECT * FROM account_project.datatest";
            }
            else
            {
                try
                {
                    string _type = type switch
                    {
                        //"nameprod" => "name_product",
                        //"nameprob" => "name_problem",
                        //"typeprob" => "type_product",
                        //"state" => "state_product",
                        //"urgency" => "urgency",
                        "Name" => "name_product",
                        "Problem" => "name_problem",
                        "Type problem" => "type_product",
                        "State" => "state_product",
                        "Urgency" => "urgency"
                    };
                    insertQuery = _type switch
                    {
                        "name_product" => "SELECT * FROM account_project.datatest WHERE name_product LIKE '" + id + "'",
                        "name_problem" => "SELECT * FROM account_project.datatest WHERE name_problem LIKE '" + id + "'",
                        "type_product" => "SELECT * FROM account_project.datatest WHERE type_product LIKE '" + id + "'",
                        "state_product" => "SELECT * FROM account_project.datatest WHERE state_product LIKE '" + id + "'",
                        "urgency" => "SELECT * FROM account_project.datatest WHERE urgency LIKE '" + id + "'"
                    };
                    
                }
                catch(Exception ex)
                {
                    AddOfferViewModel.CatchException(ex);
                    insertQuery = "SELECT * FROM account_project.datatest";
                }
            }

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

            return deal;
        }
        public static List<AnnouncmentOffers> SearchOffer(string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";
            c.OpenConnection();
            if (string.IsNullOrWhiteSpace(id))
            {
                insertQuery = "SELECT * FROM account_project.workersoffers";
            }
            else
            {
                int _id = 0;
                try
                {
                    _id = Convert.ToInt32(id);
                    insertQuery = "SELECT * FROM account_project.workersoffers WHERE id LIKE '" + id + "'";
                }
                catch
                {
                    //MessageBox.Show("Only integer is allowed!");
                    AdminListOffersVM.CatchExceptionInput();
                    insertQuery = "SELECT * FROM account_project.workersoffers";
                }

            }
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
                dl.worker_login = reader[6].ToString();
                dl.deals = Convert.ToInt32(reader[7]);
                dl.price = Convert.ToDecimal(reader[8]);

                deal.Add(dl);
            }
            c.CloseConnection();

            deal.Sort();

            return deal;
        }
        public static List<AnnouncmentOffers> FilterOffers(string type, string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";

            c.OpenConnection();
            if ((string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(type)) ||
                (string.IsNullOrWhiteSpace(id)) || (string.IsNullOrWhiteSpace(type)))
            {
                insertQuery = "SELECT * FROM account_project.workersoffers";
            }
            else
            {
                try
                {
                    string _type = type switch
                    {
                        "Name" => "name_product",
                        "Problem" => "name_problem",
                        "Type problem" => "type_product",
                        "State" => "state_product",
                        "Urgency" => "urgency"
                        //"nameprod" => "name_product",
                        //"nameprob" => "name_problem",
                        //"typeprob" => "type_product",
                        //"state" => "state_product",
                        //"urgency" => "urgency",
                    };
                    insertQuery = _type switch
                    {
                        "name_product" => "SELECT * FROM account_project.workersoffers WHERE name_product LIKE '" + id + "'",
                        "name_problem" => "SELECT * FROM account_project.workersoffers WHERE name_problem LIKE '" + id + "'",
                        "type_product" => "SELECT * FROM account_project.workersoffers WHERE type_product LIKE '" + id + "'",
                        "state_product" => "SELECT * FROM account_project.workersoffers WHERE state_product LIKE '" + id + "'",
                        "urgency" => "SELECT * FROM account_project.workersoffers WHERE urgency LIKE '" + id + "'"
                    };

                }
                catch
                {
                    //MessageBox.Show("An error has happened!");
                    AdminListOffersVM.CatchExceptionInput();
                    insertQuery = "SELECT * FROM account_project.workersoffers";
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
                dl.worker_login = reader[6].ToString();
                dl.deals = Convert.ToInt32(reader[7]);
                dl.price = Convert.ToDecimal(reader[8]);

                deal.Add(dl);
            }
            c.CloseConnection();

            deal.Sort();

            return deal;
        }
        public static void DeleteOffer(int id, string login_worker)
        {

            Connection c = new Connection();
            c.OpenConnection();
            string insertQuery = "DELETE FROM account_project.workersoffers WHERE id LIKE '" + id + "' " +
                "AND login_worker = '" + login_worker  + "'";

            DateTime dtNow = DateTime.Now;

            try
            {
                MySqlCommand exec = new MySqlCommand(insertQuery, c.con);
                if (exec.ExecuteNonQuery() > -1)
                {
                    RemoveOfferViewModel.SuccessfulRemove();
                    string txt = $"The offer under id {id} made by a specialist: {login_worker} was declined at " + dtNow.ToString() + "\n";
                    File.AppendAllText("History.txt", txt);
                }
                else
                {
                    RemoveOfferViewModel.UnsuccessfulRemove();
                    string txt = $"Failed to decline the offer under id {id} made by a specialist: {login_worker} at " + dtNow.ToString() + "\n";
                    File.AppendAllText("History.txt", txt);
                }
            }
            catch (Exception ex)
            {
                RemoveOfferViewModel.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }

        }
        public static void DealOffer(AnnouncmentOffers an1)
        {

            Connection c = new Connection();
            c.OpenConnection();
            DateOnly dt = DateOnly.FromDateTime(DateTime.Now);
            string date = dt.ToString();
            string insertQuery = "INSERT INTO account_project.deals(id, name_product, name_problem, type_product," +
                "state_product, urgency, login_worker, price, date_deal) VALUES " + 
                "('" + an1.id + "','" + an1.nameprod + "','" + an1.nameprob + "','" + an1.typeprob + "','" 
                + an1.state + "','" + an1.urgency + "','" + an1.worker_login + "','" + an1.price + "','" +
                date + "')";

            DateTime dtNow = DateTime.Now;
            int progressBar = 0;
            try
            {
                MySqlCommand exec = new MySqlCommand(insertQuery, c.con);
                if (exec.ExecuteNonQuery() > -1)
                {
                    progressBar += 1;
                    string txt = $"The offer under id {an1.id} made by a specialist: {an1.worker_login} was accepted at " + dtNow.ToString() + "\n";
                    File.AppendAllText("History.txt", txt);
                }
                else
                {
                    AdminListOffersVM.Deal_Problem();
                    string txt = $"Failed to accept the offer under id {an1.id} made by a specialist: {an1.worker_login} at " + dtNow.ToString() + "\n";
                    File.AppendAllText("History.txt", txt);
                }
            }
            catch (Exception ex)
            {
                AdminListOffersVM.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }

            c.OpenConnection();
            string insertQuery2 = "DELETE FROM account_project.datatest WHERE id LIKE '" + an1.id + "'";
            try
            {
                MySqlCommand exec2 = new MySqlCommand(insertQuery2, c.con);
                if (exec2.ExecuteNonQuery() > -1)
                {
                    progressBar += 1;
                }
                else
                {
                    AdminListOffersVM.DeleteAnnouncment_Problem();
                }
            }
            catch (Exception ex)
            {
                AdminListOffersVM.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }

            int deals_now = ModelWorker2.ReturnDeals(an1.worker_login);
            deals_now += 1;
            c.OpenConnection();

            string insertQuery3 = "UPDATE account_project.workers SET num_deals = '" + deals_now + "' " +
                "WHERE worker_login LIKE '" + an1.worker_login + "'";

            try
            {
                MySqlCommand exec3 = new MySqlCommand(insertQuery3, c.con);
                if (exec3.ExecuteNonQuery() > -1)
                {
                    progressBar += 1;
                }
                else
                {
                    AdminListOffersVM.wDataUpdate_Problem();
                }
            }
            catch (Exception ex)
            {
                AdminListOffersVM.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }

            c.OpenConnection();
            string insertQuery5 = "UPDATE account_project.workersoffers SET deals = '" + deals_now + "' " +
                "WHERE login_worker LIKE '" + an1.worker_login + "'";
            try
            {
                MySqlCommand exec5 = new MySqlCommand(insertQuery5, c.con);
                if (exec5.ExecuteNonQuery() > -1)
                {
                    progressBar += 1;
                }
                else
                {
                    MessageBox.Show("UnsuccessUpdate");
                }
            }
            catch (Exception ex)
            {
                AdminListOffersVM.CatchException(ex);
            }
            finally
            {
                c.CloseConnection();
            }

            if (progressBar == 4)
                AdminListOffersVM.SuccessfulTransaction();
        }

        public static List<Deals> SearchDeal(string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";
            c.OpenConnection();
            if (string.IsNullOrWhiteSpace(id))
            {
                insertQuery = "SELECT * FROM account_project.deals";
            }
            else
            {
                int _id = 0;
                try
                {
                    _id = Convert.ToInt32(id);
                    insertQuery = "SELECT * FROM account_project.deals WHERE id LIKE '" + id + "'";
                }
                catch
                {
                    //MessageBox.Show("Only integer is allowed!");
                    AdminWorkingSpaceVM.CatchExceptionInput();
                    insertQuery = "SELECT * FROM account_project.deals";
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
                an.worker_login = reader[6].ToString();
                an.price = Convert.ToDecimal(reader[7]);
                an.date = reader[8].ToString();

                deal.Add(an);
            }
            c.CloseConnection();
            if (string.IsNullOrWhiteSpace(id))
            {
                deal.Sort();
            }
            return deal;
        }
        public static List<Deals> FilterDeals(string type, string id)
        {
            Connection c = new Connection();
            string insertQuery = " ";

            c.OpenConnection();
            if ((string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(type)) ||
                (string.IsNullOrWhiteSpace(id)) || (string.IsNullOrWhiteSpace(type)))
            {
                insertQuery = "SELECT * FROM account_project.deals";
            }
            else
            {
                try
                {
                    string _type = type switch
                    {
                        //"nameprod" => "name_product",
                        //"nameprob" => "name_problem",
                        //"typeprob" => "type_product",
                        //"state" => "state_product",
                        //"urgency" => "urgency",
                        "Name" => "name_product",
                        "Problem" => "name_problem",
                        "Type problem" => "type_product",
                        "State" => "state_product",
                        "Urgency" => "urgency",
                        "Date" => "date"
                    };
                    insertQuery = _type switch
                    {
                        "name_product" => "SELECT * FROM account_project.deals WHERE name_product LIKE '" + id + "'",
                        "name_problem" => "SELECT * FROM account_project.deals WHERE name_problem LIKE '" + id + "'",
                        "type_product" => "SELECT * FROM account_project.deals WHERE type_product LIKE '" + id + "'",
                        "state_product" => "SELECT * FROM account_project.deals WHERE state_product LIKE '" + id + "'",
                        "urgency" => "SELECT * FROM account_project.deals WHERE urgency LIKE '" + id + "'",
                        "date" => "SELECT * FROM account_project.deals WHERE date_deal LIKE '" + id + "'"
                    };

                }
                catch(Exception ex)
                {
                    //MessageBox.Show("An error has happened!");
                    AdminWorkingSpaceVM.CatchException(ex);
                    insertQuery = "SELECT * FROM account_project.datatest";
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
                an.worker_login = reader[6].ToString();
                an.price = Convert.ToDecimal(reader[7]);
                an.date = reader[8].ToString();

                deal.Add(an);
            }
            c.CloseConnection();

            if ((string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(type)) ||
                (string.IsNullOrWhiteSpace(id)) || (string.IsNullOrWhiteSpace(type)))
            {
                deal.Sort();
            }
            return deal;
        }
        public static string ReturnName(string login)
        {
            Connection c = new Connection();
            c.OpenConnection();
            string insertQuery = "SELECT * FROM account_project.ceo WHERE CEO_login = '" + login + "'";
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
        public static void DataDeals()
        {
            Connection c = new Connection();
            string insertQuery = " ";
            c.OpenConnection();

            insertQuery = "SELECT * FROM account_project.deals";

            List<Deals> deal = new List<Deals>();

            MySqlCommand cmd = new MySqlCommand(insertQuery, c.con);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Deals dl;
                dl = new Deals();
                dl.id = Convert.ToInt32(reader[0]);
                dl.nameprod = reader[1].ToString();
                dl.nameprob = reader[2].ToString();
                dl.typeprob = reader[3].ToString();
                dl.state = reader[4].ToString();
                dl.urgency = reader[5].ToString();
                dl.worker_login = reader[6].ToString();
                dl.price = Convert.ToInt32(reader[7]);
                dl.date = reader[8].ToString();

                deal.Add(dl);
            }
            c.CloseConnection();

            var _workers = deal.GroupBy(a => a.worker_login);
            var _occupation = deal.GroupBy(a => a.typeprob);
            var _state = deal.GroupBy(a => a.state);
            var _urgency = deal.GroupBy(a => a.urgency);
            Deals maxDeal = deal.MaxBy(a => a.price);
            Deals minDeal = deal.MinBy(a => a.price);
            deal.Sort();

            using(StreamWriter sr = new StreamWriter("Deals.txt"))
            {             
                sr.WriteLine($"Max deal -> ID: {maxDeal.id} | Name: {maxDeal.nameprod} | Problem: {maxDeal.nameprob} | Type: {maxDeal.typeprob} | " +
                            $"State: {maxDeal.state} | Urgency: {maxDeal.urgency} | Price: {maxDeal.price} | Date: {maxDeal.date}" + "\n");
                
                sr.WriteLine($"Min deal -> ID: {minDeal.id} | Name: {minDeal.nameprod} | Problem: {minDeal.nameprob} | Type: {minDeal.typeprob} | " +
                           $"State: {minDeal.state} | Urgency: {minDeal.urgency} | Price: {minDeal.price} | Date: {minDeal.date}" + "\n");
                
                foreach(var _i in _workers)
                {
                    sr.WriteLine(_i.Key);
                    foreach(var i in _i)
                        sr.WriteLine($"ID: {i.id} | Name: {i.nameprod} | Problem: {i.nameprob} | Type: {i.typeprob} | " +
                            $"State: {i.state} | Urgency: {i.urgency} | Price: {i.price} | Date: {i.date}" + "\n");
                }
                foreach (var _i in _occupation)
                {
                    sr.WriteLine(_i.Key);
                    foreach (var i in _i)
                        sr.WriteLine($"ID: {i.id} | Name: {i.nameprod} | Problem: {i.nameprob} | Type: {i.typeprob} | " +
                            $"State: {i.state} | Urgency: {i.urgency} | Price: {i.price} | Date: {i.date}" + "\n");
                }

                foreach (var _i in _state)
                {
                    sr.WriteLine(_i.Key);
                    foreach (var i in _i)
                        sr.WriteLine($"ID: {i.id} | Name: {i.nameprod} | Problem: {i.nameprob} | Type: {i.typeprob} | " +
                            $"State: {i.state} | Urgency: {i.urgency} | Price: {i.price} | Date: {i.date}" + "\n");
                }

                foreach (var _i in _urgency)
                {
                    sr.WriteLine(_i.Key);
                    foreach (var i in _i)
                        sr.WriteLine($"ID: {i.id} | Name: {i.nameprod} | Problem: {i.nameprob} | Type: {i.typeprob} | " +
                            $"State: {i.state} | Urgency: {i.urgency} | Price: {i.price} | Date: {i.date}" + "\n");
                }

                sr.WriteLine("History of deals in descending order:");
                foreach(var _i in deal)
                    sr.WriteLine($"ID: {_i.id} | Name: {_i.nameprod} | Problem: {_i.nameprob} | Type: {_i.typeprob} | " +
                            $"State: {_i.state} | Urgency: {_i.urgency} | Price: {_i.price} | Date: {_i.date}" + "\n");
            }
        }
    }
}
