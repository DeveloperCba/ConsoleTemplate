namespace ConsoleAppDesignPattern.ChainOfResponsibility
{
    public static class ChainOfResponsibility 
    {
        public static void Mock()
        {
            Approver diego = new Clerk();
            Approver cesar = new AssistantManager();
            Approver jones = new Manager();

            diego.Successor = cesar;
            cesar.Successor = jones;

            // Generate and process loan requests
            var loan = new Loan { Number = 2034, Amount = 23000, Purpose = "Car Loan" };
            diego.ProcessRequest(loan);

            loan = new Loan { Number = 2035, Amount = 44500, Purpose = "Motorcycle Loan" };
            diego.ProcessRequest(loan);

            loan = new Loan { Number = 2036, Amount = 156200, Purpose = "Apartament Loan" };
            diego.ProcessRequest(loan);

            Console.WriteLine("Press any key to continue!");
            Console.ReadKey();
        }
    }
}
