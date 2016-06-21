using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApplication9.ViewModels;
using Microsoft.SqlServer;

namespace WebApplication9
{
    public class Repository
    {
        SSDDatingEntities21 db = new SSDDatingEntities21();

        public const string NAME = "Name";
        public const string AGE = "Age";
        public const string AGE_DESC = "Age_Desc";
        public const string ROLEID = "RoleId";


        //search a specifi cuser, filter by interest, gender
        IEnumerable<ClientDetailInfo> FilterClients(IEnumerable<ClientDetailInfo> clients, string searchString, string interestString, string genderString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(
                           c => c.client.UserName.ToUpper().Contains(searchString.ToUpper()));
                return clients;

            }
                          
             //filter by interest
            if (!String.IsNullOrEmpty(interestString)){

                clients = getClientsByInterest(clients, interestString);

            }
            
            //filter by gender
            if (!String.IsNullOrEmpty(genderString)){

              //  clients = clients.Where(c => c.client.gender == genderString);
                clients = getClientsByGender(clients, genderString);

            }

            return clients;
        }

        //get clients DetialInfo by interest
        IEnumerable<ClientDetailInfo> getClientsByInterest(IEnumerable<ClientDetailInfo> clients, string interestString)
        {
            List<ClientDetailInfo> GetClientsByInterests = new List<ClientDetailInfo>();
            foreach (var query in clients)
            {
                foreach(string interest in query.interests)
                {
                    if(interest == interestString)
                    {
                        GetClientsByInterests.Add(query);
                        break;
                    }

                }

            }

            return GetClientsByInterests;

         
        }


        //get clients by gender
        IEnumerable<ClientDetailInfo> getClientsByGender(IEnumerable<ClientDetailInfo> clients, string genderString)
        {
            List<ClientDetailInfo> GetClientsByGender = new List<ClientDetailInfo>();
           
            foreach (var query in clients)
            {               
                if (query.client.gender == genderString)
                {
                    GetClientsByGender.Add(query);                    
                }

            }

            return GetClientsByGender;
        }

          


        //Sort users
        IEnumerable<ClientDetailInfo> SortClient(IEnumerable<ClientDetailInfo> clients, string sortOrder)
        {
            switch (sortOrder)
            {                                
                case NAME:
                    clients = clients.OrderByDescending(c => c.client.RoleId).ThenBy(c => c.client.UserName);
                    break;
                case AGE:
                    clients = clients.OrderByDescending(c => c.client.RoleId).ThenBy(c => c.client.birthdate);
                    break;
                case AGE_DESC:
                    clients = clients.OrderByDescending(c => c.client.RoleId).ThenByDescending(c => c.client.birthdate);
                    break;
                default:
                    clients = clients.OrderByDescending(c => c.client.RoleId).ThenBy(c => c.client.UserName);
                    break;
            }
            return clients;
        }
         //get all clients detail info, including their interests
        public IEnumerable<ClientDetailInfo> getAllClientsDetailsInfo(string searchString, string interestringString, string genderString, string sortOrder)
        {
            List<ClientDetailInfo> AllClientsDetailsInfoList = new List<ClientDetailInfo>();
           
            IEnumerable<Client> clients = db.Clients.ToList();

            List<string> interests = new List<string>();
            foreach(Client client in clients)
            {
                interests = (from c_i in db.ClientInterests where c_i.UserName == client.UserName select c_i.Interest1.interest1).ToList();

                AllClientsDetailsInfoList.Add(new ClientDetailInfo(client, interests));
            }

            IEnumerable<ClientDetailInfo> AllClientDetailsInfo = AllClientsDetailsInfoList.ToList();
            AllClientDetailsInfo = FilterClients(AllClientDetailsInfo, searchString, interestringString, genderString);
            AllClientDetailsInfo = SortClient(AllClientDetailsInfo, sortOrder);

            return AllClientDetailsInfo;
        }



