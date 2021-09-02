using Microsoft.Extensions.Configuration;
using System;

namespace BlazorApp.Models
{
    /// <summary>
    /// appsettings.json
    /// </summary>
    public class JsonSettingsEntity
    {
        #region Public and private fields and properties

        public IConfiguration Configuration { get; }

        public int Key
        {
            get => Convert.ToInt32(Configuration["Section:Key"]);
            set => Configuration["Section:Key"] = value.ToString();
        }

        #endregion

        #region Constructor and destructor

        public JsonSettingsEntity(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Public and private methods

        public override string ToString()
        {
            return $"{nameof(Key)}: {Key}. ";
        }

        #endregion
    }
}
