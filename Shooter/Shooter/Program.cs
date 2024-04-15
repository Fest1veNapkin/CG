
namespace Shooter
{
    
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(900, 700))
            {
                game.Run();
            }
        }
    }

}