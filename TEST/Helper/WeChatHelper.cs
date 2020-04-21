using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HDSLJ.MiniApp.Web.Helper
{

    public class WeChatHelper
    {
        #region *系统配置
        /// <summary>
        /// 小程序的ID
        /// </summary>
        private readonly static string _appId = "wx4e45c24414f9ef1f";
        /// <summary>
        /// 小程序的秘钥
        /// </summary>
        private readonly static string _appSecret = "9314894e759ba595497148c64c6180b9";

        /// <summary>
        /// 微信服务的地址
        /// </summary>
        private readonly static string _weChatApi = "https://api.weixin.qq.com/sns/jscode2session";

        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据
        /// </summary>
        private readonly static string _getAccessToken = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={_appId}&secret={_appSecret}";

        public static object WeChatService { get; private set; }

        #endregion

        /// <summary>
        /// 获取唯一凭证
        /// </summary>
        /// <returns></returns>
        public static AccessTokenModel AccessToken()
        {
            using (HttpClient client = new HttpClient())
            {
                Byte[] resultBytes = client.GetByteArrayAsync(_getAccessToken).Result;
                string jsonStr = Encoding.UTF8.GetString(resultBytes);
                return JsonConvert.DeserializeObject<AccessTokenModel>(jsonStr);
            }
        }

        /// <summary>
        /// 检查文本是否合法
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static TextCheckModel TextCheck(string text)
        {
            using (HttpClient client = new HttpClient())
            {
                string access_token = AccessToken().access_token;
                string url = $"https://api.weixin.qq.com/wxa/msg_sec_check?access_token={access_token}";

                HttpContent httpContent = new StringContent("{" +
                    "\"content\":\"" + text + "\"" +
                    "}");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync(url, httpContent);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TextCheckModel>(response.Result.Content.ReadAsStringAsync().Result);
                }
                return new TextCheckModel {
                    errmsg="error"
                };
            }
        }

        /// <summary>
        /// 根据登录Code获取Openid
        /// </summary>
        /// <param name="loginCode"></param>
        /// <returns></returns>
        public static SessionResponse GetSession(string loginCode)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{_weChatApi}?appid={_appId}&secret={_appSecret}&js_code={loginCode}&grant_type=authorization_code";
                Byte[] resultBytes = client.GetByteArrayAsync(url).Result;
                string jsonStr = Encoding.UTF8.GetString(resultBytes);
                return JsonConvert.DeserializeObject<SessionResponse>(jsonStr);
            }
        }

        /// <summary>
        /// 获取手机号
        /// </summary>
        /// <param name="loginCode">登录Code</param>
        /// <param name="encryptedDataStr">被加密的手机号码</param>
        /// <param name="vi">加密方式</param>
        /// <param name="_session"></param>
        public static PhoneNumberResponse GetPhoneNumber(string loginCode, string encryptedDataStr, string iv, SessionResponse _session = null)
        {
            // 获取session
            SessionResponse session = _session == null ? GetSession(loginCode) : _session;
            #region 解密手机号码

            RijndaelManaged rijalg = new RijndaelManaged();
            //-----------------    
            //设置 cipher 格式 AES-128-CBC    

            rijalg.KeySize = 128;

            rijalg.Padding = PaddingMode.PKCS7;
            rijalg.Mode = CipherMode.CBC;

            rijalg.Key = Convert.FromBase64String(session.Session_Key);
            rijalg.IV = Convert.FromBase64String(iv);


            byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
            //解密    
            ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

            string result;

            using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        result = srDecrypt.ReadToEnd();
                    }
                }
            }
            #endregion

            // 转成对象
            PhoneNumberResponse rep = JsonConvert.DeserializeObject<PhoneNumberResponse>(result);
            rep.session = session;
            return rep;
        }

        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭据
        /// </summary>
        public static AccessTokenResponse GetAccessToken()
        {
            using (HttpClient client = new HttpClient())
            {
                Byte[] resultBytes = client.GetByteArrayAsync(_getAccessToken).Result;
                string jsonStr = Encoding.UTF8.GetString(resultBytes);
                return JsonConvert.DeserializeObject<AccessTokenResponse>(jsonStr);
            }
        }

    }

    public class AccessTokenModel
    {
        public string access_token { get; set; }
    }

    public class TextCheckModel {
        public string errmsg { get; set; }
    }
    /// <summary>
    /// 获取OpenId的时候返回的Modal
    /// </summary>
    public class SessionResponse
    {
        public string OpenId { get; set; }
        public string Session_Key { get; set; }
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }

    /// <summary>
    /// 获取到手机号的对象
    /// </summary>
    public class PhoneNumberResponse
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 纯净的手机号码
        /// </summary>
        public string PurePhoneNumber { get; set; }

        /// <summary>
        /// 国家代码
        /// +86
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// session对象
        /// 本次请求过后可能还要用到session对象中的openid等属性
        /// 因为logincode只能用一次
        /// 必须存起来返回回去
        /// </summary>
        public SessionResponse session { get; set; }
    }

    /// <summary>
    /// 获取小程序全局唯一后台接口调用凭据
    /// </summary>
    public class AccessTokenResponse
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string Access_Token { get; set; }
        /// <summary>
        /// 凭证有效时间，单位：秒。目前是7200秒之内的值。
        /// </summary>
        public int Expires_In { get; set; }

        /// <summary>
        /// 错误码
        /// 值 说明
        /// -1	系统繁忙，此时请开发者稍候再试
        /// 0	请求成功
        /// 40001	AppSecret 错误或者 AppSecret 不属于这个小程序，请开发者确认 AppSecret 的正确性
        /// 40002	请确保 grant_type 字段值为 client_credential
        /// 40013	不合法的 AppID，请开发者检查 AppID 的正确性，避免异常字符，注意大小写
        /// </summary>
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }
    }
}