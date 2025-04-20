using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarTracking.MobileApp.Models;
using CarTracking.MobileApp.Services;

namespace CarTracking.MobileApp.ViewModels;

public class PasswordResetViewModel : INotifyPropertyChanged
{
    private readonly UserService _userService;

    public PasswordResetViewModel(UserService userService)
    {
        _userService = userService;
    }

    private string _email;
    private string _code;
    private string _newPassword;
    private string _newPasswordRepeat;
    private bool _isCodeSent;
    private bool _noisCodeSent = true;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            OnPropertyChanged();
        }
    }

    public string NewPassword
    {
        get => _newPassword;
        set
        {
            _newPassword = value;
            OnPropertyChanged();
        }
    }

    public string NewPasswordRepeat
    {
        get => _newPasswordRepeat;
        set
        {
            _newPasswordRepeat = value;
            OnPropertyChanged();
        }
    }

    public bool IsCodeSent
    {
        get => _isCodeSent;
        set
        {
            _isCodeSent = value;
            OnPropertyChanged();
        }
    }

    public bool NoIsCodeSent
    {
        get => _noisCodeSent;
        set
        {
            _noisCodeSent = value;
            OnPropertyChanged();
        }
    }

    public ICommand SendCodeCommand => new Command(async () =>
    {
        var result = await _userService.ResetPassword(Email);
        if (result.IsSuccess)
        {
            IsCodeSent = true;
            NoIsCodeSent = false;
        }
    });

    public async Task<Result<string>> ResetPassword()
    {
        return await _userService.SetNewPassword(Email, NewPassword, Code);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}