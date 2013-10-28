#region Usings

using System.IO;

#endregion

namespace esquire.Helpers {
    public static class StreamExtensions {
        #region Public methods

        public static MemoryStream ToMemoryStream(this Stream stream) {
            if (stream.CanSeek) {
                stream.Position = 0;
            }
            var buffer = new byte[16 * 1024];
            var ms = new MemoryStream();
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0) {
                ms.Write(buffer, 0, read);
            }
            ms.Position = 0;
            return ms;
        }

        #endregion
    }
}