using BlazorApp.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Shared
{
    public partial class MainLayout
    {
        #region Public and private fields and properties - Inject

        [Inject] public JsonSettingsEntity JsonAppSettings { get; private set; }

        #endregion
    }
}
