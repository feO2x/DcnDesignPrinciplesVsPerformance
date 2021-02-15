using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using Light.GuardClauses;
using Light.ViewModels;

namespace SyncVsAsync.WpfClient
{
    public sealed class MainWindowViewModel : BaseNotifyPropertyChanged, INotifyDataErrorInfo, IRaiseErrorsChanged
    {
        private readonly DelegateCommand _callApiCommand;
        private readonly WebApiPerformanceManager _performanceManager;
        private readonly ValidationManager<string> _validationManager; 
        private bool _isBusy;
        private bool _isCallingAsynchronousApi;
        private int _numberOfCalls;
        private string _numberOfCallsText;
        private int _waitIntervalInMilliseconds;
        private string _resultText;
        private string _waitIntervalText;

        public MainWindowViewModel(WebApiPerformanceManager performanceManager)
        {
            _performanceManager = performanceManager.MustNotBeNull(nameof(performanceManager));
            _validationManager = new ValidationManager<string>(this);
            _callApiCommand = new DelegateCommand(CallApi, () => CanCallApi);
            _numberOfCalls = 500;
            _numberOfCallsText = _numberOfCalls.ToString();
            _waitIntervalInMilliseconds = 2000;
            _waitIntervalText = _waitIntervalInMilliseconds.ToString();
            _isCallingAsynchronousApi = true;
        }

        public string NumberOfCallsText
        {
            get => _numberOfCallsText;
            set
            {
                if (SetIfDifferent(ref _numberOfCallsText, value) == false)
                    return;
                _validationManager.Validate(value, ParseNumberOfCalls);
                _callApiCommand.RaiseCanExecuteChanged();
            }
        }

        public string WaitIntervalText
        {
            get => _waitIntervalText;
            set
            {
                if (SetIfDifferent(ref _waitIntervalText, value) == false)
                    return;

                _validationManager.Validate(value, ParseWaitInterval);
            }
        }

        public bool IsCallingAsynchronousApi
        {
            get => _isCallingAsynchronousApi;
            set => SetIfDifferent(ref _isCallingAsynchronousApi, value);
        }

        public ICommand CallApiCommand => _callApiCommand;

        public bool CanCallApi => HasErrors == false && IsBusy == false;

        public bool IsBusy
        {
            get => _isBusy;
            private set
            {
                Set(out _isBusy, value);
                _callApiCommand.RaiseCanExecuteChanged();
            }
        }

        public string ResultText
        {
            get => _resultText;
            private set => Set(out _resultText, value);
        }

        public IEnumerable GetErrors(string propertyName) => _validationManager.GetErrors(propertyName);

        public bool HasErrors => _validationManager.HasErrors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnErrorsChanged(string propertyName) => ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        private ValidationResult<string> ParseNumberOfCalls(string value)
        {
            if (value.IsNullOrWhiteSpace())
                return "Number of Calls must not be empty.";

            if (int.TryParse(value, out var numberOfCalls) == false)
                return "Number of Calls must be a numeric positive value.";

            if (numberOfCalls < 2)
                return "Number of Calls must be at least 2.";

            _numberOfCalls = numberOfCalls;
            return ValidationResult<string>.Valid;
        }

        private ValidationResult<string> ParseWaitInterval(string value)
        {
            if (value.IsNullOrWhiteSpace())
                return "Wait Interval must not be empty";

            if (int.TryParse(value, out var waitInterval) == false)
                return "Wait Interval must be a numeric positive value.";

            if (waitInterval < 10)
                return "Wait Interval must be at least 10ms.";

            _waitIntervalInMilliseconds = waitInterval;
            return ValidationResult<string>.Valid;
        }

        private async void CallApi()
        {
            ResultText = null;
            IsBusy = true;
            var results = await _performanceManager.MeasureApiCallsAsync(_isCallingAsynchronousApi, _numberOfCalls, _waitIntervalInMilliseconds);
            IsBusy = false;

            ResultText = results.ToString();
        }
    }
}