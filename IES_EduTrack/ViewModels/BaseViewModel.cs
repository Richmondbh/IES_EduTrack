using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace IES_EduTrack.ViewModels
{
    /// <summary>
    /// Abstract base for all ViewModels. Implements INotifyPropertyChanged
    /// so the WPF binding engine updates the UI when properties change
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Call this inside any property setter to notify the UI
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Sets the backing field and fires OnPropertyChanged only if value actually changed
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}