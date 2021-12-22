using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using consul_dotnet.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace consul_dotnet.controllers
{
    /// <summary>
    /// 测试配置中心
    /// </summary>
    [Produces("application/json")]
    [Route("config/")]
    public class ConfigController : Controller
    {
        IOptions<AppSettings> appSettings;
        IOptions<DbConfig> dbConfig;
        IConfiguration configuration;
        public ConfigController(IOptions<AppSettings> _appSettings,
            IOptions<DbConfig> _dbConfig,
            IConfiguration _configuration)
        {
            this.appSettings = _appSettings;
            this.dbConfig = _dbConfig;
            this.configuration = _configuration;
        }
        /// <summary>
        /// 本地json文件配置
        /// </summary>
        /// <returns></returns>
       [Route("appsettings")]
        public IActionResult AppSettings()
        {
            return Ok(this.appSettings.Value);
        }

        /// <summary>
        /// consul配置中心获取的配置
        /// </summary>
        /// <returns></returns>
        [Route("dbconfig")]
        public IActionResult DbConfig()
        {
            return Ok(this.dbConfig.Value);
        }

        /// <summary>
        /// 通过IConfiguration注入获取的配置（不推荐，使用太麻烦）
        /// </summary>
        /// <returns></returns>
        [Route("configuration")]
        public IActionResult IConfiguration()
        {
            return Ok(this.configuration.GetSection("ConsulServer").GetValue<string>("IP"));
        }

    }
}

