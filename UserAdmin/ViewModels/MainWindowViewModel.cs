using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Person = UserAdmin.Models.Person;

namespace UserAdmin.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Person> People { get; }

    public ObservableCollection<string> NewAccessLevels { get; } = new(DefaultAccessLevels);

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(DeleteUserCommand))]
    private Person? _selectedPerson;

    [ObservableProperty] private string _newFirstName = string.Empty;
    [ObservableProperty] private string _newLastName = string.Empty;
    [ObservableProperty] private string _newLogin = string.Empty;
    [ObservableProperty] private string _newPassword = string.Empty;
    [ObservableProperty] private string _newEmail = string.Empty;
    [ObservableProperty] private string _newAccessLevel = DefaultAccessLevels[2];
    [ObservableProperty] private string _newUserNote = string.Empty;

    [ObservableProperty] private string _errorMessage = string.Empty;

    private static readonly Faker Faker = new();

    private static readonly List<string> DefaultAccessLevels = ["Admin", "Moderator", "User", "Guest"];

    public MainWindowViewModel()
    {
        People = new ObservableCollection<Person>(GenerateInitialPeople());
    }

    [RelayCommand(CanExecute = nameof(CanDeleteUser))]
    private void DeleteUser()
    {
        if (SelectedPerson == null) return;
        People.Remove(SelectedPerson);
        SelectedPerson = null;
    }

    [RelayCommand]
    private void AddUser()
    {
        if (!ValidateNewUserFields())
        {
            ErrorMessage = "All fields must be filled to create a new user.";
            return;
        }

        Person newUser = new(NewFirstName, NewLastName, NewLogin, NewPassword, NewEmail, NewAccessLevel, NewUserNote);
        People.Add(newUser);
        ResetNewUserProperties();
        ErrorMessage = string.Empty;
    }
    
    private bool ValidateNewUserFields()
    {
        return !string.IsNullOrWhiteSpace(NewFirstName) &&
               !string.IsNullOrWhiteSpace(NewLastName) &&
               !string.IsNullOrWhiteSpace(NewLogin) &&
               !string.IsNullOrWhiteSpace(NewPassword) &&
               !string.IsNullOrWhiteSpace(NewEmail);
    }

    private void ResetNewUserProperties()
    {
        NewFirstName = string.Empty;
        NewLastName = string.Empty;
        NewLogin = string.Empty;
        NewPassword = string.Empty;
        NewEmail = string.Empty;
        NewAccessLevel = DefaultAccessLevels[2];
        NewUserNote = string.Empty;
    }

    private List<Person> GenerateInitialPeople()
    {
        var people = new List<Person>();

        for (int i = 0; i < 3; i++)
        {
            people.Add(GenerateRandomPerson());
        }

        return people;
    }

    private Person GenerateRandomPerson()
    {
        string firstName = Faker.Name.FirstName();
        string lastName = Faker.Name.LastName();
        string login = $"{firstName.ToLower()}{Faker.Random.Number(1000, 9999)}";
        string password = Faker.Internet.Password(8, true);
        string email = Faker.Internet.Email();
        string accessLevel = DefaultAccessLevels[Faker.Random.Int(0, DefaultAccessLevels.Count - 1)];
        string userNote = Faker.Lorem.Sentence();

        return new Person(firstName, lastName, login, password, email, accessLevel, userNote);
    }

    private bool CanDeleteUser() => SelectedPerson != null;
}