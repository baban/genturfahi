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
        string parseResult = "";

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

        public string ParseResult
        {
            get { return parseResult; }
            set
            {
                parseResult = value;

                OnPropertyChanged("parseResult");
            }
        }

        public void parse() { 
        }
    }
}
