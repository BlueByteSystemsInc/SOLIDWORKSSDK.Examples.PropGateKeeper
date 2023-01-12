/*
 Copyright 2023 BLUE BYTE SYSTEMS INC.
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using BlueByte.SOLIDWORKS.SDK.Attributes;
using BlueByte.SOLIDWORKS.SDK.Attributes.Menus;
using BlueByte.SOLIDWORKS.SDK.Core;
using SoftCircuits.WinSettings;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace PropGateKeeper
{
    [ComVisible(true)]
    [Guid("ef484870-5814-4d64-b091-aaa727253757")]

    [MenuItem("Prop Gate Keeper", SolidWorks.Interop.swconst.swDocumentTypes_e.swDocPART, true)]
    [MenuItem("Settings@Prop Gate Keeper", SolidWorks.Interop.swconst.swDocumentTypes_e.swDocPART, false, nameof(OpenSettings))]

    [MenuItem("Prop Gate Keeper", SolidWorks.Interop.swconst.swDocumentTypes_e.swDocASSEMBLY, true)]
    [MenuItem("Settings@Prop Gate Keeper", SolidWorks.Interop.swconst.swDocumentTypes_e.swDocASSEMBLY, false, nameof(OpenSettings))]

    [MenuItem("Prop Gate Keeper", SolidWorks.Interop.swconst.swDocumentTypes_e.swDocDRAWING, true)]
    [MenuItem("Settings@Prop Gate Keeper", SolidWorks.Interop.swconst.swDocumentTypes_e.swDocDRAWING, false, nameof(OpenSettings))]
    
    [MenuItem("Prop Gate Keeper", SolidWorks.Interop.swconst.swDocumentTypes_e.swDocNONE, true)]
    [MenuItem("Settings@Prop Gate Keeper", SolidWorks.Interop.swconst.swDocumentTypes_e.swDocNONE, false, nameof(OpenSettings))]


    [Name("PropGateKeeper")]
    [Description("PropGateKeeper")]
    [StartUp(true)]
    public class AddIn : AddInBase
    {


        public override void OnAddInPreClose()
        {
            base.OnAddInPreClose();

            this.Application.SendWarningMessage("You should be doing this !");
        }

        public void OpenSettings()
        {
            // todo: show a ui that enables the user to edit the settings 
            var settings = new Settings();
            settings.Load();

            var form = new SettingsForm(settings);

            form.ShowDialog();

            try
            {
                settings.Save();

            }
            catch (Exception)
            {

            }
            
        }

        protected override void OnConnectToSOLIDWORKS(SldWorks swApp)
        {
            base.OnConnectToSOLIDWORKS(swApp);
            this.CustomPropertyManager.CustomPropertyChanged += CustomPropertyManager_CustomPropertyChanged;
            this.CustomPropertyManager.CustomPropertyAdded += CustomPropertyManager_CustomPropertyAdded;
            this.CustomPropertyManager.CustomPropertyDeleted += CustomPropertyManager_CustomPropertyDeleted;
        }

        private void CustomPropertyManager_CustomPropertyDeleted(object sender, BlueByte.SOLIDWORKS.SDK.CustomProperties.CustomPropertyChangedEventArgs e)
        {
            var settings = new Settings();

            settings.Load();

            if (settings != null)
                if (settings.Properties.Contains(e.PropertyName, StringComparer.OrdinalIgnoreCase))
                {
                    // this will ignore the changes
                    e.Handled = true;
                    this.Application.SendErrorMessage("This operation is not permitted");
                }

        }

        private void CustomPropertyManager_CustomPropertyAdded(object sender, BlueByte.SOLIDWORKS.SDK.CustomProperties.CustomPropertyChangedEventArgs e)
        {
            var settings = new Settings();

            settings.Load();

            if (settings != null)
                if (settings.Properties.Contains(e.PropertyName, StringComparer.OrdinalIgnoreCase))
                {
                    // this will ignore the changes
                    e.Handled = true;
                    this.Application.SendErrorMessage("This operation is not permitted");
                }
        }

        private void CustomPropertyManager_CustomPropertyChanged(object sender, BlueByte.SOLIDWORKS.SDK.CustomProperties.CustomPropertyChangedEventArgs e)
        {
            var settings = new Settings();

            settings.Load();

            if (settings != null)
            if (settings.Properties.Contains(e.PropertyName, StringComparer.OrdinalIgnoreCase))
            {
                // this will ignore the changes
                e.Handled = true;
                this.Application.SendErrorMessage("This operation is not permitted");
            }
        }
    }


    public class Settings : RegistrySettings
    {

        public Settings(): base("Blue Byte Systems Inc", "PropGateKeeper", RegistrySettingsType.LocalMachine)
        {

        }

        public string[] Properties { get; set; } = new string[] { };
    }
}
