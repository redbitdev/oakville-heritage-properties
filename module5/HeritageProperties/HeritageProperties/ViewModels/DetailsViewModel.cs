using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeritageProperties.PCL
{
    public class DetailsViewModel : ViewModelBase
    {
        public DetailsViewModel()
        {

        }

        /// <summary>
        /// The <see cref="SelectedItem" /> property's name.
        /// </summary>
        public const string SelectedItemPropertyName = "SelectedItem";
        private HeritageProperty _SelectedItem;

        /// <summary>
        /// Sets and gets the SelectedItem property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public HeritageProperty SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                Set(() => SelectedItem, ref _SelectedItem, value);
            }
        }
    }
}
