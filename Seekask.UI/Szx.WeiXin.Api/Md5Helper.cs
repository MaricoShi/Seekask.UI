﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Szx.WeiXin.Api
{
    /// <summary>
    /// 
    /// 作者：石忠孝   
    /// 文件名：Md5Helper
    /// 创建时间：2016/1/1 14:17:40
    /// 修改人：石忠孝
    /// 修改时间：2016/1/1 14:17:40
    /// 说明：
    /// 
    /// </summary>
    public class Md5Helper
    {
        private static char[] toDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        public static string Md5Hex(string I_SourceArray)
        {
            string R_Result;

            byte[] temp = Encoding.UTF8.GetBytes(I_SourceArray);
            sbyte[] temp1 = Md5Helper.ByteArray2SByteArray(temp);
            R_Result = Md5Hex(temp1);

            return R_Result;
        }

        public static string Md5Hex(sbyte[] I_SourceArray)
        {
            string R_Result;

            sbyte[] md5SByteArray = Md5Helper.Md5Encode(I_SourceArray);
            char[] charArray = Md5Helper.EncodeHex(md5SByteArray);

            R_Result = new string(charArray);
            return R_Result;
        }


        private static sbyte[] Md5Encode(sbyte[] I_Source)
        {
            sbyte[] R_Result;

            byte[] temp = Md5Helper.SByteArray2ByteArray(I_Source);
            byte[] temp1 = new MD5CryptoServiceProvider().ComputeHash(temp);
            R_Result = Md5Helper.ByteArray2SByteArray(temp1);

            return R_Result;
        }


        private static char[] EncodeHex(sbyte[] data)
        {
            int l = data.Length;
            char[] R_Result = new char[l << 1];
            for (int i = 0, j = 0; i < l; i++)
            {
                R_Result[j++] = toDigits[Md5Helper.MoveByte((0xF0 & data[i]), 4)];
                R_Result[j++] = toDigits[0x0F & data[i]];
            }
            return R_Result;
        }

        private static int MoveByte(int value, int pos)
        {
            if (value < 0)
            {
                string s = Convert.ToString(value, 2);
                for (int i = 0; i < pos; i++)
                {
                    s = "0" + s.Substring(0, 31);
                }
                return Convert.ToInt32(s, 2);
            }
            else
            {
                return value >> pos;
            }
        }


        private static sbyte[] ByteArray2SByteArray(byte[] I_SourceByte)
        {
            return I_SourceByte.Select(p => Md5Helper.Byte2SByte(p)).ToArray();
        }

        private static byte[] SByteArray2ByteArray(sbyte[] I_SourceByte)
        {
            return I_SourceByte.Select(p => Md5Helper.SByte2Byte(p)).ToArray();
        }


        private static sbyte Byte2SByte(byte I_SourceSByte)
        {
            sbyte R_Result;

            if (I_SourceSByte < 128)
            {
                R_Result = (sbyte)I_SourceSByte;
            }
            else
            {
                R_Result = (sbyte)(I_SourceSByte - 256);
            }
            return R_Result;
        }


        private static byte SByte2Byte(sbyte I_SourceSByte)
        {
            byte R_Result;

            if (I_SourceSByte < 0)
            {
                R_Result = (byte)(I_SourceSByte + 256);
            }
            else
            {
                R_Result = (byte)I_SourceSByte;
            }
            return R_Result;
        }



    }

}
