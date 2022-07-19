using EtfDotNet.Types;

namespace EtfDotNet;

public class EtfMemory
{
    private ArraySegment<byte>? _arraySource;
    private int _position;
    private Stream? _streamSource;
    private bool _isStreamed;

    public static EtfMemory FromArray(ArraySegment<byte> source)
    {
        return new EtfMemory()
        {
            _arraySource = source,
            _isStreamed = false
        };
    }
    public static EtfMemory FromStream(Stream source)
    {
        return new EtfMemory()
        {
            _streamSource = source,
            _isStreamed = true
        };
    }

    public int Read(ArraySegment<byte> destination)
    {
        if (_isStreamed)
        {
            return _streamSource.Read(destination);
        }
        else
        {
            if (_position + destination.Count > _arraySource.Value.Count)
            {
                return _arraySource.Value.Count - _position;
            }
            Buffer.BlockCopy(_arraySource.Value.Array, _position + _arraySource.Value.Offset, destination.Array, destination.Offset, destination.Count);
            _position += destination.Count;
            return destination.Count;
        }
    }

    public ArraySegment<byte> ReadSlice(int length)
    {
        if (_isStreamed)
        {
            throw new InvalidOperationException("This operation cannot be performed on a streamed memory segment");
        }

        var slice = _arraySource.Value.Slice(_position, length);
        _position += length;
        return slice;
    }

    public EtfContainer ReadContainer(int length, EtfConstants type)
    {
        if (_isStreamed)
        {
            var container = EtfContainer.Make(length, type);
            ReadExactly(container.ContainedData);
            return container;
        }

        return EtfContainer.AsContainer(ReadSlice(length), type);
    }

    public void ReadExactly(ArraySegment<byte> destination)
    {
        int offset = 0;
        while (offset < destination.Count)
        {
            int readCount = Read(destination.Slice(destination.Offset + offset, destination.Count - offset));
            if (readCount == 0)
                throw new EndOfStreamException("End of the stream reached.");
            offset += readCount;
        }
    }

    public void Write(ArraySegment<byte> data)
    {
        if (_isStreamed)
        {
            _streamSource.Write(data);
        }
        else
        {
            if (_position + data.Count > _arraySource.Value.Count)
            {
                throw new IndexOutOfRangeException(
                    "The specifed data length will exceed the bounded capacity of the memory segment.");
            }
            Buffer.BlockCopy(data.Array, data.Offset, _arraySource.Value.Array, _position + _arraySource.Value.Offset, data.Count);
            _position += data.Count;
        }
    }

    public int ReadByte()
    {
        if (_isStreamed)
        {
            return _streamSource.ReadByte();
        }
        else
        {
            if (_position + 1 > _arraySource.Value.Count)
            {
                return -1;
            }

            return _arraySource.Value[_position++];
        }
    }

    public void WriteByte(byte b)
    {
        if (_isStreamed)
        {
            _streamSource.WriteByte(b);
        }
        else
        {
            if (_position + 1 > _arraySource.Value.Count)
            {
                throw new IndexOutOfRangeException(
                    "The specifed data length will exceed the bounded capacity of the memory segment.");
            }
            
            _arraySource.Value.Array[_position++ + _arraySource.Value.Offset] = b;
        }
    }
}