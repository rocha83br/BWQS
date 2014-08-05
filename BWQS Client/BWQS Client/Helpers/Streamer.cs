using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BWQS_Client.Helpers
{
    public static class Streamer
    {
        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[input.Length];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write (buffer, 0, read);
        }
    }
}
