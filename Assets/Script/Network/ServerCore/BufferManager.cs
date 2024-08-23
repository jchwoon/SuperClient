using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class BufferManager
    {
        ArraySegment<byte> _buffer;
        int _writePos;
        int _readPos;

        public int SizeOfReadToWrite { get { return _writePos - _readPos; } }
        public int SizeOfRestBuffer { get { return _buffer.Count - _writePos; } }

        public BufferManager(int capacity)
        {
            _buffer = new ArraySegment<byte>(new byte[capacity], 0, capacity);
        }

        public void Refresh()
        {
            if (_readPos == _writePos)
            {
                _writePos = 0;
                _readPos = 0;
            }
            else
            {
                //아직 처리되지 않은 패킷 사이즈
                int restSize = SizeOfReadToWrite;
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, restSize);
                _readPos = 0;
                _writePos = restSize;
            }
        }

        //전체 대여
        public ArraySegment<byte> RentBuffer()
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, SizeOfRestBuffer);
            return segment;
        }

        public ArraySegment<byte> GetSegmentFromReadToWrite()
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, SizeOfReadToWrite);
            return segment;
        }

        public bool CheckAndWrite(int size)
        {
            int restSize = SizeOfRestBuffer;

            if (restSize <= size)
                return false;

            _writePos += size;
            return true;
        }

        public bool CheckAndRead(int size)
        {
            if (_writePos < _readPos + size)
                return false;


            _readPos += size;
            return true;
        }
    }
}
