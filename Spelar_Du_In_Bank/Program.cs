namespace Spelar_Du_In_Bank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bank.");
            Console.WriteLine("Please log in: ");

            Console.WriteLine("Enter user name: ");
            string userName = Console.ReadLine();

            Console.Write("Enter pin code: ");
            string pin = Console.ReadLine();

            if (userName == "admin")
            {
                if (pin != "1234")
                {
                    Console.WriteLine("Wrong password!");
                    return;  //?
                }

                AdminActions.DoAdminTasks();
                return;
            }

            //Code here for user login *****

        }
    }
}