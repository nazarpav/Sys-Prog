using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SP_Exam
{
    public partial class MainWindow : Window
    {
        private ThreadStart _TS;
        private Thread _T;
        public ObservableCollection<string> LVVocabularyObsCol { get; set; }
        private Dictionary<string, int> TopBadWords;
        public MainWindow()
        {
            InitializeComponent();
            _TS = new ThreadStart(StartSearchAsync);
            TopBadWords = new Dictionary<string, int>();
            LVVocabularyObsCol = new ObservableCollection<string>();
            LVVocabulary.ItemsSource = LVVocabularyObsCol;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //trash
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TBTrash.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void BTStart_Pause_Click_1(object sender, RoutedEventArgs e)
        {
            //start
            try
            {
            if (BTStart_Pause.Content.ToString() == "Start")
            {
                _T = new Thread(_TS);
                _T.Start();
            }
            else if (BTStart_Pause.Content.ToString() == "Pause")
            {
                _T.Suspend();
                BTStart_Pause.Content = "Start";
            }
            }
            catch
            {
            }
        }
        private void StartSearchAsync()
        {
            try
            {
                string[] files = Directory.GetFiles(Dispatcher.Invoke(() => TBSeaFol.Text), "*.txt", SearchOption.AllDirectories);
                Dispatcher.Invoke(() => FoundesFilesWithBadWords.Content = files.Length);
                Dispatcher.Invoke(() => PRBar.Maximum = files.Length);
                Dispatcher.Invoke(() => BTStop.IsEnabled = true);
                Dispatcher.Invoke(() => BTStart_Pause.Content = "Pause");
                SearchStart();
                FindBadWordsInFiles(files);
            }
            catch
            {
                System.Windows.MessageBox.Show("Start error");
            }
        }

        private async void FindBadWordsInFiles(string[] files)
        {
            string buf = string.Empty;
            const string NEWVALUE = "*******";
            int count = 0;
            int count2 = 0;
            Stack<DataBadWordsFile> LDBWF = new Stack<DataBadWordsFile>();
            try
            {
                foreach (var file in files)
                {
                    try
                    {
                        LDBWF.Push(new DataBadWordsFile());
                        count2 = 0;
                        using (StreamReader sr = new StreamReader(file))
                        {
                            buf = await sr.ReadToEndAsync();
                        }
                        bool flag = false;
                        Dispatcher.Invoke(() => FoundesFilesWithBadWords.Content = files.Length);
                        Dispatcher.Invoke(() => CurrentFile.Text = file);
                        Dispatcher.Invoke(() => FilesProcessed.Content = count++);
                        foreach (var item in Dispatcher.Invoke(() => LVVocabulary.Items))
                        {
                            count2++;
                            if (buf.Contains(item.ToString()))
                            {
                                Thread.Sleep(70);
                                int countBadWords = buf.Split(new string[] { item.ToString() }, StringSplitOptions.None).Count() - 1;
                                Dispatcher.Invoke(() => NumberOfBadWords.Content = int.Parse(NumberOfBadWords.Content.ToString()) + countBadWords);
                                LDBWF.Peek().QuantityBadWords += countBadWords;
                                UpdateTopBadWords(item.ToString(), buf);
                                flag = true;
                                buf = buf.Replace(item.ToString(), NEWVALUE);
                            }
                        }
                        if (flag)
                        {
                            LDBWF.Peek().Name = file;
                            File.Copy(file, Dispatcher.Invoke(() => TBTrash.Text) + Revers_(GetFileName(file)));
                            using (StreamWriter sw = new StreamWriter(file, false, System.Text.Encoding.Default))
                            {
                                await sw.WriteLineAsync(buf);
                            }
                        }
                        Dispatcher.Invoke(() => PRBar.Value++);
                        //Thread.Sleep(1000);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Eror");
            }
            finally
            {
                WriteDataOfBadWordsFiles(LDBWF);
                SearchEnded();
            }
        }
        private void UpdateTopBadWords(string item, string buf)
        {
            if (!Dispatcher.Invoke(() => Dispatcher.Invoke(() => TopBadWords.ContainsKey(item))))
            {
                Dispatcher.Invoke(() => TopBadWords.Add(item.ToString(), 0));
                Dispatcher.Invoke(() => TopBadWords[item] += buf.Split(new string[] { item }, StringSplitOptions.None).Count() - 1);
            }
            else
            {
                Dispatcher.Invoke(() => TopBadWords[item] += buf.Split(new string[] { item }, StringSplitOptions.None).Count() - 1);
            }
            Dispatcher.Invoke(() => LVVocabulary2.Items.Clear());
            foreach (var i in Dispatcher.Invoke(() => Dispatcher.Invoke(() => TopBadWords.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value).Take(10))))
            {
                Dispatcher.Invoke(() => LVVocabulary2.Items.Add(i.Key));
            }
        }
        private void SearchEnded()
        {
            System.Windows.MessageBox.Show("Seach ended");
            Dispatcher.Invoke(() => PRBar.Value = 0);
            Dispatcher.Invoke(() => TBVocabulary.IsEnabled = true);
            Dispatcher.Invoke(() => TBSeaFol.IsEnabled = true);
            Dispatcher.Invoke(() => TBTrash.IsEnabled = true);
            Dispatcher.Invoke(() => CurrentFile.Text = "?");
            Dispatcher.Invoke(() => FoundesFilesWithBadWords.Content = "0");
            Dispatcher.Invoke(() => NumberOfBadWords.Content = "0");
            Dispatcher.Invoke(() => FilesProcessed.Content = "0");
            Dispatcher.Invoke(() => BTStart_Pause.Content = "Start");
        }
        private async void WriteDataOfBadWordsFiles(Stack<DataBadWordsFile> files)
        {
            using (StreamWriter sw = new StreamWriter(Dispatcher.Invoke(() => TBTrash.Text.ToString())+@"\ReportFile____________________.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine("__________________________________________________________________________________+\nDate/Time: " + DateTime.Now.ToString());
                foreach (var item in files)
                {
                    await sw.WriteLineAsync("Path: " + item + "\nCreationTime: " + File.GetCreationTime(item.Name) + "\nSize:" + new FileInfo(item.Name).Length +
                        "\nQuantity bad words: " + item.QuantityBadWords);
                    sw.WriteLine("_________________________________________________________________________________");
                }
                sw.WriteLine("Top Bad Words: ");
                foreach (var item in Dispatcher.Invoke(() => LVVocabulary2.Items))
                {
                    await sw.WriteLineAsync(item.ToString());
                }
            }
        }
        private void SearchStart()
        {
            Dispatcher.Invoke(() => LVVocabulary2.Items.Clear());
            Dispatcher.Invoke(() => BTStop.IsEnabled = true);
            Dispatcher.Invoke(() => TBVocabulary.IsEnabled = false);
            Dispatcher.Invoke(() => TBSeaFol.IsEnabled = false);
            Dispatcher.Invoke(() => TBTrash.IsEnabled = false);
            Dispatcher.Invoke(() => BTStop.IsEnabled = false);
        }
        private string Revers_(string s)
        {
            string str = string.Empty;
            foreach (var i in s.Reverse())
            {
                str += i;
            }
            return str;
        }
        private string GetFileName(string fullFileName)
        {
            string FName = string.Empty;
            foreach (var i in fullFileName.Reverse())
            {
                FName += i;
                if (i == '/' || i.ToString() == @"\")
                {
                    return FName;
                }
            }
            return FName;
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //vocabulary
            string Vocabulary = string.Empty;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    TBVocabulary.Text = dlg.FileName;
                }
                using (StreamReader sr = new StreamReader(TBVocabulary.Text))
                {
                    Vocabulary = await sr.ReadToEndAsync();///////////////////////////////////////////////асинхронно
                }
                foreach (var item in Vocabulary.Split(';'))
                {
                    if (!LVVocabularyObsCol.Contains(item.ToString()))
                    {
                        LVVocabularyObsCol.Add(item.ToString());
                    }
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Eror");
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Search folder
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TBSeaFol.Text = folderBrowserDialog.SelectedPath;
            }
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (!LVVocabulary.Items.Contains(VocabularyADD.Text.ToString()))
            {
                if (VocabularyADD.Text.ToString() != "")
                {
                    LVVocabularyObsCol.Add(VocabularyADD.Text.ToString());
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Vocabulary already contains is " + VocabularyADD.Text.ToString());
            }
        }
        private void BTStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _T.Abort();
            }
            catch
            {

            }
        }
        private async void TBVocabulary_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string Vocabulary = string.Empty;
                try
                {
                    using (StreamReader sr = new StreamReader(TBVocabulary.Text))
                    {
                        Vocabulary = await sr.ReadToEndAsync();///////////////////////////////////////////////асинхронно
                    }
                    foreach (var item in Vocabulary.Split(';'))
                    {
                        if (!LVVocabularyObsCol.Contains(item.ToString()))
                        {
                            LVVocabularyObsCol.Add(item.ToString());
                        }
                    }
                }
                catch
                {
                    System.Windows.MessageBox.Show("Eror");
                }
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<string> ElemToDel = new List<string>();
            foreach (var i in LVVocabulary.SelectedItems)
            {
                ElemToDel.Add(i.ToString());
            }
            foreach (var i in ElemToDel)
            {
                try
                {
                    LVVocabularyObsCol.Remove(i.ToString());
                }
                catch
                {
                }
            }
        }
    }
}
