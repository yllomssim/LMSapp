using System.ComponentModel;

namespace LMSGUIApp.ViewModels
{
    public class LivestockQueryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Cow> CowList { get; private set; }
        public List<Sheep> SheepList { get; private set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LivestockQueryViewModel()
        {
            CowList = Database.GetInstance().ReadItems().OfType<Cow>().ToList();
            SheepList = Database.GetInstance().ReadItems().OfType<Sheep>().ToList();
            UpdateStats();
        }

        private string _selectedType;
        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                    UpdateStats();
                }
            }
        }

        private string _selectedColor;
        public string SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged(nameof(SelectedColor));
                    OnPropertyChanged(nameof(Percentage));
                    UpdateStats();
                }
            }
        }

        private int _selectedLivestockCount;
        public int SelectedLivestockCount
        {
            get { return _selectedLivestockCount; }
            private set
            {
                if (_selectedLivestockCount != value)
                {
                    _selectedLivestockCount = value;
                    OnPropertyChanged(nameof(SelectedLivestockCount));
                }
            }
        }

        private int _totalLivestockCount;
        public int TotalLivestockCount
        {
            get { return _totalLivestockCount; }
            private set
            {
                if (_totalLivestockCount != value)
                {
                    _totalLivestockCount = value;
                    OnPropertyChanged(nameof(TotalLivestockCount));
                    OnPropertyChanged(nameof(Percentage));
                }
            }
        }

        private string _totalGovernmentTax;
        public string TotalGovernmentTax
        {
            get => _totalGovernmentTax;
            private set
            {
                if (_totalGovernmentTax != value)
                {
                    _totalGovernmentTax = value;
                    OnPropertyChanged(nameof(TotalGovernmentTax));
                }
            }
        }

        private string _totalDailyProfit;
        public string TotalDailyProfit
        {
            get => _totalDailyProfit;
            private set
            {
                if (_totalDailyProfit != value)
                {
                    _totalDailyProfit = value;
                    OnPropertyChanged(nameof(TotalDailyProfit));
                    UpdateNetDailyProfit();
                }
            }
        }

        private string _averageWeight;
        public string AverageWeight
        {
            get => _averageWeight;
            private set
            {
                if (_averageWeight != value)
                {
                    _averageWeight = value;
                    OnPropertyChanged(nameof(AverageWeight));
                }
            }
        }

        private string _totalProduce;
        public string TotalProduce
        {
            get => _totalProduce;
            private set
            {
                if (_totalProduce != value)
                {
                    _totalProduce = value;
                    OnPropertyChanged(nameof(TotalProduce));
                }
            }
        }

        private string _totalDailyCost;
        public string TotalDailyCost
        {
            get => _totalDailyCost;
            private set
            {
                if (_totalDailyCost != value)
                {
                    _totalDailyCost = value;
                    OnPropertyChanged(nameof(TotalDailyCost));
                    UpdateNetDailyProfit();
                }
            }
        }

        private string _netDailyProfit;
        public string NetDailyProfit
        {
            get => _netDailyProfit;
            private set
            {
                if (_netDailyProfit != value)
                {
                    _netDailyProfit = value;
                    OnPropertyChanged(nameof(NetDailyProfit));
                }
            }
        }

        public double Percentage
        {
            get
            {
                // Calculate the total count of all livestock in the database
                int totalLivestockInDatabase = CowList.Count + SheepList.Count;

                // Calculate the total count of the selected livestock type
                int totalSelectedLivestock = 0;
                if (SelectedColor == "All")
                {
                    if (SelectedType == "Cow")
                    {
                        totalSelectedLivestock = CowList.Count;
                    }
                    else if (SelectedType == "Sheep")
                    {
                        totalSelectedLivestock = SheepList.Count;
                    }
                }
                else
                {
                    var selectedLivestock = GetSelectedLivestock();
                    totalSelectedLivestock = selectedLivestock.Count();
                }

                if (totalLivestockInDatabase != 0)
                {
                    return ((double)totalSelectedLivestock / totalLivestockInDatabase) * 100;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void UpdateStats()
        {
            if (!string.IsNullOrEmpty(SelectedType))
            {
                var selectedLivestock = GetSelectedLivestock();
                SelectedLivestockCount = selectedLivestock.Count();

                // Calculate the total count based on the selected livestock type and color
                if (SelectedColor == "All")
                {
                    // Calculate the total count of all livestock of the selected type, regardless of color
                    if (SelectedType == "Cow")
                    {
                        TotalLivestockCount = CowList.Count;
                    }
                    else if (SelectedType == "Sheep")
                    {
                        TotalLivestockCount = SheepList.Count;
                    }
                    else
                    {
                        // Handle other types of livestock or return an error
                        TotalLivestockCount = 0;
                    }
                }
                else
                {
                    // Calculate the total count of all colors combined for the selected livestock type
                    int total = 0;
                    if (SelectedType == "Cow")
                    {
                        total = CowList.Count(c => c.Colour == SelectedColor);
                    }
                    else if (SelectedType == "Sheep")
                    {
                        total = SheepList.Count(s => s.Colour == SelectedColor);
                    }
                    TotalLivestockCount = total;
                }

                TotalDailyCost = selectedLivestock.Sum(l => l.Cost).ToString("C2");

                TotalGovernmentTax = selectedLivestock.Sum(l =>
                {
                    if (l is Cow cow)
                    {
                        return cow.Milk * 0.02; // Government tax per kg for milk
                    }
                    else if (l is Sheep sheep)
                    {
                        return sheep.Wool * 0.02; // Government tax per kg for wool
                    }
                    return 0;
                }).ToString("C2");

                TotalDailyProfit = selectedLivestock.Sum(l =>
                {
                    if (l is Cow cow)
                    {
                        return cow.Milk * 9.4; // Income from milk
                    }
                    else if (l is Sheep sheep)
                    {
                        return sheep.Wool * 6.2; // Income from wool
                    }
                    return 0;
                }).ToString("C2");

                if (selectedLivestock.Any())
                {
                    AverageWeight = selectedLivestock.Average(l => l.Weight).ToString("F2");
                }
                else
                {
                    AverageWeight = "N/A";
                }

                TotalProduce = selectedLivestock.Sum(l =>
                {
                    if (l is Cow cow)
                    {
                        return cow.Milk;
                    }
                    else if (l is Sheep sheep)
                    {
                        return sheep.Wool;
                    }
                    return 0;
                }).ToString();
            }
            else
            {
                SelectedLivestockCount = 0;
                TotalLivestockCount = 0;
                TotalGovernmentTax = "$0.00";
                NetDailyProfit = "$0.00";
                AverageWeight = "0.0";
                TotalProduce = "0";
            }
        }

        // Calculate daily profit totals
        private void UpdateNetDailyProfit()
        {
            if (TotalDailyProfit != null && TotalDailyCost != null && TotalGovernmentTax != null)
            {
                if (double.TryParse(TotalDailyProfit.Replace("$", ""), out double totalProfit) &&
                    double.TryParse(TotalDailyCost.Replace("$", ""), out double totalCost) &&
                    double.TryParse(TotalGovernmentTax.Replace("$", ""), out double totalTax))
                {
                    double netProfit = totalProfit - totalCost - totalTax;
                    NetDailyProfit = netProfit.ToString("C2");
                }
                else
                {
                    NetDailyProfit = "$0.00"; // Default value if parsing fails
                }
            }
            else
            {
                NetDailyProfit = "$0.00"; // Default value if any of the properties are null
            }
        }

        private IEnumerable<Livestock> GetSelectedLivestock()
        {
            if (SelectedType == "Cow")
            {
                return CowList.Where(c => SelectedColor == "All" || c.Colour == SelectedColor);
            }
            else if (SelectedType == "Sheep")
            {
                return SheepList.Where(s => SelectedColor == "All" || s.Colour == SelectedColor);
            }
            else
            {
                return Enumerable.Empty<Livestock>();
            }
        }
    }
}