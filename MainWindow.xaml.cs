using SDBBGuiHelper.GUI;
using SDBBGuiHelper.Sheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SDBBGuiHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        public void ReadSheet()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "CharSheet",
                DefaultExt = ".txt",
            };

            bool? result = dialog.ShowDialog();
            if (result != true)
            {
                return;
            }

            var outputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = folderDialog.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    outputPath = folderDialog.SelectedPath; 
                }
            }
            

            var sheetreader = new SheetReader(new(dialog.FileName));
            sheetreader.ReadCharSkinSheet();

            sheetreader.PrintMenus(System.IO.Path.Combine(outputPath,"skins"));
            sheetreader.PrintMenuConfig(System.IO.Path.Combine(outputPath, "skins", "configs"));
        }

        private void ReadSheet_Click(object sender, RoutedEventArgs e)
        {
            ReadSheet();
            MessageBox.Show("Success!");
        }
    }
}
