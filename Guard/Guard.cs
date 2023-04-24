
namespace DanskeBank.Guard;

public class Guard : IGuard
{
    private Guard() { }
    public static IGuard Init { get; } = new Guard();
}