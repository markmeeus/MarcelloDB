using System;

namespace Marcello.Transactions
{
    internal class Transaction
    {
        internal bool Running { get; set; }

        int Enlisted { get; set; }

        internal Transaction()
        {
           this.Running = true;
        }

        internal void Enlist()
        {
            this.Enlisted++;
        }

        internal void Leave()
        {
            this.Enlisted--;
            if (this.Enlisted == 0) {
                this.Commit();
                this.Running = false;
            }
        }

        internal void Rollback()
        {
        }

        internal void Commit()
        {
        }
    }
}

