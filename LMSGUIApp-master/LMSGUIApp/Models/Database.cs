namespace LMSGUIApp.Models
{
    public class Database
    {
        // Establish connection FIRST
        private SQLiteConnection? _connection;
        private static Database _instance;

        public Database()
        {
            // copy db to app directory
            string dbName = "FarmDataOriginal.db";
            string dbPath = Path.Combine(Current.AppDataDirectory, dbName);
            if (!File.Exists(dbPath))
            {
                //open the db file
                using Stream stream = Current.OpenAppPackageFileAsync(dbName).Result;
                using MemoryStream memoryStream = new();
                stream.CopyTo(memoryStream);
                //write db data to app directory
                File.WriteAllBytes(dbPath, memoryStream.ToArray());
            }
            //initialize the connection
            _connection = new SQLiteConnection(dbPath);

            //optional. creates the tables if they do not exist.
            _connection.CreateTables<Cow, Sheep>();
        }
        public static Database GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Database();
            }
            return _instance;
        }
        public List<Livestock> GetAllLivestock()
        {
            return ReadItems();
        }

        // Read tables once the connection has been established
        public List<Livestock> ReadItems()
        {
            var livestock = new List<Livestock>();

            // Read cows from the database
            var cows = _connection.Table<Cow>().ToList();
            foreach (var cow in cows)
            {
                livestock.Add(new Cow // Use Cow instead of Livestock
                {
                    Id = cow.Id,
                    Cost = cow.Cost,
                    Weight = cow.Weight,
                    Colour = cow.Colour,
                    Milk = cow.Milk, // Include Milk for Cow
                    Type = "Cow"
                });
            }

            // Read sheep from the database
            var sheep = _connection.Table<Sheep>().ToList();
            foreach (var s in sheep)
            {
                livestock.Add(new Sheep
                {
                    Id = s.Id,
                    Cost = s.Cost,
                    Weight = s.Weight,
                    Colour = s.Colour,
                    Wool = s.Wool, // Include Wool for Sheep
                    Type = "Sheep"
                });
            }

            return livestock;
        }

        // Insert a livestock instance into the database 
        // long means it is compatible with sql-lite and will return the new generated ID
        public int InsertItem(Livestock item)
        {
            _connection.Insert(item);
            return item.Id; // Return the ID of the inserted item
        }

        // Delete a livestock instance from the database
        public void DeleteItem(Livestock item)
        {
            // Begin the transaction
            _connection.BeginTransaction();

            try
            {
                if (item is Cow)
                {
                    _connection.Delete<Cow>(item.Id);
                }
                else if (item is Sheep)
                {
                    _connection.Delete<Sheep>(item.Id);
                }
                else
                {
                    // Handle other types of livestock or return an error
                    throw new InvalidOperationException("Invalid livestock type");
                }

                // Commit the transaction after deletion
                _connection.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");

                // Rollback the transaction if an exception occurs
                _connection.Rollback();
                throw; // Re-throw the exception to indicate failure
            }
        }

        // Update a livestock instance in the database
        public int UpdateItem(Livestock item)
        {
            return _connection.Update(item);
        }


        // Count rows in a specific table
        public int CountItems(string tableName)
        {
            try
            {
                var query = $"SELECT COUNT(*) FROM {tableName}";
                return _connection.ExecuteScalar<int>(query);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine($"Error counting items in table '{tableName}': {ex.Message}");
                return -1; // Return -1 to indicate an error
            }
        }

        // Get a livestock item by its ID
        public Livestock GetLivestockById(int id)
        {
            try
            {
                // Check if the ID corresponds to a Cow
                var cow = _connection.Table<Cow>().FirstOrDefault(c => c.Id == id);
                if (cow != null)
                {
                    return cow;
                }

                // Check if the ID corresponds to a Sheep
                var sheep = _connection.Table<Sheep>().FirstOrDefault(s => s.Id == id);
                if (sheep != null)
                {
                    return sheep;
                }

                // If the ID doesn't match any existing livestock, return null
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving livestock by ID: {ex.Message}");
                return null; // Return null to indicate an error
            }
        }
    }
}