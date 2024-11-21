namespace Knila_Projects.Entities
{
    public class UserLogin
    {
        public int UserId { get; set; }           
        public string? Username { get; set; }     
        public string? Password { get; set; }       
        public DateTime? LoginTime { get; set; }    
    }
}
