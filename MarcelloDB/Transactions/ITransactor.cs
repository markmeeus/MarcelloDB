using System;

namespace MarcelloDB.Transactions
{
    internal interface ITransactor
    {
        void SaveState();
        void RollbackState();
    }
}

