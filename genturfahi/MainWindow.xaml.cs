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
            public enum Status { OK, ERROR };
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
            lojibanParser = new LojibangParser
            {
                LojibanContent = "coi"
            };
            this.DataContext = lojibanParser;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(lojibanText.Text);
            ParseResult ret = ExecParser(lojibanText.Text.Replace(Environment.NewLine, ""));
            lojibanParser.parseResult = ret.message;
            this.DataContext = lojibanParser;
            parseResultField.Text = ret.message;
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Console.WriteLine(lojibanText.Text);
                ParseResult ret = ExecParser(lojibanText.Text.Replace(Environment.NewLine, ""));
                lojibanParser.parseResult = ret.message;
                this.DataContext = lojibanParser;
                parseResultField.Text = ret.message;
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
            string commandPath = "/c echo \"" + str + "\" | \"" + dirPath() + "\\parser.exe";
            
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

        private ParseResult setResult(string results, string error)
        {
            ParseResult ret = new ParseResult();
            if (isParseError(error))
            {
                ret.status = ParseResult.Status.OK;
                ret.message = extractErrorMessage(error);
                parseResultField.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                ret.status = ParseResult.Status.ERROR;
                ret.message = results;
                parseResultField.Foreground = new SolidColorBrush(Colors.Black);
            }
            return ret;
        }

        public bool isParseError( string error ) {
            if (System.Text.RegularExpressions.Regex.IsMatch(error, @"Unknown cmavo zee at line \d+, column \d+; selma'o UI assumed"))
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
            Console.WriteLine("extractErrorMessage:"+s);
            return s;
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
