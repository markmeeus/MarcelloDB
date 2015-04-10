using System;

namespace MarcelloDB
{

    internal interface ITransactor
    {
        void SaveState();
        void RollbackState();
    }
}

