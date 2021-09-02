using BlazorApp.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telega;
using Telega.Rpc.Dto.Functions.Users;
using Telega.Rpc.Dto.Types;

namespace BlazorApp.Pages
{
    public partial class TelegramClientPage
    {
        #region Public and private fields and properties - Inject

        [Inject] public JsonSettingsEntity JsonAppSettings { get; private set; }

        public TelegramClient TelegramClientItem { get; set; }
        public TelegramClientSettingsEntity TelegramClientSettings  { get; set; }

        #endregion

        #region Public and private fields and properties

        public List<string> Log { get; set; }

        #endregion

        #region Public and private methods

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters).ConfigureAwait(true);

            TelegramClientSettings = new TelegramClientSettingsEntity();
            Log = new List<string>();
        }

        private async Task ClearLog()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            Log.Clear();
        }

        private async Task SendCode()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            try
            {
                TelegramClientItem = await TelegramClient.Connect(TelegramClientSettings.ApiId);
                if (TelegramClientItem.Auth.IsAuthorized)
                {
                    Log.Add("You're already authorized.");
                }
                else
                {
                    Log.Add("Authorizing.");
                    TelegramClientSettings.CodeHash = await TelegramClientItem.Auth.SendCode(TelegramClientSettings.ApiHash, TelegramClientSettings.Phone);
                }
            }
            catch (Exception ex)
            {
                Log.Add($"Exception: {ex.Message}");
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    Log.Add($"InnerException: {ex.InnerException.Message}");
            }
        }

        private async Task SignIn()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            await SendCode().ConfigureAwait(true);
            try
            {
                if (TelegramClientItem.Auth.IsAuthorized)
                    return;
                await TelegramClientItem.Auth.SignIn(TelegramClientSettings.Phone, TelegramClientSettings.CodeHash, TelegramClientSettings.Code);
                if (!string.IsNullOrEmpty(TelegramClientSettings.CloudPsw))
                {
                    //await TelegramClientItem.Auth.CheckPassword(CloudPsw);
                }
            }
            catch (Exception ex)
            {
                Log.Add($"Exception: {ex.Message}");
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    Log.Add($"InnerException: {ex.InnerException.Message}");
            }
        }

        private async Task GetAccountInfo()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            try
            {
                InputUser.SelfTag myUser = new InputUser.SelfTag();
                UserFull myInfo = await TelegramClientItem.Call(new GetFullUser(myUser));
                Log.Add(myInfo.ToString());
            }
            catch (Exception ex)
            {
                Log.Add($"Exception: {ex.Message}");
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    Log.Add($"InnerException: {ex.InnerException.Message}");
            }
        }

        private async Task SendMessage()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            try
            {
                InputPeer.SelfTag recipient = new InputPeer.SelfTag();
                await TelegramClientItem.Messages.SendMessage(recipient, TelegramClientSettings.Message);
            }
            catch (Exception ex)
            {
                Log.Add($"Exception: {ex.Message}");
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    Log.Add($"InnerException: {ex.InnerException.Message}");
            }
        }

        private async Task SendPhoto()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            try
            {
                // Download photo.
                string photoUrl = "https://cdn1.img.jp.sputniknews.com/images/406/99/4069980.png";
                System.Net.WebClient webClient = new System.Net.WebClient();
                byte[] photo = await webClient.DownloadDataTaskAsync(photoUrl);
                // Upload photo.
                InputFile tgPhoto = await TelegramClientItem.Upload.UploadFile(
                    "photo.png",
                    photo.Length,
                    new System.IO.MemoryStream(photo)
                );
                // Send photo.
                InputPeer.SelfTag recipient = new InputPeer.SelfTag();
                await TelegramClientItem.Messages.SendPhoto(
                    peer: recipient,
                    file: tgPhoto,
                    message: ""
                );
            }
            catch (Exception ex)
            {
                Log.Add($"Exception: {ex.Message}");
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    Log.Add($"InnerException: {ex.InnerException.Message}");
            }
        }

        private async Task GetChannels()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
            try
            {
                Telega.Rpc.Dto.Types.Messages.Dialogs dialogs = await TelegramClientItem.Messages.GetDialogs();
                IEnumerable<Telega.Rpc.Dto.Types.Messages.Dialogs.Tag> tags = dialogs.AsTag().AsEnumerable();
                foreach (Telega.Rpc.Dto.Types.Messages.Dialogs.Tag tag in tags)
                {
                    LanguageExt.Arr<Dialog> dialogs2 = tag.Dialogs;
                    foreach (Dialog dialog2 in dialogs2)
                    {
                        foreach (Dialog.Tag item in dialog2.AsTag())
                        {
                            Log.Add($"ID: {item.Peer}");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Add($"Exception: {ex.Message}");
                if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                    Log.Add($"InnerException: {ex.InnerException.Message}");
            }
        }

        #endregion
    }
}
