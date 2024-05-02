
namespace Shooter
{
    
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1920, 1085))
            {
                game.Run();
            }
        }
    }

}