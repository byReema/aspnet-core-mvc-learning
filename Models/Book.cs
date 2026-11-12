namespace MyMvcApp.Models;

public class Book
{
    public int Id{get; set;}
    public string Title{get; set;} = string.Empty; // gives the property a starting value: an empty text "" rether than null
    public string Author{get; set;} = string.Empty;
    public int Year{get; set;} 
    // int can never be null. It automatically starts at 0
    
    
}