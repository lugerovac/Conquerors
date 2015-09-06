using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace ModuleManager
{
    public class ModuleMaker : INotifyPropertyChanged
    {
        //Source: http://www.codeproject.com/Articles/33151/Building-Modular-Silverlight-Applications

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event EventHandler<ModuleReadyEventArgs> ModuleContentReady = delegate { };
        private WebClient _webClient = null;

        private string _moduleName;
        public string ModuleName
        {
            get
            {
                return _moduleName;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("ModuleName should never be empty or null!");

                if (value != _moduleName)
                {
                    _moduleName = value;
                    StartToDownloadModule();
                }
            }
        }

        private string _moduleAssemblyName;
        public string ModuleAssemblyName
        {
            get
            {
                return (String.IsNullOrEmpty(this._moduleAssemblyName)) ? this.ModuleName + ".dll" : this._moduleAssemblyName;
            }
            set
            {
                this._moduleAssemblyName = value;
            }
        }
        public string ModuleRelativePath { get; set; }

        private string _moduleTypeName;
        public string ModuleTypeName
        {
            get
            {
                return String.IsNullOrEmpty(_moduleTypeName) ? ModuleName + "." + ModuleName : _moduleTypeName;
            }
            set
            {
                _moduleTypeName = value;
            }
        }

        public string ModuleURL
        {
            get
            {
                if (String.IsNullOrEmpty(ModuleName))
                    throw new ArgumentNullException("ModuleName needs to be set before get ModuleURL");

                string moduleFolder = String.IsNullOrEmpty(ModuleRelativePath) ? "" : ModuleRelativePath + "/";
                return moduleFolder + ModuleName + ".xap";
            }
        }

        public string Error
        {
            set
            {
                MessageBox.Show("An error occurred while trying to laod the " + ModuleName + " plug-in! " + value);
            }
        }

        private void StartToDownloadModule()
        {
            if (null == _webClient)
            {//initialize the downloader
                _webClient = new WebClient();
                _webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(onOpenReadCompleted);
            }

            if (_webClient.IsBusy)
            {//needs to cancel the previous loading
                _webClient.CancelAsync();
                return;
            }

            Uri xapUrl = new Uri(this.ModuleURL, UriKind.RelativeOrAbsolute);
            _webClient.OpenReadAsync(xapUrl);
        }

        private void onOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (null != e.Error)
            {//report the download error
                this.Error = e.Error.Message;
                return;
            }

            if (e.Cancelled)
            {// cancelled previous downloading, needs to re-start the loading
                StartToDownloadModule();
                return;
            }

            // Load a particular assembly from XAP
            Assembly aDLL = GetAssemblyFromPackage(this.ModuleAssemblyName, e.Result);
            if (null == aDLL)
            {//report the assembly extracting error
                this.Error = "Module downloaded but failed to extract the assembly. Please check the assembly name.";
                return;
            }

            // Get an instance of the XAML object
            UserControl content = aDLL.CreateInstance(this.ModuleTypeName) as UserControl;
            if (null == content)
            {//report the type instnatiating error
                this.Error = "Module downloaded and the assembly extracted but failed to instantiate the custome type. Please check the type name.";
                return;
            }

            content.Name = ModuleName;
            //tell the event handler it's ready to display
            ModuleContentReady(this, new ModuleReadyEventArgs() { ModuleContent = content });
        }

        private Assembly GetAssemblyFromPackage(string assemblyName, Stream xapStream)
        {
            Assembly aDLL = null;

            Uri assemblyUri = new Uri(assemblyName, UriKind.Relative);
            StreamResourceInfo resPackage = new StreamResourceInfo(xapStream, null);
            if (null == resPackage)
                return aDLL;

            StreamResourceInfo resAssembly = Application.GetResourceStream(resPackage, assemblyUri);
            if (null == resAssembly)
                return aDLL;

            AssemblyPart part = new AssemblyPart();
            aDLL = part.Load(resAssembly.Stream);

            return aDLL;
        }
    }
}
