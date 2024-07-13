// This line creates a new instance, and wraps the instance in a using statement so it's automatically disposed once we've exited the block.
using GraphicsApp;

using (Game game = new Game(800, 600, "LearnOpenTK"))
{
    game.Run();
}