/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2021 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Dev2;
using Dev2.Common;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Hosting;
using Dev2.Common.SaveDialog;
using Dev2.Common.Interfaces.Security;
using Dev2.Controller;
using Dev2.Studio.Core;
using Dev2.Studio.Interfaces;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Warewolf.Resource.Errors;
using Dev2.ConnectionHelpers;
using System.Text;
using Dev2.Runtime.Hosting;

namespace Warewolf.Studio.ViewModels
{
    public class RequestServiceNameViewModel : BindableBase, IRequestServiceNameViewModel
    {
        private string _name;
        private string _errorMessage;
        private ResourceName _resourceName;
        private IRequestServiceNameView _view;

        private string _selectedPath;
        private bool _hasLoaded;
        private string _header;
        private IEnvironmentViewModel _environmentViewModel;
        private IExplorerItemViewModel _explorerItemViewModel;
        private bool _isDuplicate;
        private bool _fixReferences;
        private MessageBoxResult ViewResult { get; set; }
        private IServerRepository _serverRepository;

#pragma warning disable 1998
#pragma warning disable 1998
        private async Task<IRequestServiceNameViewModel> InitializeAsync(IEnvironmentViewModel environmentViewModel, string selectedPath, string header, IExplorerItemViewModel explorerItemViewModel = null)
#pragma warning restore 1998
#pragma warning restore 1998
        {
            _environmentViewModel = environmentViewModel;
            _environmentViewModel.Connect();
            _selectedPath = selectedPath;
            _header = header;
            _explorerItemViewModel = explorerItemViewModel;
            OkCommand = new DelegateCommand(SetServiceName, () => string.IsNullOrEmpty(ErrorMessage) && HasLoaded);
            DuplicateCommand = new DelegateCommand(CallDuplicateService, CanDuplicate);
            CancelCommand = new DelegateCommand(CloseView, CanClose);
            Name = header;
            IsDuplicate = explorerItemViewModel != null;

            _serverRepository = CustomContainer.Get<IServerRepository>();
            if (_serverRepository.ActiveServer == null)
            {
                var shellViewModel = CustomContainer.Get<IShellViewModel>();
                _serverRepository.ActiveServer = shellViewModel?.ActiveServer;
            }

            return this;
        }

        private bool CanDuplicate()
        {
            var b = _explorerItemViewModel != null && string.IsNullOrEmpty(ErrorMessage) && HasLoaded && !IsDuplicating;
            return b;
        }

        private bool CanClose()
        {
            if (IsDuplicate)
            {
                return !IsDuplicating;
            }
            return true;
        }

        private readonly IEnvironmentConnection _lazyCon = CustomContainer.Get<IServerRepository>()?.ActiveServer?.Connection ?? ServerRepository.Instance.ActiveServer?.Connection;
        private ICommunicationController _communicationController = new CommunicationController { ServiceName = "DuplicateResourceService" };

        private async void CallDuplicateService()
        {
            if (ExplorerItemViewModelRename() != null)
            {
                return;
            }
            ObservableCollection<IExplorerItemViewModel> childItems = null;
            try
            {
                IsDuplicating = true;
                SetupLazyCommunicationController(); 

                var executeCommand = await _communicationController.ExecuteCommandAsync<ResourceCatalogDuplicateResult>(_lazyCon ?? _serverRepository.ActiveServer?.Connection, GlobalConstants.ServerWorkspaceID);
                var environmentViewModel = SingleEnvironmentExplorerViewModel.Environments.FirstOrDefault();
                if (executeCommand == null)
                {
                    environmentViewModel?.RefreshCommand.Execute(null);
                    CloseView();
                    ViewResult = MessageBoxResult.OK;
                }
                else
                {
                    if (executeCommand.Status == ExecStatus.Success)
                    {
                        var parentItem = SelectedItem ?? _explorerItemViewModel.Parent;
                        childItems = environmentViewModel?.CreateExplorerItemModels(executeCommand.DuplicatedItems, _explorerItemViewModel.Server, parentItem, false, false);
                        var explorerItemViewModels = parentItem.Children;
                        explorerItemViewModels.AddRange(childItems);
                        parentItem.Children = explorerItemViewModels;
                        CloseView();
                        ViewResult = MessageBoxResult.OK;
                    }
                    else
                    {
                        ErrorMessage = executeCommand.Message;
                    }
                }
            }
            catch (Exception)
            {
                //
            }
            finally
            {
                ReloadServerEvents(childItems);
                IsDuplicating = false;
            }
        }

        private void ReloadServerEvents(ObservableCollection<IExplorerItemViewModel> childItems)
        {
            ConnectControlSingleton.Instance.ReloadServer(); 
            if (childItems != null)
            {
                foreach (var childItem in childItems.Where(model => model.ResourceType == "Dev2Server"))
                {
                    FireServerSaved(childItem.ResourceId);
                }
            }
        }

