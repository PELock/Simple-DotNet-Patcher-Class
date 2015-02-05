////////////////////////////////////////////////////////////////////////////////
//
// Simple C# Patcher Class for Windows (Complex)
//
// You can patch:
//
// - byte arrays
// - single bytes
// - integers
// - strings
// - delphi strings
//
// Language : C#
// Author   : Bartosz Wójcik
// Website  : http://www.pelock.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace NetPatcher
{
    class Patcher
    {
        public FileStream inputFile;

        public Patcher()
        {
        }

        ~Patcher()
        {
            CloseFile();
        }

        public bool OpenFile(string filePath)
        {
            try
            {
                inputFile = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);

                return inputFile == null ? false : true;
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }

        public void CloseFile()
        {
            if (inputFile != null)
            {
                inputFile.Close();
            }

            inputFile = null;
        }

        public long PatchArray(long fileOffset, byte[] patchBytes)
        {
            inputFile.Seek(fileOffset, SeekOrigin.Begin);
            inputFile.Write(patchBytes, 0, patchBytes.Length);

            return inputFile.Position;
        }

        public long PatchByte(long fileOffset, byte patchByte)
        {
            inputFile.Seek(fileOffset, SeekOrigin.Begin);
            inputFile.WriteByte(patchByte);

            return inputFile.Position;
        }

        public long PatchString(long fileOffset, string patchString)
        {
            System.Text.Encoding asciiEncoding = System.Text.Encoding.ASCII;
            byte[] encodedPatchString = asciiEncoding.GetBytes(patchString);

            return PatchArray(fileOffset, encodedPatchString);
        }

        public long PatchDelphiString(long fileOffset, string patchString)
        {
            System.Text.Encoding asciiEncoding = System.Text.Encoding.GetEncoding(1250);
            
            byte[] encodedPatchString = asciiEncoding.GetBytes(patchString);

            PatchByte(fileOffset, (byte)encodedPatchString.Length);
            PatchArray(fileOffset + 1, encodedPatchString);

            return inputFile.Position;
        }

        public long PatchInt32(long fileOffset, Int32 patchInt32)
        {
            byte[] encodedInt32 = BitConverter.GetBytes(patchInt32);
            PatchArray(fileOffset, encodedInt32);

            return inputFile.Position;
        }

        public long PatchFill(long fileOffset, long Length, byte patchByteFill)
        {
            while (Length-- != 0)
            {
                PatchByte(fileOffset++, patchByteFill);
            }

            return inputFile.Position;
        }

    }
}