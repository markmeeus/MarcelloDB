using System;

namespace Marcello.Transactions
{
    internal class Transaction
    {
        Marcello Session { get; set; }

        internal bool Running { get; set; }

        int Enlisted { get; set; }

        internal Transaction(Marcello session)
        {
            this.Session = session;
            this.Running = true;
        }

        internal void Enlist()
        {
            this.Enlisted++;
        }

        internal void Leave()
        {
            this.Enlisted--;

            if (this.Enlisted == 0) 
            {
                this.Commit();
                this.Running = false;
            }
        }

        internal void Rollback()
        {
            Session.Journal.Clear();
            this.Running = false;
        }

        internal void Commit()
        {
            Session.Journal.Apply();
            this.Running = false;
        }
    }
}

