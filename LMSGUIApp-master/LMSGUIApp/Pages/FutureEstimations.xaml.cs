namespace LMSGUIApp.Pages
{
    public partial class FutureEstimations : ContentPage
    {
        private readonly Database _database;

        public FutureEstimations()
        {
            InitializeComponent();
            _database = Database.GetInstance();
        }
        // Reset button method
        private void OnCalculateButtonClicked(object sender, EventArgs e)
        {
            string selectedLivestock = LivestockPicker.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedLivestock))
            {
                ResultLabel.Text = "Please select a livestock type.";
                return;
            }

            if (!int.TryParse(QuantityEntry.Text, out int quantity) || quantity <= 0)
            {
                ResultLabel.Text = "Please enter a valid quantity.";
                return;
            }

            double averageProfitPerUnit = CalculateAverageProfit(selectedLivestock);
            double estimatedDailyProfit = averageProfitPerUnit * quantity;

            // Display results
            ResultLabel.Text = $"Buying {quantity} {selectedLivestock} would bring:\n" +
                               $"After costs & tax: {estimatedDailyProfit:C2}";
        }

        // Calculate average profit
        private double CalculateAverageProfit(string livestockType)
        {
            var livestockList = _database.GetAllLivestock();

            switch (livestockType.ToLower())
            {
                case "cow":
                    double avgCowProfit = livestockList.OfType<Cow>().Average(c =>
                    {
                        double produceProfit = 9.4 * c.Milk;
                        double tax = c.Milk * 0.02;
                        return produceProfit - c.Cost - tax;
                    });
                    return avgCowProfit;
                case "sheep":
                    double avgSheepProfit = livestockList.OfType<Sheep>().Average(s =>
                    {
                        double produceProfit = 6.2 * s.Wool;
                        double tax = s.Wool * 0.02;
                        return produceProfit - s.Cost - tax;
                    });
                    return avgSheepProfit;
                default:
                    return 0.0;
            }
        }

        //Reset picker and labels
        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            LivestockPicker.SelectedIndex = -1;
            QuantityEntry.Text = string.Empty;
            ResultLabel.Text = string.Empty;
        }
    }
}