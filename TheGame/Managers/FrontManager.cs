public class FrontManager
{
    private static FrontManager _instance;
    private static readonly object lockObj = new object();

    public static FrontManager GetInstance()
    {
        if (_instance == null)
        {
            lock (lockObj)
            {
                _instance ??= new FrontManager();
            }
        }
        return _instance;
    }

    private protected FrontManager()
    {
    }

    public void Printer(string output)
    {
        Console.WriteLine(output);
    }

}
