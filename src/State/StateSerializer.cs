using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Text;

namespace UnityTas {
public class StateSerializer : IDisposable {
  public static void Run(string filePath) {
    var fs = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write,
                       FileShare.Read);
    var serializer = new StateSerializer(fs);
    serializer.Serialize();
    fs.Close();
    serializer = null;
    GC.Collect();
  }

  private BinaryWriter _bw;
  // TODO: Apparently we should build our own WeakMap for performance reasons
  // https://stackoverflow.com/questions/750947/net-unique-object-identifier#comment31500949_9789370
  private Dictionary<object, int> _seenObjects = new Dictionary<object, int>();

  private StateSerializer(FileStream fs) {
    _bw = new BinaryWriter(fs, Encoding.UTF8, true);
  }

  public void Dispose() { _bw.Dispose(); }

  public void Serialize() {
    FieldCache.Populate();
    SerializeStaticFields();
  }

  private void SerializeStaticFields() {
    foreach (var entry in FieldCache.Cache.Values) {
      foreach (var field in entry.fields) {
        if (!field.IsStatic)
          continue;
        var value = field.GetValue(null);
        Console.WriteLine(
            $"=== {field.DeclaringType.FullName} -> {field.Name} = {value}");
        SerializeValue(value);
      }
    }
  }

  private void SerializeValue(object value) {
    var type = value.GetType();
    var typeCode = Type.GetTypeCode(type);
    switch (typeCode) {
    case TypeCode.Empty:
      WriteValueHeader(typeCode, 0);
      break;
    case TypeCode.Object:
      break;
    case TypeCode.DBNull:
      WriteValueHeader(typeCode, 0);
      break;
    case TypeCode.Boolean:
      WriteValueHeader(typeCode, 0);
      _bw.Write((byte)((bool)value ? 1 : 0));
      break;
    case TypeCode.Char:
      WriteValueHeader(typeCode, 0);
      _bw.Write((char)value);
      break;
    case TypeCode.SByte:
      WriteValueHeader(typeCode, 0);
      _bw.Write((sbyte)value);
      break;
    case TypeCode.Byte:
      WriteValueHeader(typeCode, 0);
      _bw.Write((byte)value);
      break;
    case TypeCode.Int16:
      WriteValueHeader(typeCode, 0);
      _bw.Write((Int16)value);
      break;
    case TypeCode.UInt16:
      WriteValueHeader(typeCode, 0);
      _bw.Write((UInt16)value);
      break;
    case TypeCode.Int32:
      WriteValueHeader(typeCode, 0);
      _bw.Write((Int32)value);
      break;
    case TypeCode.UInt32:
      WriteValueHeader(typeCode, 0);
      _bw.Write((UInt32)value);
      break;
    case TypeCode.Int64:
      WriteValueHeader(typeCode, 0);
      _bw.Write((Int64)value);
      break;
    case TypeCode.UInt64:
      WriteValueHeader(typeCode, 0);
      _bw.Write((UInt64)value);
      break;
    case TypeCode.Single:
      WriteValueHeader(typeCode, 0);
      _bw.Write((Single)value);
      break;
    case TypeCode.Double:
      WriteValueHeader(typeCode, 0);
      _bw.Write((double)value);
      break;
    case TypeCode.Decimal:
      WriteValueHeader(typeCode, 0);
      _bw.Write((decimal)value);
      break;
    case TypeCode.DateTime:
      WriteValueHeader(typeCode, 0);
      _bw.Write(((DateTime)value).ToBinary());
      break;
    case TypeCode.String:
      var bytes = Encoding.UTF8.GetBytes((string)value);
      WriteValueHeader(typeCode, bytes.Length);
      _bw.Write(bytes);
      break;
    default:
      throw new Exception($"Unknown typecode: {typeCode}");
    }
  }

  /// <summary>
  /// The <i>value header</i> is a short (1-3 byte) header that appears before
  /// each value in the save state and contains metadata about the value.
  ///
  /// Bits 0-5 = the TypeCode of the value
  /// Bit 6    = reserved
  /// Bit 7    = 1 iff the following byte(s) contain the byte length of the
  /// value in VLQ format (used for variable-length types like String)
  ///
  /// VLQ basically means that bits 0-6 contain the integer value and the last
  /// bit indicates whether the next byte contains more of the integer value.
  /// </summary>
  private void WriteValueHeader(TypeCode typeCode, int length) {
    _bw.Write((byte)((byte)typeCode | (byte)(length > 0 ? 1 << 7 : 0)));
    while (length > 0) {
      var part = length & 0b01111111;
      length >>= 7;
      var vlqBit = length > 0 ? 1 << 7 : 0;
      _bw.Write((byte)(part | vlqBit));
    }
  }
}
}
