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

            var sheetreader = new SheetReader(new(dialog.FileName));
            sheetreader.ReadCharSkinSheet();

            sheetreader.PrintMenus(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"skins"));
            sheetreader.PrintMenuConfig(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "skins", "configs"));
        }

        public void RunTest()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "TestOutput",
                DefaultExt = ".yml"
            };

            bool? result = dialog.ShowDialog();

            if (result != true)
            {
                return;
            }
            string filename = dialog.FileName;
            StreamWriter outputFile = new StreamWriter(filename);

            GuiMenu testMenu = new("Drip Sam''s Skins", "dripsamskin");

            GuiItem testItem1 = new("defaultskin_red", "&r&eDefault Skin(blue)", "STONE", 1);

            testItem1.AddLore("Basic Sam Skin (Red)");

            GuiRequirement viewRequirementsRed = new();
            GuiReqItem redTeamCheck = new("team_check", "string_equals");
            redTeamCheck.AddExtraInfo("input: '%team_name%'");
            redTeamCheck.AddExtraInfo("output: 'Red'");
            viewRequirementsRed.AddReqItem(redTeamCheck);

            testItem1.AddViewRequirement(viewRequirementsRed);

            GuiRequirement clickRequirements = new();
            GuiReqItem scoreCheck = new("score_check", "string equals");
            scoreCheck.AddExtraInfo("input: '%objective_score_{heroType}%'");
            scoreCheck.AddExtraInfo("output: '1'");
            clickRequirements.AddReqItem(scoreCheck);

            testItem1.AddClickRequirement(clickRequirements);

            testItem1.AddClickCommand("[player] skin url my_url");
            testItem1.AddClickCommand("[close]");



            GuiItem testItem2 = new("defaultskin_blue", "&r&eDefault Skin(blue)", "STONE", 2);

            testItem2.AddLore("Basic Sam Skin (Blue)");

            GuiRequirement viewRequirementsBlue = new();
            GuiReqItem blueTeamCheck = new("team_check", "string_equals");
            blueTeamCheck.AddExtraInfo("input: '%team_name%'");
            blueTeamCheck.AddExtraInfo("output: 'Blue'");
            viewRequirementsBlue.AddReqItem(blueTeamCheck);

            testItem2.AddViewRequirement(viewRequirementsBlue);
            testItem2.AddClickRequirement(clickRequirements);

            testItem2.AddClickCommand("[player] skin url my_url");
            testItem2.AddClickCommand("[close]");

            testMenu.AddItem(testItem1);
            testMenu.AddItem(testItem2);

            testMenu.PrintMenu(outputFile);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RunTest();
        }

        private void ReadSheet_Click(object sender, RoutedEventArgs e)
        {
            ReadSheet();
            MessageBox.Show("Success!");
        }
    }
}
