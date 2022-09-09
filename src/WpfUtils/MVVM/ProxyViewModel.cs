using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtils.MVVM
{
    /// <summary>
    /// A custom ViewModel implementation which serves as another layer of abstraction between the ViewModel and the data.
    /// 
    /// Instead of declaring your properties inside the ViewModel, you declare them inside the <typeparamref name="InnerModelType"/> class.
    /// 
    /// You then create "dummy properties" to update the values inside the <typeparamref name="InnerModelType"/> object.
    /// </summary>
    /// <typeparam name="InnerModelType">The type of the inner model.</typeparam>
    /// <typeparam name="ParentType">The type of your parent control. This can be a window, user-control, etc.</typeparam>
    public class ProxyViewModel<InnerModelType, ParentType> : BaseViewModel<ParentType> where InnerModelType : class
    {
        public ProxyViewModel(ParentType Parent = default) : base(Parent)
        {
            InnerModel = Activator.CreateInstance<InnerModelType>();
            OnPropertyChanged("");
        }

        /// <summary>
        /// The Inner ViewModel.
        /// 
        /// The object based on this class, should implement "proxy properties" and communicate with it.
        /// </summary>
        public InnerModelType InnerModel { get; private set; }

        /// <summary>
        /// Override the inner model.
        /// </summary>
        /// <param name="overrider">The model which should override the current inner Model.</param>
        public void Override(InnerModelType overrider)
        {
            if (overrider == null)
                return;

            InnerModel = overrider;
            RaisePropertyChanged("");
        }

        /// <summary>
        /// Clone the current object.
        /// </summary>
        /// <returns></returns>
        public ProxyViewModel<InnerModelType, ParentType> Clone()
        {
            return MemberwiseClone() as ProxyViewModel<InnerModelType, ParentType>;
        }
    }
}
