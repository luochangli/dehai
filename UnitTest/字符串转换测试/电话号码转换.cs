using FormUI.OperationLayer;
using Machine.Specifications;

namespace UnitTest.字符串转换测试
{
    public class 手机号码转换到十六进制
    {
        private static string result;
        private It 应该转换成功 = () => { result.ShouldEqual("8160813439F3"); };
        private Because 执行了 = () => { result = AT.PhoneEncoder("18873688664"); };
    }

    public class 十六进制转换到手机号码
    {
        private static string result;
        private It 应该转换成功 = () => { result.ShouldEqual("18873688664"); };
        private Because 执行了 = () => { result = AT.PhoneDecoder("8160813439F3"); };
    }

    public class 偶数号码转换到十六进制
    {
        private static string result;
        private It 应该转换成功 = () => { result.ShouldEqual("706321436587"); };
        private Because 执行了 = () => { result = AT.PhoneEncoder("073612345678"); };
    }

    public class 十六进制转换到偶数号码
    {
        private static string result;
        private It 应该转换成功 = () => { result.ShouldEqual("073612345678"); };
        private Because 执行了 = () => { result = AT.PhoneDecoder("706321436587"); };
    }

    public class 基数号码转换到十六进制
    {
        private static string result;
        private It 应该转换成功 = () => { result.ShouldEqual("0180F6"); };
        private Because 执行了 = () => { result = AT.PhoneEncoder("10086"); };
    }

    public class 十六进制转换到基数号码
    {
        private static string result;
        private It 应该转换成功 = () => { result.ShouldEqual("10086"); };
        private Because 执行了 = () => { result = AT.PhoneDecoder("0180F6"); };
    }
}