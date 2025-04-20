using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarTracking.MobileApp.DTOs;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.Pages;
using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.ViewModels;

public class UserLoginViewModel : INotifyPropertyChanged
{
    private readonly UserService _userService;
    bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }
    private bool _isPassword = true;

    public bool IsPassword
    {
        get => _isPassword;
        set
        {
            if (_isPassword != value)
            {
                _isPassword = value;
                OnPropertyChanged();
            }
        }
    }
    public UserLoginViewModel(UserService userService)
    {
        _userService = userService;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task<Result<LoginDto>> SendLoginRequest(string username, string password)
    {
        var loginResult = await _userService.LoginUser(username, password);
        if (loginResult.IsSuccess)
        {
            if (loginResult.Value!.IsAssignedToAccount)
            {
                await Shell.Current.GoToAsync($"/{nameof(ModeSelectPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"/{nameof(AccountAssigningPage)}");
            }
        }

        return loginResult;
    }
}