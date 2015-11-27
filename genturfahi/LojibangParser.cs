using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace genturfahi
{
    class LojibangParser : INotifyPropertyChanged
    {
        string lojibanContent = "";
        public string parseResult;
        public string message;

        public string fileName = null;

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public string LojibanContent {
            get { return lojibanContent; }
            set {
                lojibanContent = value;

                OnPropertyChanged("lojibanContent");
            }
        }

        public bool isParseError(string error)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(error, @"Unknown cmavo zee at line \d+, column \d+; selma'o UI assumed"))
            {
                return false;
            }
            return true;
        }

        public string ParseResult
        {
            get { return parseResult; }
            set
            {
                parseResult = value;

                OnPropertyChanged("parseResult");
            }
        }
    }
}
