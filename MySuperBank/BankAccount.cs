using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace MySuperBank
{
    public class BankAccount
    {
        public string Number { get; }
        public string Owner { get; set; }
        public decimal Balance {
            get
            {
                decimal balance = 0;
                foreach (var item in _allTransactions)
                {
                    balance += item.Amount;
                }

                return balance;
            }

        }
        private static int s_accountNumberSeed = 1234567890;
        //The accountNumberSeed is a data member.It's private, which means it can only be accessed by code inside the BankAccount class. It's a way of separating the 
        //    public responsibilities(like having an account number) from the private implementation(how account numbers are generated).
        //    It's also static, which means it's shared by all of the BankAccount objects.The value of a non-static variable is unique to each instance of the BankAccount object. 
        //    The accountNumberSeed is a private static field and thus has the s_ prefix as per C# naming conventions. 
        //    The s denoting static and _ denoting private field. Add the following two lines to the constructor to assign the account number. 

        private List<Transaction> _allTransactions = new List<Transaction>();
        public BankAccount(string name, decimal initialBalance)
        {
            Owner = name;
            MakeDeposit(initialBalance, DateTime.Now, "Initial balance"); //DateTime.Now is a property that returns the current date and time.
            //this.Balance = initialBalance; 
            //The this qualifier is only required when a local variable or parameter has the same name as that field or property.
            //The this qualifier is omitted throughout the remainder of this article unless it's necessary.
            Number = s_accountNumberSeed.ToString();
            s_accountNumberSeed++;
        }
        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
                //The throw statement throws an exception. Execution of the current block ends, and control transfers to the first matching catch block found in the call stack.
                //You'll add a catch block to test this code a little later on.
            }
            var deposit = new Transaction(amount, date, note);
            _allTransactions.Add(deposit);
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }
            if (Balance - amount < 0)
            {
                throw new InvalidOperationException("Not sufficient funds for this withdrawal");
            }
            var withdrawal = new Transaction(-amount, date, note);
            _allTransactions.Add(withdrawal);
        }
        public string GetAccountHistory()
        {
            var report = new System.Text.StringBuilder();

            decimal balance = 0;
            //Header
            report.AppendLine("Date\t\tAmount\tBalance\tNote");
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
                //Rows
                report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}"); //ToShortDateString() format for date to display
            }
            //The history uses the StringBuilder class to format a string that contains one line for each transaction.
            //You've seen the string formatting code earlier in these tutorials.
            //One new character is \t. That inserts a tab to format the output.
            return report.ToString();
        }
    }
}
