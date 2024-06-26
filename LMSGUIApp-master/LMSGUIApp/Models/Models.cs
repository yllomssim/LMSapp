public class Livestock
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public float Cost { get; set; }
    public float Weight { get; set; }
    public string Type { get; set; }
    public string? Colour { get; set; }
    public float GovernmentTax { get; set; }
    public float DailyProfit { get; set; }

    public override string ToString()
    {
        return $"{this.GetType().Name,-15}{this.Id,-15} {this.Id,-10} {this.Cost,-10} {this.Weight,-10} {this.Colour,-10}";
    }
}

//Maps the class from Sheep to the table in the database.
[Table("Sheep")]
public class Sheep : Livestock
{
    //sheep is a subclass of the livestock class
    public float Wool { get; set; }
    public override string ToString()
    {
        //overrides the method in the Livestock class, due to override and it is a subclass of the Livestock class.
        return base.ToString() + $"{this.Wool}";
    }
}

[Table("Cow")]
public class Cow : Livestock
{
    public float Milk { get; set; }
    public override string ToString()
    {
        return base.ToString() + $"{this.Milk}";
    }
}