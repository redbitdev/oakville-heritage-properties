using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Gms.Maps;
using System.Threading.Tasks;
using Android.Gms.Maps.Model;

namespace HeritageProperties.Droid
{
    [Activity(Label = "HeritageProperties.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            System.Diagnostics.Debug.Write("------------> OnCreate()");

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Load the data
            LoadData();

            base.OnCreate(bundle);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // create a menu option to switch between map and lsit
            menu.Add(0, 0, 0, "Map View")
                .SetIcon(Resource.Drawable.map)
                .SetShowAsAction(ShowAsAction.IfRoom);
            return base.OnCreateOptionsMenu(menu);
        }
        
        /// <summary>
        /// Contains a list of heritage properties
        /// </summary>
        public List<HeritageProperty> Properties { get; set; }

        /// <summary>
        /// Load the data from the service
        /// </summary>
        private async void LoadData()
        {
            // Load the properties async
            this.Properties = await HeritagePropertyService.Default.Load(this.Resources.OpenRawResource(Resource.Raw.data));

            // find the lsit view
            var listView = this.FindViewById<ListView>(Resource.Id.lstProperties);
            if (listView != null)
            {
                // set the adapter
                var adapter = new HeritagePropertyAdapter(this, this.Properties);
                listView.Adapter = adapter;

                // wire up the click event
                listView.ItemClick += (s, e) =>
                {
                    // set the selected item
                    HeritagePropertyDetail.SelectedItem = this.Properties[e.Position];

                    // set the intent to navigate to it
                    var intent = new Intent(this, typeof(HeritagePropertyDetail));
                    this.StartActivity(intent);
                };
            }
        }


        private MapFragment map;
        /// <summary>
        /// Helper method to load the map
        /// </summary>
        private void LoadMap()
        {
            if (map == null)
            {
                // add the map
                map = MapFragment.NewInstance();

                // make sure we have the map property set
                Action callback = new Action(() => { });
                callback = async () =>
                {
                    // just delay so we are not overloading the CPU
                    await Task.Delay(1000);

                    // make sure we run on the UI thread
                    RunOnUiThread(() =>
                    {
                        // if map is still not populated try again
                        if (map.Map == null)
                        {
                            Task.Run(callback);
                        }
                        else
                        {
                            // wire up the map click
                            map.Map.InfoWindowClick += (s, e) =>
                            {
                                var index = -1;
                                Int32.TryParse(e.Marker.Id.Replace("m", ""), out index);
                                if (index != -1)
                                {
                                    // set the selected item
                                    HeritagePropertyDetail.SelectedItem = this.Properties[index];

                                    // create the intent
                                    var intent = new Intent(this, typeof(HeritagePropertyDetail));

                                    // Start the intent
                                    this.StartActivity(intent);
                                }
                            };

                            // loop all the properties and add them as markers
                            foreach (var item in this.Properties)
                            {
                                // create the marker
                                var m = new MarkerOptions();
                                m.SetPosition(new LatLng(item.Latitude, item.Longitude));
                                m.SetTitle(item.Name);

                                // add to map
                                map.Map.AddMarker(m);
                            }

                            // zoom and center map
                            ZoomAndCenterMap();
                        }
                    });
                };
                Task.Run(callback);

                // add the fragment
                FragmentTransaction tx = FragmentManager.BeginTransaction();
                tx.Add(Resource.Id.rootLayout, map);
                tx.Commit();
            }
            else
            {
                // unhide the fragment
                FragmentTransaction tx = FragmentManager.BeginTransaction();
                tx.Show(map);
                tx.Commit();

                // zoom and center map
                ZoomAndCenterMap();
            }
        }

        private void ZoomAndCenterMap()
        {
            // update the camera but give some time for the map to show
            Task.Run(async () =>
            {
                await Task.Delay(100);
                RunOnUiThread(() =>
                {
                    // create a bounds builder
                    LatLngBounds.Builder builder = new LatLngBounds.Builder();

                    // loop all the properties and add them as markers
                    foreach (var item in this.Properties)
                        builder.Include(new LatLng(item.Latitude, item.Longitude));

                    // zoom the map in
                    CameraUpdate cu = CameraUpdateFactory.NewLatLngBounds(builder.Build(), 100);
                    map.Map.MoveCamera(cu);
                    map.Map.AnimateCamera(cu);
                });
            });
        }


        /// <summary>
        /// Helper method to hide the map
        /// </summary>
        private void HideMap()
        {
            if (map != null)
            {
                FragmentTransaction tx = FragmentManager.BeginTransaction();
                tx.Hide(map);
                tx.Commit();
            }
        }

        private ListView listView;
        /// <summary>
        /// Set the visibility for the list
        /// </summary>
        /// <param name="show"></param>
        private void SetListVisibility(bool show)
        {
            // make sure we have a reference to the list view stored
            if (listView == null)
                listView = this.FindViewById<ListView>(Resource.Id.lstProperties);

            // check for null
            if (listView != null)
            {
                // get the root layout
                var layout = this.FindViewById<LinearLayout>(Resource.Id.rootLayout);

                // remove or add the listview as required
                if (show)
                    layout.AddView(listView);
                else
                    layout.RemoveView(listView);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var title = item.TitleFormatted.ToString();
            switch (title)
            {
                case "Map View":
                    // set the title and icon
                    item
                        .SetTitle("List View")
                        .SetIcon(Resource.Drawable.list);

                    // load the map
                    LoadMap();

                    // set the list visibility
                    SetListVisibility(false);
                    break;
                case "List View":
                    // set the title and icon
                    item
                        .SetTitle("Map View")
                        .SetIcon(Resource.Drawable.map);

                    // set the list visibility
                    SetListVisibility(true);

                    // hide the map
                    HideMap();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }



        protected override void OnStart()
        {
            System.Diagnostics.Debug.Write("------------> OnStart()");
            base.OnStart();
        }

        protected override void OnRestart()
        {
            System.Diagnostics.Debug.Write("------------> OnRestart()- Activity restarted");
            base.OnRestart();
        }

        protected override void OnResume()
        {
            System.Diagnostics.Debug.Write("------------> OnResume() - Activity is running");
            base.OnResume();
        }

        protected override void OnPause()
        {
            System.Diagnostics.Debug.Write("------------> OnPause()");
            base.OnPause();
        }

        protected override void OnStop()
        {
            System.Diagnostics.Debug.Write("------------> OnStop() - Activity is Backgrounded");
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            System.Diagnostics.Debug.WriteLine("------------> OnDestroy()");
            base.OnDestroy();
        }
    }
}

