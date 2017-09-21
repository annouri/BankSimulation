﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBank
{

    class Program
    {

        
        class User
        {
            public static int ID_user { get; set; }
            public string Username { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
            public User()
            {
                ID_user = ID_user + 1;
                Username = "null";
                Firstname = "null";
                Lastname = "null";
                Password = "null";
                Email = "null";
            }
            public User(string username, string firstname, string lastname, string password, string email)
            {
                ID_user = ID_user + 1;
                Username = username;
                Firstname = firstname;
                Lastname = lastname;
                Password = password;
                Email = email;
            }
            public User Get_user_by_username(string username)
            {
                foreach (User user in Bank.Clients)
                {
                    if (user.Username == username)
                    {
                        User user_temp = new User(user.Username, user.Firstname, user.Lastname, user.Password, user.Email);
                        return user_temp;
                    }
                }
                return null;
            }
            public void Show_users(List<User> users)
            {
                foreach (User user in users)
                {
                    Console.WriteLine(user.Username + " " + user.Firstname + " " + user.Lastname + " " + user.Email);
                }
            }
        }
        class Premuim_user : User
        {
            public string Phone_number { get; set; }
            public Premuim_user() : base()
            {
                Phone_number = "xxxxxxx";
            }
            public Premuim_user(string phone_number,string username,string firstname,string lastname,string password,string email) : base( username,  firstname,  lastname,  password,  email)
            {
                Phone_number = phone_number;
            }
            
        }
        class Bank
        {
            public static List<User> Clients { get; set; }
            public static List<Virement> Transactions { get; set; }
            public static List<Bank_Transaction> BankTransactions { get; set; }
            public string Phonenumber { get; set; }
            public string Name { get; set; }
            public string Creationdate { get; set; }
            public Bank()
            {
                Clients = new List<User>();
                Transactions = new List<Virement>();
                BankTransactions = new List<Bank_Transaction>();
                Phonenumber = "+34 324-256-18";
                Name = "BMCI";
                Creationdate = "12/12/1995";
                
            }
            public void ShowClients()
            {
                foreach(User user in Clients)
                {
                    Console.WriteLine(user.Firstname+" "+user.Lastname);
                }
            }
            public bool Authentificaion(string username, string password)
            {

                foreach (User user in Clients)
                {
                    if (user.Username == username && user.Password == password)
                        return true;


                }
                return false;
            }

        }
        abstract class Transaction
        {
            public float Moneysum { get; set; }
            public User Client { get; set; }
            public Transaction() {
                Moneysum = 0;
                Client = null;
            }
            public Transaction(User client,float money) {
                Client = client;
                Moneysum = money;
            }
            public void TotalTransactions() {}
            public void ClientTotalTransactions() { }
            public void ShowAllTransactions() { }
            public void ShowAllClientTransactions() { }
        }

        class Virement : Transaction
        {
            public User To_User { get; set; }
            public Virement() : base()
            {
                To_User = null;
            }
            public Virement(User to_user,float money, User user ) : base(user, money)
            {
                To_User = to_user;
            }
            public Virement(User client, float money)
            {
                Virement virement = new Virement();
                virement.Moneysum = money;
                virement.Client = client;
                virement.To_User = null;
                Bank.Transactions.Add(virement);
            }
            public void ShowAllTransactions()
            {
                Console.WriteLine("From         Sum         To ");
                foreach (Virement transaction in Bank.Transactions)
                {
                    Console.WriteLine(transaction.Client.Username + " " + transaction.Moneysum + "USD " + transaction.To_User.Username);
                }
            }
            public void ShowAllClientTransactions(string username)
            {
                foreach (Virement transaction in Bank.Transactions)
                {
                    if (transaction.To_User!=null)
                    {
                        if (transaction.Client.Username == username)
                        {
                            Console.WriteLine(transaction.To_User.Username + " Payment send " + transaction.Moneysum + "USD");
                        }
                        else if (transaction.To_User.Username == username)
                        {
                            Console.WriteLine(transaction.Client.Username + " Money receivid " + transaction.Moneysum + "USD");
                        }
                    }
                    else if(transaction.Client.Username == username && transaction.To_User == null)
                    {
                        Console.WriteLine("Refill Bank Account " + transaction.Moneysum + "USD");
                    }
                }
            }
            public float TotalTransactions()
            {
                float total = 0;
                foreach (Virement transaction in Bank.Transactions)
                {
                    if (transaction.To_User == null)
                    {
                        total += transaction.Moneysum;
                    }
                    else
                    {
                        total -= transaction.Moneysum;
                    }
                    
                }
                return total;
            }
            public float ClientTotalTransactions(string username)
            {
                float sum = 0;
                foreach (Virement transaction in Bank.Transactions)
                {
                    if (transaction.To_User.Username == username)
                    {
                        sum += transaction.Moneysum;
                    }
                    else if (transaction.Client.Username == username)
                    {
                        sum -= transaction.Moneysum;
                    }
                    else if (transaction.Client.Username == username && transaction.To_User==null)
                    {
                        sum += transaction.Moneysum;
                    }
                }
                return sum;
            }
            public void PaymentUser(string client, float money, string receiver)
            {
                foreach (User user in Bank.Clients)
                {
                    if (user.Username == receiver)
                    {
                        Virement tmp = new Virement();
                        tmp.To_User = user.Get_user_by_username(receiver);
                        tmp.Client = user.Get_user_by_username(client);
                        tmp.Moneysum = money;
                        Bank.Transactions.Add(tmp);
                    }
                }
            }
            public void RefillAccount(User client, float money)
            {
                Virement virement = new Virement();
                virement.Moneysum = money;
                virement.Client = client;
                virement.To_User = null;
                Bank.Transactions.Add(virement);
            }
        }
        class Bank_Transaction:Transaction
        {
                public string MachineCode { get; set; }
                public string Location { get; set; }
                public Bank_Transaction() : base()
                {
                    MachineCode = "null";
                    Location = "null";
                }                    
                public Bank_Transaction(String bankname,string location,User client, float money):base(client,money)
                {
                    MachineCode = bankname;
                    Location = location;
                }
                public float TotalTransactions()
                {
                    float some = 0;
                    foreach (Bank_Transaction bank in Bank.BankTransactions)
                    {
                        some -= bank.Moneysum;
                    }
                    return some;
                }
                public void MakeWithdraw(User client,float money)
                {
                        string machineCode = "BMCI Machin X22145";
                        string location = "Nador, Morocco";
                        Bank_Transaction bank = new Bank_Transaction(machineCode,location,client,money);
                        Bank.BankTransactions.Add(bank);
                }
                public void ShowAllTransactions()
                {
                    foreach (Bank_Transaction bank in Bank.BankTransactions)
                    {
                        Console.WriteLine(bank.Client.Firstname+" "+bank.Client.Lastname+" "+bank.Moneysum+" "+bank.MachineCode+" "+bank.Location);
                    }
                }
                public void ShowAllClientTransactions(User client)
                {
               
                foreach (Bank_Transaction bank in Bank.BankTransactions)
                    {    
                        if (bank.Client.Username==client.Username)
                        {
                            Console.WriteLine(bank.Moneysum + " " + bank.MachineCode + " " + bank.Location);
                        }
                    }
                }
                public float ClientTotalTransactions(User client)
                {
                    float some = 0;
                    foreach (Bank_Transaction bank in Bank.BankTransactions)
                    {
                        if (bank.Client.Username==client.Username)
                        {
                            some -= bank.Moneysum;
                        }

                    }
                    return some;
                }
                
        }

           static void Main(string[] args)
            {
            List<User> users = new List<User>();
            //Creating Bank
            Bank bank = new Bank();
            Bank.Clients = users;
            //Creating our clients
                User temp = new User("an.ilias", "ilias", "annouri", "0011", "annouri.ilias@gmail.com");
                users.Add(temp);
                User temp1 = new User("ha.elgarrab", "hamza", "elgarrab", "0022", "hamza.elgarrab@gmail.com");
                users.Add(temp1);
                User temp2= new User("to.sandra ", "Sandra ", "Townsend", "0000", "Townsend@gmail.com");
                users.Add(temp2);
                User temp3 = new User("Lynnha", "Lynn ", "Hamil", "0000", "Hamil.Lynn@gmail.com");
                users.Add(temp3);
                User temp4 = new User("Danielle", "Danielle", "Ngo", "0000", "Danielle.Ngo@gmail.com");
                users.Add(temp4);

            List<Virement> virement = new List<Virement>();
            //Refill Account
            Virement p1 = new Virement(temp,1000);
            Virement p2 = new Virement(temp1, 350);
            Virement p3 = new Virement(temp2, 500);
            Virement p4 = new Virement(temp3, 150);
            Virement p5 = new Virement(temp4, 50);
            virement.Add(p1);
            virement.Add(p2);
            virement.Add(p3);
            virement.Add(p4);
            virement.Add(p5);
            //Payment Money
                Virement transaction = new Virement(temp, 120, temp1);
                Virement transaction1 = new Virement(temp, 12, temp2);
                Virement transaction2 = new Virement(temp, 20, temp3);
                Virement transaction3 = new Virement(temp, 15, temp1);
                Virement transaction4 = new Virement(temp, 25, temp2);
                Virement transaction14 = new Virement(temp3, 5, temp);
                Virement transaction11 = new Virement(temp2, 23, temp1);
                Virement transaction12 = new Virement(temp2, 2, temp);
                Virement transaction13 = new Virement(temp3, 55, temp3);
                virement.Add(transaction);
                virement.Add(transaction1);
                virement.Add(transaction2);
                virement.Add(transaction3);
                virement.Add(transaction4);
                virement.Add(transaction11);
                virement.Add(transaction12);
                virement.Add(transaction13);
                virement.Add(transaction14);

            //Retrait
                List<Bank_Transaction> retrait = new List<Bank_Transaction>();
                Bank_Transaction bankt1 = new Bank_Transaction("BMC1 X11","NY",temp,30);
                Bank_Transaction bankt2 = new Bank_Transaction("BMC1 c15", "CAL", temp4, 10);
                Bank_Transaction bankt3 = new Bank_Transaction("BMC1 d11", "NY", temp1, 10);
                Bank_Transaction bankt4 = new Bank_Transaction("BMC1 X11", "NY", temp2, 100);
                Bank_Transaction bankt5 = new Bank_Transaction("BMC1 z10", "CAL", temp3, 50);
                retrait.Add(bankt1);
                retrait.Add(bankt2);
                retrait.Add(bankt3);
                retrait.Add(bankt4);
                retrait.Add(bankt5);


            Console.WriteLine("Welcome to BBank");
                    Console.WriteLine("Loing informtion");
                    Console.WriteLine("username :");
                    string username = Console.ReadLine();
                    Console.WriteLine("password");
                    string password = Console.ReadLine();
                    if(bank.Authentificaion(username,password))
                    {
                        User user = new User();
                        user=user.Get_user_by_username(username);
                    string smoney;
                    float money;
                    Console.WriteLine("Welcome back  " + user.Firstname);

                    Console.WriteLine(bank.Name + " " +Bank.Clients.Count+ " Clietns"); Console.WriteLine("Annual revenue " +(bankt1.TotalTransactions() + p1.TotalTransactions())+ "USD");
                    Console.WriteLine("1-ALL Transfers      2-All Withdraws          3-Transfer      4-Withdraw      5-Balance");
                    int i=1;
                    string enter;
                    while (i!=0)
                        {
                        enter = Console.ReadLine();
                            i = int.Parse(enter);
                            switch (i)
                            {
                                case 1: Console.Clear(); transaction.ShowAllClientTransactions(username); break;
                                case 2: Console.Clear(); bankt1.ShowAllClientTransactions(user); break;
                                case 3:
                                Console.Clear();
                                Console.WriteLine("Payment to :");
                                string name;
                                name=Console.ReadLine();
                                Console.WriteLine("Money to send (usd) :");
                                smoney = Console.ReadLine();
                                money = float.Parse(smoney);
                                transaction.PaymentUser(username,money,name);break;
                                case 4:
                                Console.Clear();
                                Console.WriteLine("Money to retrait (usd) :");
                                smoney = Console.ReadLine();
                                money = float.Parse(smoney);
                                bankt1.MakeWithdraw(user,money); break;
                                case 5:
                                Console.Clear();
                                money = transaction.ClientTotalTransactions(username)+bankt1.ClientTotalTransactions(user);
                                Console.WriteLine("Balance : "+money+"USD");break;
                                default:break;
                            }
                        Console.WriteLine("1-ALL Transfers      2-All Withdraws          3-Transfer      4-Withdraw      5-Balance");
                        }
                   

                }
                Console.Read();
            }

        
    }
    }

