using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Snow.ReadTemplate.Common
{
    /// <summary>
    /// Provides a standard change-notification implementation.
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the property with the specified
        /// name, or the calling property if no name is specified.
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// Checks whether the value of the specified field is different than the specified value, and
        /// if they are different, updates the field and raises the PropertyChanged event for
        /// the property with the specified name, or the calling property if no name is specified. 
        /// </summary>
        protected bool SetProperty<T>(ref T storage, T value,
            [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Checks if a property already matches a desired value. Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool Set<T>(ref T storage, T value,
            [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
