using System.ComponentModel;

namespace LMSGUIApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Livestock> _livestock;
        public ObservableCollection<Livestock> Livestock
        {
            get { return _livestock; }
            set
            {
                if (_livestock != value)
                {
                    _livestock = value;
                    OnPropertyChanged(nameof(Livestock));
                    UpdateTaxAndProfitCalculations();
                }
            }
        }

        private readonly Database _database;

        public MainViewModel()
        {
            _database = new Database();
            Livestock = new ObservableCollection<Livestock>();

            // Read items from the database and add them to the Livestock collection
            try
            {
                var items = _database.ReadItems();
                foreach (var item in items)
                {
                    Livestock.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during database read
                Console.WriteLine($"Error reading items from database: {ex.Message}");
            }

            // Subscribe to the CollectionChanged event to update calculations when the collection changes
            Livestock.CollectionChanged += (sender, e) =>
            {
                UpdateTaxAndProfitCalculations();
                UpdateLivestockStats();
                //CalculateAverageWeightDB();
            };

            UpdateTaxAndProfitCalculations();
            UpdateLivestockStats();
        }

        #region Tax and Profits
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

        private double _totalDailyProfit;
        public double TotalDailyProfit
        {
            get { return _totalDailyProfit; }
            set
            {
                if (_totalDailyProfit != value)
                {
                    _totalDailyProfit = value;
                    OnPropertyChanged(nameof(TotalDailyProfit));
                }
            }
        }

        private double _totalAverageWeight;
        public double TotalAverageWeight
        {
            get => _totalAverageWeight;
            private set
            {
                if (_totalAverageWeight != value)
                {
                    _totalAverageWeight = value;
                    OnPropertyChanged(nameof(TotalAverageWeight));
                }
            }
        }

        private double _averageWeight;
        public double AverageWeight
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

        private double _dailyProfitBeforeCost;
        public double DailyProfitBeforeCost
        {
            get { return _dailyProfitBeforeCost; }
            set
            {
                if (_dailyProfitBeforeCost != value)
                {
                    _dailyProfitBeforeCost = value;
                    OnPropertyChanged(nameof(DailyProfitBeforeCost));
                }
            }
        }

        private double _totalCost;
        public double TotalCost
        {
            get { return _totalCost; }
            set
            {
                if (_totalCost != value)
                {
                    _totalCost = value;
                    OnPropertyChanged(nameof(TotalCost));
                    UpdateTaxAndProfitCalculations();
                }
            }
        }

        private double _totalDailyCost;
        public double TotalDailyCost
        {
            get { return _totalDailyCost; }
            set
            {
                if (_totalDailyCost != value)
                {
                    _totalDailyCost = value;
                    OnPropertyChanged(nameof(TotalDailyCost));
                }
            }
        }

        private double _currentDailyProfitOfAllCows;
        public double CurrentDailyProfitOfAllCows
        {
            get { return _currentDailyProfitOfAllCows; }
            set
            {
                if (_currentDailyProfitOfAllCows != value)
                {
                    _currentDailyProfitOfAllCows = value;
                    OnPropertyChanged(nameof(CurrentDailyProfitOfAllCows));
                }
            }
        }

        private double _currentDailyProfitOfAllSheep;
        public double CurrentDailyProfitOfAllSheep
        {
            get { return _currentDailyProfitOfAllSheep; }
            set
            {
                if (_currentDailyProfitOfAllSheep != value)
                {
                    _currentDailyProfitOfAllSheep = value;
                    OnPropertyChanged(nameof(CurrentDailyProfitOfAllSheep));
                }
            }
        }
        private double _totalProfitAfterTaxAndCostCows;
        public double TotalProfitAfterTaxAndCostCows
        {
            get { return _totalProfitAfterTaxAndCostCows; }
            set
            {
                if (_totalProfitAfterTaxAndCostCows != value)
                {
                    _totalProfitAfterTaxAndCostCows = value;
                    OnPropertyChanged(nameof(TotalProfitAfterTaxAndCostCows));
                }
            }
        }

        private double _totalProfitAfterTaxAndCostSheep;
        public double TotalProfitAfterTaxAndCostSheep
        {
            get { return _totalProfitAfterTaxAndCostSheep; }
            set
            {
                if (_totalProfitAfterTaxAndCostSheep != value)
                {
                    _totalProfitAfterTaxAndCostSheep = value;
                    OnPropertyChanged(nameof(TotalProfitAfterTaxAndCostSheep));
                }
            }
        }

        private double _averageDailyProfitPerCow;
        public double AverageDailyProfitPerCow
        {
            get => _averageDailyProfitPerCow;
            set
            {
                if (_averageDailyProfitPerCow != value)
                {
                    _averageDailyProfitPerCow = value;
                    OnPropertyChanged(nameof(AverageDailyProfitPerCow));
                }
            }
        }

        private double _averageDailyProfitPerSheep;
        public double AverageDailyProfitPerSheep
        {
            get => _averageDailyProfitPerSheep;
            set
            {
                if (_averageDailyProfitPerSheep != value)
                {
                    _averageDailyProfitPerSheep = value;
                    OnPropertyChanged(nameof(AverageDailyProfitPerSheep));
                }
            }
        }

        private double _farmProfit;
        public double FarmProfit
        {
            get { return _farmProfit; }
            set
            {
                if (_farmProfit != value)
                {
                    _farmProfit = value;
                    OnPropertyChanged(nameof(FarmProfit));
                }
            }
        }

        private double _monthlyLivestockTax;
        public double MonthlyLivestockTax
        {
            get { return _monthlyLivestockTax; }
            set
            {
                if (_monthlyLivestockTax != value)
                {
                    _monthlyLivestockTax = value;
                    OnPropertyChanged(nameof(MonthlyLivestockTax));
                }
            }
        }

        private double _totalWeight;
        public double TotalWeight
        {
            get { return _totalWeight; }
            set
            {
                if (_totalWeight != value)
                {
                    _totalWeight = value;
                    OnPropertyChanged(nameof(TotalWeight));
                }
            }
        }

        private double _livestockCost;
        public double LivestockCost
        {
            get { return _livestockCost; }
            set
            {
                if (_livestockCost != value)
                {
                    _livestockCost = value;
                    OnPropertyChanged(nameof(LivestockCost));
                }
            }
        }

        private double _averageWeightDB;
        public double AverageWeightDB
        {
            get { return _averageWeightDB; }
            set
            {
                if (_averageWeightDB != value)
                {
                    _averageWeightDB = value;
                    OnPropertyChanged(nameof(AverageWeightDB));
                }
            }
        }

        private void CalculateAverageDailyProfit()
        {
            if (Livestock == null || Livestock.Count == 0)
            {
                AverageDailyProfitPerCow = 0.0;
                AverageDailyProfitPerSheep = 0.0;
                return;
            }

            // Calculate average daily profit per cow
            var cows = Livestock.OfType<Cow>();
            if (cows.Any())
            {
                // Calculate total daily profit for all cows
                double totalCowProfit = cows.Sum(c => (c.Milk * 9.4) - (c.Milk * 0.02));

                // Calculate total daily cost for all cows
                double totalCowCost = cows.Sum(c => c.Cost);

                // Calculate average daily profit per cow
                double averageDailyProfitPerCow = (totalCowProfit - totalCowCost) / cows.Count();

                AverageDailyProfitPerCow = averageDailyProfitPerCow;
            }
            else
            {
                AverageDailyProfitPerCow = 0.0;
            }

            // Calculate average daily profit per sheep
            var sheeps = Livestock.OfType<Sheep>();
            if (sheeps.Any())
            {
                // Calculate total daily profit for all sheep
                double totalSheepProfit = sheeps.Sum(s => (s.Wool * 6.2) - (s.Wool * 0.02));

                // Calculate total daily cost for all sheep
                double totalSheepCost = sheeps.Sum(s => s.Cost);

                // Calculate average daily profit per sheep
                double averageDailyProfitPerSheep = (totalSheepProfit - totalSheepCost) / sheeps.Count();

                AverageDailyProfitPerSheep = averageDailyProfitPerSheep;
            }
            else
            {
                AverageDailyProfitPerSheep = 0.0;
            }
        }

        private void UpdateTaxAndProfitCalculations()
        {
            // Calculate total variables for cows
            double totalMilkCows = 0.0;
            double totalCostCows = 0.0;
            double totalProfitCows = 0.0;
            double totalTaxCows = 0.0;

            foreach (var livestock in Livestock.OfType<Cow>())
            {
                double milk = livestock.Milk;
                double cost = livestock.Cost;

                totalMilkCows += milk;
                totalCostCows += cost;
                totalProfitCows += milk * 9.4;
                totalTaxCows += milk * 0.02;
            }

            // Calculate total variables for sheep
            double totalWoolSheep = 0.0;
            double totalCostSheep = 0.0;
            double totalProfitSheep = 0.0;
            double totalTaxSheep = 0.0;

            foreach (var livestock in Livestock.OfType<Sheep>())
            {
                double wool = livestock.Wool;
                double cost = livestock.Cost;

                totalWoolSheep += wool;
                totalCostSheep += cost;
                totalProfitSheep += wool * 6.2;
                totalTaxSheep += wool * 0.02;
            }

            // Calculate total daily profit for cows and sheep after deducting tax and cost
            double totalProfitAfterTaxAndCostCows = totalProfitCows - totalTaxCows - totalCostCows;
            double totalProfitAfterTaxAndCostSheep = totalProfitSheep - totalTaxSheep - totalCostSheep;

            // Assign the calculated values to the ViewModel properties
            TotalProfitAfterTaxAndCostCows = totalProfitAfterTaxAndCostCows;
            TotalProfitAfterTaxAndCostSheep = totalProfitAfterTaxAndCostSheep;

            // Calculate total farm profit
            double farmProfit = totalProfitAfterTaxAndCostCows + totalProfitAfterTaxAndCostSheep;
            FarmProfit = farmProfit;

            // Calculate monthly livestock tax
            double monthlyLivestockTax = (totalTaxSheep + totalTaxCows) * 30; // 30 days in a month
            MonthlyLivestockTax = monthlyLivestockTax;

            // Calculate total livestock cost
            double livestockCost = totalCostCows + totalCostSheep;
            LivestockCost = livestockCost;

            // Calculate and update total average weight
            double totalWeightDB = Livestock.Sum(l => l.Weight);
            double averageWeightDB = totalWeightDB / Livestock.Count;
            AverageWeightDB = averageWeightDB;

            CalculateAverageDailyProfit();
        }

        private void TaxAndProfitCalculations()
        {
            // Calculate total government tax
            double totalGovernmentTax = 0.0;
            foreach (var livestock in Livestock)
            {
                if (livestock is Cow cow)
                {
                    totalGovernmentTax += cow.Milk * 0.02; // Calculate tax based on milk production for cows
                }
                else if (livestock is Sheep sheep)
                {
                    totalGovernmentTax += sheep.Wool * 0.02; // Calculate tax based on wool production for sheep
                }
            }
            TotalGovernmentTax = totalGovernmentTax.ToString("C2"); // Format as currency with $ symbol and 2 decimal places

            // Calculate total daily income before cost and tax
            double totalIncomeBeforeCostAndTax = 0.0;
            foreach (var livestock in Livestock)
            {
                if (livestock is Cow cow)
                {
                    totalIncomeBeforeCostAndTax += (cow.Milk * 9.4); // Calculate income from milk production for cows
                }
                else if (livestock is Sheep sheep)
                {
                    totalIncomeBeforeCostAndTax += (sheep.Wool * 6.2); // Calculate income from wool production for sheep
                }
            }

            // Calculate total weight of all livestock
            double totalWeight = Livestock.Sum(l =>
            {
                if (l is Cow cow)
                {
                    return cow.Weight;
                }
                else if (l is Sheep sheep)
                {
                    return sheep.Weight;
                }
                else
                {
                    return 0.0;
                }
            });

            TotalWeight = totalWeight;

            // Apply total tax deduction
            totalIncomeBeforeCostAndTax -= totalGovernmentTax;

            DailyProfitBeforeCost = totalIncomeBeforeCostAndTax;

            // Calculate total daily cost
            TotalDailyCost = Livestock.Sum(l => l.Cost);

            // Calculate total daily profit after deducting cost and tax
            TotalDailyProfit = DailyProfitBeforeCost - TotalDailyCost;

            // Calculate total daily profit for all cows
            double totalDailyProfitCows = Livestock.OfType<Cow>().Sum(c => (c.Milk * 9.4)) - Livestock.OfType<Cow>().Sum(c => c.Cost);
            CurrentDailyProfitOfAllCows = totalDailyProfitCows;

            // Calculate total daily profit for all sheep
            double totalDailyProfitSheep = Livestock.OfType<Sheep>().Sum(s => (s.Wool * 6.2)) - Livestock.OfType<Sheep>().Sum(s => s.Cost);
            CurrentDailyProfitOfAllSheep = totalDailyProfitSheep;
        }
        #endregion

        #region Livestock Statistics
        private string _selectedType;
        public string SelectedType
        {
            get => _selectedType;
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                    UpdateLivestockStats();
                }
            }
        }

        private string _selectedColor;
        public string SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged(nameof(SelectedColor));
                    UpdateLivestockStats();
                }
            }
        }

        private string _livestockStats;
        public string LivestockStats
        {
            get => _livestockStats;
            set
            {
                if (_livestockStats != value)
                {
                    _livestockStats = value;
                    OnPropertyChanged(nameof(LivestockStats));
                }
            }
        }

        private void UpdateLivestockStats()
        {
            if (string.IsNullOrEmpty(SelectedType) || string.IsNullOrEmpty(SelectedColor))
            {
                LivestockStats = "Please select both type and color.";
                return;
            }

            // Check the count of livestock that match the selected type and color
            var selectedLivestock = Livestock.Where(l => l.Type.Equals(SelectedType, StringComparison.OrdinalIgnoreCase) && l.Colour.Equals(SelectedColor, StringComparison.OrdinalIgnoreCase)).ToList();

            if (selectedLivestock.Count == 0)
            {
                LivestockStats = $"No {SelectedType}s found with {SelectedColor} color.";
                AverageWeight = 0.0; // Set average weight to 0 if no matching livestock
                return;
            }

            // Calculate total weight and average weight
            double totalWeight = selectedLivestock.Sum(l => l.Weight);
            double averageWeight = totalWeight / selectedLivestock.Count;

            // Update average weight property
            AverageWeight = averageWeight;

            // Update statistics display
            LivestockStats = $"Total {SelectedType}s with {SelectedColor} color: {selectedLivestock.Count}\n";
            LivestockStats += $"Average weight: {averageWeight:F2} kg";
        }
        #endregion

        public string QueryByLivestockType(string type)
        {
            List<Livestock> sts = Livestock.Where(x => x.GetType().Name.Equals(type)).ToList();
            string results = $"Number of {type}:\t{sts.Count}\n";
            results += $"Average total cost:\t{sts.Average(x => x.Cost)}\n";
            results += $"Average total weight:\t{sts.Average(x => x.Weight)}";
            return results;
        }
        public void RefreshData()
        {
            var livestockData = _database.GetAllLivestock();
            Livestock.Clear();
            foreach (var livestock in livestockData)
            {
                Livestock.Add(livestock);
            }
        }

        public void RefreshLivestockData()
        {
            try
            {
                Livestock.Clear(); // Clear the existing collection

                // Read items from the database and add them to the Livestock collection
                var items = _database.ReadItems();
                foreach (var item in items)
                {
                    Livestock.Add(item);
                }
                OnPropertyChanged(nameof(Livestock));
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during database read
                Console.WriteLine($"Error reading items from database: {ex.Message}");
            }
        }
    }
}