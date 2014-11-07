using System;

namespace Marcello.Transactions
{
    internal class Transaction
    {
        Marcello Session { get; set; }

        internal bool Running { get; set; }

        internal bool IsCommitting { get; set; }

        int Enlisted { get; set; }

        internal Transaction(Marcello session)
        {
            this.Session = session;
            this.Running = true;
            this.IsCommitting = false;
        }

        internal void Enlist()
        {
            if (this.IsCommitting)
                return;

            this.Enlisted++;
        }

        internal void Leave()
        {
            if (this.IsCommitting)
                return;

            this.Enlisted--;

            if (this.Enlisted == 0) 
            {
                this.Commit();
                this.Running = false;
            }
        }

        internal void Rollback()
        {
            Session.Journal.ClearUncommitted();
            this.Running = false;
        }

        internal void Commit()
        {
            this.IsCommitting = true;
            Session.Journal.Commit();
            Session.Journal.Apply();
            this.IsCommitting = false;
        }
    }
}

