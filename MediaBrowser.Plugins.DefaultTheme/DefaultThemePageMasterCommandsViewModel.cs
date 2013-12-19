﻿using System;
using System.Windows.Input;
using MediaBrowser.Common;
using MediaBrowser.Model.ApiClient;
using MediaBrowser.Model.Logging;
using MediaBrowser.Plugins.DefaultTheme.UserProfileMenu;
using MediaBrowser.Theater.Interfaces.Presentation;
using MediaBrowser.Theater.Interfaces.ViewModels;
using MediaBrowser.Theater.Interfaces.Navigation;
using MediaBrowser.Theater.Interfaces.Session;

namespace MediaBrowser.Plugins.DefaultTheme
{
    public class DefaultThemePageMasterCommandsViewModel : MasterCommandsViewModel
    {
        protected readonly IImageManager ImageManager;

        public ICommand UserCommand { get; private set; }
        public ICommand DisplayPreferencesCommand { get; private set; }
        public ICommand SortOptionsCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        private bool _displayPreferencesEnabled;
        public bool DisplayPreferencesEnabled
        {
            get { return _displayPreferencesEnabled; }

            set
            {
                var changed = _displayPreferencesEnabled != value;

                _displayPreferencesEnabled = value;
                if (changed)
                {
                    OnPropertyChanged("DisplayPreferencesEnabled");
                }
            }
        }

        private bool _sortEnabled;
        public bool SortEnabled
        {
            get { return _sortEnabled; }

            set
            {
                var changed = _sortEnabled != value;

                _sortEnabled = value;
                if (changed)
                {
                    OnPropertyChanged("SortEnabled");
                }
            }
        }

        private bool _powerOptionsEnabled;
        public bool PowerOptionsEnabled
        {
            get { return _powerOptionsEnabled; }

            set
            {
                var changed = _powerOptionsEnabled != value;

                _powerOptionsEnabled = value;
                if (changed)
                {
                    OnPropertyChanged("PowerOptionsEnabled");
                }
            }
        }

        public DefaultThemePageMasterCommandsViewModel(INavigationService navigationService, ISessionManager sessionManager, IPresentationManager presentationManager, IApiClient apiClient, ILogger logger, IApplicationHost appHost, IServerEvents serverEvents, IImageManager imageManager) 
            : base(navigationService, sessionManager, presentationManager, apiClient, logger, appHost, serverEvents)
        {
            ImageManager = imageManager;

            UserCommand = new RelayCommand(i => ShowUserMenu());
            DisplayPreferencesCommand = new RelayCommand(i => ShowDisplayPreferences());
            SortOptionsCommand = new RelayCommand(i => ShowSortMenu());
            LogoutCommand = new RelayCommand(i => Logout());

            PowerOptionsEnabled = true;
        }

        protected virtual void ShowUserMenu()
        {
            new UserProfileWindow(this, SessionManager, ImageManager, ApiClient).ShowModal(PresentationManager.Window);
        }

        protected virtual void ShowDisplayPreferences()
        {
            var page = NavigationService.CurrentPage as IHasDisplayPreferences;

            if (page != null)
            {
                page.ShowDisplayPreferencesMenu();
            }
        }

        protected virtual void ShowSortMenu()
        {
            var page = NavigationService.CurrentPage as IHasDisplayPreferences;

            if (page != null)
            {
                page.ShowSortMenu();
            }
        }

        protected async void Logout()
        {
            if (SessionManager.CurrentUser == null)
            {
                throw new InvalidOperationException("The user is not logged in.");
            }

            await SessionManager.Logout();
        }

        protected override void Dispose(bool dispose)
        {
            if (dispose)
            {
                
            }

            base.Dispose(dispose);
        }
    }
}
