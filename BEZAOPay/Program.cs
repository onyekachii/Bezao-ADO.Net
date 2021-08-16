using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEZAOPayDAL;


namespace BEZAOPay
{
    class Program
    {
        static void Main(string[] args)
        {            
            var db = new BEZAODAL();

            var users = db.GetAllUsers();
            Console.WriteLine(" ************** All BezaoPay Users ************** ");
            Console.WriteLine("ID\tName\t\t\tEmail");            
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id}\t{user.Name}\t\t{user.Email}\n");
            }

            var targetUser = db.GetUser(10);
            Console.WriteLine($"{targetUser} is the User");

            var NewUser = db.AddUser(new BEZAOPayDAL.Models.User
            {
                Email = "newUSer@Bezao.com",
                Name = "UGM",
                Id = 17
            });
            Console.WriteLine($"\n************** Dear {NewUser.Name}, welcome to BezaoPay ************** ");

            db.DeleteUser("UGM");

            Console.ReadKey();
        }
    }
}
