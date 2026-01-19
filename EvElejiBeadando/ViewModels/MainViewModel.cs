using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using ReactiveUI;
using EvElejiBeadando.Models;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Linq;

namespace EvElejiBeadando.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<Package> Packages { get; } = new ObservableCollection<Package>();

    private Package? _selectedPackage;
    public Package? SelectedPackage { get; set; }


    private string _newName = "";
    public string NewName
    {
        get => _newName;
        set => this.RaiseAndSetIfChanged(ref _newName, value);
    }
    private string _newFromCity = "";
    public string NewFromCity
    {
        get => _newFromCity;
        set => this.RaiseAndSetIfChanged(ref _newFromCity, value);
    }

    private string _newToCity = "";
    public string NewToCity
    {
        get => _newToCity;
        set => this.RaiseAndSetIfChanged(ref _newToCity, value);
    }

    private decimal _newPrice;
    public decimal NewPrice
    {
        get => _newPrice;
        set => this.RaiseAndSetIfChanged(ref _newPrice, value);
    }

    private int _newDays;
    public int NewDays
    {
        get => _newDays;
        set => this.RaiseAndSetIfChanged(ref _newDays, value);
    }

    private PackageStatus _newStatus;
    public PackageStatus NewStatus
    {
        get => _newStatus;
        set => this.RaiseAndSetIfChanged(ref _newStatus, value);
    }


    public IEnumerable<PackageStatus> PackageStatuses { get; } =
        Enum.GetValues<PackageStatus>();

    public RelayCommand AddPackageCommand { get; }
    public RelayCommand SaveCommand { get; }
    public RelayCommand LoadCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }

    public MainViewModel()
    {
        AddPackageCommand = new RelayCommand(
            AddPackage
        );

        SaveCommand = new RelayCommand(
            SavePackages
        );

        LoadCommand = new RelayCommand(
            LoadPackages    
        );

        DeleteCommand = new RelayCommand<Package>(DeletePackage);


        EditCommand = new RelayCommand<Package>(EditPackage);

    }

    private void AddPackage()
    {
        if (NewPrice <= 0)
            return;

        if (SelectedPackage == null)
        {
            // ADD
            Packages.Add(new Package
            {
                Id = Packages.Any() ? Packages.Max(p => p.Id) + 1 : 1,
                Name = NewName,
                FromCity = NewFromCity,
                ToCity = NewToCity,
                Price = NewPrice,
                DaysRemaining = NewStatus is PackageStatus.Kiszallitva or PackageStatus.Torolve
                    ? 0
                    : NewDays,
                Status = NewStatus,
                PostedAt = DateTime.Now
            });
        }
        else
        {
            // EDIT
            SelectedPackage.Name = NewName;
            SelectedPackage.FromCity = NewFromCity;
            SelectedPackage.ToCity = NewToCity;
            SelectedPackage.Price = NewPrice;
            SelectedPackage.DaysRemaining = NewDays;
            SelectedPackage.Status = NewStatus;

            SelectedPackage = null;
        }
    }

    private void SavePackages()
    {
        using StreamWriter writer = new("packages.txt");
        foreach (var package in Packages)
        {
            writer.WriteLine($"{package.Id},{package.Name},{package.PostedAt},{package.FromCity},{package.ToCity},{package.Status},{package.Price},{package.DaysRemaining}");
        }
    }
    private void LoadPackages()
    {
        using StreamReader reader = new("packages.txt");
        Packages.Clear();
        for(string? line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
            var parts = line.Split(',');
            Packages.Add(new Package
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                PostedAt = DateTime.Parse(parts[2]),
                FromCity = parts[3],
                ToCity = parts[4],
                Status = Enum.Parse<PackageStatus>(parts[5]),
                Price = decimal.Parse(parts[6]),
                DaysRemaining = int.Parse(parts[7])
            });
        }
    }
    private void DeletePackage(Package package)
    {
        if (package == null)
            return;

        Packages.Remove(package);
    }
    private void EditPackage(Package package)
    {
        if (package == null)
            return;

        SelectedPackage = package;

        NewName = package.Name;
        NewFromCity = package.FromCity;
        NewToCity = package.ToCity;
        NewPrice = package.Price;
        NewDays = package.DaysRemaining;
        NewStatus = package.Status;
    }
}
