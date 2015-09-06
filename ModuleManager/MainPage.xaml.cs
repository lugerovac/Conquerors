using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ModuleManager
{
    public partial class MainPage : UserControl
    {
        //Source: http://www.codeproject.com/Articles/33151/Building-Modular-Silverlight-Applications

        private List<ModuleMaker> _moduleMakers = new List<ModuleMaker>();
        private ModuleMaker currentModule = new ModuleMaker();

        public MainPage()
        {
            InitializeComponent();
            InitializeModuleManager();
        }

        private void InitializeModuleManager()
        {
            ModuleMaker moduleMaker = new ModuleMaker();
            currentModule = moduleMaker;
            moduleMaker.ModuleContentReady += new EventHandler<ModuleReadyEventArgs>(onModuleContentReady);
            LayoutRoot.DataContext = moduleMaker;
            _moduleMakers.Add(moduleMaker);
        }

        private void onModuleContentReady(object sender, ModuleReadyEventArgs e)
        {
            LayoutRoot.Children.Add(e.ModuleContent);
        }

        public void ClearCanvas()
        {
            foreach(UserControl child in LayoutRoot.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
            
        }

        public void ShowControl(string module)
        {
            bool check = false;
            foreach(ModuleMaker mod in _moduleMakers)
            {
                if(string.Equals(mod.ModuleName, module))
                {
                    check = true;
                    break;
                }
            }
            if(!check) ModuleName = module;
            else
            {
                foreach(UserControl child in LayoutRoot.Children)
                {
                    if(string.Equals(child.Name, module))
                    {
                        child.Visibility = Visibility.Visible;
                        break;
                    }
                }
            }
        }

        public void SetModuleName(string newValue)
        {
            //this property setter will start the module downloading automatically
            currentModule.ModuleName = newValue;
        }

        private void SetModuleTypeName(string newValue)
        {
            currentModule.ModuleTypeName = newValue;
        }

        private void SetModuleAssemblyName(string newValue)
		{
            currentModule.ModuleAssemblyName = newValue;
		}

        private void SetModuleRelativePath(string newValue)
		{
            currentModule.ModuleRelativePath = newValue;
		}

        public static readonly DependencyProperty ModuleNameDependencyProperty =
			DependencyProperty.Register("ModuleName", typeof(string), typeof(MainPage), 
			new PropertyMetadata("abc", new PropertyChangedCallback(OnModuleNamePropertyChanged)));

        private static void OnModuleNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
			string newValue = (string)e.NewValue;
			if (String.IsNullOrEmpty(newValue))
				return;
			
			MainPage ctrl = (MainPage)d;
			ctrl.SetModuleName(newValue);
		}

        public string ModuleName
		{
            get { return (string)GetValue(ModuleNameDependencyProperty); }
			set { SetValue(ModuleNameDependencyProperty, value); }
		}

        public static readonly DependencyProperty ModuleTypeNameDependencyProperty =
            DependencyProperty.Register("ModuleTypeName", typeof(string), typeof(MainPage),
            new PropertyMetadata("", new PropertyChangedCallback(OnModuleTypeNamePropertyChanged)));

        private static void OnModuleTypeNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            if (String.IsNullOrEmpty(newValue))
                return;

            MainPage ctrl = (MainPage)d;
            ctrl.SetModuleTypeName(newValue);
        }

        public string ModuleTypeName
        {
            get { return (string)GetValue(ModuleTypeNameDependencyProperty); }
            set { SetValue(ModuleTypeNameDependencyProperty, value); }
        }

        public static readonly DependencyProperty ModuleAssemblyNameDependencyProperty =
            DependencyProperty.Register("ModuleAssemblyName", typeof(string), typeof(MainPage),
            new PropertyMetadata("", new PropertyChangedCallback(OnModuleAssemblyPropertyChanged)));


        private static void OnModuleAssemblyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            if (String.IsNullOrEmpty(newValue))
                return;

            MainPage ctrl = (MainPage)d;
            ctrl.SetModuleAssemblyName(newValue);
        }

        public string ModuleAssemblyName
        {
            get { return (string)GetValue(ModuleAssemblyNameDependencyProperty); }
            set { SetValue(ModuleAssemblyNameDependencyProperty, value); }
        }

        public static readonly DependencyProperty ModuleRelativePathDependencyProperty =
            DependencyProperty.Register("ModuleRelativePath", typeof(string), typeof(MainPage),
            new PropertyMetadata("", new PropertyChangedCallback(OnModuleRelPathPropertyChanged)));

        private static void OnModuleRelPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string newValue = (string)e.NewValue;
            if (String.IsNullOrEmpty(newValue))
                return;

            MainPage ctrl = (MainPage)d;
            ctrl.SetModuleRelativePath(newValue);
        }

        public string ModuleRelativePath
        {
            get { return (string)GetValue(ModuleRelativePathDependencyProperty); }
            set { SetValue(ModuleRelativePathDependencyProperty, value); }
        }
    }
}
