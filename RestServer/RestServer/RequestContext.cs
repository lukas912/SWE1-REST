using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.CompilerServices;
using SWE1_MCTG;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: InternalsVisibleTo("SWE1-MCTG")]

namespace RestServer
{
    class RequestContext
    {
        List<Message> messages = new List<Message>();
        int id_count = 0;
        private string[] output = { "", "" };
        
        //0 = Content
        //1 = Statuscode

        public void CheckContext(string url, string type, List<Attribute> queries, List<Attribute> body_data, string header_data)
        {
            switch (type)
            {
                case "GET":
                    CheckGET(url, header_data);
                    break;
                case "POST":
                    //AddMessage(url, body_data);
                    CheckPOST(url, body_data, header_data);
                    break;
                case "PUT":
                    //EditMessage(url, body_data);
                    CheckPUT(url, body_data, header_data);
                    break;
                case "DELETE":
                    //DeleteMessage(url);
                    checkDelete(url, body_data, header_data);
                    break;
                default:
                    output[0] = "Error: This method is not allowed";
                    output[1] = "405 Method Not Allowed";
                    break;
            }
        }

        private void checkDelete(string url, List<Attribute> body_data, string header_data)
        {
            if(url.StartsWith("/tradings/"))
            {
                DB_Connection dbc = new DB_Connection();

                if(dbc.Auth(header_data))
                {
                    if(dbc.deleteTradingDeal(url.Split("/")[2], header_data)) {
                        output[0] = "Trading Deal geloescht";
                        output[1] = "200 OK";
                    }

                    else
                    {
                        output[0] = "Kein Zugriff auf diesen Trading Deal!";
                        output[1] = "401 Unauthorized";
                    }
                }

                else
                {
                    output[0] = "Session ID is invalid!";
                    output[1] = "401 Unauthorized";
                }
            }

            if(url.StartsWith("/user"))
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();

                    if (dbc.Auth(header_data))
                    {
                        string username = dbc.GetUsernameByToken(header_data);

                        if (dbc.DeleteUser(username))
                        {
                            output[0] = "User " + username + " wurde geloescht!";
                            output[1] = "200 OK";

                            dbc.EndSessoin(username);
                        }


                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }
                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }

            }
        }

        private void CheckGET(string url, string header_data)
        {
            if (url == "/messages/")
            {
                ListAllMessages(url);
            }

            if (url.StartsWith("/messages/") && url.Length >= 11)
            {
                ListMessage(url);
            }

            if (url.StartsWith("/cards"))
            {

                output[0] = "";
                DB_Connection dbc = new DB_Connection();

                if(dbc.Auth(header_data))
                {
                    List<ICard> cards = dbc.ShowUserCards(header_data);
                    int counter = 1;

                    foreach (ICard card in cards)
                    {
                        output[0] += "\n\nCard " + counter + ": " +
                            "\n Card ID: " + card.CardID
                            + "\n Name: " + card.Name +
                            "\n Type: " + card.CardType +
                            "\n Damage: " + card.Damage +
                            "\n Weakness " + card.Weakness +
                            "\n Package ID: " + card.PackageID;

                        counter++;
                    }

                    //output[0] = JObject.Parse(output[0]).ToString();

                    output[1] = "200 OK";
                }

                else
                {
                    output[0] = "Session ID is invalid!";
                    output[1] = "401 Unauthorized";
                }


            }

            if(url.StartsWith("/deck"))
            {
                DB_Connection dbc = new DB_Connection();

                if(dbc.Auth(header_data))
                {
                    List<ICard> cards = dbc.ShowUserDeck(header_data);
                    int counter = 1;

                    output[0] = "Deck Cards:";

                    foreach (ICard card in cards)
                    {
                        output[0] += "\n\nCard " + counter + ": " +
                            "\n Card ID: " + card.CardID
                            + "\n Name: " + card.Name +
                            "\n Type: " + card.CardType +
                            "\n Damage: " + card.Damage +
                            "\n Weakness " + card.Weakness +
                            "\n Package ID: " + card.PackageID;

                        counter++;
                    }

                    output[1] = "200 OK";
                }

                else
                {
                    output[0] = "Session ID is invalid!";
                    output[1] = "401 Unauthorized";
                }
                
            }

            if(url.StartsWith("/users/"))
            {
                
                try
                {
                    DB_Connection dbc = new DB_Connection();

                    if(dbc.Auth(header_data))
                    {
                        User user = dbc.ShowUserData(header_data, url.Split("/")[2]);

                        if (user != null)
                        {
                            output[0] = "Username: " + user.Username +
                                "\nDisplayname: " + user.DisplayName +
                                "\nPassword: " + user.Password +
                                "\nCoins: " + user.Coins +
                                "\nE-Mail: " + user.Email +
                                "\nBio: " + user.Bio +
                                "\nImage: " + user.Image;
                            output[1] = "200 OK";
                        }

                        else
                        {
                            output[0] = "No User found or wrong token";
                            output[1] = "400 Bad Request";
                        }
                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }

                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }



            }

            if(url.StartsWith("/stats"))
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();
                    if(dbc.Auth(header_data)) {
                        string stats = dbc.GetUserStats(header_data);

                        output[0] = stats;
                        output[1] = "200 OK";
                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }



                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }

            if (url.StartsWith("/score/"))
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();
                    if(dbc.Auth(header_data))
                    {
                        string score = dbc.GetUserScore(url.Split("/")[2]);

                        output[0] = score;
                        output[1] = "200 OK";
                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }

                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }

            if(url.StartsWith("/tradings"))
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();
                    if(dbc.Auth(header_data))
                    {
                        string td = dbc.ShowTradingDeals();

                        output[0] = td;
                        output[1] = "200 OK";
                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }

                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }
        }

        private void CheckPOST(string url, List<Attribute> body_data, string header_data)
        {

            if (url.StartsWith("/logout/"))
            {

                DB_Connection dbc = new DB_Connection();

                if (dbc.Auth(header_data))
                {
                    if (dbc.EndSessoin(url.Split("/")[2]))
                    {
                        output[0] = "Logout successfull";
                        output[1] = "200 OK";
                    }

                    else
                    {
                        output[0] = "Logout failed";
                        output[1] = "400 Bad Request";
                    }


                }

                else
                {
                    output[0] = "Session ID is invalid!";
                    output[1] = "401 Unauthorized";
                }

            }

            if (url.StartsWith("/users"))
            {
                try
                {
                    User user = new User(body_data.Find(obj => { return obj.Name == "Username"; }).Content, "not implemented", "not implemented", body_data.Find(obj => { return obj.Name == "Password"; }).Content, 20, "not implemented", "not implemented");
                    DB_Connection dbc = new DB_Connection();
                    if(dbc.CreateUser(user) == true)
                    {
                        output[0] = "User added";
                        output[1] = "200 OK";
                    }

                    else
                    {
                        output[0] = "User already exists";
                        output[1] = "400 Bad Request";
                    }


                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
               
            }

            if(url.StartsWith("/packages"))
            {
                try
                {

                    DB_Connection dbc = new DB_Connection();
                    string pid = dbc.CreatePackage();

                    List<ICard> cards = ExtractCardsFromJSON(body_data.Find(obj => { return obj.Name == "cards"; }).Content, pid);

                    

                    dbc.AddCardsToPackage(pid, cards);

                    output[0] = "Package added";
                    output[1] = "200 OK";
                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }

            if(url.StartsWith("/transactions/packages"))
            {
                try
                {

                    DB_Connection dbc = new DB_Connection();
                    //string pid = dbc.createPackage();
                    if(dbc.Auth(header_data))
                    {
                        if (dbc.AcquirePackage(header_data) == true)
                        {
                            output[0] = "Package acquired";
                            output[1] = "200 OK";
                        }

                        else
                        {
                            output[0] = "No coins left or no Package avaiable";
                            output[1] = "404 not found";
                        }
                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }



                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }

            if (url.StartsWith("/sessions"))
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();
                    if(dbc.CheckUser(body_data.Find(obj => { return obj.Name == "Username"; }).Content, body_data.Find(obj => { return obj.Name == "Password"; }).Content) == true)
                    {
                        output[0] = "Login Successfull";
                        output[0] += "\nYour Session ID: " + dbc.getSessionID(body_data.Find(obj => { return obj.Name == "Username"; }).Content);
                        output[1] = "200 OK";
                    }

                    if (dbc.CheckUser(body_data.Find(obj => { return obj.Name == "Username"; }).Content, body_data.Find(obj => { return obj.Name == "Password"; }).Content) == false)
                    {
                        output[0] = "Login failed";
                        output[1] = "400 Bad Request";
                    }




                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }

            if(url.StartsWith("/tradings") && url.Length < 10)
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();
                    if(dbc.Auth(header_data))
                    {
                        dbc.CreateTradingDeal(header_data, body_data.Find(obj => { return obj.Name == "Id"; }).Content,
                            body_data.Find(obj => { return obj.Name == "CardToTrade"; }).Content,
                            body_data.Find(obj => { return obj.Name == "Type"; }).Content,
                            body_data.Find(obj => { return obj.Name == "MinimumDamage"; }).Content);

                        output[0] = "Trading Deal created";
                        output[1] = "200 OK";
                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }

                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
                
            }

            if(url.StartsWith("/tradings/"))
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();
                    if (dbc.Auth(header_data))
                    {
                        if (dbc.Trade(header_data, url.Split("/")[2], body_data.Find(obj => { return obj.Name == "card"; }).Content))
                        {
                            output[0] = "Card traded!";
                            output[1] = "200 OK";
                        }

                        else
                        {
                            output[0] = "Trading konnte nicht durchgefuehrt werden!";
                            output[1] = "400 Bad Request";
                        }


                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }

                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }

        }

        private void CheckPUT(string url, List<Attribute> body_data, string header_data)
        {
            if(url.StartsWith("/deck"))
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();
                    if(dbc.Auth(header_data))
                    {
                        List<string> card_ids = ExtractCardIDs(body_data);

                        List<ICard> cards = dbc.ShowUserCardsDeck(header_data, card_ids);

                        if (cards.Count < 4)
                        {
                            output[0] = "Zu wenige Karten";
                            output[1] = "400 Bad Request";
                        }

                        else if (cards.Count > 4)
                        {
                            output[0] = "Zu viele Karten";
                            output[1] = "400 Bad Request";
                        }

                        else
                        {
                            Deck deck = new Deck(cards, dbc.GetUserByToken(header_data));
                            Console.WriteLine("DA GEHTS " + dbc.GetUserByToken(header_data).Username);
                            dbc.CreateDeck(deck);

                            output[0] = "Deck created";
                            output[1] = "200 OK";
                        }
                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }

                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }

            if(url.StartsWith("/users/"))
            {
                try
                {
                    DB_Connection dbc = new DB_Connection();

                    if(dbc.Auth(header_data))
                    {
                        //check User
                        User user = dbc.ShowUserData(header_data, url.Split("/")[2]);

                        if (user != null)
                        {
                            User edituser = new User(user.Username, body_data.Find(obj => { return obj.Name == "Name"; }).Content, user.Email, user.Password,
                                user.Coins, body_data.Find(obj => { return obj.Name == "Image"; }).Content, body_data.Find(obj => { return obj.Name == "Bio"; }).Content);
                            //edit user data

                            if (dbc.EditUserData(edituser) == true)
                            {
                                //return edited user

                                User newuser = dbc.ShowUserData(header_data, url.Split("/")[2]);

                                output[0] = "Username: " + user.Username +
                                    "\nDisplayname: " + user.DisplayName +
                                    "\nPassword: " + user.Password +
                                    "\nCoins: " + user.Coins +
                                    "\nE-Mail: " + user.Email +
                                    "\nBio: " + user.Bio +
                                    "\nImage: " + user.Image;
                                output[1] = "200 OK";
                            }

                            else
                            {
                                output[0] = "Error: Server Error";
                                output[1] = "500 Server Error";
                            }

                        }

                        else
                        {
                            output[0] = "Error: User not found or wrong token";
                            output[1] = "400 Bad Request";
                        }
                    }

                    else
                    {
                        output[0] = "Session ID is invalid!";
                        output[1] = "401 Unauthorized";
                    }

                    
                }

                catch
                {
                    output[0] = "Error: Server Error";
                    output[1] = "500 Server Error";
                }
            }


        }

        private List<string> ExtractCardIDs(List<Attribute> body_data)
        {
            var obj = JsonConvert.DeserializeObject(body_data[0].Content);

            List<string> cards = new List<string>();

            foreach (var item in ((JArray)obj))
            {
                //Console.WriteLine(string.Format("id:{0}, name:{1}, damage:{2}",item.Value<int>("id"),item.Value<string>("name"),item.Value<double>("damage")));
                cards.Add(item.ToString());

            }

            return cards;
        }

        private List<ICard> ExtractCardsFromJSON(string json, string pid)
        {
            var obj = JsonConvert.DeserializeObject(json);

            List<ICard> cards = new List<ICard>();

            foreach (var item in ((JArray)obj))
            {
                //Console.WriteLine(string.Format("id:{0}, name:{1}, damage:{2}",item.Value<int>("id"),item.Value<string>("name"),item.Value<double>("damage")));

                if(item.Value<string>("Name").Contains("Spell") == false)
                {
                    ICard card = new MonsterCard(item.Value<string>("Id"), item.Value<string>("Name"), item.Value<double>("Damage"), pid, "MonsterCard", item.Value<double>("Weakness"));
                    cards.Add(card);
                }

                if (item.Value<string>("Name").Contains("Spell"))
                {
                    ICard card = new SpellCard(item.Value<string>("Id"), item.Value<string>("Name"), item.Value<double>("Damage"), pid, "SpellCard", item.Value<double>("Weakness"));
                    cards.Add(card);
                }


            }

            return cards;
        }

        private void ListAllMessages(string url)
        {
            output[0] = "";

            if(messages.Count == 0)
            {
                output[0] = "Error: No Messages found";
                output[1] = "404 Not Found";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    output[0] += msg.Msg + "\n";
                }

                output[1] = "200 OK";
            }

        }

        private void ListMessage(string url)
        {
            int id = Convert.ToInt32(GetID(url));

            if(messages.Exists(msg => msg.ID == id) == false)
            {
                output[0] = "Error: No Message found with this ID";
                output[1] = "404 Not found";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == id)
                    {
                        output[0] = msg.Msg;
                    }

                }

                output[1] = "200 OK";
            }

        }

        private void AddMessage(string url, List<Attribute> body_data)
        {
            if(body_data[0].Content == "" || body_data[0].Content == null)
            {
                output[0] = "Error: Message can't be empty";
                output[1] = "400 Bad Request";
            }

            else
            {
                foreach(Attribute item in body_data)
                {
                    Message msg = new Message(id_count, item.Content);
                    messages.Add(msg);
                    IncrID();
                    output[0] = "Message added";
                    output[1] = "201 Created";
                }

            }

        }

        private void EditMessage(string url, List<Attribute> bd)
        {
            int id = Convert.ToInt32(GetID(url));

            if (messages.Exists(msg => msg.ID == id) == false)
            {
                output[0] = "Error: No Message found with this ID";
                output[1] = "404 Not found";
            }

            else if(bd[0].Content == "" || (bd[0].Content == null)) {
                output[0] = "Error: Message can't be empty";
                output[1] = "400 Bad Request";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == id)
                    {
                        msg.Msg = bd[0].Content;
                        output[0] = "Message edited";
                    }

                }

                output[1] = "200 OK";
            }

        }

        private void DeleteMessage(string url)
        {
            int id = Convert.ToInt32(GetID(url));
            Message rm = null;

            if (messages.Exists(msg => msg.ID == id) == false)
            {
                output[0] = "Error: No Message found with this ID";
                output[1] = "404 Not found";
            }

            else
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == id)
                    {
                        rm = msg;
                        output[0] = "Deleted Message";
                    }
                }

                output[1] = "200 OK";

                messages.Remove(rm);
            }
        }

        private string GetID(string url)
        {
            string output = url.Split("/")[2];
            return output;
        }

        private void IncrID()
        {
            id_count++;
        }

        public string[] GetOutput()
        {
            return output;
        }
    }
}
