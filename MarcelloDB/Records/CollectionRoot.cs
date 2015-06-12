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

         
        #endregion
        internal CollectionRoot()
		{         
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
            var bufferWriter = new BufferWriter(bytes);

            bufferWriter.WriteInt32(this.FormatVersion);

            return bufferWriter.GetTrimmedBuffer(); 
        }

        internal static CollectionRoot FromBytes(byte[] bytes)
        {        
            var bufferReader = new BufferReader(bytes);
            var formatVersion = bufferReader.ReadInt32();

            return new CollectionRoot(){                    
                FormatVersion = formatVersion
            };
        }           
	}
}

