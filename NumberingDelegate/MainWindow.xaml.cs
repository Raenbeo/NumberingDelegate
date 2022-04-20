using System;
using System.Collections.Generic;
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
using System.IO;
using System.Windows.Forms;

namespace NumberingDelegate
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            bool canPass = true;
             if(string.IsNullOrEmpty(NowCommonPathTxt.Text) == true) canPass = false;
             if(string.IsNullOrEmpty(CoummonExtensionTxt.Text) == true) canPass = false;

            if (canPass == false) 
            {
                System.Windows.Forms.MessageBox.Show("빈칸없이 채워주세요 ^^");
                return;
            }

            if (System.Windows.MessageBox.Show("입력하신 정보를 바탕으로 넘버링을 시작하시겠습니까?", "확인작업", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (NumberDGrid.Items.Count >= 1)
                {
                    //string newName = NowCommonPathTxt.Text + CommonNameTxt.Text+ + "." + CoummonExtensionTxt.Text;

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Clear();

                    int count = 1;
                    foreach(NumberingItem ain in NumberDGrid.Items)
                    {
                        stringBuilder.Append(NowCommonPathTxt.Text).Append("\\").Append(CommonNameTxt.Text).Append(count.ToString()).Append(".").Append(CoummonExtensionTxt.Text);
                        System.Diagnostics.Debug.WriteLine(stringBuilder.ToString());
                        ain.Rename(stringBuilder.ToString());
                        stringBuilder.Clear();
                        count += 1;
                    }

                    System.Windows.MessageBox.Show("넘버링 완료");

                    int itemCount = NumberDGrid.Items.Count;
                    for (int i = 0; i < itemCount; i++) NumberDGrid.Items.RemoveAt(0);

                    NumberDGrid.Items.Refresh();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("작업을 취소하였습니다");
            }
        }

        private void OpenFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Multiselect = true;

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] Names = fileDialog.SafeFileNames;
                string[] AllPathes = fileDialog.FileNames;

                for (int i = 0; i < Names.Length; i++)
                {
                    NumberDGrid.Items.Add(new NumberingItem(Names[i],AllPathes[i]));
                }

                NumberDGrid.Items.Refresh();

                string removeName = "\\" + Names[0]; 

                string path = AllPathes[0].Replace(removeName,string.Empty);

                NowCommonPathTxt.Text = path;
                AllCountTxt.Text = NumberDGrid.Items.Count.ToString();
            }
        }
    }

}

public class NumberingItem
{
    public NumberingItem(string filename, string allpath)
    {
        FileName = filename;
        AllPath = allpath;
    }

    public string FileName { get; set; }
    private string AllPath { get; set; }

    public void Rename(string newName)
    {
        System.IO.File.Move(AllPath, newName);
    }


}