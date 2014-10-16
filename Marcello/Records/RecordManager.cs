using System;

namespace Marcello
{
    internal class RecordManager
    {   
        Marcello Session { get; set; }

        internal RecordManager (Marcello session)
        {
            Session = session;
        }

        internal Record GetFirstRecord(){
            return null;
        }

        internal Record GetNextRecord(Record current){
            return null;
        }

        internal Record GetPreviousRecord(Record current){
            return null;
        }

        internal Record GetLastRecord(){
            return null;
        }
    }
}

