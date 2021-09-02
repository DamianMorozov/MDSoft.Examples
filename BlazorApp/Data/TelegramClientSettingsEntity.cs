namespace BlazorApp.Models
{
    public class TelegramClientSettingsEntity
    {
        #region Public and private fields and properties

        public int ApiId { get; set; }
        public string ApiHash { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public string CloudPsw { get; set; }
        public string CodeHash { get; set; }
        public string Message { get; set; }

        #endregion

        #region Constructor and destructor

        public TelegramClientSettingsEntity()
        {
            //
        }

        #endregion

        #region Public and private methods

        public override string ToString()
        {
            return $"{nameof(ApiId)}: {ApiId}. " +
                   $"{nameof(ApiHash)}: {ApiHash}. " +
                   $"{nameof(Phone)}: {Phone}. " +
                   $"{nameof(Code)}: {Code}. " +
                   $"{nameof(CodeHash)}: {CodeHash}. " +
                   $"{nameof(Message)}: {Message}. ";
        }

        #endregion
    }
}
