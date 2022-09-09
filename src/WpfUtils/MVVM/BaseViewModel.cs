using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtils.MVVM
{
    /// <summary>
    /// A basic yet powerful implementation of the INotifyPropertyChanged-interface.
    /// 
    /// You are supposed to inherit this class in your ViewModel. <typeparamref name="T"/> is the type of your parent-object.
    /// This can be a window, user-control, etc.
    /// 
    /// You can also access all the ViewModel's globally using the static-functions provided by this class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseViewModel<T> : INotifyPropertyChanged
    {
        #region Static
        private static List<object> _viewModels = new List<object>();
        /// <summary>
        /// A list of ALL the viewmodels that were created using this class.
        /// </summary>
        public static List<object> ViewModels
        {
            get
            {
                foreach (var item in _viewModels.Where(vm => vm == null).ToList())
                {
                    _viewModels.Remove(item);
                }

                return _viewModels;
            }
        }

        /// <summary>
        /// Find a ViewModel by its Parent's Type.
        /// </summary>
        /// <typeparam name="ParentType">The type of the parent, which is currently using your viewmodel.</typeparam>
        /// <returns></returns>
        public static ParentType GetViewModel<ParentType>(Type viewModelType = null) where ParentType : class
        {
            return ViewModels.FirstOrDefault(vm => ((BaseViewModel<ParentType>)vm).Parent is ParentType
                                                    && vm != null ? vm.GetType() == viewModelType : true) as ParentType;
        }
        #endregion

        public BaseViewModel(T Parent = default)
        {
            this.Parent = Parent;
            ViewModels.Add(this);
        }

        /// <summary>
        /// The Element that owns this ViewModel.
        /// </summary>
        public T Parent { get; private set; }

        private string _guid = null;
        /// <summary>
        /// A Guid for this View Model.
        /// </summary>
        public string Guid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_guid) == true)
                    _guid = System.Guid.NewGuid().ToString();

                return _guid;
            }
        }

        /// <summary>
        /// Gets raised whenever a Property Changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raise the propertyChanged-event for the specific property.
        /// Pass an empty string if you want to raise the event for ALL properties.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raise a Property Changed Event for a specific Property.
        /// </summary>
        /// <param name="propertyName">The name of the Property for this ViewModel.</param>
        public void RaisePropertyChanged(string propertyName) => OnPropertyChanged(propertyName);
    }
}
