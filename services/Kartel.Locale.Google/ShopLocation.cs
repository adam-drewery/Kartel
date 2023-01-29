using System.ComponentModel.DataAnnotations;
using Kartel.Environment;

namespace Kartel.Locale.Google;

public class ShopLocation
{
    [Key]
    public string Key { get; set; }
    
    public Shop Shop { get; set; }
}