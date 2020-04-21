using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TEST.JWT
{
    /// <summary>
    /// Token实体
    /// </summary>
    public class TokenEntity
    {
        /// <summary>
        /// token字符串
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 过期时差
        /// </summary>
        public int expires_in { get; set; }
    }
    /// <summary>
    /// token提供属性
    /// </summary>
    public class TokenProviderOptions
    {
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅者
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间间隔
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(1826);
        /// <summary>
        /// 签名证书
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }

    /// <summary>
    /// Token提供类
    /// </summary>
    public class TokenProvider
    {
        readonly TokenProviderOptions _options;
        public TokenProvider(TokenProviderOptions options)
        {
            _options = options;
        }
        /// <summary>
        /// 生成令牌
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="role">角色</param>
        /// <returns></returns>
        public TokenEntity GenerateToken(string username)
        {
            var identity = GetIdentity(username);
            if (identity == null)
            {
                return null;
            }
            var now = DateTime.Now;
            //声明
            var claims = new Claim[]
            {
             new Claim(JwtRegisteredClaimNames.Sub,username),
             new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.Iat,ToUnixEpochDate(now).ToString(),ClaimValueTypes.Integer64),
             //new Claim(ClaimTypes.Role,role),
             new Claim(ClaimTypes.Name,username)
            };
            //Jwt安全令牌
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            //生成令牌字符串
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new TokenEntity
            {
                access_token = $"Bearer {encodedJwt}",
                expires_in = (int)_options.Expiration.TotalSeconds
            };
            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        /// <summary>
        /// 查看令牌是否存在
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        private ClaimsIdentity GetIdentity(string username)
        {
            return new ClaimsIdentity(new System.Security.Principal.GenericIdentity(username, "token"),
                new Claim[] {
                    new Claim(ClaimTypes.Name, username)
                });
        }
    }
}
