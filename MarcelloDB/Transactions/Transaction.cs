using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MarcelloDB.Transactions
{
    internal class Transaction : SessionBoundObject
    {
        internal bool Running { get; set; }

        internal bool IsCommitting { get; set; }

        List<ITransactor> Transactors { get; set; }

        int Enlisted { get; set; }

        internal Transaction(Session session) : base(session)
        {
            Transactors = new List<ITransactor>();
            this.Running = true;
            this.IsCommitting = false;
        }

        internal void Enlist()
        {
            if (this.IsCommitting)
                return;

            this.Enlisted++;
        }

        internal void AddTransactor(ITransactor transactor)
        {
            Transactors.Add(transactor);
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
            foreach (var transactor in Transactors)
            {
                transactor.RollbackState();
            }
            this.Running = false;
        }

        internal void Commit()
        {
            this.IsCommitting = true;

            foreach (var transactor in Transactors)
            {
                transactor.SaveState();
            }

            Session.Journal.Commit();

            this.IsCommitting = false;
            this.TryApply();
        }

        void Apply()
        {
            lock (this.Session.SyncLock)
            {
                Session.Journal.Apply();
            }
        }

        void TryApply()
        {
            try{
                Apply();
            }catch(Exception){}
        }
    }
}