        private void SetupLazyCommunicationController()
        {
            if (_explorerItemViewModel.IsFolder)
            {
                _communicationController = new CommunicationController { ServiceName = "DuplicateFolderService" };
                _communicationController.AddPayloadArgument("FixRefs", FixReferences.ToString());
            }
            _communicationController.AddPayloadArgument("NewResourceName", Name);

            if (!_explorerItemViewModel.IsFolder)
            {
                _communicationController.AddPayloadArgument("ResourceID", _explorerItemViewModel.ResourceId.ToString());
            }

            _communicationController.AddPayloadArgument("sourcePath", _explorerItemViewModel.ResourcePath);
            _communicationController.AddPayloadArgument("destinationPath", Path);
        }

        private void FireServerSaved(Guid savedServerId, bool isDeleted = false)
        {
            if (_environmentViewModel.Server.UpdateRepository.ServerSaved != null)
            {
                var handler = _environmentViewModel.Server.UpdateRepository.ServerSaved;
                handler.Invoke(savedServerId, isDeleted);
            }
        }

        public bool FixReferences
        {
            get => _fixReferences;
            set
            {
                _fixReferences = value;
                OnPropertyChanged(() => FixReferences);
            }
        }

        private void SingleEnvironmentExplorerViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem))
            {
                ValidateName();

                HasLoaded = false;

                if (SingleEnvironmentExplorerViewModel?.SelectedEnvironment != null)
                {
                    HasLoaded = true;
                }
                else
                {
                    if (SingleEnvironmentExplorerViewModel?.SelectedItem != null && SingleEnvironmentExplorerViewModel.SelectedItem.IsFolder)
                    {
                        HasLoaded = true;
                    }
                }
                if (SingleEnvironmentExplorerViewModel?.SelectedItem != null && !SingleEnvironmentExplorerViewModel.SelectedItem.IsFolder)
                {
                    HasLoaded = false;
                    ErrorMessage = ErrorResource.SaveToFolderOrRootOnly;
                }
            }
        }

        private bool HasLoaded
        {
            get => _hasLoaded;
            set
            {
                _hasLoaded = value;
                RaiseCanExecuteChanged();
            }
        }

        public static Task<IRequestServiceNameViewModel> CreateAsync(IEnvironmentViewModel environmentViewModel, string selectedPath, string header) => CreateAsync(environmentViewModel, selectedPath, header, null);
        public static Task<IRequestServiceNameViewModel> CreateAsync(IEnvironmentViewModel environmentViewModel, string selectedPath, string header, IExplorerItemViewModel explorerItemViewModel)
        {
            if (environmentViewModel == null)
            {
                throw new ArgumentNullException(nameof(environmentViewModel));
            }
            var ret = new RequestServiceNameViewModel();
            return ret.InitializeAsync(environmentViewModel, selectedPath, header, explorerItemViewModel);
        }

        private void CloseView()
        {
            _view.RequestClose();
            ViewResult = MessageBoxResult.Cancel;
            SingleEnvironmentExplorerViewModel = null;
        }

        private void SetServiceName()
        {          
            if (ExplorerItemViewModelRename() != null)
            {
                return;
            }
            var path = Path;
            if (!string.IsNullOrEmpty(path))
            {
                path = path.TrimStart('\\') + "\\";
            }
            _resourceName = new ResourceName(path, Name);
            ViewResult = MessageBoxResult.OK;
            
            _view.RequestClose();
        }

        private string Path
        {
            get
            {
                var selectedItem = SelectedItem;
                if (selectedItem != null)
                {
                    var parent = selectedItem.Parent;
                    var parentNames = new List<string>();
                    while (parent != null)
                    {
                        if (parent.ResourceType != "ServerSource")
                        {
                            parentNames.Add(parent.ResourceName);
                        }
                        parent = parent.Parent;
                    }
                    var path = new StringBuilder();
                    if (parentNames.Count > 0)
                    {
                        for (var index = parentNames.Count; index > 0; index--)
                        {
                            var parentName = parentNames[index - 1];
                            path.Append("\\" + parentName);
                        }
                    }
                    if (selectedItem.ResourceType == "Folder")
                    {
                        path.Append("\\" + selectedItem.ResourceName);
                    }
                    return path.ToString();
                }
                return "";
            }
        }

        private bool _isDuplicating;
        private IExplorerTreeItem SelectedItem => SingleEnvironmentExplorerViewModel?.SelectedItem;

        private void RaiseCanExecuteChanged()
        {
            if (OkCommand is DelegateCommand command)
            {
                command.RaiseCanExecuteChanged();
            }
            if (DuplicateCommand is DelegateCommand dupCommand)
            {
                dupCommand.RaiseCanExecuteChanged();
            }
        }

        public MessageBoxResult ShowSaveDialog()
        {
            _view = CustomContainer.GetInstancePerRequestType<IRequestServiceNameView>();

            SingleEnvironmentExplorerViewModel = new SingleEnvironmentExplorerViewModel(_environmentViewModel, Guid.Empty, false);
            SingleEnvironmentExplorerViewModel.PropertyChanged += SingleEnvironmentExplorerViewModelPropertyChanged;
            SingleEnvironmentExplorerViewModel.SearchText = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(_selectedPath))
                {
                    _environmentViewModel.SelectItem(_selectedPath, b =>
                    {
                        _environmentViewModel.SelectAction(b);
                        b.IsSelected = true;
                    });
                }
                _environmentViewModel.IsSaveDialog = true;
                _environmentViewModel.Children?.Flatten(model => model.Children).Apply(model => model.IsSaveDialog = true);
            }
            catch (Exception)
            {
                //
            }

            HasLoaded = true;
            ValidateName();
            _view.DataContext = this;
            _view.ShowView();
            
            UpdateEnvironment();

            return ViewResult;
        }

        private void UpdateEnvironment()
        {
            _environmentViewModel.Filter(string.Empty);
            _environmentViewModel.IsSaveDialog = false;
            _environmentViewModel.Children?.Flatten(model => model.Children).Apply(model => model.IsSaveDialog = false);

            var windowsGroupPermission = _environmentViewModel.Server?.Permissions?[0];
            if (windowsGroupPermission != null)
            {
                _environmentViewModel.SetPropertiesForDialogFromPermissions(windowsGroupPermission);
            }

            var permissions = _environmentViewModel.Server?.GetPermissions(_environmentViewModel.ResourceId);
            if (permissions != null)
            {
                _environmentViewModel.Children?.Flatten(model => model.Children).Apply(model => model.SetPermissions((Permissions)permissions));
            }

            var mainViewModel = CustomContainer.Get<IShellViewModel>();
            if (mainViewModel?.ExplorerViewModel != null)
            {
                mainViewModel.ExplorerViewModel.SearchText = string.Empty;
            }
        }

        public ResourceName ResourceName => _resourceName;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
                ValidateName();
            }
        }

        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged(() => Header);
            }
        }
        public bool IsDuplicate
        {
            get => _isDuplicate;
            set
            {
                _isDuplicate = value;
                OnPropertyChanged(() => IsDuplicate);
            }
        }

        private void ValidateName()
        {
            if (string.IsNullOrEmpty(Name))
            {
                ErrorMessage = ErrorResource.CannotBeNull;
            }
            else if (NameHasInvalidCharacters(Name))
            {
                ErrorMessage = ErrorResource.ContainsInvalidCharecters;
            }
            else if (Name.Trim() != Name)
            {
                ErrorMessage = ErrorResource.ContainsLeadingOrTrailingWhitespace;
            }
            else if (HasDuplicateName(Name))
            {
                ErrorMessage = ErrorResource.ItemWithNameAlreadyExists;
            }
            else
            {
                ErrorMessage = "";
            }
        }

        private bool HasDuplicateName(string requestedServiceName)
        {
            if (SingleEnvironmentExplorerViewModel != null)
            {
                var explorerTreeItem = SingleEnvironmentExplorerViewModel.SelectedItem;
                if (explorerTreeItem != null && explorerTreeItem.ResourceType == "Folder")
                {
                    return explorerTreeItem.Children.Any(model => model.ResourceName.Equals(requestedServiceName) && model.ResourceType != "Folder");
                }
                if (SingleEnvironmentExplorerViewModel.Environments.FirstOrDefault() != null)
                {
                    var explorerItemViewModels = SingleEnvironmentExplorerViewModel.Environments.First().Children;
                    return explorerItemViewModels != null && explorerItemViewModels.Any(model =>
                    {
                        var areSame = requestedServiceName != null && model.ResourceName != null && model.ResourceName.Equals(requestedServiceName);
                        if (!IsDuplicate)
                        {
                            areSame &= model.ResourceType != "Folder";
                        }
                        return areSame;
                    });
                }
            }
            return false;
        }

        private static bool NameHasInvalidCharacters(string name) => Regex.IsMatch(name, @"[^a-zA-Z0-9._\s-]");

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(() => ErrorMessage);
                RaiseCanExecuteChanged();
            }
        }

        public ICommand OkCommand { get; set; }
        public ICommand DuplicateCommand { get; set; }

        public ICommand CancelCommand { get; private set; }

        public IExplorerViewModel SingleEnvironmentExplorerViewModel { get; set; }
        public bool IsDuplicating
        {
            get => _isDuplicating;
            set
            {
                _isDuplicating = value;
                OnPropertyChanged(() => IsDuplicating);
                ViewModelUtils.RaiseCanExecuteChanged(DuplicateCommand);
                ViewModelUtils.RaiseCanExecuteChanged(CancelCommand);
            }
        }

        public ICommand DoneCommand => IsDuplicate ? DuplicateCommand : OkCommand;

        public IExplorerItemViewModel ExplorerItemViewModelRename() => _environmentViewModel?.Children?.Flatten(model => model.Children).FirstOrDefault(model => model.IsRenaming);

        public IExplorerItemViewModel ExplorerItemViewModelIsSelected() => _environmentViewModel?.Children.Flatten(model => model.Children).FirstOrDefault(model => model.IsSelected);

        public void Dispose()
        {
            SingleEnvironmentExplorerViewModel?.Dispose();
            _environmentViewModel?.Dispose();
        }
    }
}
