using Microsoft.Win32;
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

namespace genturfahi
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public struct ParseResult
        {
            public enum Status { INIT, OK, ERROR };
            public Status status;
            public string message;
        }

        // エラーパターン:
        // 空文字
        // 解析結果に/FA'O/が帰る
        // Unknown cmavo zee at line 2, column 0; selma'o UI assumed

        //  coi .i (%) ke7e

        LojibangParser lojibanParser = null;

        public MainWindow()
        {
            InitializeComponent();

            string initStr = "coi";

            if (App.CommandLineArgs != null)
            {
                initStr = LoadInitFile();
            }

            lojibanParser = new LojibangParser
            {
                LojibanContent = initStr,
                parseResult = genturfahi.Properties.Resources.ResultFieldDescription
            };
            this.DataContext = lojibanParser;
        }

        private string LoadInitFile()
        {
            string filename = App.CommandLineArgs.First();
            return loadFile(filename);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(lojibanText.Text);
            setParseResult();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                setParseResult();
                Console.WriteLine(lojibanText.Text);
            }
        }

        private void setParseResult() {
            ParseResult ret = ExecParser(lojibanText.Text.Replace(Environment.NewLine, ""));
            lojibanParser.parseResult = ret.message;
            this.DataContext = lojibanParser;
            parseResultField.Text = ret.message;
            if (ret.status == ParseResult.Status.OK) {
                parseResultField.Foreground = new SolidColorBrush(Colors.Black);
            }
            else if (ret.status == ParseResult.Status.ERROR)
            {
                parseResultField.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private string dirPath()
        {
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            System.Diagnostics.Debug.WriteLine(appPath);
            string dirPath = System.IO.Path.GetDirectoryName(appPath);
            System.Diagnostics.Debug.WriteLine(dirPath);
            return dirPath;
        }

        private ParseResult ExecParser(string str)
        {
            if( str=="" ){
                return BlankError();
            }

            string commandPath = "/c echo \"" + str + "\" | \"" + dirPath() + "\\bin\\parser.exe";
            
            //Processオブジェクトを作成
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            //出力を読み取れるようにする
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.ErrorDataReceived += p_ErrorDataReceived;

            //ウィンドウを表示しないようにする
            p.StartInfo.CreateNoWindow = true;
            //コマンドラインを指定（"/c"は実行後閉じるために必要）
            //p.StartInfo.Arguments = @"/c dir c:\ /w";
            p.StartInfo.Arguments = commandPath;

            //起動
            p.Start();
            Console.WriteLine("end");
            //出力を読み取る
            //string results = p.StandardOutput.ReadToEnd();
            string results = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            Console.WriteLine("readed");
            //プロセス終了まで待機する
            //WaitForExitはReadToEndの後である必要がある
            //(親プロセス、子プロセスでブロック防止のため)
            p.WaitForExit();
            p.Close();
            //出力された結果を表示
            Console.WriteLine("results:" + results);
            Console.WriteLine("error:"+error);

            ParseResult ret = setResult( results, error );

            return ret;
        }

        private ParseResult BlankError()
        {
            ParseResult ret = new ParseResult();
            ret.status = ParseResult.Status.ERROR;
            ret.message = "text is blank";
            return ret;
        }

        private ParseResult setResult(string results, string error)
        {
            ParseResult ret = new ParseResult();
            if (isParseError(error))
            {
                ret.status = ParseResult.Status.ERROR;
                ret.message = extractErrorMessage(error);
                return ret;
            }
            else
            {
                ret.status = ParseResult.Status.OK;
                ret.message = results;
                return ret;
            }
        }

        public bool isParseError( string error ) {
            if (System.Text.RegularExpressions.Regex.IsMatch(error, "line"))
            {
                return true;
            }
            return false;
        }

        private string extractErrorMessage(string error) {
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches( error, ".*?line.*?\n"  );
            string s = "";
            foreach(System.Text.RegularExpressions.Match match in matches){
                s += match.Value;
            }
            //Console.WriteLine("extractErrorMessage:"+s);
            return s;
        }

        public void OnSelect_Open(object sender, RoutedEventArgs e)
        {
            loadNewFile();
        }

        public void OnSelect_Save(object sender, RoutedEventArgs e)
        {
            if (lojibanParser.fileName == null)
            {
                saveNewFile();
            }
            else
            {
                saveFile(lojibanParser.fileName);
            }
        }

        public void OnSelect_SaveAs(object sender, RoutedEventArgs e)
        {
            saveNewFile();
        }

        public void OnDrop() {
        }

        public void OnSelect_Copyright(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.FileVersionInfo app =
                System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(app);
            string messageBoxText = String.Format("{0} Version: {1} ", app.ProductName, app.ProductVersion);
            string caption = app.ProductName;
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.None;
            MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void saveFile( string fileName ){
            using (StreamWriter sr = new StreamWriter(fileName, false))
            {
                string txt = lojibanText.Text;
                sr.Write(txt);
            }
        }

        private void loadNewFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "";
            ofd.DefaultExt = "*.*";
            ofd.FilterIndex = 1;
            ofd.Filter = "テキスト ファイル(.txt)|*.txt|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                
                using (Stream fileStream = ofd.OpenFile())
                {
                    lojibanText.Text = loadFile( filename );
                    lojibanParser.fileName = filename;
                }
            }
        }

        private string loadFile(string fileName) {
            return loadFile(new FileStream(fileName, FileMode.Open));
        }

        private string loadFile(Stream fileStream)
        {
            string txt = "";
            using (fileStream)
            {
                StreamReader sr = new StreamReader(fileStream, true);
                txt = sr.ReadToEnd();
            }
            return txt;
        }

        private void saveNewFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.Filter = "テキスト ファイル(.txt)|*.txt|HTML File(*.html, *.htm)|*.html;*.htm|All Files (*.*)|*.*";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string txt = lojibanText.Text;
                string filename = saveFileDialog.FileName;
                
                using (Stream fileStream = saveFileDialog.OpenFile()) {
                    
                    using (StreamWriter sr = new StreamWriter(fileStream))
                    {
                        sr.Write(txt);
                    }
                }
                lojibanParser.fileName = filename;
            }
        }

        //ErrorDataReceivedイベントハンドラ
        static void p_ErrorDataReceived(object sender,
            System.Diagnostics.DataReceivedEventArgs e)
        {
            //エラー出力された文字列を表示する
            Console.WriteLine("ERR>{0}", e.Data);
        }
    }
}
