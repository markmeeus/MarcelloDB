using System;
using MarcelloDB.Serialization;

namespace MarcelloDB.Records
{
	internal class CollectionRoot
	{		
        const int CURRENT_FORMAT_VERSION = 1;

        #region properties
        Int32 _formatVersion;
        internal Int32 FormatVersion 
        { 
            get {return _formatVersion;} 
            set
            {
                _formatVersion = value;
                _dirty = true;
            }
        }

        Int64 _namedRecordIndexAddress; 
        internal Int64 NamedRecordIndexAddress {
            get { return _namedRecordIndexAddress; } 
            set {
                _namedRecordIndexAddress = value;
                _dirty = true;
            }
        }

        Int64 _head;
        internal Int64 Head {
            get { return _head; } 
            set {
                _head = value;
                _dirty = true;
            }
        }
        #endregion
        internal CollectionRoot()
		{
            this.Head = MaxByteSize;
            this.FormatVersion = CURRENT_FORMAT_VERSION;
            this.Clean();
		}

        #region dirty state
        bool _dirty = false;
        internal bool Dirty { get {return _dirty;} }
        internal void Clean(){ _dirty = false; }
        #endregion

        internal static int MaxByteSize
        {
            get { return 1024; } //some padding for future use 
        }

        internal byte[] AsBytes()
        {
            var bytes = new byte[MaxByteSize];
            var bufferWriter = new BufferWriter(bytes, BitConverter.IsLittleEndian);

            bufferWriter.WriteInt32(this.FormatVersion);
            bufferWriter.WriteInt64(this.NamedRecordIndexAddress);
            bufferWriter.WriteInt64(Head);

            return bufferWriter.GetTrimmedBuffer(); 
        }

        internal static CollectionRoot FromBytes(byte[] bytes)
        {        
            var bufferReader = new BufferReader(bytes, BitConverter.IsLittleEndian);
            var formatVersion = bufferReader.ReadInt32();
            var namedRecordIndexAddress = bufferReader.ReadInt64();
            var head = bufferReader.ReadInt64();

            if (head == 0)
            {
                head = MaxByteSize; //when reading from empty data
            }

            return new CollectionRoot(){                    
                NamedRecordIndexAddress = namedRecordIndexAddress,
                Head = head,
                FormatVersion = formatVersion
            };
        }           
	}
}