        //get one client Detial Info, including interests
        public ClientDetailInfo getOneUserDetailInfo(string id)
        {
            Client client = db.Clients.Find(id);
            List<string> interests = new List<string>();
            interests = (from c_i in db.ClientInterests where c_i.UserName == client.UserName
                         select c_i.Interest1.interest1).ToList();

            ClientDetailInfo ClientDetailinfo = new ClientDetailInfo(client, interests);
            return ClientDetailinfo;
        }
        public ClientInterestViewModel getClientInterest(string id)
        {
            AspNetUser User = (from i in db.AspNetUsers
                            where i.Id == id
                            select i).FirstOrDefault();
            ClientInterestViewModel ClientInterest = new ClientInterestViewModel();
            ClientInterest.email = User.Email;
            ClientInterest.userId = User.Id;
            ClientInterest.userName = User.UserName;
            ClientInterest.interests = (from i in db.Interests
                                        select i.interest1).ToList();
            return ClientInterest;
        }
        //get all clients in one province
        public IEnumerable<ClientDetailInfo> getAllClientsInOneLocation(string UserName, string searchString, string interestringString, string genderString, string sortOrder, string country, string state)
        {
            List<ClientDetailInfo> AllClientsDetailsInfoList = new List<ClientDetailInfo>();

            Client getClient = db.Clients.Find(UserName);
            IEnumerable<Client> clients = null;

            if (!String.IsNullOrEmpty(country) && !String.IsNullOrEmpty(state))
            {
                clients = (from c in db.Clients where c.country == country && c.province == state && c.UserName != getClient.UserName select c).ToList();
            }
            else
            {

                clients = (from c in db.Clients where c.country == getClient.country && c.province == getClient.province && c.UserName != getClient.UserName select c).ToList();
            }

           

            List<string> interests = new List<string>();
            foreach (Client client in clients)
            {
                interests = (from c_i in db.ClientInterests where c_i.UserName == client.UserName select c_i.Interest1.interest1).ToList();

                AllClientsDetailsInfoList.Add(new ClientDetailInfo(client, interests));
            }

            IEnumerable<ClientDetailInfo> AllClientDetailsInfo = AllClientsDetailsInfoList.ToList();
            AllClientDetailsInfo = FilterClients(AllClientDetailsInfo, searchString, interestringString, genderString);
            AllClientDetailsInfo = SortClient(AllClientDetailsInfo, sortOrder);

            return AllClientDetailsInfo;
        }


        public List<AspNetRole> getUserRole()
        {
            List<AspNetRole> roles = new List<AspNetRole>();
            roles = db.AspNetRoles.ToList();
            return roles;
        }

        public void saveClientInfo(ClientInterestViewModel clientInterestViewModel)
        {
            Client client = new Client();
            client.UserId = clientInterestViewModel.userId;
            client.UserName = clientInterestViewModel.userName;
            client.gender = clientInterestViewModel.gender;
            client.email = clientInterestViewModel.email;
            client.country = clientInterestViewModel.country;
            client.province = clientInterestViewModel.province;
            client.city = clientInterestViewModel.city;
            client.birthdate = clientInterestViewModel.birthdate;
            client.latitude = clientInterestViewModel.latitude ;
            client.longitude = clientInterestViewModel.longitude;

            db.Clients.Add(client);
            db.SaveChanges();

            foreach (string interest in clientInterestViewModel.interests)
            {
                ClientInterest clientInterest = new ClientInterest();
                clientInterest.UserName = clientInterestViewModel.userName;
                clientInterest.interest = interest;
                db.ClientInterests.Add(clientInterest);
            }

            db.SaveChanges();
        }

       
        public void saveAvailableDate(String userName, DateTime availableDate)
        {
            Client client = db.Clients.Find(userName);
            client.availableDate = availableDate;
            db.SaveChanges();
        }

        public void foundDates()
        {

        }

        //Get Asp USER
        public AspNetUser GetUser(string username)
        {
            AspNetUser client = (from i in db.AspNetUsers
                            where i.UserName == username
                            select i).FirstOrDefault();
            return client;
        }

        public void updatgeProfile(Client clientUpdate, string[] interests, string country, string state)
        {
            //udpate client table
            Client client = db.Clients.Find(clientUpdate.UserName);
            if (clientUpdate.gender != null)
            {
            client.gender = clientUpdate.gender;
            }

            if(clientUpdate.birthdate != null)
            {
            client.birthdate = clientUpdate.birthdate;
            }

            if(country!=null && country != "")
            {
                client.country = country;
            }
         
            if(state !=null && state != "")
            {
                client.province = state;

            }

            if (interests != null)
            {
                var queryInterests = from c_i in db.ClientInterests where c_i.UserName == clientUpdate.UserName select c_i;

                if (queryInterests != null)
                {
                    foreach (var interest in queryInterests)
                    {
                        db.ClientInterests.Remove(interest);
                    }
                }


                foreach (string interest in interests)
                {
                    ClientInterest clientInterest = new ClientInterest();
                    clientInterest.UserName = clientUpdate.UserName;
                    clientInterest.interest = interest;

                    db.ClientInterests.Add(clientInterest);
                }
            }
        
           

            db.SaveChanges();

        }

        //get a user's interests
       public List<ClientInterest> getUserInterests(string userName)
        {
            List<ClientInterest> clientInterests = new List<ClientInterest>();
            clientInterests = (from c_i in db.ClientInterests where c_i.UserName == userName select c_i).ToList();
            return clientInterests;           
        }     
       //get all interests
       public IEnumerable<Interest> getAllInterests()
        {
            IEnumerable<Interest> interests = db.Interests.ToList();
            return interests;
        } 
    }
}