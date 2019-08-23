using System;
using System.IO;
using System.Text;
using MimeKit;

namespace Yambr.Email.Loader.Extensions
{
    public static class TextPartExtension
    {
        public static string GetTextInUtf8(this TextPart textPart)
        {
            var utf8 = Encoding.UTF8;
            if (!textPart.IsHtml && textPart.ContentTransferEncoding == ContentEncoding.SevenBit)
            {
                //TODO другие допилить если будет ерунда
                return textPart.GetText(utf8);
            }

            return textPart.Text;
        }
        

        private static string FromBase64Text(MimePart textPart)
        {
            var dataBytes = new byte[textPart.Content.Stream.Length];
            textPart.Content.Stream.Read(dataBytes, 0, (int)textPart.Content.Stream.Length);
            var encodingOfDataBytes =GetEncoding(textPart.Content.Stream) ?? Encoding.UTF8;
            var base64String = encodingOfDataBytes.GetString(dataBytes);
            var contentBytes = Convert.FromBase64String(base64String);
            return FromBinaryText(contentBytes, textPart.ContentType?.Charset);
        }

        private static string FromBinaryText(Stream stream, string charset)
        {
            //если есть кодировка то возьмем в ней если нет то попробуем вернуть в utf8
            var encoding = !string.IsNullOrWhiteSpace(charset) ?
                Encoding.GetEncoding(charset) :
                //пробуем узнать кодировку
                GetEncoding(stream);
            var contentBytes = new byte[stream.Length];
            stream.Read(contentBytes, 0, (int)stream.Length);
            return ConverToUtf8(encoding, contentBytes);
        }

        private static string FromBinaryText(byte[] bytes, string charset)
        {
            //если есть кодировка то возьмем в ней если нет то попробуем вернуть в utf8
            var encoding = !string.IsNullOrWhiteSpace(charset) ?
                Encoding.GetEncoding(charset) :
                //пробуем узнать кодировку
                GetEncoding(bytes, 0, bytes.Length);
            return ConverToUtf8(encoding, bytes);
        }

        private static string ConverToUtf8(Encoding encoding, byte[] bytes)
        {
            var utf8 = Encoding.UTF8;
            return utf8.GetString(encoding.Equals(utf8) ? bytes : 
                Encoding.Convert(encoding, utf8, bytes));
        }

        public static Encoding GetEncoding(Stream stream)
        {
            var cdet = new Ude.CharsetDetector();
            cdet.Feed(stream);
            cdet.DataEnd();
            return cdet.Charset != null ? Encoding.GetEncoding(cdet.Charset) : null;
        }
        public static Encoding GetEncoding(byte[] bytes, int offset, int length)
        {
            var cdet = new Ude.CharsetDetector();
            cdet.Feed(bytes, offset, length);
            cdet.DataEnd();
            return cdet.Charset != null ? Encoding.GetEncoding(cdet.Charset) : null;
        }
    }
}
