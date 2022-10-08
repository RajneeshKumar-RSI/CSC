using CSC;

Console.WriteLine(".........................");
Console.WriteLine(".........................");
Console.WriteLine(".. Application Started ..");
Console.WriteLine(".........................");
Console.WriteLine(".........................");
Console.WriteLine();

bool ifDbExists = false;

using (var context = new DatabaseContext())
{
    context.Database.EnsureDeleted();
    ifDbExists = context.Database.EnsureCreated();
}

if (ifDbExists)
{
    var downloader = new Downloader();

    await downloader.RunAsync();
}

Console.WriteLine();
Console.WriteLine(".........................");
Console.WriteLine(".........................");
Console.WriteLine(".. Application Stoped ..");
Console.WriteLine(".........................");
Console.WriteLine(".........................");

