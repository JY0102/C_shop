using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Riot_Search_Client
{
    internal class Match_Request : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #region request
        [JsonProperty(Order = 1)]
        public string gamenickname
        {
            get => _gamenickname;
            set { _gamenickname = value; OnPropertyChanged(); }
        }
        private string _gamenickname;

        [JsonProperty(Order = 2)]
        public string gametype
        {
            get
            {
                if (_gametype == null)
                    return string.Empty;
                else if (_gametype != "All")
                    return _gametype;
                else
                    return string.Empty;
            }
            set
            {
                _gametype = value;
                OnPropertyChanged();
            }
        }
        private string _gametype;

        [JsonProperty(Order = 3)]
        public string gamecount
        {
            get => _gamecount;
            set { _gamecount = value; OnPropertyChanged(); }
        }
        private string _gamecount = " 1~20 ";
        #endregion
    }
}
