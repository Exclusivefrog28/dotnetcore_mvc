namespace beadando.Models;

public class Toy
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public int CategoryID { get; set; }
    
    public virtual Category? Category { get; set; }
}