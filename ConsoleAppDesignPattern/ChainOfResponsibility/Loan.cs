namespace ConsoleAppDesignPattern.ChainOfResponsibility
{

    public class Loan
    {
        public decimal Amount { get; set; }
        public string Purpose { get; set; }
        public int Number { get; set; }
    }

    public class LoanEventArgs : EventArgs
    {
        internal Loan Loan { get; set; }
    }

    public abstract class Approver
    {
        // Loan event 
        public EventHandler<LoanEventArgs> Loan;
        // Loan event handler
        public abstract void LoanHandler(object sender, LoanEventArgs e);
        // Constructor
        public Approver()
        {
            Loan += LoanHandler;
        }
        public void ProcessRequest(Loan loan)
        {
            OnLoan(new LoanEventArgs { Loan = loan });
        }
        // Invoke the Loan event
        public virtual void OnLoan(LoanEventArgs e)
        {
            if (Loan != null)
            {
                Loan(this, e);
            }
        }
        // Sets or gets the next approver
        public Approver Successor { get; set; }
    }

    class AssistantManager : Approver
    {
        public override void LoanHandler(object sender, LoanEventArgs e)
        {
            if (e.Loan.Amount < 45000)
            {
                Console.WriteLine($"{GetType().Name} approved request# {e.Loan.Number}");
            }
            else if (Successor != null)
            {
                Successor.LoanHandler(this, e);
            }
        }
    }

    class Clerk : Approver
    {
        public override void LoanHandler(object sender, LoanEventArgs e)
        {
            if (e.Loan.Amount < 25000)
            {
                Console.WriteLine($"{GetType().Name} approved request# {e.Loan.Number}");
            }
            else if (Successor != null)
            {
                Successor.LoanHandler(this, e);
            }
        }
    }

    class Manager : Approver
    {
        public override void LoanHandler(object sender, LoanEventArgs e)
        {
            if (e.Loan.Amount < 100000)
            {
                Console.WriteLine($"{GetType().Name} approved request# {e.Loan.Number}");
            }
            else if (Successor != null)
            {
                Successor.LoanHandler(this, e);
            }
            else
            {
                Console.WriteLine(
                "Request# {0} requer uma reunião executiva!",
                e.Loan.Number);
            }
        }
    }
}
