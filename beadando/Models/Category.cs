namespace beadando.Models;

public class Category
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Toy>? Toys { get; set; }
}