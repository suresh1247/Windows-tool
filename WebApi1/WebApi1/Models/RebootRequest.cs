
using System.ComponentModel.DataAnnotations;

public class RebootRequest
{
    RebootRequest()
    {
        
    }
    [Key]
     public DateTime dateTime { get; set; }
    public string ServerName { get; set; }
    public string Comment { get; set; }
}
 