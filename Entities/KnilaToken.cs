namespace Knila_Projects.Entities
{
    public class KnilaToken
    {
        public int Id { get; set; }                 
        public string Token { get; set; }            
        public DateTime TokenTime { get; set; }
        public DateTime ExpiryTime { get; set; }

        public int UserId { get; set; }              
        public UserLogin User { get; set; }
    }
}
