namespace SushiAssemblerFinal.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }

        public string ImagePath { get; set; }

        public double Price { get; set; }

        public Food()
        {

        }
    }
}
