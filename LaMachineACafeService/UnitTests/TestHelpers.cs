using LaMachineACafe.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public static class TestHelpers
    {
        private static Random random = new Random();

        public static T AssertResultCode<T>(this T response, ResultCodeEnum code, string message = null)
            where T : BaseResponse
        {
            Assert.IsTrue(response.ResultCode == code, message + " - Got " + response.ResultCode.ToString() + " --" + response.Message + " --");
            return response;
        }

        public static T AssertSuccess<T>(this T response, string message = null)
            where T : BaseResponse
        {
            AssertResultCode(response, ResultCodeEnum.Success, message);
            return response;
        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
