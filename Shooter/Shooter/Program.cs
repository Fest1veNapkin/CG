
namespace Shooter
{
    
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1919, 1079))
            {
                game.Run();
            }
        }
    }

}