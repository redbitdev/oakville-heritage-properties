using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HeritageProperties.Pages;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HeritageProperties.PCL
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IGpsService gps, HeritagePropertyService service, INavigation navigation)
        {
            this.Navigation = navigation;
            this.GpsService = gps;
            this.HeritagePropertyService = service;
        }

        /// <summary>
        /// Contains the navigation object from Xamarin.Forms
        /// </summary>
        public INavigation Navigation { get; set; }
        

        private IGpsService GpsService { get; set; }

        public HeritagePropertyService HeritagePropertyService { get; set; }

        /// <summary>
        /// The <see cref="HeritageProperties property's name.
        /// </summary>
        public const string HeritagePropertiesPropertyName = "HeritageProperties";
        private ObservableCollection<HeritageProperty> _HeritageProperties;

        /// <summary>
        /// Sets and gets the HeritageProperties property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<HeritageProperty> HeritageProperties
        {
            get
            {
                return _HeritageProperties;
            }
            set
            {
                Set(() => HeritageProperties, ref _HeritageProperties, value);
            }
        }

        private RelayCommand _LoadHeritagePropertiesCommand;
        /// <summary>
        /// Gets the LoadHeritagePropertiesCommand.
        /// </summary>
        public RelayCommand LoadHeritagePropertiesCommand
        {
            get
            {
                return _LoadHeritagePropertiesCommand
                    ?? (_LoadHeritagePropertiesCommand = new RelayCommand(
                                          async () =>
                                          {
                                              var items = await HeritagePropertyService.Load();
                                              this.HeritageProperties = new ObservableCollection<HeritageProperty>(items);
                                          }));
            }
        }

        private RelayCommand _CurrentLocationCommand;

        /// <summary>
        /// Gets the CurrentLocationCommand.
        /// </summary>
        public RelayCommand CurrentLocationCommand
        {
            get
            {
                return _CurrentLocationCommand
                    ?? (_CurrentLocationCommand = new RelayCommand(
                                            async () =>
                                            {
                                                this.CurrentLocation = await GpsService.GetLocation();
                                            }));
            }
        }

        /// <summary>
        /// The <see cref="CurrentLocation" /> property's name.
        /// </summary>
        public const string CurrentLocationPropertyName = "CurrentLocation";
        private Location _CurrentLocation;

        /// <summary>
        /// Sets and gets the CurrentLocation property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Location CurrentLocation
        {
            get
            {
                return _CurrentLocation;
            }
            set
            {
                Set(() => CurrentLocation, ref _CurrentLocation, value);
            }
        }
        
        private RelayCommand<int> _HeritagePropertySelectedCommand;

        /// <summary>
        /// Gets the HeritagePropertySelectedCommand.
        /// </summary>
        public RelayCommand<int> HeritagePropertySelectedCommand
        {
            get
            {
                return _HeritagePropertySelectedCommand
                    ?? (_HeritagePropertySelectedCommand = new RelayCommand<int>(
                                          p =>
                                          {
                                              ServiceLocator.Current.GetInstance<DetailsViewModel>().SelectedItem = this.HeritageProperties[p];
                                          }));
            }
        }

        private RelayCommand<HeritageProperty> _HeritagePropertyItemSelectedCommand;

        /// <summary>
        /// Gets the HeritagePropertyItemSelectedCommand.
        /// </summary>
        public RelayCommand<HeritageProperty> HeritagePropertyItemSelectedCommand
        {
            get
            {
                return _HeritagePropertyItemSelectedCommand
                    ?? (_HeritagePropertyItemSelectedCommand = new RelayCommand<HeritageProperty>(
                                          p =>
                                          {
                                              ServiceLocator.Current.GetInstance<DetailsViewModel>().SelectedItem = p;
                                              Navigation.PushAsync(new DetailsPage());
                                          }));
            }
        }
    }
}
