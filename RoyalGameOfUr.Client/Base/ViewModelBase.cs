using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RoyalGameOfUr.Client.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IDictionary<string, object> values = new Dictionary<string, object>();

        public void SetValue<T>(T val, [CallerMemberName] string name = "")
        {
            values.TryGetValue(name, out object oldValue);
            
            if(!(oldValue?.Equals(val) == true))
            {
                values[name] = val;
                OnPropertyChanged(name);
            }
        }

        public T GetValue<T>([CallerMemberName] string name = "")
        {
            if(values.TryGetValue(name, out object value))
            {
                if(value is T returnValue)
                {
                    return returnValue;
                }
            }

            return default;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
