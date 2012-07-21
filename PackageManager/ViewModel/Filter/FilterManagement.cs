using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoApp.PackageManager.Model;
using CoApp.Packaging.Common;
using CoApp.Toolkit.Collections;
using CoApp.Toolkit.Extensions;

namespace CoApp.PackageManager.ViewModel.Filter
{

    public class FilterManagement
    {
        internal FilterManagement()
        {
            ActualAppliedFilters = new XList<FrictionlessFilter>();
         
            AllFilters = new XList<GUIFilterBase>
                             {
                                 new FilterOnText(this)
                                     {
                                         Category = CAT.DisplayName,
                                         NiceName = "Name",
                                         NumberOfFilter =
                                             NumOfFilter.Multiple,
                                        IsPartOfSuperFilter = true

                                     },
                                   new FilterOnText(this)
                                     {
                                         Category = CAT.Tag,
                                         NiceName = "Tag",
                                         NumberOfFilter =
                                             NumOfFilter.Multiple,
                                         IsPartOfSuperFilter = true

                                     },
                                 new FilterOnDateRange(this)
                                     {
                                       Category   = CAT.PublishDate,
                                       NiceName = "Date Published",
                                       NumberOfFilter = NumOfFilter.Single,
                                     },
                                 new FilterOnText(this)
                                     {
                                         Category = CAT.Description,
                                         NiceName = "Description",
                                         NumberOfFilter =
                                             NumOfFilter.Multiple,
                                         IsPartOfSuperFilter = true

                                     },

                                 new FilterOnText(this)
                                     {
                                         Category = CAT.Summary,
                                         NiceName = "Summary",
                                         NumberOfFilter =
                                             NumOfFilter.Multiple,
                                         IsPartOfSuperFilter = true

                                     },
                                 new FilterOnText(this)
                                     {
                                         Category = CAT.Flavor,
                                         NiceName = "Flavor",
                                         NumberOfFilter =
                                             NumOfFilter.Multiple,
                                         IsPartOfSuperFilter = true

                                     },
                               
                                 new FilterOnBoolean(this)
                                     {
                                         Category = CAT.IsDependency,
                                         NiceName = "Is a dependency",
                                         NumberOfFilter =
                                             NumOfFilter.Single
                                     },
                                 new FilterOnBoolean(this)
                                     {
                                         Category = CAT.IsBlocked,
                                         NiceName = "Is blocked",
                                         NumberOfFilter =
                                             NumOfFilter.Single
                                     },
                                 new FilterOnBoolean(this)
                                     {
                                         Category = CAT.IsInstalled,
                                         NumberOfFilter =
                                             NumOfFilter.Single,
                                         NiceName = "Is installed"
                                     },
                                 new FilterOnBoolean(this)
                                     {
                                         Category = CAT.IsWanted,
                                         NumberOfFilter =
                                             NumOfFilter.Single,
                                         NiceName = "Is wanted"
                                     },
                                 
                                new FilterOnBoolean(this)
                                    {
                                         Category = CAT.IsActive,
                                         NumberOfFilter =
                                             NumOfFilter.Single,
                                         NiceName = "Is Active"
                                    },
                              


                             };


            
                AllFilters.Add(new FilterOnFeedUrls(this)
                                                    {
                                                        Category =
                                                            CAT.FeedUrl,
                                                        NumberOfFilter =
                                                            NumOfFilter.
                                                            Multiple,
                                                        NiceName =
                                                            "Feed URL",
                                                        AllChoices =
                                                            new ObservableCollection
                                                            <KeyValuePair<string, string>>(
                                                            new LocalServiceLocator().CoAppService.SystemFeeds.Result.Where(
                                                                feed
                                                                =>
                                                                feed.
                                                                    FeedState ==
                                                                FeedState
                                                                    .
                                                                    Active)
                                                                .Select(
                                                                    feed
                                                                    =>
                                                                    feed.
                                                                        Location).Select(v => new KeyValuePair<string,string>(v, v))),
                                                        
                                                    }
                                    );
            var archFilter = new FilterOnArchitecture(this)
                {
                    Category =
                        CAT.Architecture,
                    NumberOfFilter =
                        NumOfFilter.
                            Multiple,
                    NiceName =
                        "Architecture",
                    IsPartOfSuperFilter = true
                };
            AllFilters.Insert(3, archFilter);
             AllFiltersChanged();
                                                                            
        }

        private IList<FrictionlessFilter> ActualAppliedFilters { get; set; }

        public ReadOnlyCollection<FrictionlessFilter> AppliedFilters
        {
            get { return new ReadOnlyCollection<FrictionlessFilter>(ActualAppliedFilters); }
        }

        
        public IList<GUIFilterBase> AllFilters { get; private set; }
        

        public ReadOnlyCollection<GUIFilterBase> AvailableFilters
        {
            get { return new ReadOnlyCollection<GUIFilterBase>(AllFilters.Where(f => f.CanBeCreated()).ToArray()); }
        }

        public event Action AllFiltersChanged = delegate { };
        public event Action<GUIFilterBase, FrictionlessFilter> FilterApplied = delegate { };

        public void ApplyFilter(GUIFilterBase f)
        {
            FrictionlessFilter appliedFilter = f.Create();
            ActualAppliedFilters.Add(appliedFilter);
          
            FilterApplied(f, appliedFilter);
        }

        public void RemoveFilter(FrictionlessFilter f)
        {
            ActualAppliedFilters.Remove(f);
            FilterApplied(null, f);
        }
    }
}