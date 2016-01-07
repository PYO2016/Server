using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;     // DLL support
using System.Diagnostics;

// Put PyoCore.dll in the /bin folder.

namespace PyoCore
{
    static class PyoCore
    {
        /* Interface of PyoCore. */

        public static String ProcessPngImage(String imageFileName)
        {
            const uint len = 100000;

            StringBuilder buffer = new StringBuilder(Convert.ToInt32(len));
            Trace.WriteLine("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@!!");
            bool success = NativePyoCore.processImageFileW(imageFileName, ImageFileType.IMAGE_FILE_TYPE_PNG,
                buffer, len, false);
            Trace.WriteLine("#############################!!" + success);
            if (!success)
            {
                ErrorCode errorCode = NativePyoCore.getErrorCode();
                throw new PyoCoreException(errorCode);
            }

            return buffer.ToString();
        }
    }

    /* Exception Class of PyoCore. */

    class PyoCoreException : Exception
    {
        public PyoCoreException(ErrorCode errorCode)
        {
            this.errorCode = errorCode;
        }

        public ErrorCode getErrorCode()
        {
            return errorCode;
        }

        private ErrorCode errorCode;
    }

    /* Native interface of DLL. */

    static class NativePyoCore
    {
        [DllImport("PyoCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool processImageFileW(
            [param: MarshalAs(UnmanagedType.LPWStr)]
            String imageFileName,
            [param: MarshalAs(UnmanagedType.I4)]
            ImageFileType imageFileType,
            [param: MarshalAs(UnmanagedType.LPWStr)]
            StringBuilder resultBuffer,
            [param: MarshalAs(UnmanagedType.U4)]
            uint resultBufferLen,
            [param: MarshalAs(UnmanagedType.Bool)]
            Boolean isDebug
            );

        [DllImport("PyoCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool processImageFileA(
            [param: MarshalAs(UnmanagedType.LPStr)]
            String imageFileName,
            [param: MarshalAs(UnmanagedType.I4)]
            ImageFileType imageFileType,
            [param: MarshalAs(UnmanagedType.LPStr)]
            StringBuilder resultBuffer,
            [param: MarshalAs(UnmanagedType.U4)]
            uint resultBufferLen,
            [param: MarshalAs(UnmanagedType.Bool)]
            Boolean isDebug
            );

        [DllImport("PyoCore.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ErrorCode getErrorCode();
    }

    enum ImageFileType
    {
        IMAGE_FILE_TYPE_PNG,

        /* Not image file type. Just for the number of image file types */
        IMAGE_FILE_TYPE_CNT
    };

    enum ErrorCode
    {
        /* No error */
        ERROR_NONE,
        /* Unknown error */
        ERROR_UNKNOWN,
        /* Not supported image file type */
        ERROR_IMAGE_FILE_TYPE,

        /* Not error code. Just for the number of error codes */
        ERROR_CODE_CNT
    }
}
